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

[assembly: Dependency(typeof(Forms9Patch.Droid.ToPngService))]
namespace Forms9Patch.Droid
{

    public class ToPngService : Java.Lang.Object, IToPngService
    {

        public async Task<ToFileResult> ToPngAsync(string html, string fileName)
        {
            if (!await Permissions.WriteExternalStorage.ConfirmOrRequest())
                return new ToFileResult(true, "Write External Stoarge permission must be granted for PNG images to be available.");
            var taskCompletionSource = new TaskCompletionSource<ToFileResult>();
            ToPng(taskCompletionSource, html, fileName);
            return await taskCompletionSource.Task;
        }

        public async Task<ToFileResult> ToPngAsync(Xamarin.Forms.WebView webView, string fileName)
        {
            if (!await Permissions.WriteExternalStorage.ConfirmOrRequest())
                return new ToFileResult(true, "Write External Stoarge permission must be granted for PNG images to be available.");
            var taskCompletionSource = new TaskCompletionSource<ToFileResult>();
            ToPng(taskCompletionSource, webView, fileName);
            return await taskCompletionSource.Task;
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0067:Dispose objects before losing scope", Justification = "CustomWebView is disposed in Callback.Compete")]
        public void ToPng(TaskCompletionSource<ToFileResult> taskCompletionSource, string html, string fileName)
        {

            var size = new Size(8.5, 11);
            var externalPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            using (var dir = new Java.IO.File(externalPath))
            using (var file = new Java.IO.File(dir + "/" + fileName + ".png"))
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

                webView.SetWebViewClient(new WebViewCallBack(taskCompletionSource, fileName, OnPageFinished));
                webView.LoadData(html, "text/html; charset=utf-8", "UTF-8");
            }
        }

        public void ToPng(TaskCompletionSource<ToFileResult> taskCompletionSource, Xamarin.Forms.WebView xfWebView, string fileName)
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
                    using (var file = new Java.IO.File(dir + "/" + fileName + ".png"))
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

        async Task OnPageFinished(Android.Webkit.WebView view, string fileName, TaskCompletionSource<ToFileResult> taskCompletionSource)
        {
            var widthString = await view.EvaluateJavaScriptAsync("document.documentElement.offsetWidth");
            var width = double.Parse(widthString.ToString());

            var heightString = await view.EvaluateJavaScriptAsync("document.documentElement.offsetHeight");
            var height = double.Parse(heightString.ToString());

            int specWidth = MeasureSpecFactory.MakeMeasureSpec((int)(width * Display.Scale), MeasureSpecMode.Exactly);
            int specHeight = MeasureSpecFactory.MakeMeasureSpec((int)(height * Display.Scale), MeasureSpecMode.Exactly);
            view.Measure(specWidth, specHeight);
            view.Layout(0, 0, view.MeasuredWidth, view.MeasuredHeight);

            await Task.Delay(50);
            using (var _dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments))
            {
                if (!_dir.Exists())
                    _dir.Mkdir();

                var path = _dir.Path + "/" + fileName + ".png";
                using (var file = new Java.IO.File(path))
                {
                    if (!file.Exists())
                        file.CreateNewFile();
                    using (var stream = new FileStream(file.Path, FileMode.Create, System.IO.FileAccess.Write))
                    {

                        if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Honeycomb)
                        {
                            await Task.Delay(1000);

                            using (var bitmap = Bitmap.CreateBitmap(System.Math.Max(view.MeasuredWidth, view.ContentWidth()), view.MeasuredHeight, Bitmap.Config.Argb8888))
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
                        taskCompletionSource.SetResult(new ToFileResult(false, path));
                        view.Dispose();
                    }
                }
            }

        }
    }

    class WebViewCallBack : WebViewClient
    {
        bool _complete;
        readonly string _fileName;
        readonly TaskCompletionSource<ToFileResult> _taskCompletionSource;
        readonly Func<Android.Webkit.WebView, string, TaskCompletionSource<ToFileResult>, Task> _onPageFinished;

        public WebViewCallBack(TaskCompletionSource<ToFileResult> taskCompletionSource, string fileName, Func<Android.Webkit.WebView, string, TaskCompletionSource<ToFileResult>, Task> onPageFinished)
        {
            _fileName = fileName;
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
                    _onPageFinished?.Invoke(view, _fileName, _taskCompletionSource);
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
