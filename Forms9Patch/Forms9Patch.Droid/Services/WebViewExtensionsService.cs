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

            //string result = await viewToPrint.EvaluateJavaScriptAsync($"window.location.search");
            if (viewToPrint.Source is Xamarin.Forms.UrlWebViewSource urlSource && urlSource.Url is string urlString && !string.IsNullOrWhiteSpace(urlString) && new System.Uri(urlString) is System.Uri url && url.Query is string query && query.Length > 0)
            {
                var parameters = query.TrimStart('?')
                                .Split(new[] { '&', ';' }, System.StringSplitOptions.RemoveEmptyEntries)
                                .Select(parameter => parameter.Split(new[] { '=' }, System.StringSplitOptions.RemoveEmptyEntries))
                                .GroupBy(parts => parts[0],
                                         parts => parts.Length > 2 ? string.Join("=", parts, 1, parts.Length - 1) : (parts.Length > 1 ? parts[1] : ""))
                                .ToDictionary(grouping => grouping.Key,
                                              grouping => string.Join(",", grouping));

                if (parameters.TryGetValue("file", out string filePath) && !string.IsNullOrWhiteSpace(filePath) && filePath.EndsWith(".pdf", System.StringComparison.OrdinalIgnoreCase) && filePath.StartsWith(P42.Utils.EmbeddedResourceCache.FolderPath(null), System.StringComparison.OrdinalIgnoreCase))
                {
                    System.Diagnostics.Debug.WriteLine("WebViewExtensionsService" + P42.Utils.ReflectionExtensions.CallerString() + ": ");
                    var printManager = (PrintManager)Settings.Context.GetSystemService(Context.PrintService);
                    try
                    {
                        PrintDocumentAdapter printAdapter = new PdfDocumentAdapter(filePath);

                        printManager.Print(Forms9Patch.ApplicationInfoService.Name + " PDF Document", printAdapter, new PrintAttributes.Builder().Build());
                    }
                    catch (Java.Lang.Exception e)
                    {
                        //Logger.logError(e);
                        System.Console.WriteLine("Forms9Patch.Droid.WebViewExtensionService.Print threw exception: " + e.Message);
                    }
                }
            }


            //var droidViewToPrint = Platform.CreateRenderer(viewToPrint).ViewGroup.GetChildAt(0) as Android.Webkit.WebView;

            //if (Platform.CreateRenderer(viewToPrint).View is Android.Webkit.WebView droidViewToPrint)
            if (Platform.CreateRendererWithContext(viewToPrint, Settings.Context) is IVisualElementRenderer renderer)
            {
                Android.Webkit.WebView droidWebView = renderer.View as Android.Webkit.WebView;
                if (droidWebView == null && renderer.View is Xamarin.Forms.Platform.Android.WebViewRenderer xfWebViewRenderer)
                    droidWebView = xfWebViewRenderer.Control;
                if (droidWebView != null)
                {
                    droidWebView.Settings.JavaScriptEnabled = true;
                    droidWebView.Settings.DomStorageEnabled = true;
                    droidWebView.SetLayerType(Android.Views.LayerType.Software, null);
                    // Only valid for API 19+
                    var version = Android.OS.Build.VERSION.SdkInt;

                    if (version >= Android.OS.BuildVersionCodes.Kitkat)
                    {
                        if (string.IsNullOrWhiteSpace(jobName))
                            jobName = Forms9Patch.ApplicationInfoService.Name;
                        var printMgr = (PrintManager)Settings.Context.GetSystemService(Context.PrintService);
                        printMgr.Print(jobName, droidWebView.CreatePrintDocumentAdapter(jobName), null);

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
                            System.Diagnostics.Debug.WriteLine("{0}[{1}] FAILED TO WRITE", P42.Utils.ReflectionExtensions.CallerString(), GetType());
                            //_onComplete?.Invoke(null);
                        }
                        */
                    }
                }
            }
        }

        public bool CanPrint()
        {
            return Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Kitkat;
        }

    }

    class PdfDocumentAdapter : PrintDocumentAdapter
    {
        readonly string Path;

        public PdfDocumentAdapter(string path)
        {
            Path = path;
        }

        public override void OnLayout(PrintAttributes oldAttributes, PrintAttributes newAttributes, CancellationSignal cancellationSignal, LayoutResultCallback callback, Bundle extras)
        {
            if (cancellationSignal.IsCanceled)
            {
                callback.OnLayoutCancelled();
            }
            else
            {
                PrintDocumentInfo.Builder builder = new PrintDocumentInfo.Builder("print_output.pdf");
                var info = builder.SetContentType(Android.Print.PrintContentType.Document)  //Android.Print.PrintDocumentInfo.ContentTypeDocument)
                       .SetPageCount(PrintDocumentInfo.PageCountUnknown)
                       .Build();
                //callback.OnLayoutFailed(builder.Build(),
                //        !oldAttributes.Equals(newAttributes));
                //callback.OnLayoutFailed("layout failed");
                callback.OnLayoutFinished(info, !oldAttributes.Equals(newAttributes));
            }
        }

        public override void OnWrite(PageRange[] pages, ParcelFileDescriptor destination, CancellationSignal cancellationSignal, WriteResultCallback callback)
        {
            Java.IO.InputStream inputStream = null;
            Java.IO.OutputStream outputStream = null;
            try
            {
                Java.IO.File file = new Java.IO.File(Path);
                inputStream = new Java.IO.FileInputStream(file);
                outputStream = new Java.IO.FileOutputStream(destination.FileDescriptor);

                byte[] buf = new byte[16384];
                int size;

                while ((size = inputStream.Read(buf)) >= 0 && !cancellationSignal.IsCanceled)
                {
                    outputStream.Write(buf, 0, size);
                    System.Diagnostics.Debug.WriteLine("PdfDocumentAdapter" + P42.Utils.ReflectionExtensions.CallerString() + ": [" + size + "] Bytes Written ");
                }

                if (cancellationSignal.IsCanceled)
                {
                    callback.OnWriteCancelled();
                }
                else
                {
                    callback.OnWriteFinished(new PageRange[] { PageRange.AllPages });
                }
            }
            catch (Java.Lang.Exception e)
            {
                callback.OnWriteFailed(e.Message);
                //Logger.logError(e);
                System.Console.WriteLine("Forms9Patch.Droid.PdfDocumentAdapter threw exception: " + e.Message);
            }
            finally
            {
                try
                {
                    inputStream.Close();
                    outputStream.Close();
                }
                catch (Java.IO.IOException e)
                {
                    //Logger.logError(e);
                    System.Console.WriteLine("Forms9Patch.Droid.PdfDocumentAdapter threw exception: " + e.Message);
                }
            }
        }
    }
}
