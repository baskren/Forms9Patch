using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
#if NETSTANDARD
#else
using PCLStorage;
#endif

[assembly: Dependency(typeof(Forms9Patch.UWP.HtmlToPngService))]
namespace Forms9Patch.UWP
{


    /// <summary>
    /// HTML to PDF service.
    /// </summary>
    public class HtmlToPngService : IHtmlToPngPdfService
    {
        DependencyProperty PngFileNameProperty = DependencyProperty.Register("PngFileName", typeof(string), typeof(HtmlToPngService), null);
        DependencyProperty OnCompleteProperty = DependencyProperty.Register("OnPngComplete", typeof(Action<string>), typeof(HtmlToPngService), null);
        DependencyProperty WebViewProperty = DependencyProperty.Register("WebView", typeof(Windows.UI.Xaml.Controls.WebView), typeof(HtmlToPngService), null);
        DependencyProperty HtmlStringProperty = DependencyProperty.Register("HtmlString", typeof(string), typeof(HtmlToPngService), null);

#pragma warning disable CS1998
        /// <summary>
        /// Converts HTML to PNG
        /// </summary>
        /// <param name="html">HTML.</param>
        /// <param name="fileName">File name.</param>
        /// <param name="onComplete">On complete.</param>
        public async void ToPng(string html, string fileName, Action<string> onComplete)
        {
            var currentPage = Forms9Patch.PageExtensions.FindCurrentPage(Xamarin.Forms.Application.Current.MainPage);
            var rootPageRenderer = (Xamarin.Forms.Platform.UWP.PageRenderer)Platform.GetRenderer(currentPage);

            var size = new Size(8.5, 11);
            var webView = new Windows.UI.Xaml.Controls.WebView
            {
                DefaultBackgroundColor = Windows.UI.Colors.White,
                Width = (size.Width - 0.5) * 72 / 2,
                Height = 10, // (size.Height - 0.5) * 72,

            };
            /*
            var transform = new TranslateTransform();
            transform.TransformPoint(new Windows.Foundation.Point(0, 0));
            webView.RenderTransform = transform;
            */
            rootPageRenderer.Children.Add(webView);
            webView.Visibility = Visibility.Visible;

            webView.SetValue(PngFileNameProperty, fileName);
            webView.SetValue(OnCompleteProperty, onComplete);
            webView.SetValue(HtmlStringProperty, html);

            webView.NavigationCompleted += NavigationCompleteA;
            webView.NavigationFailed += WebView_NavigationFailed;

            webView.NavigateToString(html);
        }
#pragma warning restore CS1998

        private void WebView_NavigationFailed(object sender, WebViewNavigationFailedEventArgs e)
        {
            var webView = (Windows.UI.Xaml.Controls.WebView)sender;
            if (webView != null)
            {
                var onComplete = (Action<string>)webView.GetValue(OnCompleteProperty);
                onComplete.Invoke(null);
            }
        }

        private async void NavigationCompleteA(Windows.UI.Xaml.Controls.WebView webView, WebViewNavigationCompletedEventArgs args)
        {
            var contentSize = await webView.WebViewContentSizeAsync();
            webView.NavigationCompleted -= NavigationCompleteA;

            System.Diagnostics.Debug.WriteLine("A contentSize=[" + contentSize + "]");
            System.Diagnostics.Debug.WriteLine("A webView.Size=[" + webView.Width + "," + webView.Height + "]");

            webView.Width = contentSize.Width;
            webView.Height = contentSize.Height;
            webView.UpdateLayout();

            webView.NavigationCompleted += NavigationCompleteB;

            var html = (string)webView.GetValue(HtmlStringProperty);
            webView.NavigateToString(html);
        }

        private async void NavigationCompleteB(Windows.UI.Xaml.Controls.WebView webView, WebViewNavigationCompletedEventArgs args)
        {
            System.Diagnostics.Debug.WriteLine("B webView.Size=[" + webView.Width + "," + webView.Height + "]");


            using (InMemoryRandomAccessStream ms = new InMemoryRandomAccessStream())
            {
                await webView.CapturePreviewToStreamAsync(ms);

                var decoder = await BitmapDecoder.CreateAsync(ms);

                var transform = new BitmapTransform
                {
                    ScaledHeight = (uint)decoder.PixelHeight,
                    ScaledWidth = (uint)decoder.PixelWidth
                };

                var pixelData = await decoder.GetPixelDataAsync(
                    BitmapPixelFormat.Bgra8,
                    BitmapAlphaMode.Straight,
                    transform,
                    ExifOrientationMode.RespectExifOrientation,
                    ColorManagementMode.DoNotColorManage);
                var bytes = pixelData.DetachPixelData();


                var piclib = Windows.Storage.ApplicationData.Current.TemporaryFolder;
                var fileName = (string)webView.GetValue(PngFileNameProperty) + ".png";
                var file = await piclib.CreateFileAsync(fileName, Windows.Storage.CreationCollisionOption.GenerateUniqueName);
                using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
                    encoder.SetPixelData(BitmapPixelFormat.Bgra8,
                                     BitmapAlphaMode.Ignore,
                                     (uint)decoder.PixelWidth, (uint)decoder.PixelHeight,
                                     0, 0, bytes);
                    await encoder.FlushAsync();
                }

                var onComplete = (Action<string>)webView.GetValue(OnCompleteProperty);
                onComplete?.Invoke(file.Path);
            }
        }
    }
}