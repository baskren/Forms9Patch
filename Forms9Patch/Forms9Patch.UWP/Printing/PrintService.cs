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

		public Task PrintAsync(Xamarin.Forms.WebView webView, string jobName, FailAction failAction)
		{
			Device.BeginInvokeOnMainThread(async () =>
			{
				if (string.IsNullOrWhiteSpace(jobName))
					jobName = Forms9Patch.ApplicationInfoService.Name;
				WebViewPrintHelper printHelper = null;
				var properties = new Dictionary<string, string>
							{
								{ "class", "Forms9Patch.UWP.PrintService" },
								{ "method", "PrintAsync" },
							};
				try
				{
					if (webView.Source is HtmlWebViewSource htmlSource && !string.IsNullOrWhiteSpace(htmlSource.Html))
					{
						properties["line"] = "47";
						printHelper = new WebViewPrintHelper(htmlSource.Html, htmlSource.BaseUrl, jobName);
					}
					else if (webView.Source is UrlWebViewSource urlSource && !string.IsNullOrWhiteSpace(urlSource.Url))
					{
						properties["line"] = "53";
						printHelper = new WebViewPrintHelper(urlSource.Url, jobName);
					}
					else if (Platform.GetRenderer(webView) is WebViewRenderer renderer && renderer.Control is Windows.UI.Xaml.Controls.WebView nativeWebView)
					{
						properties["line"] = "57";
						printHelper = new WebViewPrintHelper(nativeWebView, jobName);
					}
				}
				catch (Exception e)
				{
					Analytics.TrackException?.Invoke(e, properties);
					if (failAction == FailAction.ShowAlert)
						using (Forms9Patch.Toast.Create("Print Service Error", "Could not initiate print WebViewPrintHelper.  Please try again.\n\nException: " + e.Message + "\n\nInnerException: " + e.InnerException)) { }
					else if (failAction == FailAction.ThrowException)
						throw e;
				}

				if (printHelper != null)
				{
					try
					{
						properties["line"] = "71";
						printHelper.RegisterForPrinting();
						properties["line"] = "73";
						await printHelper.Init();
						properties["line"] = "75";
						bool showprint = await PrintManager.ShowPrintUIAsync();
					}
					catch (Exception e)
                    {
						Analytics.TrackException?.Invoke(e, properties);
						if (failAction == FailAction.ShowAlert)
							using (Forms9Patch.Toast.Create("Print Service Error", "Could not Show Print UI Async.  Please try again.\n\nException: " + e.Message + "\n\nInnerException: " + e.InnerException)) { }
						else if (failAction == FailAction.ThrowException)
							throw e;
					}
				}
			});
			return Task.CompletedTask;
		}

		public Task PrintAsync(string html, string jobName, FailAction failAction)
		{
			var webView = new Xamarin.Forms.WebView
			{
				Source = new HtmlWebViewSource
				{
					Html = html
				}
			};
			WebViewPrintEffect.ApplyTo(webView);
			return PrintAsync(webView, jobName, failAction);
		}
	}
}