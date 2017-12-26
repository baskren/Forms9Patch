using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using PCL.Utils;
#if NETSTANDARD
#else
using PCLStorage;
#endif

[assembly: Dependency(typeof(Forms9Patch.UWP.FontService))]
namespace Forms9Patch.UWP
{
    internal class FontService : IFontService
    {

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
                return Windows.UI.Xaml.Media.FontFamily.XamlAutoFontFamily.Source;

            string localStorageFileName = null;
            string resourceId = null;

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
                string targetAsmNameA = "invalid_assembly_name";
                if (targetParts.Contains("Resources"))
                {
                    targetAsmNameA = "";
                    foreach (var part in targetParts)
                    {
                        if (part == "Resources")
                            break;
                        targetAsmNameA += part + ".";
                    }
                    if (targetAsmNameA.Length > 0)
                        targetAsmNameA.Substring(0, targetAsmNameA.Length - 1);
                }
                var targetAsmNameB = targetParts.First();

                var appAsmName = Forms9Patch.ApplicationInfoService.Assembly.GetName().Name;
                if (targetAsmNameA == appAsmName || targetAsmNameB == appAsmName)
                    localStorageFileName = EmbeddedResourceCache.LocalStorageSubPathForEmbeddedResource(resourceId, Forms9Patch.ApplicationInfoService.Assembly);

                // if that doesn't work, look through all known assemblies
                if (localStorageFileName == null)
                {
                    foreach (var asm in Settings.AssembliesToInclude)
                    {
                        var asmName = asm.GetName().Name;
                        if (targetAsmNameA == asmName || targetAsmNameB == asmName)
                        {
                            localStorageFileName = EmbeddedResourceCache.LocalStorageSubPathForEmbeddedResource(resourceId, Forms9Patch.ApplicationInfoService.Assembly);
                            break;
                        }
                    }
                }
            }

            if (localStorageFileName == null)
                return f9pFontFamily;

            string fontName = null;
            if (idParts.Count() > 1)
                fontName = idParts.Last();
            else
            {
#if NETSTANDARD
                var cachedFilePath = System.IO.Path.Combine(PCL.Utils.Environment.ApplicationDataPath, localStorageFileName);
                fontName = TTFAnalyzer.FontFamily(cachedFilePath);
#else
                var cachedFile = FileSystem.Current.LocalStorage.GetFile(localStorageFileName);
                fontName = TTFAnalyzer.FontFamily(cachedFile);
#endif
            }
            var uwpFontFamily = "ms-appdata:///local/" + localStorageFileName.Replace('\\','/') + (string.IsNullOrWhiteSpace(fontName) ? null : "#" + fontName);
            //var uwpFontFamily = "ms-appdata:///local/EmbeddedResourceCache/02fe60e0da81514d145d946ab9ad9b97#Pacifico";
            //foreach (var c in uwpFontFamily)
            //    System.Diagnostics.Debug.WriteLine("c=["+c+"]["+(int)c+"]");
            EmbeddedFontSources.Add(f9pFontFamily, uwpFontFamily);
            return uwpFontFamily;
        }

        public double LineHeight(string fontFamily, double fontSize, FontAttributes fontAttributes)
        {
            return FontExtensions.LineHeightForFontSize(fontSize);
        }

        public double LineSpace(string fontFamily, double fontSize, FontAttributes fontAttributes)
        {
            return 0;
        }
    }

   
}
