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
        public void ToPng(string html, string fileName, Action<string> onComplete)
        {
            if (CanWriteExternalStorage())
            {
                var size = new Size(8.5, 11);
                var externalPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
                var dir = new Java.IO.File(externalPath);
                var file = new Java.IO.File(dir + "/" + fileName + ".png");
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
            else
                onComplete.Invoke(null);
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
        readonly Action<string> _onComplete;
        CustomWebView webView;

        readonly Java.IO.File _dir;


        public WebViewCallBack(string fileName, Action<string> onComplete)
        {
            _dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments);
            //_dir = Android.OS.Environment.DownloadCacheDirectory;
            //_dir = Settings.Context.ExternalCacheDir;
            //var cacheDirPath = Settings.Context.CacheDir.AbsolutePath;
            //var cacheDirPath = Settings.Context.ExternalCacheDir.AbsolutePath;
            //_dir = new Java.IO.File(cacheDirPath);
            //_dir = Settings.Context.DataDir;
            // the below doesn't work woith Xam.Plugins.Messaging for email attachments
            //_dir = new Java.IO.File(P42.Utils.Environment.TemporaryStoragePath);
            _fileName = fileName;
            _onComplete = onComplete;
        }

        public void OnReceiveValue(Java.Lang.Object value)
        {

            System.Diagnostics.Debug.WriteLine("value=[" + value + "]");
            var height = Convert.ToInt32(value.ToString());

            int specWidth = MeasureSpecFactory.MakeMeasureSpec(webView.ContentWidth, MeasureSpecMode.Exactly);
            int specHeight = MeasureSpecFactory.MakeMeasureSpec(height + 36, MeasureSpecMode.Exactly);
            webView.Measure(specWidth, specHeight);
            webView.Layout(0, 0, webView.MeasuredWidth, webView.MeasuredHeight);
            System.Diagnostics.Debug.WriteLine("spec [" + specWidth + ", " + specHeight + "]");
            System.Diagnostics.Debug.WriteLine("webView.Measured [" + webView.MeasuredWidth + ", " + webView.MeasuredHeight + "]");
            System.Diagnostics.Debug.WriteLine("webView.Layout [" + webView.Left + ", " + webView.Top + ", " + webView.Width + ", " + webView.Height + "]");

            Complete();
        }

        async Task Complete()
        {
            await Task.Delay(1000);
            var bitmap = Bitmap.CreateBitmap(webView.DrawingCache);


            if (!_dir.Exists())
                _dir.Mkdir();

            //Java.IO.File tempFile = Java.IO.File.CreateTempFile(_fileName, ".png", Settings.Context.ExternalCacheDir);


            var path = _dir.Path + "/" + _fileName + ".png";
            //var path = System.IO.Path.Combine(Settings.Context.CacheDir.AbsolutePath, "pizza.png");
            var file = new Java.IO.File(path);
            if (!file.Exists())
                file.CreateNewFile();
            var stream = new FileStream(file.Path, FileMode.Create, System.IO.FileAccess.Write);

            //var stream = new FileStream(tempFile.Path, FileMode.Create, System.IO.FileAccess.Write);

            bitmap.Compress(Bitmap.CompressFormat.Png, 80, stream);
            stream.Flush();
            stream.Close();
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
            _onComplete?.Invoke(path);
            //_onComplete?.Invoke(tempFile.Path);

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
