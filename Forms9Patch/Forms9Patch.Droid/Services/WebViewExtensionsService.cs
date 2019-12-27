using System.Linq;
using Android.Content;
using Android.OS;
using Android.Print;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Dependency(typeof(Forms9Patch.Droid.WebViewExtensionsService))]
namespace Forms9Patch.Droid
{
    public class WebViewExtensionsService : IWebViewExtensionService
    {
        public void Print(WebView viewToPrint, string jobName)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat)
            {
                IVisualElementRenderer existingRenderer = Platform.GetRenderer(viewToPrint);
                if ((existingRenderer ?? Platform.CreateRendererWithContext(viewToPrint, Settings.Context)) is IVisualElementRenderer renderer)
                {
                    Android.Webkit.WebView droidWebView = renderer.View as Android.Webkit.WebView;
                    if (droidWebView == null && renderer.View is WebViewRenderer xfWebViewRenderer)
                        droidWebView = xfWebViewRenderer.Control;
                    if (droidWebView != null)
                    {
                        droidWebView.Settings.JavaScriptEnabled = true;
                        droidWebView.Settings.DomStorageEnabled = true;
                        droidWebView.SetLayerType(Android.Views.LayerType.Software, null);

                        // Only valid for API 19+
                        if (string.IsNullOrWhiteSpace(jobName))
                            jobName = Forms9Patch.ApplicationInfoService.Name;
                        var printMgr = (PrintManager)Settings.Context.GetSystemService(Context.PrintService);
                        printMgr.Print(jobName, droidWebView.CreatePrintDocumentAdapter(jobName), null);
                    }

                    if (existingRenderer == null)
                        renderer.Dispose();
                }
            }
        }

        public bool CanPrint()
        {
            return Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat;
        }

    }

}
