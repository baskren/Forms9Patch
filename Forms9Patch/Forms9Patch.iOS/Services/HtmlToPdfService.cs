using System;
using System.IO;
using CoreGraphics;
using Foundation;
using PCLStorage;
using UIKit;
using Xamarin.Forms;
using PCL.Utils;

[assembly: Dependency(typeof(Forms9Patch.iOS.HtmlToPdfService))]
namespace Forms9Patch.iOS
{
	public class HtmlToPdfService : IHtmlToPdfService
	{
		public void ToPdf(string html, Size size, IFolder folder, string fileName, Action<IFile> onComplete)
		{
			var webView = new UIWebView(new CGRect(0, 0, (size.Width-0.5) * 72, (size.Height-0.5) * 72));

			var callback = new WebViewCallBack(size, folder, fileName, onComplete);
			webView.Delegate = callback;
			webView.ScalesPageToFit = false;
			webView.UserInteractionEnabled = false;
			webView.BackgroundColor = UIColor.White;
			webView.LoadHtmlString(html, null);
			Device.StartTimer(TimeSpan.FromSeconds(10), () =>
			{
				if (!callback.Failed && !callback.Completed)
				{
					System.Diagnostics.Debug.WriteLine("TIMEOUT!!!");
					callback.LoadingFinished(webView);
				}
				return false;
			});
		}

	}

	class WebViewCallBack : UIWebViewDelegate
	{

		readonly IFolder _folder;
		readonly string _fileName;
		readonly Action<IFile> _onComplete;
		Size _size;
		public bool Completed
		{
			get;
			private set;
		}
		public bool Failed
		{
			get; private set;
		}



		public WebViewCallBack(Size size, IFolder folder, string fileName, Action<IFile> onComplete)
		{
			_folder = folder;
			_fileName = fileName;
			_onComplete = onComplete;
			_size = size;
			Completed = false;
			Failed = false;
		}

		public override void LoadFailed(UIWebView webView, NSError error)
		{
			base.LoadFailed(webView, error);
			Failed = true;
			_onComplete?.Invoke(null);

		}

		public override void LoadingFinished(UIWebView webView)
		{
			System.Diagnostics.Debug.WriteLine("LoadingFinished");

			if (Failed || Completed)
				return;
			Completed = true;

			var pages = 1;


			var vertMargin = (nfloat)(0.25 * 72);
			var horzMargin = vertMargin;
			var height = (nfloat)((_size.Height * pages - 0.5) * 72);
			var width = (nfloat)((_size.Width - 0.5) * 72);

			//width = 595.2;  // A4 210mm x 297mm (~8.26" x ~11.69") (595.27pt x 841.89pt)
			//height = 841.8; // Letter 8.5" x 11" (612px x 792pt)
							// 1/4" = 18pt
			//header = 18;
			//sidespace = 18;


			var pageMargins = new UIEdgeInsets(vertMargin, horzMargin, vertMargin, horzMargin);
			webView.ViewPrintFormatter.ContentInsets = pageMargins;
			var renderer = new UIPrintPageRenderer();

			/*
			 * this only renders the first page
			if (renderer != null)
			{
				UIGraphics.BeginImageContext(webView.Bounds.Size);
				webView.Layer.RenderInContext(UIGraphics.GetCurrentContext());
    			var image = UIGraphics.GetImageFromCurrentImageContext();
				UIGraphics.EndImageContext();

				// Write image to PNG
				var path = Path.Combine(_folder.Path, _fileName.Replace(".pdf",".png"));
				var data = image.AsPNG();
				File.WriteAllBytes(path, data.ToArray());
				var file = _folder.GetFile(_fileName.Replace(".pdf", ".png"));
				_onComplete?.Invoke(file);
				return;
			}
			this only renderes the first page
			*/

			/* and same with this approach in spite of what StackOverflow says 
			if (renderer != null)
			{
				UIImage image = null;
				CGRect oldFrame = webView.Frame;
				CGSize fullSize = webView.ScrollView.ContentSize;
				var bodyHeight = webView.EvaluateJavascript("document.body.scrollHeight");
				fullSize.Height = nfloat.Parse(bodyHeight);
				UIGraphics.BeginImageContext(fullSize);
				var resizedContext = UIGraphics.GetCurrentContext();
				webView.Layer.RenderInContext(resizedContext);
				image = UIGraphics.GetImageFromCurrentImageContext();
				UIGraphics.EndImageContext();
				var data = image.AsPNG();
				if (data != null)
				{
					var path = Path.Combine(_folder.Path, _fileName + ".png");
					File.WriteAllBytes(path, data.ToArray());
					var file = _folder.GetFile(_fileName + ".png");
					_onComplete?.Invoke(file);
					return;
				}
				else
					System.Diagnostics.Debug.WriteLine("{0}[{1}] data==null", PCL.Utils.ReflectionExtensions.CallerString(), GetType());
			}
			*/


			if (renderer != null)
			{
				renderer.AddPrintFormatter(webView.ViewPrintFormatter, 0);

				var printableRect = new CGRect(horzMargin,
				                               vertMargin,
				                               width,
				                               height);
				var paperRect = new CGRect(0, 0, _size.Width*72, pages*_size.Height*72);
				renderer.SetValueForKey(FromObject(paperRect), (NSString)"paperRect");
				renderer.SetValueForKey(FromObject(printableRect), (NSString)"printableRect");
				var data = PrintToPDFWithRenderer(renderer, paperRect);
				if (data != null)
				{
					var path = Path.Combine(_folder.Path, _fileName+".pdf");
					File.WriteAllBytes(path, data.ToArray());
					var file = _folder.GetFile(_fileName+ ".pdf");
					_onComplete?.Invoke(file);
					return;
				}
				else
					System.Diagnostics.Debug.WriteLine("{0}[{1}] data==null", PCL.Utils.ReflectionExtensions.CallerString(), GetType());
			}
			else
				System.Diagnostics.Debug.WriteLine("{0}[{1}] renderer==null", PCL.Utils.ReflectionExtensions.CallerString(), GetType());

			Failed = true;
			_onComplete?.Invoke(null);
		}

		NSData PrintToPDFWithRenderer(UIPrintPageRenderer renderer, CGRect paperRect)
		{
			var pdfData = new NSMutableData();
			UIGraphics.BeginPDFContext(pdfData, paperRect, null);

			renderer.PrepareForDrawingPages(new NSRange(0, renderer.NumberOfPages));

			var bounds = UIGraphics.PDFContextBounds;

			for (int i = 0; i < renderer.NumberOfPages; i++)
			{
				UIGraphics.BeginPDFPage();
				renderer.DrawPage(i, paperRect);
			}
			UIGraphics.EndPDFContent();

			return pdfData;
		}

	}
}
