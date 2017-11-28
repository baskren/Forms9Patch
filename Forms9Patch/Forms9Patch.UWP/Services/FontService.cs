using PCLStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(Forms9Patch.UWP.FontService))]
namespace Forms9Patch.UWP
{
    class FontService : IFontService
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



        Dictionary<string,EmbeddedResourceFontSource> _embeddedFontSources = new Dictionary<string, EmbeddedResourceFontSource>();
        public async Task<FontSource> GetEmbeddedResourceFontSource(string embeddedResourceId, Assembly assembly)
        {
            if (assembly ==null || string.IsNullOrWhiteSpace(embeddedResourceId))
                return null;

            if (_embeddedFontSources.ContainsKey(embeddedResourceId))
                return _embeddedFontSources[embeddedResourceId];

            var idParts = embeddedResourceId.Split('#');
            var id = idParts[0];
            var localStorageFileName = await PCL.Utils.EmbeddedResourceCache.LocalStorageSubPathForEmbeddedResourceAsync(id, assembly);

            if (localStorageFileName == null)
                return null;

            string fontName = null;
            if (idParts.Count() > 1)
                fontName = idParts.Last();
            else
            {
                var cachedFile = await PCLStorage.FileSystem.Current.LocalStorage.GetFileAsync(localStorageFileName);
                fontName = PCL.Utils.TTFAnalyzer.FontFamily(cachedFile);
            }
            var fontFamilyString = "ms-appdata:///local/" + localStorageFileName + "#" + fontName;
            var result = new EmbeddedResourceFontSource { FamilyName = fontName, EmbeddedResourceId = id, Assembly = assembly, XamarinFormsFontFamily = fontFamilyString };
            _embeddedFontSources.Add(embeddedResourceId,result);
            return result;
        }


        Dictionary<string,FileFontSource> _fileFontSources = new Dictionary<string, FileFontSource>();
        public async Task<FontSource> GetFileFontSource(IFile file)
        {
            if (file == null)
                return null;

            if (_fileFontSources.ContainsKey(file.Path))
                return _fileFontSources[file.Path];

            var localStorageFileName = await PCL.Utils.FileCache.CacheAsync(file);
            var cachedFile = await PCLStorage.FileSystem.Current.LocalStorage.GetFileAsync(localStorageFileName);
            var fontName = PCL.Utils.TTFAnalyzer.FontFamily(cachedFile);

            var fontFamilyString = "ms-appdata:///local/" + localStorageFileName + "#" + fontName;
            var result = new FileFontSource { FamilyName = fontName, File = file, XamarinFormsFontFamily = fontFamilyString };
            _fileFontSources.Add(file.Path, result);
            return result;
        }

        Dictionary<string,UrlFontSource> _urlFontSources = new Dictionary<string, UrlFontSource>();
        public async Task<FontSource> GetUriFontSource(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return null;

            if (_urlFontSources.ContainsKey(url))
                // this will return UrlFontSources that are actively caching
                return _urlFontSources[url];

            if (PCL.Utils.DownloadCache.IsCached(url))
            {
                // this will happen if remote font was cached in a previous session
                var localStorageFileName = await PCL.Utils.DownloadCache.DownloadAsync(url);
                var cachedFile = await PCLStorage.FileSystem.Current.LocalStorage.GetFileAsync(localStorageFileName);
                var fontName = PCL.Utils.TTFAnalyzer.FontFamily(cachedFile);

                var fontFamilyString = "ms-appdata:///local/" + localStorageFileName + "#" + fontName;
                var result = new UrlFontSource { FamilyName = fontName, Url = url, XamarinFormsFontFamily = fontFamilyString };
                _urlFontSources.Add(url, result);
                return result;
            }
            else
            {
                var cachingFamilyName = PCL.Utils.MD5.GetMd5String("caching:" + url);
                var result = new UrlFontSource { Url = url, XamarinFormsFontFamily = cachingFamilyName };
                _urlFontSources.Add(url, result);
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
