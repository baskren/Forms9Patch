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

        public async Task<ToFileResult> ToPdfAsync(string html, string fileName, PageSize pageSize, PageMargin margin)
        {
            //if (!await XamarinEssentialsExtensions.ConfirmOrRequest<Xamarin.Essentials.Permissions.StorageWrite>())
            //    return new ToFileResult(true, "Write External Stoarge permission must be granted for PNG images to be available.");
            var taskCompletionSource = new TaskCompletionSource<ToFileResult>();
            ToPdf(taskCompletionSource, html, fileName, pageSize, margin);
            return await taskCompletionSource.Task;
        }

        public async Task<ToFileResult> ToPdfAsync(Xamarin.Forms.WebView webView, string fileName, PageSize pageSize, PageMargin margin)
        {
            //if (!await XamarinEssentialsExtensions.ConfirmOrRequest<Xamarin.Essentials.Permissions.StorageWrite>())
            //    return new ToFileResult(true, "Write External Stoarge permission must be granted for PNG images to be available.");
            var taskCompletionSource = new TaskCompletionSource<ToFileResult>();
            ToPdf(taskCompletionSource, webView, fileName, pageSize, margin);
            return await taskCompletionSource.Task;
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0067:Dispose objects before losing scope", Justification = "CustomWebView is disposed in Callback.Compete")]
        public void ToPdf(TaskCompletionSource<ToFileResult> taskCompletionSource, string html, string fileName, PageSize pageSize, PageMargin margin)
        {
            //var externalPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            //using (var dir = new Java.IO.File(externalPath))
            //using (var file = new Java.IO.File(dir + "/" + fileName + ".pdf"))
            //{
            //    if (!dir.Exists())
            //        dir.Mkdir();
            //    if (file.Exists())
            //        file.Delete();

                var webView = new Android.Webkit.WebView(Android.App.Application.Context);
                webView.Settings.JavaScriptEnabled = true;
#pragma warning disable CS0618 // Type or member is obsolete
                webView.DrawingCacheEnabled = true;
#pragma warning restore CS0618 // Type or member is obsolete
                webView.SetLayerType(LayerType.Software, null);

                //webView.Layout(0, 0, (int)((size.Width - 0.5) * 72), (int)((size.Height - 0.5) * 72));
                webView.Layout(0, 0, (int)System.Math.Ceiling(pageSize.Width), (int)System.Math.Ceiling(pageSize.Height));

                webView.LoadData(html, "text/html; charset=utf-8", "UTF-8");
                webView.SetWebViewClient(new WebViewCallBack(taskCompletionSource, fileName, pageSize, margin, OnPageFinished));
            //}
        }

        public void ToPdf(TaskCompletionSource<ToFileResult> taskCompletionSource, Xamarin.Forms.WebView xfWebView, string fileName, PageSize pageSize, PageMargin margin)
        {
            if (Platform.CreateRendererWithContext(xfWebView, Settings.Context) is IVisualElementRenderer renderer)
            {
                var droidWebView = renderer.View as Android.Webkit.WebView;
                if (droidWebView == null && renderer.View is WebViewRenderer xfWebViewRenderer)
                    droidWebView = xfWebViewRenderer.Control;
                if (droidWebView != null)
                {
                    //var size = new Size(8.5, 11);
                    
                    //var externalPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
                    //using (var dir = new Java.IO.File(externalPath))
                    //using (var file = new Java.IO.File(dir + "/" + fileName + ".pdf"))
                    //{
                        //if (!dir.Exists())
                        //    dir.Mkdir();
                        //if (file.Exists())
                        //    file.Delete();

                        droidWebView.SetLayerType(LayerType.Software, null);
                        droidWebView.Settings.JavaScriptEnabled = true;
#pragma warning disable CS0618 // Type or member is obsolete
                        droidWebView.DrawingCacheEnabled = true;
                        droidWebView.BuildDrawingCache();
#pragma warning restore CS0618 // Type or member is obsolete

                        droidWebView.SetWebViewClient(new WebViewCallBack(taskCompletionSource, fileName, pageSize, margin, OnPageFinished));
                    //}
                }
            }
        }



        async Task OnPageFinished(Android.Webkit.WebView webView, string fileName, PageSize pageSize, PageMargin margin, TaskCompletionSource<ToFileResult> taskCompletionSource)
        {
            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Kitkat)
            {
                await Task.Delay(5);
                var builder = new PrintAttributes.Builder();
                //builder.SetMediaSize(PrintAttributes.MediaSize.NaLetter);
                builder.SetMediaSize(new PrintAttributes.MediaSize(pageSize.Name, pageSize.Name, (int)(pageSize.Width * 1000 / 72), (int)(pageSize.Height * 1000 / 72)));
                builder.SetResolution(new PrintAttributes.Resolution("pdf", "pdf", 72, 72));
                if (margin is null)
                    builder.SetMinMargins(PrintAttributes.Margins.NoMargins);
                else
                    builder.SetMinMargins(new PrintAttributes.Margins((int)(margin.Left * 1000 / 72), (int)(margin.Top *1000/72), (int)(margin.Right * 1000/72), (int)(margin.Bottom * 1000 / 72)));
                var attributes = builder.Build();

                var adapter = webView.CreatePrintDocumentAdapter(Guid.NewGuid().ToString());

                var layoutResultCallback = new PdfLayoutResultCallback();
                layoutResultCallback.Adapter = adapter;
                layoutResultCallback.TaskCompletionSource = taskCompletionSource;
                layoutResultCallback.FileName = fileName;
                adapter.OnLayout(null, attributes, null, layoutResultCallback, null);
            }
        }
    }

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
            if (!(Handle != IntPtr.Zero))
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

        public override void OnLayoutFinished(PrintDocumentInfo info, bool changed)
        {
            //using (var _dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments))
            //using (var _dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads))
            //using (var _dir = Forms9Patch.Droid.Settings.Context.FilesDir)
            using (var _dir = Forms9Patch.Droid.Settings.Context.CacheDir)
            {
                if (!_dir.Exists())
                    _dir.Mkdir();

                // var path = _dir.Path + "/" + FileName + ".pdf";
                //var path = System.IO.Path.Combine(_dir.AbsolutePath, Android.OS.Environment.DirectoryDownloads, FileName + ".pdf");
                //var file = new Java.IO.File(path);
                /*
                var file = new Java.IO.File(_dir, FileName + ".pdf");
                int iter = 0;
                while (file.Exists())
                {
                    file.Dispose();
                    iter++;
                    //path = _dir.Path + "/" + FileName + "_" + iter.ToString("D3") + ".pdf";
                    //path = System.IO.Path.Combine(_dir.AbsolutePath, Android.OS.Environment.DirectoryDownloads, FileName + "_" + iter.ToString("D4") + ".pdf");
                    //file = new Java.IO.File(path);
                    file = new Java.IO.File(_dir, FileName + "_" + iter.ToString("D3") + ".pdf");
                }


                //file.CreateNewFile();
                */
                var file = Java.IO.File.CreateTempFile(FileName + ".", ".pdf", _dir);

                var fileDescriptor = ParcelFileDescriptor.Open(file, ParcelFileMode.ReadWrite);

                var writeResultCallback = new PdfWriteResultCallback(TaskCompletionSource, file.AbsolutePath);

                Adapter.OnWrite(new Android.Print.PageRange[] { PageRange.AllPages }, fileDescriptor, new CancellationSignal(), writeResultCallback);

                file.Dispose();

                //Android.Media.MediaScannerConnection.ScanFile(Forms9Patch.Droid.Settings.Context, new string[] { file.AbsolutePath }, new string[] { "application/pdf" }, null);
                //Android.Media.MediaScannerConnection.ScanFile(file.AbsolutePath, "application/pdf");
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
            if (!(Handle != IntPtr.Zero))
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
            /*
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Q)
            {
                // You can add more columns.. Complete list of columns can be found at 
                // https://developer.android.com/reference/android/provider/MediaStore.Downloads
                var contentValues = new Content.ContentValues();
                contentValues.Put(Android.Provider.MediaStore.DownloadColumns.Title, System.IO.Path.GetFileName(_path));
                contentValues.Put(Android.Provider.MediaStore.DownloadColumns.DisplayName, System.IO.Path.GetFileName(_path));
                contentValues.Put(Android.Provider.MediaStore.DownloadColumns.MimeType, "image/png");
                contentValues.Put(Android.Provider.MediaStore.DownloadColumns.Size, File.ReadAllBytes(_path).Length);

                // If you downloaded to a specific folder inside "Downloads" folder
                //contentValues.Put(Android.Provider.MediaStore.DownloadColumns.RelativePath, Android.OS.Environment.DirectoryDownloads + File.separator + "Temp");

                // Insert into the database
                var database = Forms9Patch.Droid.Settings.Context.ContentResolver; // getContentResolver();
                database.Insert(Android.Provider.MediaStore.Downloads.ExternalContentUri, contentValues);
            }
            else
            {
                // notify download manager!
                var downloadManager = Android.App.DownloadManager.FromContext(Android.App.Application.Context);
                var length = File.ReadAllBytes(_path).Length;
#pragma warning disable CS0618 // Type or member is obsolete
                downloadManager.AddCompletedDownload(
                    System.IO.Path.GetFileName(_path),
                    System.IO.Path.GetFileName(_path),
                    true, "application/pdf", _path,
                    length, true);
#pragma warning restore CS0618 // Type or member is obsolete
            }
            */
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
