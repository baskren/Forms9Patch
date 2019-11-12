using System;
using System.IO;
using Xamarin.Forms;
using Android.Content;
using Android.Webkit;
using Android.Graphics;
using Android.Views;
using System.Threading.Tasks;

[assembly: Dependency(typeof(Forms9Patch.Droid.HtmlToPngService))]
namespace Forms9Patch.Droid
{
    public class HtmlToPngService : Java.Lang.Object, IHtmlToPngPdfService
    {
        public async Task<HtmlToPngResult> ToPngAsync(ActivityIndicatorPopup popup, string html, string fileName)
        {
            HtmlToPngResult result = default;
            await Task.Delay(50);
            ToPng(popup, html, fileName, (HtmlToPngResult x) => result = x);
            while (result == default)
            {
                await Task.Delay(50);
            }
            return result;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0067:Dispose objects before losing scope", Justification = "CustomWebView is disposed in Callback.Compete")]
        public void ToPng(ActivityIndicatorPopup popup, string html, string fileName, Action<HtmlToPngResult> onComplete)
        {
            if (CanWriteExternalStorage())
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

                    var webView = new CustomWebView(Android.App.Application.Context);
                    webView.Settings.JavaScriptEnabled = true;
                    webView.DrawingCacheEnabled = true;

                    webView.Layout(0, 0, (int)((size.Width - 0.5) * 72), (int)((size.Height - 0.5) * 72));

                    webView.LoadData(html, "text/html; charset=utf-8", "UTF-8");
                    webView.SetWebViewClient(new WebViewCallBack(fileName, onComplete));
                }
            }
            else
                onComplete.Invoke(new HtmlToPngResult(true, "Cannot write External Storage."));
        }

        bool CanWriteExternalStorage()
        {
            var hasPermission = (Android.Support.V4.Content.ContextCompat.CheckSelfPermission(Settings.Activity, Android.Manifest.Permission.WriteExternalStorage) == Android.Content.PM.Permission.Granted);
            if (!hasPermission)
            {
                Android.Support.V4.App.ActivityCompat.RequestPermissions(Settings.Activity,
                    new string[] { Android.Manifest.Permission.WriteExternalStorage },
                                                                         007);// REQUEST_WRITE_STORAGE);
            }
            return hasPermission;
        }
    }

    class WebViewCallBack : Android.Webkit.WebViewClient, IValueCallback
    {

        readonly string _fileName;
        readonly Action<HtmlToPngResult> _onComplete;
        CustomWebView webView;


        public WebViewCallBack(string fileName, Action<HtmlToPngResult> onComplete)
        {
            _fileName = fileName;
            _onComplete = onComplete;
        }

        public void OnReceiveValue(Java.Lang.Object value)
        {

            System.Diagnostics.Debug.WriteLine("value=[" + value + "]");
            var height = Convert.ToInt32(value.ToString());

            System.Diagnostics.Debug.WriteLine("WebViewCallBack" + P42.Utils.ReflectionExtensions.CallerString() + ": Scale=[" + Display.Scale + "]");

            int specWidth = MeasureSpecFactory.MakeMeasureSpec((int)(webView.ContentWidth * Display.Scale), MeasureSpecMode.Exactly);
            int specHeight = MeasureSpecFactory.MakeMeasureSpec((int)((height + 36) * Display.Scale), MeasureSpecMode.Exactly);
            webView.Measure(specWidth, specHeight);
            webView.Layout(0, 0, webView.MeasuredWidth, webView.MeasuredHeight);
            System.Diagnostics.Debug.WriteLine("spec [" + specWidth + ", " + specHeight + "]");
            System.Diagnostics.Debug.WriteLine("webView.Measured [" + webView.MeasuredWidth + ", " + webView.MeasuredHeight + "]");
            System.Diagnostics.Debug.WriteLine("webView.Layout [" + webView.Left + ", " + webView.Top + ", " + webView.Width + ", " + webView.Height + "]");

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Complete();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        async Task Complete()
        {
            await Task.Delay(1000);
            using (var _dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments))
            using (var bitmap = Bitmap.CreateBitmap(webView.DrawingCache))
            {
                if (!_dir.Exists())
                    _dir.Mkdir();

                var path = _dir.Path + "/" + _fileName + ".png";
                using (var file = new Java.IO.File(path))
                {
                    if (!file.Exists())
                        file.CreateNewFile();
                    using (var stream = new FileStream(file.Path, FileMode.Create, System.IO.FileAccess.Write))
                    {
                        bitmap.Compress(Bitmap.CompressFormat.Png, 80, stream);
                        stream.Flush();
                        stream.Close();
                    }
                    /*
                     * The below is left here because it was a pain to find a bug.  Turns out that 
                     * (1) it's way to easy to put the provider Tags outside of the Application tags in the Android manifest ... and there will be no feedback that you screwed up.
                     * (2) capitalization really matters in the provider tag!
                    var uri = Android.Support.V4.Content.FileProvider.GetUriForFile(Settings.Context,
                                            Settings.Context.PackageName + ".fileprovider",
                                            file);

                    var intentAction = Intent.ActionSend;
                    var emailIntent = new Intent(intentAction);
                    emailIntent.SetType("message/rfc822");
                    emailIntent.PutExtra(Intent.ExtraSubject, "ATTACHMENT TEST");
                    emailIntent.PutExtra(Intent.ExtraText, "test for attachment success");
                    //emailIntent.PutExtra(Intent.ExtraText, html);
                    emailIntent.PutExtra(Intent.ExtraStream, uri);
                    emailIntent.AddFlags(ActivityFlags.GrantReadUriPermission);
                    emailIntent.SetFlags(ActivityFlags.ClearTop);
                    emailIntent.SetFlags(ActivityFlags.NewTask);

                    Android.App.Application.Context.StartActivity(emailIntent);
                    */
                    _onComplete?.Invoke(new HtmlToPngResult(false, path));
                    webView.Dispose();
                }
            }
        }

        /*
        private void RequestPermission(Android.App.Activity context)
        {
            var hasPermission = (Android.Support.V4.Content.ContextCompat.CheckSelfPermission(context, Android.Manifest.Permission.WriteExternalStorage) == Android.Content.PM.Permission.Granted);
            if (!hasPermission)
            {
                Android.Support.V4.App.ActivityCompat.RequestPermissions(context,
                    new string[] { Android.Manifest.Permission.WriteExternalStorage },
                                                                         007);// REQUEST_WRITE_STORAGE);
            }
            else
            {
                // You are allowed to write external storage:
                var path = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/new_folder";
                var storageDir = new Java.IO.File(path);
                if (!storageDir.Exists() && !storageDir.Mkdirs())
                {
                    // This should never happen - log handled exception!
                }
            }
        }
        */

        public override void OnPageFinished(Android.Webkit.WebView view, string url)
        {
            webView = view as CustomWebView;
            webView.EvaluateJavascript("Math.max( document.body.scrollHeight, document.body.offsetHeight)", this);
        }
    }

    class CustomWebView : Android.Webkit.WebView
    {
        public CustomWebView(Context context) : base(context) { }

        public int ContentWidth
        {
            get
            {
                return base.ComputeHorizontalScrollRange();
            }
        }

        public new int ContentHeight
        {
            get
            {
                return base.ComputeVerticalScrollRange();
            }
        }
    }
}
