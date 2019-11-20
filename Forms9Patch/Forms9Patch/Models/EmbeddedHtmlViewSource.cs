using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Forms9Patch
{
    [DesignTimeVisible(true)]
    public class EmbeddedHtmlViewSource : Xamarin.Forms.HtmlWebViewSource, IDisposable
    {
        #region Properties
        #region EmbeddedResourceFolder Property
        public static readonly BindableProperty EmbeddedResourceFolderProperty = BindableProperty.Create(nameof(EmbeddedResourceFolder), typeof(string), typeof(EmbeddedHtmlViewSource), default(string));
        public string EmbeddedResourceFolder
        {
            get => (string)GetValue(EmbeddedResourceFolderProperty);
            set => SetValue(EmbeddedResourceFolderProperty, value);
        }
        #endregion

        #region HtmlFileName Property
        public static readonly BindableProperty HtmlDocEmbeddedResourceIdProperty = BindableProperty.Create(nameof(HtmlDocEmbeddedResourceId), typeof(string), typeof(EmbeddedHtmlViewSource), default(string));
        public string HtmlDocEmbeddedResourceId
        {
            get => (string)GetValue(HtmlDocEmbeddedResourceIdProperty);
            set => SetValue(HtmlDocEmbeddedResourceIdProperty, value);
        }
        #endregion

        #endregion


        #region Fields
        Assembly Assembly;
        #endregion

        private EmbeddedHtmlViewSource() { }

        private EmbeddedHtmlViewSource(string embeddedResourceFolder, string htmlDocEmbeddedResourceId, Assembly assembly = null)
        {
            EmbeddedResourceFolder = embeddedResourceFolder.Trim(new[] { '.', ' ' });
            HtmlDocEmbeddedResourceId = htmlDocEmbeddedResourceId;

            if (string.IsNullOrWhiteSpace(EmbeddedResourceFolder))
                throw new Exception("Invalid EmbeddedResourceFolder: cannot be null, empty, or whitespace.");
            if (string.IsNullOrWhiteSpace(HtmlDocEmbeddedResourceId))
                throw new Exception("Invalid HtmlFileName: cannot be null, empty, or whitespace.");

            if (!HtmlDocEmbeddedResourceId.StartsWith(EmbeddedResourceFolder, StringComparison.Ordinal))
                HtmlDocEmbeddedResourceId = EmbeddedResourceFolder + "." + HtmlDocEmbeddedResourceId;

            Assembly = assembly ?? EmbeddedResourceExtensions.FindAssemblyForResource(HtmlDocEmbeddedResourceId);

            if (Assembly == null)
                throw new Exception("Was not provided and cannot find assembly with EmbeddedResourceId=[" + HtmlDocEmbeddedResourceId + "]");

            var resourceNames = Assembly.GetManifestResourceNames();
            if (!resourceNames.Contains(HtmlDocEmbeddedResourceId))
                throw new Exception("Cannot find EmbeddedResourceId=[" + HtmlDocEmbeddedResourceId + "] in assemby [" + Assembly.FullName + "]");
        }

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _disposed = true;
                Clear(EmbeddedResourceFolder, Assembly);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        public static async Task<EmbeddedHtmlViewSource> Create(string embeddedResourceFolder, string htmlDocEmbeddedResourceId, Assembly assembly = null)
        {
            if (new EmbeddedHtmlViewSource(embeddedResourceFolder, htmlDocEmbeddedResourceId, assembly) is EmbeddedHtmlViewSource source)
            {
                await source.Initialize();
                return source;
            }
            return null;
        }

        public static void Clear(string embeddedResourceFolder, Assembly assembly)
        {
            P42.Utils.EmbeddedResourceCache.Clear(null, assembly, embeddedResourceFolder);
        }

        public async Task Initialize()
        {
            if (string.IsNullOrWhiteSpace(EmbeddedResourceFolder) || string.IsNullOrWhiteSpace(HtmlDocEmbeddedResourceId))
                return;

            string htmlDocPath = null;

            Assembly = Assembly ?? EmbeddedResourceExtensions.FindAssemblyForResource(HtmlDocEmbeddedResourceId);
            if (Assembly != null)
            {
                var htmlPaths = new List<string>();
                var resourceNames = Assembly.GetManifestResourceNames();
                foreach (var resourceId in resourceNames)
                {
                    if (resourceId.EndsWith(".html", StringComparison.OrdinalIgnoreCase) && resourceId.StartsWith(EmbeddedResourceFolder, StringComparison.Ordinal))
                    {
                        var path = await P42.Utils.EmbeddedResourceCache.LocalStorageFullPathForEmbeddedResourceAsync(resourceId, Assembly, EmbeddedResourceFolder);
                        if (resourceId == HtmlDocEmbeddedResourceId)
                            htmlDocPath = path;
                        htmlPaths.Add(path);
                    }
                }

                if (string.IsNullOrWhiteSpace(htmlDocPath))
                {
                    Console.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerString() + ": HtmlDoc [" + HtmlDocEmbeddedResourceId + "] not found in EmbeddedResourceFolder [" + EmbeddedResourceFolder + "]");
                    return;
                }

                foreach (var htmlPath in htmlPaths)
                {
                    var html = File.ReadAllText(htmlPath);
                    foreach (var resourceId in resourceNames)
                    {
                        if (resourceId.StartsWith(EmbeddedResourceFolder, StringComparison.Ordinal))
                        {
                            var path = '"' + await P42.Utils.EmbeddedResourceCache.LocalStorageFullPathForEmbeddedResourceAsync(resourceId, Assembly, EmbeddedResourceFolder) + '"';
                            System.Diagnostics.Debug.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerString() + ": path=[" + path + "]");
                            var srcName = '"' + resourceId.Substring(EmbeddedResourceFolder.Length + 1) + '"';
                            html = html.Replace(srcName, path);
                        }
                    }
                    File.WriteAllText(htmlPath, html);
                }
                BaseUrl = null;
                Html = File.ReadAllText(htmlDocPath);
            }
        }
    }
}
