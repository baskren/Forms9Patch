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
    /// <summary>
    /// WebSource for EmbeddedResources
    /// </summary>
    [Preserve(AllMembers = true)]
    [DesignTimeVisible(true)]
    public class EmbeddedHtmlViewSource : Xamarin.Forms.HtmlWebViewSource, IDisposable
    {
        #region Properties

        #region EmbeddedResourceFolder Property
        /// <summary>
        /// EmbeddedResourceFolder BindableProperty
        /// </summary>
        public static readonly BindableProperty EmbeddedResourceFolderProperty = BindableProperty.Create(nameof(EmbeddedResourceFolder), typeof(string), typeof(EmbeddedHtmlViewSource), default(string));
        /// <summary>
        /// The portion of the EmbeddedResource path under which lies all content for the WebViewSource
        /// </summary>
        public string EmbeddedResourceFolder
        {
            get => (string)GetValue(EmbeddedResourceFolderProperty);
            set => SetValue(EmbeddedResourceFolderProperty, value);
        }
        #endregion

        #region HtmlFileName Property
        /// <summary>
        /// HtmlDocEmbeddedResourceId BindableProperty
        /// </summary>
        public static readonly BindableProperty HtmlDocEmbeddedResourceIdProperty = BindableProperty.Create(nameof(HtmlDocEmbeddedResourceId), typeof(string), typeof(EmbeddedHtmlViewSource), default(string));
        /// <summary>
        /// The EmbeddedResourceId of the html document to display
        /// </summary>
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


        #region Construction / Disposal
        /// <summary>
        /// Primary method of creation
        /// </summary>
        /// <param name="embeddedResourceFolder"></param>
        /// <param name="htmlDocEmbeddedResourceId"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static async Task<EmbeddedHtmlViewSource> Create(string embeddedResourceFolder, string htmlDocEmbeddedResourceId, Assembly assembly = null)
        {
            if (new EmbeddedHtmlViewSource(embeddedResourceFolder, htmlDocEmbeddedResourceId, assembly) is EmbeddedHtmlViewSource source)
            {
                await source.Initialize();
                return source;
            }
            return null;
        }

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

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _disposed = true;
                Clear(EmbeddedResourceFolder, Assembly);
            }
        }

        /// <summary>
        /// Disposal
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion


        #region Clear
        /// <summary>
        /// A way to clear any embedded resource files that have been cached locally.
        /// </summary>
        /// <param name="embeddedResourceFolder"></param>
        /// <param name="assembly"></param>
        public static void Clear(string embeddedResourceFolder, Assembly assembly)
        {
            P42.Utils.EmbeddedResourceCache.Clear(null, assembly, embeddedResourceFolder);
        }
        #endregion


        #region Initialization
        /// <summary>
        /// A way to generate the content of EmbeddedHtmlViewSource
        /// </summary>
        /// <returns></returns>
        public async Task Initialize()
        {
            if (string.IsNullOrWhiteSpace(EmbeddedResourceFolder) || string.IsNullOrWhiteSpace(HtmlDocEmbeddedResourceId))
                return;

            string htmlDocPath = null;
            BaseUrl = null;

            Assembly = Assembly ?? EmbeddedResourceExtensions.FindAssemblyForResource(HtmlDocEmbeddedResourceId);
            if (Assembly != null)
            {

                if (Xamarin.Forms.Device.RuntimePlatform == Device.Android)
                {
                    var htmlPaths = new List<string>();
                    var resourceNames = Assembly.GetManifestResourceNames();
                    foreach (var resourceId in resourceNames)
                    {
                        if (resourceId.StartsWith(EmbeddedResourceFolder, StringComparison.Ordinal))
                        {
                            var path = await P42.Utils.EmbeddedResourceCache.LocalStorageFullPathForEmbeddedResourceAsync(resourceId, Assembly, EmbeddedResourceFolder);
                            if (resourceId.EndsWith(".html", StringComparison.OrdinalIgnoreCase))
                            {
                                if (resourceId == HtmlDocEmbeddedResourceId)
                                    htmlDocPath = path;
                                htmlPaths.Add(path);
                            }
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
                    Html = File.ReadAllText(htmlDocPath);
                }
                else if (Xamarin.Forms.Device.RuntimePlatform == Device.iOS)
                {
                    var resourceNames = Assembly.GetManifestResourceNames();
                    foreach (var resourceId in resourceNames)
                    {
                        if (resourceId.StartsWith(EmbeddedResourceFolder, StringComparison.Ordinal))
                        {
                            var path = await P42.Utils.EmbeddedResourceCache.LocalStorageFullPathForEmbeddedResourceAsync(resourceId, Assembly, EmbeddedResourceFolder);
                            if (resourceId.EndsWith(".html", StringComparison.OrdinalIgnoreCase))
                            {
                                if (resourceId == HtmlDocEmbeddedResourceId)
                                    htmlDocPath = path;
                            }
                        }
                    }

                    if (string.IsNullOrWhiteSpace(htmlDocPath))
                    {
                        Console.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerString() + ": HtmlDoc [" + HtmlDocEmbeddedResourceId + "] not found in EmbeddedResourceFolder [" + EmbeddedResourceFolder + "]");
                        return;
                    }

                    BaseUrl = P42.Utils.EmbeddedResourceCache.FolderPath(Assembly, EmbeddedResourceFolder);
                    Html = File.ReadAllText(htmlDocPath);
                }
                else
                {
                    htmlDocPath = await P42.Utils.EmbeddedResourceCache.LocalStorageFullPathForEmbeddedResourceAsync(HtmlDocEmbeddedResourceId, Assembly, EmbeddedResourceFolder);
                    var html = File.ReadAllText(htmlDocPath);
                    var resourceNames = Assembly.GetManifestResourceNames();
                    foreach (var resourceId in resourceNames)
                    {
                        if (resourceId.StartsWith(EmbeddedResourceFolder, StringComparison.Ordinal))
                        {
                            var resourcePath = resourceId.Split('.');
                            var suffix = resourcePath.LastOrDefault()?.ToLower();
                            if (suffix == "png"
                                || suffix == "jpg" || suffix == "jpeg"
                                || suffix == "svg"
                                || suffix == "gif"
                                || suffix == "tif" || suffix == "tiff"
                                || suffix == "pdf"
                                || suffix == "bmp"
                                || suffix == "ico")
                            {
                                var relativeSource = '"' + resourceId.Substring(EmbeddedResourceFolder.Length + 1) + '"';

                                if (html.Contains(relativeSource))
                                {
                                    try
                                    {
                                        byte[] bytes;
                                        using (var resourceStream = Assembly.GetManifestResourceStream(resourceId))
                                        {
                                            using (var memoryStream = new MemoryStream())
                                            {
                                                resourceStream.CopyTo(memoryStream);
                                                bytes = memoryStream.ToArray();
                                            }
                                            string base64 = '"' + "data:" + (suffix == "pdf" ? "application/" : "image/") + suffix + ";base64," + Convert.ToBase64String(bytes) + '"';
                                            html = html.Replace(relativeSource, base64);
                                        }
                                    }
                                    catch (Exception) { }
                                }
                            }
                        }
                    }
                    Html = html;
                }


            }
        }
        #endregion

    }
}
