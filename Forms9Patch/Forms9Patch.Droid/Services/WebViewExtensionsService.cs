using System;
using Android.Content;
using Android.Print;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using PCL.Utils;
using PCLStorage;

[assembly: Dependency(typeof(Forms9Patch.Droid.WebViewExtensionsService))]
namespace Forms9Patch.Droid
{
	public class WebViewExtensionsService : IWebViewExtensionService
	{
		public void Print(WebView viewToPrint, string jobName)
		{
			var droidViewToPrint = Platform.CreateRenderer(viewToPrint).ViewGroup.GetChildAt(0) as Android.Webkit.WebView;

			if (droidViewToPrint != null)
			{
				// Only valid for API 19+
				var version = Android.OS.Build.VERSION.SdkInt;

				if (version >= Android.OS.BuildVersionCodes.Kitkat)
				{
					var printMgr = (PrintManager)Forms.Context.GetSystemService(Context.PrintService);
					printMgr.Print(jobName, droidViewToPrint.CreatePrintDocumentAdapter(jobName), null);
				}
			}
		}

		public bool CanPrint()
		{
			return Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Kitkat;
		}

	}
}
