using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using P42.Utils;
using SharpDX.DirectWrite;
using Windows.UI.Xaml.Controls;

[assembly: Dependency(typeof(Forms9Patch.UWP.FontService))]
namespace Forms9Patch.UWP
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    internal class FontService : IFontService
    {
        static bool _xamlAutoFontFamilyPresent;
        static bool _xamlAutoFontFamilyPresentSet;
        public static bool XamlAutoFontFamilyPresent
        {
            get
            {
                if (!_xamlAutoFontFamilyPresentSet)
                {
                    _xamlAutoFontFamilyPresent = Windows.Foundation.Metadata.ApiInformation.IsPropertyPresent("Windows.UI.Xaml.Media.FontFamily", "XamlAutoFontFamily");
                    _xamlAutoFontFamilyPresentSet = true;
                }
                return _xamlAutoFontFamilyPresent;
            }
        }

        static string _defaultFontFamily;
        public static string DefaultSystemFontFamily
        {
            get
            {
                if (_defaultFontFamily==null)
                {
                    if (XamlAutoFontFamilyPresent)
                        _defaultFontFamily = Windows.UI.Xaml.Media.FontFamily.XamlAutoFontFamily.Source;
                    else
                        _defaultFontFamily = "DEFAULT_FONT_FAMILY_NOT_AVAIABLE";
                }
                return _defaultFontFamily;
            }
        }


        public static Dictionary<string, string> EmbeddedFontSources = new Dictionary<string, string>();

        public static Windows.UI.Xaml.Media.FontFamily GetWinFontFamily(string f9pFontFamily)
        {
            if (f9pFontFamily == null)
                return (Windows.UI.Xaml.Media.FontFamily)Windows.UI.Xaml.Application.Current.Resources["ContentControlThemeFontFamily"];
            return new Windows.UI.Xaml.Media.FontFamily(ReconcileFontFamily(f9pFontFamily));
        }

        public static string ReconcileFontFamily(string f9pFontFamily)
        {
            if (string.IsNullOrWhiteSpace(f9pFontFamily))
                return DefaultSystemFontFamily;

            string localStorageFileName = null;
            Assembly localStorageAssemby = null;
            string uri = null;
            string resourceId;

            switch (f9pFontFamily.ToLower())
            {
                case "monospace":
                    return "Consolas";
                case "serif":
                    return "Cambria";
                case "sans-serif":
                    return "Segoe UI";
                case "stixgeneral":
                    resourceId = "Forms9Patch.Resources.Fonts.STIXGeneral.otf";
                    f9pFontFamily = resourceId + "#" + "STIXGeneral";
                    break;
            }

            if (EmbeddedFontSources.ContainsKey(f9pFontFamily))
                return EmbeddedFontSources[f9pFontFamily];

            var idParts = f9pFontFamily.Split('#');
            resourceId = idParts[0];



            if (localStorageFileName==null)
            {
                // we've got to go hunting for this ... and UWP doesn't give us much help
                // first, try the main assembly!
                var targetParts = f9pFontFamily.Split('.');
                var targetAsmNameA = "invalid_assembly_name";
                if (targetParts.Contains("Resources"))
                {
                    targetAsmNameA = "";
                    foreach (var part in targetParts)
                    {
                        if (part == "Resources")
                            break;
#pragma warning disable CC0039 // Don't concatenate strings in loops
                        targetAsmNameA += part + ".";
#pragma warning restore CC0039 // Don't concatenate strings in loops
                    }
                    if (targetAsmNameA.Length > 0)
                        targetAsmNameA = targetAsmNameA.Substring(0, targetAsmNameA.Length - 1);
                }
                var targetAsmNameB = targetParts.First();

                var appAsmName = Forms9Patch.ApplicationInfoService.Assembly.GetName().Name;
                if (targetAsmNameA == appAsmName || targetAsmNameB == appAsmName)
                {
                    localStorageFileName = EmbeddedResourceCache.LocalStorageSubPathForEmbeddedResource(resourceId, Forms9Patch.ApplicationInfoService.Assembly);
                    localStorageAssemby = Forms9Patch.ApplicationInfoService.Assembly;
                    if (localStorageFileName != null)
                        uri = EmbeddedResourceCache.ApplicationUri(resourceId, Forms9Patch.ApplicationInfoService.Assembly);
                }

                // if that doesn't work, look through all known assemblies
                if (localStorageFileName == null)
                {
                    foreach (var asm in Settings.AssembliesToInclude)
                    {
                        var asmName = asm.GetName().Name;
                        if (targetAsmNameA == asmName || targetAsmNameB == asmName)
                        {
                            localStorageFileName = EmbeddedResourceCache.LocalStorageSubPathForEmbeddedResource(resourceId, asm);
                            localStorageAssemby = asm;
                            uri = EmbeddedResourceCache.ApplicationUri(resourceId, asm);
                            break;
                        }
                    }
                }
            }

            if (localStorageFileName == null)
                return f9pFontFamily;

            string fontName;
            if (idParts.Count() > 1)
                fontName = idParts.Last();
            else
            {
                //var cachedFilePath = System.IO.Path.Combine(P42.Utils.Environment.ApplicationDataPath, localStorageFileName);
                var cachedFilePath = System.IO.Path.Combine(P42.Utils.EmbeddedResourceCache.FolderPath(localStorageAssemby), localStorageFileName);
                fontName = TTFAnalyzer.FontFamily(cachedFilePath);
            }
            //var uwpFontFamily = "ms-appdata:///local/" + localStorageFileName.Replace('\\','/') + (string.IsNullOrWhiteSpace(fontName) ? null : "#" + fontName);
            var uwpFontFamily = uri + (string.IsNullOrWhiteSpace(fontName) ? null : "#" + fontName);
            //var uwpFontFamily = "ms-appdata:///local/EmbeddedResourceCache/02fe60e0da81514d145d946ab9ad9b97#Pacifico";
            //foreach (var c in uwpFontFamily)
            //    System.Diagnostics.Debug.WriteLine("c=["+c+"]["+(int)c+"]");
            EmbeddedFontSources.Add(f9pFontFamily, uwpFontFamily);
            return uwpFontFamily;
        }

        public double LineHeight(string fontFamily, double fontSize, FontAttributes fontAttributes)
        {
            var dxFont = FontExtensions.GetDxFont(fontFamily, FontWeight.Normal, FontStretch.Normal, fontAttributes.ToDxFontStyle());
            //return FontExtensions.LineHeightForFontSize(fontSize);
            return dxFont.Metrics.HeightForLinesAtFontSize(1, fontSize);
        }

        public double LineSpace(string fontFamily, double fontSize, FontAttributes fontAttributes)
        {
            return 0;
        }
    }

   
}
