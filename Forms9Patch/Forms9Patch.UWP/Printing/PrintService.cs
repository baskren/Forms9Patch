using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Graphics.Printing;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: Dependency(typeof(Forms9Patch.UWP.PrintService))]
namespace Forms9Patch.UWP
{
	/// <summary>
	/// Web view extensions service.
	/// </summary>
	public class PrintService : IPrintService
	{



		/// <summary>
		/// Cans the print.
		/// </summary>
		/// <returns><c>true</c>, if print was caned, <c>false</c> otherwise.</returns>
		public bool CanPrint()
		{
			return PrintManager.IsSupported();
		}

		public void Print(Xamarin.Forms.WebView webView, string jobName)
		{
			Device.BeginInvokeOnMainThread(async () =>
			{
				if (string.IsNullOrWhiteSpace(jobName))
					jobName = Forms9Patch.ApplicationInfoService.Name;
				WebViewPrintHelper printHelper = null;
				if (Platform.GetRenderer(webView) is WebViewRenderer renderer && renderer.Control is Windows.UI.Xaml.Controls.WebView nativeWebView)
					printHelper = new WebViewPrintHelper(nativeWebView, jobName);
				else if (webView.Source is HtmlWebViewSource htmlSource && !string.IsNullOrWhiteSpace(htmlSource.Html))
					printHelper = new WebViewPrintHelper(htmlSource.Html, htmlSource.BaseUrl, jobName);
				else if (webView.Source is UrlWebViewSource urlSource && !string.IsNullOrWhiteSpace(urlSource.Url))
					printHelper = new WebViewPrintHelper(urlSource.Url, jobName);
				if (printHelper != null)
				{
					printHelper.RegisterForPrinting();
					await printHelper.Init();
					bool showprint = await PrintManager.ShowPrintUIAsync();
				}
			});
		}

		public void Print(string html, string jobName)
		{
			var webView = new Xamarin.Forms.WebView
			{
				Source = new HtmlWebViewSource
				{
					Html = html
				}
			};
			WebViewPrintEffect.ApplyTo(webView);
			Print(webView, jobName);
		}
	}
}