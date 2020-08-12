using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using Xamarin.Forms;

namespace Forms9Patch
{
    /// <summary>
    /// Effect required to enable Printing of content of Xamarin.Forms.WebView
    /// </summary>
    [Preserve(AllMembers = true)]
    [DesignTimeVisible(true)]
    public class WebViewPrintEffect : RoutingEffect
    {
        #region ActualSource
        /// <summary>
        /// Backing store for WebViewPrintEffect's ActualSource property
        /// </summary>
        public static readonly BindableProperty ActualSourceProperty = BindableProperty.Create("ActualSource", typeof(WebViewSource), typeof(WebViewPrintEffect), default);
        #endregion

        internal static string ButtheadPath;

        WebView WebView;

        /// <summary>
        /// Effect required to enable printing of content of Xamarin.Forms.Webview
        /// </summary>
        /// <param name="webView"></param>
        protected WebViewPrintEffect(WebView webView) : base("Forms9Patch.WebViewPrintEffect")
        {
            Settings.ConfirmInitialization();
            WebView = webView;
        }

        /// <summary>
        /// Apply WebViewPrintEffect to Xamarin.Forms.WebView
        /// </summary>
        /// <param name="webView"></param>
        /// <returns></returns>
        public static bool ApplyTo(WebView webView)
        {
            if (webView.Effects.Any(e => e is WebViewPrintEffect))
                return true;
            if (new WebViewPrintEffect(webView) is WebViewPrintEffect effect)
            {
                if (webView.Effects is ObservableCollection<Effect> effects)
                {
                    webView.Effects.Add(effect);
                    effects.CollectionChanged += Effects_CollectionChanged;
                    webView.PropertyChanged += WebView_PropertyChanged;
                    return webView.Effects.Contains(effect);
                }
            }
            return false;
        }

        private static void WebView_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is WebView webView && e.PropertyName == WebView.SourceProperty.PropertyName)
            {
                if (webView.Source is UrlWebViewSource urlSource)
                {
                    if (urlSource.Url == ButtheadPath)
                        return;
                    if (urlSource.Url.StartsWith($"file://") && webView.GetValue(ActualSourceProperty) is HtmlWebViewSource htmlWebViewSource)
                    {
                        var newPath = urlSource.Url.Substring(7).TrimEnd('/');
                        var oldPath = htmlWebViewSource.BaseUrl.TrimEnd('/');

                        newPath = WebUtility.UrlDecode(newPath);
                        oldPath = WebUtility.UrlDecode(oldPath);

                        if (newPath == oldPath)
                            return;
                    }
                }
                webView.SetValue(ActualSourceProperty, webView.Source);
            }
        }

        private static void Effects_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs args)
        {
            if (args.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove || args.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset)
            {
                if (sender is ObservableCollection<Effect> effects && args.OldItems.Cast<Effect>().FirstOrDefault(e => e is WebViewPrintEffect) is WebViewPrintEffect effect)
                {
                    if (effect.WebView != null)
                        effect.WebView.PropertyChanged -= WebView_PropertyChanged;
                    effect.WebView = null;
                    effects.CollectionChanged -= Effects_CollectionChanged;
                }
            }
        }
    }
}
