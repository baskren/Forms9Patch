using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using Forms9Patch;

[assembly: Dependency(typeof(Forms9Patch.UWP.ToPngService))]
namespace Forms9Patch.UWP
{


	/// <summary>
	/// HTML to PDF service.
	/// </summary>
	[Xamarin.Forms.Internals.Preserve(AllMembers = true)]
	public class ToPngService : IToPngService
	{
		readonly static DependencyProperty PngFileNameProperty = DependencyProperty.Register("PngFileName", typeof(string), typeof(ToPngService), null);
		readonly static DependencyProperty TaskCompletionSourceProperty = DependencyProperty.Register("OnPngComplete", typeof(TaskCompletionSource<ToFileResult>), typeof(ToPngService), null);
		//readonly static DependencyProperty WebViewProperty = DependencyProperty.Register("WebView", typeof(Windows.UI.Xaml.Controls.WebView), typeof(ToPngService), null);
		readonly static DependencyProperty HtmlStringProperty = DependencyProperty.Register("HtmlString", typeof(string), typeof(ToPngService), null);
		readonly static DependencyProperty PngWidthProperty = DependencyProperty.Register("PngWidth", typeof(int), typeof(ToPngService), null);

		readonly static DependencyProperty BeforeWidthProperty = DependencyProperty.Register("BeforeWidth", typeof(int), typeof(ToPngService), null);
		readonly static DependencyProperty BeforeHeightProperty = DependencyProperty.Register("BeforeHeight", typeof(int), typeof(ToPngService), null);

		readonly static DependencyProperty ToFileResultProperty = DependencyProperty.Register("ToFileResult", typeof(string), typeof(ToPngService), null);

		public async Task<ToFileResult> ToPngAsync(string html, string fileName, int width)
		{
			var taskCompletionSource = new TaskCompletionSource<ToFileResult>();
			ToPng(taskCompletionSource, html, fileName, width);
			return await taskCompletionSource.Task;
		}

		public async Task<ToFileResult> ToPngAsync(Xamarin.Forms.WebView webView, string fileName, int width)
		{
			var taskCompletionSource = new TaskCompletionSource<ToFileResult>();
			ToPng(taskCompletionSource, webView, fileName, width);
			return await taskCompletionSource.Task;
		}

		Windows.UI.Xaml.Controls.WebView _webView;
		int instanceCount = 0;

#pragma warning disable CS1998
		public void ToPng(TaskCompletionSource<ToFileResult> taskCompletionSource, string html, string fileName, int width)
		{
			Device.BeginInvokeOnMainThread(async () =>
			{
				//var size = new Size(5, 11);
				//if (Platform.GetRenderer(popup.WebView) is Xamarin.Forms.Platform.UWP.WebViewRenderer renderer)
				{
					//var webView = renderer.Control;
					_webView = new Windows.UI.Xaml.Controls.WebView(WebViewExecutionMode.SameThread)
					{
						Name = "PrintWebView" + (instanceCount++).ToString("D3"),
						DefaultBackgroundColor = Windows.UI.Colors.White,
						Visibility = Visibility.Visible,

					};
					_webView.Settings.IsJavaScriptEnabled = true;
					_webView.Settings.IsIndexedDBEnabled = true;

					PrintHelper.RootPanel.Children.Insert(0, _webView);
					var webView = _webView;

					webView.DefaultBackgroundColor = Windows.UI.Colors.White;
					webView.Width = width;
					webView.Height = 10; // (size.Height - 0.5) * 72,

					webView.Visibility = Visibility.Visible;

					webView.SetValue(PngFileNameProperty, fileName);
					webView.SetValue(TaskCompletionSourceProperty, taskCompletionSource);
					webView.SetValue(HtmlStringProperty, html);
					webView.SetValue(PngWidthProperty, width);
					webView.Width = width;

					webView.NavigationCompleted += NavigationCompleteA;
					webView.NavigationFailed += WebView_NavigationFailed;

					webView.NavigateToString(html);

					await taskCompletionSource.Task;
					PrintHelper.RootPanel.Children.Remove(webView);
				}
			});
		}

		public void ToPng(TaskCompletionSource<ToFileResult> taskCompletionSource, Xamarin.Forms.WebView xfWebView, string fileName, int width)
		{
			Device.BeginInvokeOnMainThread(async () =>
			{
				if (Platform.GetRenderer(xfWebView) is Xamarin.Forms.Platform.UWP.WebViewRenderer renderer)
				{
					var webView = renderer.Control;

					var contentSize = await webView.WebViewContentSizeAsync();
					System.Diagnostics.Debug.WriteLine("A contentSize=[" + contentSize + "]");
					System.Diagnostics.Debug.WriteLine("A webView.Size=[" + webView.Width + "," + webView.Height + "] IsOnMainThread=[" + P42.Utils.Environment.IsOnMainThread + "]");

					webView.SetValue(BeforeWidthProperty, contentSize.Width);
					webView.SetValue(BeforeHeightProperty, contentSize.Height);

					webView.SetValue(PngFileNameProperty, fileName);
					webView.SetValue(TaskCompletionSourceProperty, taskCompletionSource);
					webView.SetValue(PngWidthProperty, width);
					webView.Width = width;

					webView.NavigationCompleted += NavigationCompleteA;
					webView.NavigationFailed += WebView_NavigationFailed;

					//webView.Width = width;
					NavigationCompleteA(webView, null);
				}
			});
		}

#pragma warning restore CS1998

		private void WebView_NavigationFailed(object sender, WebViewNavigationFailedEventArgs e)
		{
			var webView = (Windows.UI.Xaml.Controls.WebView)sender;
			if (webView != null)
			{
				var onComplete = (Action<string>)webView.GetValue(TaskCompletionSourceProperty);
				onComplete.Invoke(null);
			}
		}

		private async void NavigationCompleteA(Windows.UI.Xaml.Controls.WebView webView, WebViewNavigationCompletedEventArgs args)
		{
			//IsMainPageChild(webView);

			webView.NavigationCompleted -= NavigationCompleteA;
			var contentSize = await webView.WebViewContentSizeAsync();
			System.Diagnostics.Debug.WriteLine("A contentSize=[" + contentSize + "]");
			System.Diagnostics.Debug.WriteLine("A webView.Size=[" + webView.Width + "," + webView.Height + "] IsOnMainThread=[" + P42.Utils.Environment.IsOnMainThread + "]");

			//webView.Width = contentSize.Width;
			var width = (int)webView.GetValue(PngWidthProperty);
			webView.Width = width;
			webView.Height = contentSize.Height;

			//webView.UpdateLayout();

			webView.NavigationCompleted += NavigationCompleteB;

			//if (webView.GetValue(HtmlStringProperty) is string html)
			//	webView.NavigateToString(html);
			//else
				//NavigationCompleteB(webView, args);
				webView.Refresh();
		}

		private async void NavigationCompleteB(Windows.UI.Xaml.Controls.WebView webView, WebViewNavigationCompletedEventArgs args)
		{
			webView.NavigationCompleted -= NavigationCompleteB;
			webView.NavigationCompleted += NavigationCompleteC;

			IsMainPageChild(webView);

			using (InMemoryRandomAccessStream ms = new InMemoryRandomAccessStream())
			{
				System.Diagnostics.Debug.WriteLine("B webView.Size=[" + webView.Width + "," + webView.Height + "] IsOnMainThread=[" + P42.Utils.Environment.IsOnMainThread + "]");
				try
				{
					var width = (int)webView.GetValue(PngWidthProperty);
					System.Diagnostics.Debug.WriteLine("B width=[" + width + "]");

					var contentSize = await webView.WebViewContentSizeAsync();
					System.Diagnostics.Debug.WriteLine("B contentSize=[" + contentSize + "]");
					System.Diagnostics.Debug.WriteLine("B webView.Size=[" + webView.Width + "," + webView.Height + "] IsOnMainThread=[" + P42.Utils.Environment.IsOnMainThread + "]");

					if (contentSize.Height != webView.Height || width != webView.Width)
					{
						webView.Width = contentSize.Width;
						webView.Height = contentSize.Height;
						System.Diagnostics.Debug.WriteLine("B webView.Size=[" + webView.Width + "," + webView.Height + "] IsOnMainThread=[" + P42.Utils.Environment.IsOnMainThread + "]");

						webView.InvalidateMeasure();
						System.Diagnostics.Debug.WriteLine("B webView.Size=[" + webView.Width + "," + webView.Height + "] IsOnMainThread=[" + P42.Utils.Environment.IsOnMainThread + "]");
					}

					//await Task.Delay(2000);
					await webView.CapturePreviewToStreamAsync(ms);

					var decoder = await BitmapDecoder.CreateAsync(ms);

					var transform = new BitmapTransform
					{
						//ScaledHeight = (uint)decoder.PixelHeight,
						//ScaledWidth = (uint)decoder.PixelWidth
						ScaledHeight = (uint)Math.Ceiling(webView.Height),
						ScaledWidth = (uint)Math.Ceiling(webView.Width)
					};

					var pixelData = await decoder.GetPixelDataAsync(
						BitmapPixelFormat.Bgra8,
						BitmapAlphaMode.Straight,
						transform,
						ExifOrientationMode.RespectExifOrientation,
						ColorManagementMode.DoNotColorManage);
					var bytes = pixelData.DetachPixelData();


					var piclib = Windows.Storage.ApplicationData.Current.TemporaryFolder;
					var fileName = (string)webView.GetValue(PngFileNameProperty) + ".png";
					var file = await piclib.CreateFileAsync(fileName, Windows.Storage.CreationCollisionOption.GenerateUniqueName);
					using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
					{
						var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
						encoder.SetPixelData(BitmapPixelFormat.Bgra8,
											BitmapAlphaMode.Ignore,
											//(uint)decoder.PixelWidth, (uint)decoder.PixelHeight,
											(uint)Math.Ceiling(webView.Width),(uint)Math.Ceiling(webView.Height),
											0, 0, bytes);
						await encoder.FlushAsync();
					}

					webView.Width = (int)webView.GetValue(BeforeWidthProperty);
					webView.Height = (int)webView.GetValue(BeforeHeightProperty);
					webView.Refresh();

					var toFileResult = new ToFileResult(false, file.Path);
					webView.SetValue(ToFileResultProperty, toFileResult);

				}
				catch (Exception e)
				{
					webView.Width = (int)webView.GetValue(BeforeWidthProperty);
					webView.Height = (int)webView.GetValue(BeforeHeightProperty);
					webView.Refresh();

					var toFileResult = new ToFileResult(true, e.InnerException?.Message ?? e.Message);
					webView.SetValue(ToFileResultProperty, toFileResult);

				}
			}
			webView.Refresh();
		}

		private void NavigationCompleteC(Windows.UI.Xaml.Controls.WebView webView, WebViewNavigationCompletedEventArgs args)
        {
			webView.NavigationCompleted -= NavigationCompleteC;
			if (webView.GetValue(TaskCompletionSourceProperty) is TaskCompletionSource<ToFileResult> onComplete)
			{
				if (webView.GetValue(ToFileResultProperty) is ToFileResult result)
				{
					System.Diagnostics.Debug.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerMemberName() + ": Complete[" + result.Result + "]");
					onComplete.SetResult(result);
				}
				else
				{
					onComplete.SetResult(new ToFileResult(true, "unknown error generating PNG."));
				}
			}
			else
				throw new Exception("Failed to get TaskCompletionSource for UWP WebView.ToPngAsync");
		}

		bool IsMainPageChild(Windows.UI.Xaml.Controls.WebView webView)
		{
			var currentPage = Forms9Patch.PageExtensions.FindCurrentPage(Xamarin.Forms.Application.Current?.MainPage);
			var rootPageRenderer = (Xamarin.Forms.Platform.UWP.PageRenderer)Platform.GetRenderer(currentPage);

			var result = rootPageRenderer.Children.Contains(webView);

			System.Diagnostics.Debug.WriteLine("IsMainPageChild : [" + result + "]  WebView.Parent=[" + webView.Parent + "]");

			return result;
		}

	}
}