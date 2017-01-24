using System;
using PCLStorage;
using System.IO;
using PCL.Utils;
using Xamarin.Forms;

[assembly: Dependency(typeof(Forms9Patch.Droid.HtmlToPdfService))]
namespace Forms9Patch.Droid
{
	public class HtmlToPdfService : IHtmlToPdfService
	{
		
		public void ToPdf(string html, Size size, IFolder folder, string fileName, Action<IFile> onComplete)
		{
			var dir = new Java.IO.File(folder.Path);
			var file = new Java.IO.File(dir + "/" + fileName);
			if (!dir.Exists())
				dir.Mkdir();
			if (file.Exists())
				file.Delete();

			var webPage = new Android.Webkit.WebView(Android.App.Application.Context);

			webPage.Layout(0,0,2102, 2973);
			webPage.LoadDataWithBaseURL("", html, "text/html", "UTF-8", null);
			webPage.SetWebViewClient(new WebViewCallBack(size, folder,fileName, onComplete));
		}

	}

	class WebViewCallBack : Android.Webkit.WebViewClient
	{

		readonly IFolder _folder;
		readonly string _fileName;
		readonly Action<IFile> _onComplete;
		Size _size;

		public WebViewCallBack(Size size, IFolder folder, string fileName, Action<IFile> onComplete)
		{
			_folder = folder;
			_fileName = fileName;
			_onComplete = onComplete;
			_size = size;
		}


		public override void OnPageFinished(Android.Webkit.WebView view, string url)
		{
			var document = new Android.Graphics.Pdf.PdfDocument();
			var page = document.StartPage(new Android.Graphics.Pdf.PdfDocument.PageInfo.Builder(2120, 3000, 1).Create());

			view.Draw(page.Canvas);
			document.FinishPage(page);
			Stream filestream = new MemoryStream();
			var fos = new Java.IO.FileOutputStream(_folder.Path+"/"+_fileName, false);
			try
			{
				document.WriteTo(filestream);
				fos.Write(((MemoryStream)filestream).ToArray(), 0, (int)filestream.Length);
				fos.Close();
				var file = _folder.GetFile(_fileName);
				_onComplete?.Invoke(file);
			}
			catch
			{
				_onComplete?.Invoke(null);
			}
		}
	}
}
