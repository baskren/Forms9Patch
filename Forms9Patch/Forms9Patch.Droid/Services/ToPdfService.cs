using System.IO;
using Xamarin.Forms;
using Android.Webkit;
using Android.Graphics;
using Android.Views;
using System.Threading.Tasks;
using Xamarin.Forms.Platform.Android;
using System.Reflection;
using Android.Print;
using Android.Runtime;
using System;
using Android.OS;
using Java.Lang;
using Forms9Patch;
using Java.Interop;

[assembly: Dependency(typeof(Forms9Patch.Droid.ToPdfService))]
namespace Forms9Patch.Droid
{

    public class ToPdfService : Java.Lang.Object, IToPdfService
    {
        public bool IsAvailable => Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Kitkat;

        public async Task<ToFileResult> ToPdfAsync(ActivityIndicatorPopup popup, string html, string fileName)
        {
            if (!await Permissions.WriteExternalStorage.ConfirmOrRequest())
                return new ToFileResult(true, "Write External Stoarge permission must be granted for PNG images to be available.");
            var taskCompletionSource = new TaskCompletionSource<ToFileResult>();
            ToPdf(taskCompletionSource, html, fileName);
            return await taskCompletionSource.Task;
        }

        public async Task<ToFileResult> ToPdfAsync(ActivityIndicatorPopup popup, Xamarin.Forms.WebView webView, string fileName)
        {
            if (!await Permissions.WriteExternalStorage.ConfirmOrRequest())
                return new ToFileResult(true, "Write External Stoarge permission must be granted for PNG images to be available.");
            var taskCompletionSource = new TaskCompletionSource<ToFileResult>();
            ToPdf(taskCompletionSource, webView, fileName);
            return await taskCompletionSource.Task;
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0067:Dispose objects before losing scope", Justification = "CustomWebView is disposed in Callback.Compete")]
        public void ToPdf(TaskCompletionSource<ToFileResult> taskCompletionSource, string html, string fileName)
        {
            var size = new Size(8.5, 11);
            var externalPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            using (var dir = new Java.IO.File(externalPath))
            using (var file = new Java.IO.File(dir + "/" + fileName + ".pdf"))
            {
                if (!dir.Exists())
                    dir.Mkdir();
                if (file.Exists())
                    file.Delete();

                var webView = new Android.Webkit.WebView(Android.App.Application.Context);
                webView.Settings.JavaScriptEnabled = true;
#pragma warning disable CS0618 // Type or member is obsolete
                webView.DrawingCacheEnabled = true;
#pragma warning restore CS0618 // Type or member is obsolete
                webView.SetLayerType(LayerType.Software, null);

                webView.Layout(0, 0, (int)((size.Width - 0.5) * 72), (int)((size.Height - 0.5) * 72));

                webView.LoadData(html, "text/html; charset=utf-8", "UTF-8");
                webView.SetWebViewClient(new WebViewCallBack(taskCompletionSource, fileName, OnPageFinished));
            }
        }

        public void ToPdf(TaskCompletionSource<ToFileResult> taskCompletionSource, Xamarin.Forms.WebView xfWebView, string fileName)
        {
            if (Platform.CreateRendererWithContext(xfWebView, Settings.Context) is IVisualElementRenderer renderer)
            {
                Android.Webkit.WebView droidWebView = renderer.View as Android.Webkit.WebView;
                if (droidWebView == null && renderer.View is WebViewRenderer xfWebViewRenderer)
                    droidWebView = xfWebViewRenderer.Control;
                if (droidWebView != null)
                {
                    //var size = new Size(8.5, 11);
                    var externalPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
                    using (var dir = new Java.IO.File(externalPath))
                    using (var file = new Java.IO.File(dir + "/" + fileName + ".pdf"))
                    {
                        if (!dir.Exists())
                            dir.Mkdir();
                        if (file.Exists())
                            file.Delete();

                        droidWebView.SetLayerType(LayerType.Software, null);
                        droidWebView.Settings.JavaScriptEnabled = true;
#pragma warning disable CS0618 // Type or member is obsolete
                        droidWebView.DrawingCacheEnabled = true;
                        droidWebView.BuildDrawingCache();
#pragma warning restore CS0618 // Type or member is obsolete

                        droidWebView.SetWebViewClient(new WebViewCallBack(taskCompletionSource, fileName, OnPageFinished));
                    }
                }
            }
        }



        async Task OnPageFinished(Android.Webkit.WebView webView, string fileName, TaskCompletionSource<ToFileResult> taskCompletionSource)
        {
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Kitkat)
            {
                var builder = new PrintAttributes.Builder();
                builder.SetMediaSize(PrintAttributes.MediaSize.NaLetter);
                builder.SetResolution(new PrintAttributes.Resolution("pdf", "pdf", 600, 600));
                builder.SetMinMargins(PrintAttributes.Margins.NoMargins);
                var attributes = builder.Build();

                var adapter = webView.CreatePrintDocumentAdapter(Guid.NewGuid().ToString());
                /*


                IntPtr klass = JNIEnv.FindClass("android/print/PdfLayoutResultCallback");
                IntPtr constructor = JNIEnv.GetMethodID(klass, "<init>", "void(V)");
                var Foo = Java.Lang.Object.GetObject<Java.Lang.Class>(klass, JniHandleOwnership.TransferGlobalRef);

                //var instance = JNIEnv.StartCreateInstance(klass, )
                */

                var layoutResultCallback = new PdfLayoutResultCallback();
                layoutResultCallback.Adapter = adapter;
                layoutResultCallback.TaskCompletionSource = taskCompletionSource;
                layoutResultCallback.FileName = fileName;
                adapter.OnLayout(null, attributes, null, layoutResultCallback, null);

                /*
                var adapter = new PdfDocumentAdapterX(taskCompletionSource, fileName);
                adapter.OnLayout(null, attributes, null, null, null);
                */

                System.Diagnostics.Debug.WriteLine("ToPdfService" + P42.Utils.ReflectionExtensions.CallerString() + ": ");
            }



        }

    }

    /*
    class PdfDocumentAdapterX : PrintDocumentAdapter
    {
        //readonly string Path;
        public TaskCompletionSource<ToFileResult> TaskCompletionSource { get; private set; }
        public string FileName { get; private set; }
        //public PrintDocumentAdapter Adapter { get; set; }

        public PdfDocumentAdapterX(TaskCompletionSource<ToFileResult> taskCompletionSource, string fileName)
        {
            TaskCompletionSource = taskCompletionSource;
            FileName = fileName;
        }

        public override void OnLayout(PrintAttributes oldAttributes, PrintAttributes newAttributes, CancellationSignal cancellationSignal, LayoutResultCallback callback, Bundle extras)
        {
            if (cancellationSignal?.IsCanceled ?? false)
            {
                TaskCompletionSource.SetResult(new ToFileResult(true, "PDF Layout Cancelled"));
                callback?.OnLayoutCancelled();
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
                callback?.OnLayoutFinished(info, !oldAttributes.Equals(newAttributes));

                using (var _dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments))
                {
                    if (!_dir.Exists())
                        _dir.Mkdir();

                    var path = _dir.Path + "/" + FileName + ".pdf";
                    var file = new Java.IO.File(path);
                    int iter = 0;
                    while (file.Exists())
                    {
                        iter++;
                        path = _dir.Path + "/" + FileName + "_" + iter.ToString("D3") + ".pdf";
                        file = new Java.IO.File(path);
                    }
                    file.CreateNewFile();

                    var fileDescriptor = ParcelFileDescriptor.Open(file, ParcelFileMode.ReadWrite);

                    OnWrite(new Android.Print.PageRange[] { PageRange.AllPages }, fileDescriptor, new CancellationSignal(), null);
                }

            }
        }

        public override void OnWrite(PageRange[] pages, ParcelFileDescriptor destination, CancellationSignal cancellationSignal, WriteResultCallback callback)
        {
            Java.IO.InputStream inputStream = null;
            Java.IO.OutputStream outputStream = null;
            try
            {
                Java.IO.File file = new Java.IO.File(FileName);
                inputStream = new Java.IO.FileInputStream(file);
                outputStream = new Java.IO.FileOutputStream(destination.FileDescriptor);

                byte[] buf = new byte[16384];
                int size;

                while ((size = inputStream.Read(buf)) >= 0 && !(cancellationSignal?.IsCanceled ?? false))
                {
                    outputStream.Write(buf, 0, size);
                    System.Diagnostics.Debug.WriteLine("PdfDocumentAdapter" + P42.Utils.ReflectionExtensions.CallerString() + ": [" + size + "] Bytes Written ");
                }

                if (cancellationSignal?.IsCanceled ?? false)
                {
                    TaskCompletionSource.SetResult(new ToFileResult(true, "PDF Write Cancelled"));
                    callback?.OnWriteCancelled();
                }
                else
                {
                    TaskCompletionSource.SetResult(new ToFileResult(false, destination.ToString()));
                    callback?.OnWriteFinished(new PageRange[] { PageRange.AllPages });
                }
            }
            catch (Java.Lang.Exception e)
            {
                TaskCompletionSource.SetResult(new ToFileResult(true, e.Message));
                callback?.OnWriteFailed(e.Message);
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
    */
}


namespace Android.Print
{


    [Register("android/print/PdfLayoutResultCallback")]
    public class PdfLayoutResultCallback : PrintDocumentAdapter.LayoutResultCallback
    {
        public TaskCompletionSource<ToFileResult> TaskCompletionSource { get; set; }
        public string FileName { get; set; }
        public PrintDocumentAdapter Adapter { get; set; }

        public PdfLayoutResultCallback(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer) { }

        public PdfLayoutResultCallback() : base(IntPtr.Zero, JniHandleOwnership.DoNotTransfer)
        {
            if (!(base.Handle != IntPtr.Zero))
            {
                unsafe
                {
                    JniObjectReference val = JniPeerMembers.InstanceMethods.StartCreateInstance("()V", GetType(), null);
                    SetHandle(val.Handle, JniHandleOwnership.TransferLocalRef);
                    JniPeerMembers.InstanceMethods.FinishCreateInstance("()V", this, null);
                }
            }

        }

        public override void OnLayoutCancelled()
        {
            base.OnLayoutCancelled();
            TaskCompletionSource.SetResult(new ToFileResult(true, "PDF Layout was cancelled"));
        }

        public override void OnLayoutFailed(ICharSequence error)
        {
            base.OnLayoutFailed(error);
            TaskCompletionSource.SetResult(new ToFileResult(true, error.ToString()));
        }

        //[Android.Runtime.Register("onLayoutFinished", "(Landroid/print/PrintDocumentInfo;Z)V", "GetOnLayoutFinished_Landroid_print_PrintDocumentInfo_ZHandler")]
        public override void OnLayoutFinished(PrintDocumentInfo info, bool changed)
        {
            using (var _dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments))
            {
                if (!_dir.Exists())
                    _dir.Mkdir();

                var path = _dir.Path + "/" + FileName + ".pdf";
                var file = new Java.IO.File(path);
                int iter = 0;
                while (file.Exists())
                {
                    iter++;
                    path = _dir.Path + "/" + FileName + "_" + iter.ToString("D3") + ".pdf";
                    file = new Java.IO.File(path);
                }
                file.CreateNewFile();

                var fileDescriptor = ParcelFileDescriptor.Open(file, ParcelFileMode.ReadWrite);

                var writeResultCallback = new PdfWriteResultCallback(TaskCompletionSource, path);

                Adapter.OnWrite(new Android.Print.PageRange[] { PageRange.AllPages }, fileDescriptor, new CancellationSignal(), writeResultCallback);
            }
            base.OnLayoutFinished(info, changed);
        }


    }

    [Register("android/print/PdfWriteResult")]
    public class PdfWriteResultCallback : PrintDocumentAdapter.WriteResultCallback
    {
        readonly TaskCompletionSource<ToFileResult> _taskCompletionSource;
        readonly string _path;

        public PdfWriteResultCallback(TaskCompletionSource<ToFileResult> taskCompletionSource, string path, IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            _taskCompletionSource = taskCompletionSource;
            _path = path;
        }

        public PdfWriteResultCallback(TaskCompletionSource<ToFileResult> taskCompletionSource, string path) : base(IntPtr.Zero, JniHandleOwnership.DoNotTransfer)
        {
            if (!(base.Handle != IntPtr.Zero))
            {
                unsafe
                {
                    JniObjectReference val = JniPeerMembers.InstanceMethods.StartCreateInstance("()V", GetType(), null);
                    SetHandle(val.Handle, JniHandleOwnership.TransferLocalRef);
                    JniPeerMembers.InstanceMethods.FinishCreateInstance("()V", this, null);
                }
            }
            _taskCompletionSource = taskCompletionSource;
            _path = path;
        }


        public override void OnWriteFinished(PageRange[] pages)
        {
            base.OnWriteFinished(pages);
            _taskCompletionSource.SetResult(new ToFileResult(false, _path));
        }

        public override void OnWriteCancelled()
        {
            base.OnWriteCancelled();
            _taskCompletionSource.SetResult(new ToFileResult(true, "PDF Write was cancelled"));
        }

        public override void OnWriteFailed(ICharSequence error)
        {
            base.OnWriteFailed(error);
            _taskCompletionSource.SetResult(new ToFileResult(true, error.ToString()));
        }
    }


}
