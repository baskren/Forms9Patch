using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: Dependency(typeof(Forms9Patch.iOS.WebViewExtensionsService))]
namespace Forms9Patch.iOS
{
	/// <summary>
	/// Web view extensions service.
	/// </summary>
	public class WebViewExtensionsService : IWebViewExtensionService
	{
		/// <summary>
		/// Print the specified viewToPrint and jobName.
		/// </summary>
		/// <param name="viewToPrint">View to print.</param>
		/// <param name="jobName">Job name.</param>
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

		/// <summary>
		/// Cans the print.
		/// </summary>
		/// <returns><c>true</c>, if print was caned, <c>false</c> otherwise.</returns>
		public bool CanPrint()
		{
			return true;
		}

	}	
}
