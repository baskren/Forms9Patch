using System;
using Android.Content;
using Android.Print;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using PCL.Utils;
using PCLStorage;
using System.IO;

[assembly: Dependency(typeof(Forms9Patch.Droid.WebViewExtensionsService))]
namespace Forms9Patch.Droid
{
    public class WebViewExtensionsService : IWebViewExtensionService
    {
        public void Print(WebView viewToPrint, string jobName)
        {
            //var droidViewToPrint = Platform.CreateRenderer(viewToPrint).ViewGroup.GetChildAt(0) as Android.Webkit.WebView;

            if (Platform.CreateRenderer(viewToPrint).View is Android.Webkit.WebView droidViewToPrint)
            {
                // Only valid for API 19+
                var version = Android.OS.Build.VERSION.SdkInt;

                if (version >= Android.OS.BuildVersionCodes.Kitkat)
                {
                    var printMgr = (PrintManager)Forms.Context.GetSystemService(Context.PrintService);
                    printMgr.Print(jobName, droidViewToPrint.CreatePrintDocumentAdapter(jobName), null);

                    /*
					var document = new Android.Graphics.Pdf.PdfDocument();
					var page = document.StartPage(new Android.Graphics.Pdf.PdfDocument.PageInfo.Builder(360, 720, 1).Create());

					droidViewToPrint.Draw(page.Canvas);
					document.FinishPage(page);

					Stream memoryStream = new MemoryStream();
					//var os = new ByteArrayOutputStream();
					var fos = new Java.IO.FileOutputStream("/data/data/com.awc.hgts_areas/files" + "/" + "TEST" + ".pdf", false);
					try
					{
						document.WriteTo(memoryStream);
						fos.Write(((MemoryStream)memoryStream).ToArray(), 0, (int)memoryStream.Length);
						fos.Close();
						//var file = FileSystem.Current.LocalStorage.GetFile("TEST" + ".pdf");
						//_onComplete?.Invoke(file);
					}
					catch
					{
						System.Diagnostics.Debug.WriteLine("{0}[{1}] FAILED TO WRITE", PCL.Utils.ReflectionExtensions.CallerString(), GetType());
						//_onComplete?.Invoke(null);
					}
					*/
                }
            }


        }

        public bool CanPrint()
        {
            return Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Kitkat;
        }

    }
}
