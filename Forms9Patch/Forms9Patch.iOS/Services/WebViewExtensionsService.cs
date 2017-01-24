using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: Dependency(typeof(Forms9Patch.iOS.WebViewExtensionsService))]
namespace Forms9Patch.iOS
{
	public class WebViewExtensionsService : IWebViewExtensionService
	{
		public void Print(WebView viewToPrint, string jobName)
		{
			var appleViewToPrint = Platform.CreateRenderer(viewToPrint).NativeView;

			var printInfo = UIPrintInfo.PrintInfo;

			printInfo.OutputType = UIPrintInfoOutputType.General;
			printInfo.JobName = jobName;
			printInfo.Orientation = UIPrintInfoOrientation.Portrait;
			printInfo.Duplex = UIPrintInfoDuplex.None;

			var printController = UIPrintInteractionController.SharedPrintController;

			printController.PrintInfo = printInfo;
			printController.ShowsPageRange = true;
			printController.PrintFormatter = appleViewToPrint.ViewPrintFormatter;

			printController.Present(true, (printInteractionController, completed, error) => { });
		}

		public bool CanPrint()
		{
			return true;
		}

	}	
}
