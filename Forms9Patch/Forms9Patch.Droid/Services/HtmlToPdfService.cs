using System;
using PCLStorage;
using System.IO;
using PCL.Utils;
using Xamarin.Forms;
using Android.Print;
using System.Security.Policy;
using Android.Content;
using Android.Print.Pdf;
using Android.Graphics.Pdf;
using System.Security.AccessControl;
using Java.IO;
using Android.Webkit;
using Java.Lang;

[assembly: Dependency(typeof(Forms9Patch.Droid.HtmlToPdfService))]
namespace Forms9Patch.Droid
{
	public class HtmlToPdfService : Java.Lang.Object, IHtmlToPdfService
	{
		public void ToPdf(string html, Size size, string fileName, Action<IFile> onComplete)
		{
			var externalPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
			var dir = new Java.IO.File(externalPath);
			var file = new Java.IO.File(dir + "/" + fileName + ".pdf");
			if (!dir.Exists())
				dir.Mkdir();
			if (file.Exists())
				file.Delete();

			//var webPage = new Android.Webkit.WebView(Android.App.Application.Context);
			var webView = new CustomWebView(Android.App.Application.Context);


			webView.Measure(5 * 72, 10 * 72);
			webView.Layout(0,0,5*72,10*72);
			//webPage.LoadDataWithBaseURL("", html, "text/html", "UTF-8", null);
			webView.LoadData(html,"text/html","UTF-8");
			webView.SetWebViewClient(new WebViewCallBack(size, fileName, onComplete));
		}

	}

	class WebViewCallBack : Android.Webkit.WebViewClient, IValueCallback
	{

		readonly IFolder _folder;
		readonly string _fileName;
		readonly Action<IFile> _onComplete;
		Size _size;

		public WebViewCallBack(Size size, string fileName, Action<IFile> onComplete)
		{
			_folder = PCLStorage.FileSystem.Current.GetFolderFromPath(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath);
			_fileName = fileName;
			_onComplete = onComplete;
			_size = size;
		}

		public void OnReceiveValue(Java.Lang.Object value)
		{
			System.Diagnostics.Debug.WriteLine("value=["+value+"]");
		}


		public override void OnPageFinished(Android.Webkit.WebView view, string url)
		{
			var baseHeight = view.ContentHeight;
			var webView = view as CustomWebView;
			if (webView == null)
				throw new System.Exception("failed to cast view as CustomWebView");
			var contentHeight = webView.ContentHeight;
			var contentWidth = webView.ContentWidth;

			System.Diagnostics.Debug.WriteLine("{0}[{1}] ContentWidth=["+contentWidth+"] ContentHeight=["+contentHeight+"]", PCL.Utils.ReflectionExtensions.CallerString(), GetType());

			webView.EvaluateJavascript("document.body.scrollHeight", this);


			webView.Measure(5 * 72, 10 * 72);
			webView.Layout(0, 0, 5 * 72, 10 * 72);

			var document = new Android.Graphics.Pdf.PdfDocument();
			var page = document.StartPage(new Android.Graphics.Pdf.PdfDocument.PageInfo.Builder(300, 300, 1).Create());

			view.Draw(page.Canvas);
			document.FinishPage(page);

			Stream memoryStream = new MemoryStream();
			//var os = new ByteArrayOutputStream();
			var fos = new Java.IO.FileOutputStream(_folder.Path+"/"+_fileName+".pdf", false);
			try
			{
				document.WriteTo(memoryStream);
				fos.Write(((MemoryStream)memoryStream).ToArray(), 0, (int)memoryStream.Length);
				fos.Close();
				var file = _folder.GetFile(_fileName+ ".pdf");
				System.Diagnostics.Debug.WriteLine("{0}[{1}] path=["+file.Path+"]", PCL.Utils.ReflectionExtensions.CallerString(), GetType());
				_onComplete?.Invoke(file);
			}
			catch
			{
				System.Diagnostics.Debug.WriteLine("{0}[{1}] FAILED TO WRITE", PCL.Utils.ReflectionExtensions.CallerString(), GetType());
				_onComplete?.Invoke(null);
			}



			/*
			var builder = new PrintAttributes.Builder();
			builder.SetColorMode(PrintColorMode.Color);
			builder.SetMediaSize(PrintAttributes.MediaSize.IsoA4); // or ISO_A0
			builder.SetMinMargins(PrintAttributes.Margins.NoMargins);
			builder.SetResolution(new PrintAttributes.Resolution("1", "label", 300, 300));

			var document = new PrintedPdfDocument(Android.App.Application.Context, builder.Build());
			var page = document.StartPage(new PdfDocument.PageInfo.Builder(contentWidth, contentHeight, 1).Create());
			view.Draw(page.Canvas);
			document.FinishPage(page);

			//Stream filestream = new MemoryStream();
			//var fos = new Java.IO.FileOutputStream(_folder.Path + "/" + _fileName + ".pdf", false);

			//var fileStream = new FileStream(_folder.Path + "/"+ _fileName + ".pdf", System.IO.FileAccess.Write);
			using (var fileStream = new System.IO.FileStream(_folder.Path + "/" + _fileName + ".pdf", System.IO.FileMode.Create))
			{
				document.WriteTo(fileStream);
				var file = _folder.GetFile(_fileName + ".pdf");

				_onComplete?.Invoke(file);
				return;
			}
			_onComplete?.Invoke(null);
			*/

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
