using PCLStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using PCL.Utils;

[assembly: Dependency(typeof(Forms9Patch.UWP.FontService))]
namespace Forms9Patch.UWP
{
    internal class FontService : IFontService
    {

        // general concept:
        // 1) if the font is cached, return it.
        // 2) if the font is a file and can be copied and cached, do that and then return it.
        // 3) if the font is a url:
        //      a) kick off a background process to get the font
        //      b) return a hash (which can't be loaded - so the default font will be loaded instead).  
        //      c) when the background process is complete, go through all the VisualElements:
        //          i) check for the "FontFamily" properties. 
        //          ii) if it exists, reset it to the cached FontFamily
        //

        public static Dictionary<string, string> EmbeddedFontSources = new Dictionary<string, string>();

        //public static Dictionary<string, string> FileFontSources = new Dictionary<string, string>();

        //public static Dictionary<string, string> UrlFontSources = new Dictionary<string, string>();


        public static string ReconcileFontFamily(string embeddedResourceId, Assembly assembly=null)
        {
            if (string.IsNullOrWhiteSpace(embeddedResourceId))
                return Windows.UI.Xaml.Media.FontFamily.XamlAutoFontFamily.Source;

            if (EmbeddedFontSources.ContainsKey(embeddedResourceId))
                return EmbeddedFontSources[embeddedResourceId];

            var idParts = embeddedResourceId.Split('#');
            var id = idParts[0];

            string localStorageFileName = null;
            if (assembly!=null)
                localStorageFileName = EmbeddedResourceCache.LocalStorageSubPathForEmbeddedResource(id, assembly);
            if (localStorageFileName==null)
            {
                // we've got to go hunting for this ... and UWP doesn't give us much help
                // first, try the main assembly!
                var targetAsmName = embeddedResourceId.Split('.').First();
               
                if (targetAsmName == Forms9Patch.ApplicationInfoService.Assembly.GetName().Name)
                    localStorageFileName = EmbeddedResourceCache.LocalStorageSubPathForEmbeddedResource(id, Forms9Patch.ApplicationInfoService.Assembly);

                // if that doesn't work, look through all known assemblies
                if (localStorageFileName == null)
                {
                    foreach (var asm in Settings.AssembliesToInclude)
                    {
                        if (targetAsmName == asm.GetName().Name)
                        {
                            localStorageFileName = EmbeddedResourceCache.LocalStorageSubPathForEmbeddedResource(id, Forms9Patch.ApplicationInfoService.Assembly);
                            break;
                        }
                    }
                }
            }

            if (localStorageFileName == null)
                return embeddedResourceId;

            string fontName = null;
            if (idParts.Count() > 1)
                fontName = idParts.Last();
            else
            {
                var cachedFile = FileSystem.Current.LocalStorage.GetFile(localStorageFileName);
                fontName = TTFAnalyzer.FontFamily(cachedFile);
            }
            var uwpFontFamily = "ms-appdata:///local/" + localStorageFileName + (string.IsNullOrWhiteSpace(fontName) ? null : "#" + fontName);
            EmbeddedFontSources.Add(embeddedResourceId, uwpFontFamily);
            return uwpFontFamily;
        }

        /*
        public async Task<FontSource> GetFileFontSource(IFile file)
        {
            if (file == null)
                return null;

            if (FileFontSources.ContainsKey(file.Path))
                return FileFontSources[file.Path];

            var localStorageFileName = await PCL.Utils.FileCache.CacheAsync(file);
            var cachedFile = await PCLStorage.FileSystem.Current.LocalStorage.GetFileAsync(localStorageFileName);
            var fontName = PCL.Utils.TTFAnalyzer.FontFamily(cachedFile);

            var fontFamilyString = "ms-appdata:///local/" + localStorageFileName + "#" + fontName;
            var result = new FileFontSource { FamilyName = fontName, File = file, XamarinFormsFontFamily = fontFamilyString };
            FileFontSources.Add(file.Path, result);
            return result;
        }

        public async Task<FontSource> GetUriFontSource(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return null;

            if (UrlFontSources.ContainsKey(url))
                // this will return UrlFontSources that are actively caching
                return UrlFontSources[url];

            if (PCL.Utils.DownloadCache.IsCached(url))
            {
                // this will happen if remote font was cached in a previous session
                var localStorageFileName = await PCL.Utils.DownloadCache.DownloadAsync(url);
                var cachedFile = await PCLStorage.FileSystem.Current.LocalStorage.GetFileAsync(localStorageFileName);
                var fontName = PCL.Utils.TTFAnalyzer.FontFamily(cachedFile);

                var fontFamilyString = "ms-appdata:///local/" + localStorageFileName + "#" + fontName;
                var result = new UrlFontSource { FamilyName = fontName, Url = url, XamarinFormsFontFamily = fontFamilyString };
                UrlFontSources.Add(url, result);
                return result;
            }
            else
            {
                var cachingFamilyName = PCL.Utils.MD5.GetMd5String("caching:" + url);
                var result = new UrlFontSource { Url = url, XamarinFormsFontFamily = cachingFamilyName };
                UrlFontSources.Add(url, result);
                var task = new Task(async () =>
                {
                    var localStorageFileName = await PCL.Utils.DownloadCache.DownloadAsync(url);
                    var cachedFile = await PCLStorage.FileSystem.Current.LocalStorage.GetFileAsync(localStorageFileName);
                    var fontName = PCL.Utils.TTFAnalyzer.FontFamily(cachedFile);

                    var fontFamilyString = "ms-appdata:///local/" + localStorageFileName + "#" + fontName;
                    var oldXamarinFormsFontFamily = result.XamarinFormsFontFamily;
                    result.XamarinFormsFontFamily = fontFamilyString;
                    result.FamilyName = fontName;

                    var textElements = FormsGestures.VisualElementExtensions.FindVisualElementsWithProperty("FontFamily");
                    var f9pLabels = FormsGestures.VisualElementExtensions.FindVisualElementsOfType<Forms9Patch.Label>("F9PFormattedString");
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        foreach (var textElement in textElements)
                        {
                            var fontFamilyValue = PCL.Utils.ReflectionExtensions.GetPropertyValue(textElement, "FontFamily") as string;
                            if (fontFamilyValue == cachingFamilyName)
                                PCL.Utils.ReflectionExtensions.SetPropertyValue(textElement, "FontFamily", fontFamilyString);
                            if (PCL.Utils.ReflectionExtensions.GetPropertyValue(textElement, "FormattedString") is Xamarin.Forms.FormattedString formattedString)
                            {
                                foreach (var span in formattedString.Spans)
                                    if (span.FontFamily == cachingFamilyName)
                                        span.FontFamily = fontFamilyString;
                            }
                        }

                        foreach (var label in f9pLabels)
                        {
                            var spans = label.F9PFormattedString._spans;
                            foreach (var span in spans)
                                if (span is Forms9Patch.FontFamilySpan fontFamilySpan && fontFamilySpan.FontFamilyName == cachingFamilyName)
                                    fontFamilySpan.FontFamilyName = fontFamilyString;
                        }
                    });
                });
                task.Start();
                return result;
            }
        }
        */


        public double LineHeight(string fontFamily, double fontSize, FontAttributes fontAttributes)
        {
            throw new NotImplementedException();
        }

        public double LineSpace(string fontFamily, double fontSize, FontAttributes fontAttributes)
        {
            throw new NotImplementedException();
        }
    }

   
}
