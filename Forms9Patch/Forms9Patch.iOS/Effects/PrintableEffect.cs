using System;
using System.Net;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(Forms9Patch.iOS.PrintableEffect), "PrintableEffect")]
namespace Forms9Patch.iOS
{
    public class PrintableEffect : PlatformEffect
    {
        static string ButtheadPath = $"file://{NSBundle.MainBundle.BundlePath}/";


        #region ActualSource
        /// <summary>
        /// Backing store for PrintableEffect's ActualSource property
        /// </summary>
        public static readonly BindableProperty ActualSourceProperty = BindableProperty.Create("ActualSource", typeof(WebViewSource), typeof(PrintableEffect), default);
        #endregion

        WebViewSource _oldSource;

        protected override void OnAttached()
        {
            SetActualSource();
            Element.PropertyChanged += Element_PropertyChanged; ;
        }

        protected override void OnDetached()
        {
            Element.PropertyChanged -= Element_PropertyChanged; ;
        }

        private void Element_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == WebView.SourceProperty.PropertyName)
                SetActualSource();
        }

        void SetActualSource()
        {
            if (Element is WebView webView)
            {
                if (webView.Source is UrlWebViewSource urlSource)
                {
                    if(urlSource.Url == ButtheadPath)
                        return;
                    if(urlSource.Url.StartsWith($"file://") &&  webView.GetValue(ActualSourceProperty) is HtmlWebViewSource htmlWebViewSource)
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

    }
}
