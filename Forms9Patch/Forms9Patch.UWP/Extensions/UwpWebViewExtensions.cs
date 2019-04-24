using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forms9Patch.UWP
{
    static class UwpWebViewExtensions
    {
        public static async Task<SizeI> WebViewContentSizeAsync(this Windows.UI.Xaml.Controls.WebView webView)
        {
            var widthString = await webView.InvokeScriptAsync("eval", new[] { "document.body.scrollWidth.toString()" });
            if (!int.TryParse(widthString, out int contentWidth))
                throw new Exception(string.Format("failure/width:{0}", widthString));

            // ask the content its height
            var heightString = await webView.InvokeScriptAsync("eval", new[] { "document.body.scrollHeight.toString()" });
            if (!int.TryParse(heightString, out int contentHeight))
                throw new Exception(string.Format("failure/height:{0}", heightString));

            return new SizeI(contentWidth, contentHeight);
        }

    }
}
