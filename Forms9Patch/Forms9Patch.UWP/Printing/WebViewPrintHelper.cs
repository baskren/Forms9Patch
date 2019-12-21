using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Printing;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Forms9Patch.UWP
{
    class WebViewPrintHelper : PrintHelper
    {
        WebView _webView;
        WebView _sourceWebView;

        public WebViewPrintHelper(WebView webView) : base()
        {
            _sourceWebView = webView;
        }

        public override async Task Init()
        {
            //_webView = _sourceWebView;

            
            var contentSize = await _sourceWebView.WebViewContentSizeAsync();
            _webView = new WebView
            {
                Name = "SpecialSuperDuperWebViewAtLarge",
                DefaultBackgroundColor = Windows.UI.Colors.White,
                Width = contentSize.Width,
                Height = contentSize.Height,
                Visibility = Visibility.Visible
            };

            RootPanel.Children.Insert(0, _webView);


            var html = await _sourceWebView.GetHtml();
            _webView.NavigationCompleted += _webView_NavigationCompleted; ;
            _webView.NavigateToString(html);

            
        }

        private void _webView_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
       
            PrintCanvas.Children.Add(new TextBlock { Text = "THIS IS A TEST" });
            PrintCanvas.InvalidateMeasure();
            PrintCanvas.UpdateLayout();
        }

        protected override async Task<IEnumerable<UIElement>> GeneratePagesAsync(PrintPageDescription pageDescription)
            => await GenerateWebViewPagesAsync(pageDescription.PageSize);
        

        async Task<IEnumerable<UIElement>> GenerateWebViewPagesAsync(Windows.Foundation.Size paperSize)
        {
            var contentSize = await _webView.WebViewContentSizeAsync();

            // how many pages will there be?
            var scale = paperSize.Width / contentSize.Width;
            var scaledHeight = (contentSize.Height * scale);
            var pageCount = scaledHeight / paperSize.Height;
            pageCount += (pageCount > (int)pageCount) ? 1 : 0;

            // create the pages
            var pages = new List<UIElement>();
            for (int i = 0; i < (int)pageCount; i++)
            {
                var rect = GenerateWebViewPage(contentSize, paperSize, i);
                pages.Add(rect);
            }

            return pages;
        }


        UIElement GenerateWebViewPage(SizeI contentSize, Windows.Foundation.Size paperSize, int pageNumber)
        {

            var scale = paperSize.Width / contentSize.Width;
            var scaledHeight = (contentSize.Height * scale);

            var translateY = -paperSize.Height * pageNumber;
            System.Diagnostics.Debug.WriteLine("\t\t translateY: " + translateY);

            var rect = new Windows.UI.Xaml.Shapes.Rectangle
            {
                Height = paperSize.Height,
                Width = paperSize.Width,
                Margin = new Windows.UI.Xaml.Thickness(48),
                Tag = new TranslateTransform { Y = translateY },
                
            };

            rect.Loaded += (s, e) =>
            {
                var sRect = s as Windows.UI.Xaml.Shapes.Rectangle;
                System.Diagnostics.Debug.WriteLine("rect.Loaded: rect: {" + sRect.Width + "," +sRect.Height+ "}  e: " + e.OriginalSource?.ToString());
                var brush = GetWebViewBrush(contentSize);
                brush.Stretch = Stretch.UniformToFill;
                brush.AlignmentY = AlignmentY.Top;
                brush.Transform = rect.Tag as TranslateTransform;
                rect.Fill = brush;
            };
            rect.Visibility = Visibility.Visible;
            return rect;
        }

        WebViewBrush GetWebViewBrush(SizeI contentSize)
        {
            
            // resize width to content
            var originalWebViewWidth = _webView.Width;
            _webView.Width = contentSize.Width;

            // resize height to content
            var originalWebViewHeight = _webView.Height;
            _webView.Height = contentSize.Height;

            // create brush
            var originalWebViewVisibility = _webView.Visibility;
            _webView.Visibility = Visibility.Visible;
            
            var brush = new WebViewBrush
            {
                SourceName = _webView.Name,
                Stretch = Stretch.Uniform,
            };
            brush.Redraw();
            
            // reset, return
            _webView.Width = originalWebViewWidth;
            _webView.Height = originalWebViewHeight;
            _webView.Visibility = originalWebViewVisibility;
            
            return brush;
        }



    }
}
