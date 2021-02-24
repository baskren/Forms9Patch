using System.IO;
using Xamarin.Forms;
using Android.Webkit;
using Android.Graphics;
using Android.Views;
using System.Threading.Tasks;
using Xamarin.Forms.Platform.Android;
using System.Reflection;
using System;
using Android.Runtime;
using Android.OS;
using Android.Content;

[assembly: Dependency(typeof(Forms9Patch.Droid.ToPngService))]
namespace Forms9Patch.Droid
{

    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class ToPngService : Java.Lang.Object, IToPngService
    {

        public async Task<ToFileResult> ToPngAsync(string html, string fileName, int width)
        {
            //if (!await XamarinEssentialsExtensions.ConfirmOrRequest<Xamarin.Essentials.Permissions.StorageWrite>())
            //    return new ToFileResult(true, "Write External Stoarge permission must be granted for PNG images to be available.");
            var taskCompletionSource = new TaskCompletionSource<ToFileResult>();
            ToPng(taskCompletionSource, html, fileName, width);
            return await taskCompletionSource.Task;
        }

        public async Task<ToFileResult> ToPngAsync(Xamarin.Forms.WebView webView, string fileName, int width)
        {
            //if (!await XamarinEssentialsExtensions.ConfirmOrRequest<Xamarin.Essentials.Permissions.StorageWrite>())
            //    return new ToFileResult(true, "Write External Stoarge permission must be granted for PNG images to be available.");
            var taskCompletionSource = new TaskCompletionSource<ToFileResult>();
            ToPng(taskCompletionSource, webView, fileName, width);
            return await taskCompletionSource.Task;
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0067:Dispose objects before losing scope", Justification = "CustomWebView is disposed in Callback.Compete")]
        public void ToPng(TaskCompletionSource<ToFileResult> taskCompletionSource, string html, string fileName, int width)
        {

            //var size = new Size(8.5, 11);
            //var externalPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            //using (var dir = new Java.IO.File(externalPath))
            //using (var file = new Java.IO.File(dir + "/" + fileName + ".png"))
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
                webView.Layout(0, 0, width, width);

                webView.SetWebViewClient(new WebViewCallBack(taskCompletionSource, fileName, new PageSize { Width = width }, null, OnPageFinished));
                webView.LoadData(html, "text/html; charset=utf-8", "UTF-8");
            //}
        }

        public void ToPng(TaskCompletionSource<ToFileResult> taskCompletionSource, Xamarin.Forms.WebView xfWebView, string fileName, int width)
        {
            if (Platform.CreateRendererWithContext(xfWebView, Settings.Context) is IVisualElementRenderer renderer)
            {
                var droidWebView = renderer.View as Android.Webkit.WebView;
                if (droidWebView == null && renderer.View is WebViewRenderer xfWebViewRenderer)
                    droidWebView = xfWebViewRenderer.Control;
                if (droidWebView != null)
                {
                    //var size = new Size(8.5, 11);
                    //var bounds = new Rectangle(droidWebView.Left, droidWebView.Top, droidWebView.Width, droidWebView.Height);
                    //var externalPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
                    //using (var dir = new Java.IO.File(externalPath))
                    //using (var file = new Java.IO.File(dir + "/" + fileName + ".png"))
                    //{
                    //    if (!dir.Exists())
                    //        dir.Mkdir();
                    //    if (file.Exists())
                    //        file.Delete();

                        /*
                        Android.Widget.FrameLayout.LayoutParams tmpParams = new Android.Widget.FrameLayout.LayoutParams(width, width);
                        droidWebView.LayoutParameters = tmpParams;
                        droidWebView.Layout(0, 0, width, width);
                        int specWidth = MeasureSpecFactory.MakeMeasureSpec((int)(width * Display.Scale), MeasureSpecMode.Exactly);
                        //int specHeight = MeasureSpecFactory.MakeMeasureSpec((int)(width * Display.Scale), MeasureSpecMode.Exactly);
                        int specHeight = MeasureSpecFactory.MakeMeasureSpec(0, MeasureSpecMode.Unspecified);
                        droidWebView.Measure(specWidth, specHeight);
                        droidWebView.Layout(0, 0, droidWebView.MeasuredWidth, droidWebView.MeasuredHeight);
                        */


                        droidWebView.SetLayerType(LayerType.Software, null);
                        droidWebView.Settings.JavaScriptEnabled = true;
#pragma warning disable CS0618 // Type or member is obsolete
                        droidWebView.DrawingCacheEnabled = true;
                        droidWebView.BuildDrawingCache();
#pragma warning restore CS0618 // Type or member is obsolete

                        droidWebView.SetWebViewClient(new WebViewCallBack(taskCompletionSource, fileName, new PageSize { Width = width }, null, OnPageFinished));
                    //}
                }
            }
        }

        async Task OnPageFinished(Android.Webkit.WebView view, string fileName, PageSize pageSize, PageMargin margin, TaskCompletionSource<ToFileResult> taskCompletionSource)
        {
            /*
            var widthString = await view.EvaluateJavaScriptAsync("document.documentElement.offsetWidth");
            var width = (int)System.Math.Ceiling(double.Parse(widthString.ToString()));

            var heightString = await view.EvaluateJavaScriptAsync("document.documentElement.offsetHeight");
            var height = (int)System.Math.Ceiling(double.Parse(heightString.ToString()));
            var contentHeight = view.ContentHeight;
            */
            //var imageView = new WebViewImage(view, fileName, taskCompletionSource);
            int specWidth = MeasureSpecFactory.MakeMeasureSpec((int)(pageSize.Width), MeasureSpecMode.Exactly);
            int specHeight = MeasureSpecFactory.MakeMeasureSpec(0, MeasureSpecMode.Unspecified);
            view.Measure(specWidth, specHeight);
            var height = view.ContentHeight;
            view.Layout(0, 0, view.MeasuredWidth, height);
            //imageView.Layout(0, 0, width, height);

            if (height < 1)
            {
                var heightString = await view.EvaluateJavaScriptAsync("document.documentElement.offsetHeight");
                height = (int)System.Math.Ceiling(double.Parse(heightString.ToString()));
            }

            var width = view.MeasuredWidth;

            if (width < 1)
            {
                var widthString = await view.EvaluateJavaScriptAsync("document.documentElement.offsetWidth");
                width = (int)System.Math.Ceiling(double.Parse(widthString.ToString()));
            }

            if (height < 1 || width < 1)
            {
                taskCompletionSource.SetResult(new ToFileResult(true, "WebView width or height is zero."));
                return;
            }


            await Task.Delay(50);
            //using (var _dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments))
            //using (var _dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads))
            using (var _dir = Forms9Patch.Droid.Settings.Context.GetExternalFilesDir(Android.OS.Environment.DirectoryDownloads))
            {
                if (!_dir.Exists())
                    _dir.Mkdir();

                //var path = _dir.Path + "/" + fileName + ".png";
                //var path = System.IO.Path.Combine(_dir.AbsolutePath, Android.OS.Environment.DirectoryDownloads, fileName + ".png");
                //var file = new Java.IO.File(path);
                /*
                var file = new Java.IO.File(_dir, fileName + ".png");
                int iter = 0;
                while (file.Exists())
                {
                    file.Dispose();
                    iter++;
                    //path = System.IO.Path.Combine(_dir.AbsolutePath, Android.OS.Environment.DirectoryDownloads, fileName + "_" + iter.ToString("D4") + ".png");
                    //file = new Java.IO.File(path);
                    file = new Java.IO.File(_dir, fileName + "_" + iter.ToString("D4") + ".png");
                }
                //file.CreateNewFile();
                file = Java.IO.File.CreateTempFile(fileName + "_" + iter.ToString("D4"), "png", _dir);
                */
                var file = Java.IO.File.CreateTempFile(fileName + ".", ".png", _dir);
                var path = file.AbsolutePath;

                using (var stream = new FileStream(file.Path, FileMode.Create, System.IO.FileAccess.Write))
                {
                    if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Honeycomb)
                    {
                        await Task.Delay(1000);

                        //using (var bitmap = Bitmap.CreateBitmap(System.Math.Max(view.MeasuredWidth, view.ContentWidth()), view.MeasuredHeight, Bitmap.Config.Argb8888))
                        using (var bitmap = Bitmap.CreateBitmap(view.MeasuredWidth, height, Bitmap.Config.Argb8888))
                        {
                            using (var canvas = new Canvas(bitmap))
                            {
                                if (view.Background != null)
                                    view.Background.Draw(canvas);
                                else
                                    canvas.DrawColor(Android.Graphics.Color.White);

                                view.SetClipChildren(false);
                                view.SetClipToPadding(false);
                                view.SetClipToOutline(false);

                                await Task.Delay(50);
                                view.Draw(canvas);
                                await Task.Delay(50);
                                bitmap.Compress(Bitmap.CompressFormat.Png, 80, stream);
                            }
                        }
                    }
                    else
                    {
                        await Task.Delay(1000);
#pragma warning disable CS0618 // Type or member is obsolete
                        using (var bitmap = Bitmap.CreateBitmap(view.DrawingCache))
#pragma warning restore CS0618 // Type or member is obsolete
                        {
                            bitmap.Compress(Bitmap.CompressFormat.Png, 80, stream);
                        }
                    }
                    stream.Flush();
                    stream.Close();
                    /*
                    if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Q)
                    {
                        // You can add more columns.. Complete list of columns can be found at 
                        // https://developer.android.com/reference/android/provider/MediaStore.Downloads
                        var contentValues = new ContentValues();
                        contentValues.Put(Android.Provider.MediaStore.DownloadColumns.Title, System.IO.Path.GetFileName(path));
                        contentValues.Put(Android.Provider.MediaStore.DownloadColumns.DisplayName, System.IO.Path.GetFileName(path));
                        contentValues.Put(Android.Provider.MediaStore.DownloadColumns.MimeType, "image/png");
                        contentValues.Put(Android.Provider.MediaStore.DownloadColumns.Size, File.ReadAllBytes(path).Length);

                        // If you downloaded to a specific folder inside "Downloads" folder
                        //contentValues.Put(Android.Provider.MediaStore.DownloadColumns.RelativePath, Android.OS.Environment.DirectoryDownloads + File.separator + "Temp");

                        // Insert into the database
                        ContentResolver database = Forms9Patch.Droid.Settings.Context.ContentResolver; // getContentResolver();
                        database.Insert(Android.Provider.MediaStore.Downloads.ExternalContentUri, contentValues);
                    }
                    else
                    {
                        // notify download manager!
                        var downloadManager = Android.App.DownloadManager.FromContext(Android.App.Application.Context);
#pragma warning disable CS0618 // Type or member is obsolete
                        downloadManager.AddCompletedDownload(
                            System.IO.Path.GetFileName(path),
                            System.IO.Path.GetFileName(path),
                            true, "image/png", path,
                            File.ReadAllBytes(path).Length, true);
#pragma warning restore CS0618 // Type or member is obsolete
                    }
                    */
                    taskCompletionSource.SetResult(new ToFileResult(false, path));
                    view.Dispose();
                }
                file.Dispose();
            }
            
        }
    }

    class WebViewCallBack : WebViewClient
    {
        bool _complete;
        readonly string _fileName;
        readonly PageSize _pageSize;
        readonly PageMargin _margin;
        readonly TaskCompletionSource<ToFileResult> _taskCompletionSource;
        readonly Func<Android.Webkit.WebView, string, PageSize, PageMargin, TaskCompletionSource<ToFileResult>, Task> _onPageFinished;

        public WebViewCallBack(TaskCompletionSource<ToFileResult> taskCompletionSource, string fileName, PageSize pageSize, PageMargin margin, Func<Android.Webkit.WebView, string, PageSize, PageMargin, TaskCompletionSource<ToFileResult>, Task> onPageFinished)
        {
            _fileName = fileName;
            _pageSize = pageSize;
            _margin = margin;
            _taskCompletionSource = taskCompletionSource;
            _onPageFinished = onPageFinished;
        }

        public override void OnPageStarted(Android.Webkit.WebView view, string url, Bitmap favicon)
        {
            System.Diagnostics.Debug.WriteLine("WebViewCallBack" + P42.Utils.ReflectionExtensions.CallerString() + ": ");
            base.OnPageStarted(view, url, favicon);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Potential Code Quality Issues", "RECS0165:Asynchronous methods should return a Task instead of void", Justification = "Needed to invoke async code on main thread.")]
        public override void OnPageFinished(Android.Webkit.WebView view, string url)
        {
            System.Diagnostics.Debug.WriteLine("WebViewCallBack" + P42.Utils.ReflectionExtensions.CallerString() + ": SUCCESS!");
            if (!_complete)
            {
                _complete = true;

                Device.BeginInvokeOnMainThread(() =>
                {
                    _onPageFinished?.Invoke(view, _fileName, _pageSize, _margin, _taskCompletionSource);
                });
            }
        }

        public override void OnReceivedError(Android.Webkit.WebView view, IWebResourceRequest request, WebResourceError error)
        {
            base.OnReceivedError(view, request, error);
            _taskCompletionSource.SetResult(new ToFileResult(true, error.Description));
        }

        public override void OnReceivedHttpError(Android.Webkit.WebView view, IWebResourceRequest request, WebResourceResponse errorResponse)
        {
            base.OnReceivedHttpError(view, request, errorResponse);
            _taskCompletionSource.SetResult(new ToFileResult(true, errorResponse.ReasonPhrase));
        }

        public override bool OnRenderProcessGone(Android.Webkit.WebView view, RenderProcessGoneDetail detail)
        {
            System.Diagnostics.Debug.WriteLine("WebViewCallBack" + P42.Utils.ReflectionExtensions.CallerString() + ": ");
            return base.OnRenderProcessGone(view, detail);
        }

        public override void OnLoadResource(Android.Webkit.WebView view, string url)
        {
            System.Diagnostics.Debug.WriteLine("WebViewCallBack" + P42.Utils.ReflectionExtensions.CallerString() + ": ");
            Task.Delay(1000).Wait();
            base.OnLoadResource(view, url);
            Device.StartTimer(TimeSpan.FromSeconds(10), () =>
            {
                if (!_complete)
                    OnPageFinished(view, url);
                return false;
            });
        }

        public override void OnPageCommitVisible(Android.Webkit.WebView view, string url)
        {
            System.Diagnostics.Debug.WriteLine("WebViewCallBack" + P42.Utils.ReflectionExtensions.CallerString() + ": ");
            base.OnPageCommitVisible(view, url);
        }

        public override void OnUnhandledKeyEvent(Android.Webkit.WebView view, KeyEvent e)
        {
            System.Diagnostics.Debug.WriteLine("WebViewCallBack" + P42.Utils.ReflectionExtensions.CallerString() + ": ");
            base.OnUnhandledKeyEvent(view, e);
        }

        public override void OnUnhandledInputEvent(Android.Webkit.WebView view, InputEvent e)
        {
            System.Diagnostics.Debug.WriteLine("WebViewCallBack" + P42.Utils.ReflectionExtensions.CallerString() + ": ");
            base.OnUnhandledInputEvent(view, e);
        }
    }

}
