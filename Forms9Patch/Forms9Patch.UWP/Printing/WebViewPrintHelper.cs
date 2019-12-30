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
        string Html;
        string BaseUrl;
        string Url;

        const string LocalScheme = "ms-appx-web:///";
        const string BaseInsertionScript = @"
var head = document.getElementsByTagName('head')[0];
var bases = head.getElementsByTagName('base');
if(bases.length == 0){
    head.innerHTML = 'baseTag' + head.innerHTML;
}";

        internal WebViewPrintHelper(WebView webView, string jobName) : base(jobName)
        {
            _sourceWebView = webView;
        }

        internal WebViewPrintHelper(string html, string baseUri, string jobName): base(jobName)
        {
            Html = html;
            BaseUrl = baseUri;
            if (string.IsNullOrEmpty(BaseUrl))
                BaseUrl = LocalScheme;
        }

        internal WebViewPrintHelper(string url, string jobName) : base(jobName)
        {
            Url = url;
        }

        int instanceCount = 0;

        public override async Task Init()
        {

            _webView = new WebView
            {
                Name = "PrintWebView" + (instanceCount++).ToString("D3"),
                DefaultBackgroundColor = Windows.UI.Colors.White,
                Visibility = Visibility.Visible,
            };
            RootPanel.Children.Insert(0, _webView);
            PrintContent = _webView;
            _webView.NavigationCompleted += _webView_NavigationCompleted; ;

            if (_sourceWebView!=null)
            {
                var contentSize = await _sourceWebView.WebViewContentSizeAsync();
                _webView.Width = contentSize.Width;
                _webView.Height = contentSize.Height;
                Html = await _sourceWebView.GetHtml();
                _webView.NavigateToString(Html);
            }
            else if (!string.IsNullOrWhiteSpace(Html))
            {
                _webView.Width = 96 * 8;
                _webView.Height = 96 * 10.5;
                var baseTag = $"<base href=\"{BaseUrl}\"></base>";
                string htmlWithBaseTag;
                var internalWebView = new Windows.UI.Xaml.Controls.WebView();
                internalWebView.NavigationCompleted += async (sender, args) =>
                {
                    var script = BaseInsertionScript.Replace("baseTag", baseTag);
                    await sender.InvokeScriptAsync("eval", new[] { script });
                    htmlWithBaseTag = await sender.InvokeScriptAsync("eval", new[] { "document.documentElement.outerHTML;" });
                    _webView.NavigateToString(!string.IsNullOrEmpty(htmlWithBaseTag) ? htmlWithBaseTag : Html);
                };
                internalWebView.NavigateToString(Html);
            }
            else if (!string.IsNullOrWhiteSpace(Url))
            {
                _webView.Width = 96 * 8;
                _webView.Height = 96 * 10.5;
                Uri uri = new Uri(Url, UriKind.RelativeOrAbsolute);
                if (!uri.IsAbsoluteUri)
                    uri = new Uri(LocalScheme + Url, UriKind.RelativeOrAbsolute);
                _webView.Source = uri;
            }
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
            System.Diagnostics.Debug.WriteLine("GenerateWebViewPagesAsync: {" + paperSize.Width + "," +paperSize.Height+ "}" );
            var contentSize = await _webView.WebViewContentSizeAsync();
            _webView.Width = contentSize.Width;
            _webView.Height = contentSize.Height;

            var printArea = new Windows.Foundation.Size(paperSize.Width * (1 - 2 * ApplicationContentMarginLeft), paperSize.Height * (1 - 2 * ApplicationContentMarginTop));

            // how many pages will there be?
            var scale = printArea.Width / contentSize.Width;
            var scaledHeight = contentSize.Height * scale;
            var pageCount = scaledHeight / printArea.Height;
            pageCount += (pageCount > (int)pageCount) ? 1 : 0;

            // create the pages
            var pages = new List<UIElement>();
            for (int i = 0; i < (int)pageCount; i++)
            {
                var panel = GenerateWebViewPanel(paperSize, i);
                pages.Add(panel);
            }

            return pages;
        }


        UIElement GenerateWebViewPanel(Windows.Foundation.Size paperSize, int pageNumber)
        {
            var printArea = new Windows.Foundation.Size(paperSize.Width * (1 - 2 * ApplicationContentMarginLeft), paperSize.Height * (1 - 2 * ApplicationContentMarginTop));

            var scale = printArea.Width / _webView.Width;
            var scaledHeight = _webView.Height * scale;

            var translateY = -printArea.Height * pageNumber;

            var rect = new Windows.UI.Xaml.Shapes.Rectangle
            {
                Tag = new TranslateTransform { Y = translateY },
                Margin = new Windows.UI.Xaml.Thickness(ApplicationContentMarginLeft * paperSize.Width, ApplicationContentMarginTop * paperSize.Height, ApplicationContentMarginLeft * paperSize.Width, ApplicationContentMarginTop * paperSize.Height),
            };
            var panel = new Windows.UI.Xaml.Controls.Grid
            {
                Height = paperSize.Height,
                Width = paperSize.Width,
                Children = { rect },
            };

            rect.Loaded += (s, e) =>
            {
                var sRect = s as Windows.UI.Xaml.Shapes.Rectangle;
                System.Diagnostics.Debug.WriteLine("rect.Loaded: rect: {" + sRect.Width + "," +sRect.Height+ "}  e: " + e.OriginalSource?.ToString());
                var brush = new WebViewBrush
                {
                    SourceName = _webView.Name,
                    Stretch = Stretch.Uniform,
                };
                brush.Redraw();
                brush.Stretch = Stretch.UniformToFill;
                brush.AlignmentY = AlignmentY.Top;
                brush.Transform = rect.Tag as TranslateTransform;
                rect.Fill = brush;
            };
            rect.Visibility = Visibility.Visible;
            return panel;
        }


    }
}
