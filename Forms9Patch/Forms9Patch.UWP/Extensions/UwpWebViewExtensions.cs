using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Forms9Patch.UWP
{
    static class UwpWebViewExtensions
    {
        public static async Task<SizeI> WebViewContentSizeAsync(this Windows.UI.Xaml.Controls.WebView webView)
        {
            int contentWidth = 72 * 8;
            int contentHeight = 72 * 10;
            try
            {
                var widthString = await webView.InvokeScriptAsync("eval", new[] { "document.body.scrollWidth.toString()" });
                int.TryParse(widthString, out contentWidth);

                Debug.WriteLine("elementHeight = " + webView.Height);

                //var rect = await webView.InvokeScriptAsync("pizzx", new[] { "document.getElementById( 'rasta' ).clientHeight.toString()" });
                // ask the content its height
                //var heightString = await webView.InvokeScriptAsync("eval", new[] { "document.documentElement.scrollHeight.toString()" });
                //var heightString = await webView.InvokeScriptAsync("eval", new[] { "document.body.scrollHeight.toString()" });
                //var heightString = await webView.InvokeScriptAsync("eval", new[] { "document.documentElement.getBoundingClientRect().height.toString()" });
                //var heightString = await webView.InvokeScriptAsync("eval", new[] { "self.innerHeight.toString()" });
                //var heightString = await webView.InvokeScriptAsync("eval", new[] { "document.body.offsetHeight.toString()" });
                var heightString = await webView.InvokeScriptAsync("eval", new[] { "Math.max(document.body.scrollHeight, document.body.offsetHeight, document.documentElement.clientHeight, document.documentElement.scrollHeight ).toString()" });//, document.documentElement.offsetHeight ).toString()" });
                int.TryParse(heightString, out contentHeight);

            }
            catch (Exception e)
            {
                await Forms9Patch.Settings.RequestUserHelp(e);
            }
            return new SizeI(contentWidth, contentHeight);
        }

        public static async Task<string> GetHtml(this Windows.UI.Xaml.Controls.WebView webView)
        {
            string html = await webView.InvokeScriptAsync("eval", new string[] { "document.documentElement.outerHTML;" });
            return html;
        }
    }
}
