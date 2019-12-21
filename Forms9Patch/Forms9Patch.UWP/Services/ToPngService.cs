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
	public class ToPngService : IToPngService
	{
		readonly static DependencyProperty PngFileNameProperty = DependencyProperty.Register("PngFileName", typeof(string), typeof(ToPngService), null);
		readonly static DependencyProperty TaskCompletionSourceProperty = DependencyProperty.Register("OnPngComplete", typeof(TaskCompletionSource<ToPngResult>), typeof(ToPngService), null);
		//readonly static DependencyProperty WebViewProperty = DependencyProperty.Register("WebView", typeof(Windows.UI.Xaml.Controls.WebView), typeof(ToPngService), null);
		readonly static DependencyProperty HtmlStringProperty = DependencyProperty.Register("HtmlString", typeof(string), typeof(ToPngService), null);


		public async Task<ToPngResult> ToPngAsync(ActivityIndicatorPopup popup, string html, string fileName)
		{
			var taskCompletionSource = new TaskCompletionSource<ToPngResult>();
			ToPng(taskCompletionSource, popup, html, fileName);
			return await taskCompletionSource.Task;
		}

		public async Task<ToPngResult> ToPngAsync(ActivityIndicatorPopup popup, Xamarin.Forms.WebView webView, string fileName)
		{
			var taskCompletionSource = new TaskCompletionSource<ToPngResult>();
			ToPng(taskCompletionSource, webView, fileName);
			return await taskCompletionSource.Task;
		}



#pragma warning disable CS1998
		public void ToPng(TaskCompletionSource<ToPngResult> taskCompletionSource, ActivityIndicatorPopup popup, string html, string fileName)
		{
			Device.BeginInvokeOnMainThread(async () =>
			{
				var size = new Size(8.5, 11);
				if (Platform.GetRenderer(popup.WebView) is Xamarin.Forms.Platform.UWP.WebViewRenderer renderer)
				{
					var webView = renderer.Control;

					webView.DefaultBackgroundColor = Windows.UI.Colors.White;
					webView.Width = (size.Width - 0.5) * 72 / 2;
					webView.Height = 10; // (size.Height - 0.5) * 72,

					webView.Visibility = Visibility.Visible;

					webView.SetValue(PngFileNameProperty, fileName);
					webView.SetValue(TaskCompletionSourceProperty, taskCompletionSource);
					webView.SetValue(HtmlStringProperty, html);

					webView.NavigationCompleted += NavigationCompleteA;
					webView.NavigationFailed += WebView_NavigationFailed;

					webView.NavigateToString(html);
				}
			});
		}

		public void ToPng(TaskCompletionSource<ToPngResult> taskCompletionSource, Xamarin.Forms.WebView xfWebView, string fileName)
		{
			Device.BeginInvokeOnMainThread(async () =>
			{
				var size = new Size(8.5, 11);
				if (Platform.GetRenderer(xfWebView) is Xamarin.Forms.Platform.UWP.WebViewRenderer renderer)
				{
					var webView = renderer.Control;

					webView.SetValue(PngFileNameProperty, fileName);
					webView.SetValue(TaskCompletionSourceProperty, taskCompletionSource);

					webView.NavigationCompleted += NavigationCompleteA;
					webView.NavigationFailed += WebView_NavigationFailed;

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
			IsMainPageChild(webView);

			var contentSize = await webView.WebViewContentSizeAsync();
			webView.NavigationCompleted -= NavigationCompleteA;

			System.Diagnostics.Debug.WriteLine("A contentSize=[" + contentSize + "]");
			System.Diagnostics.Debug.WriteLine("A webView.Size=[" + webView.Width + "," + webView.Height + "] IsOnMainThread=[" + P42.Utils.Environment.IsOnMainThread + "]");

			webView.Width = contentSize.Width;
			webView.Height = contentSize.Height;
			webView.UpdateLayout();

			webView.NavigationCompleted += NavigationCompleteB;

			var html = (string)webView.GetValue(HtmlStringProperty);
			webView.NavigateToString(html);
		}

		private async void NavigationCompleteB(Windows.UI.Xaml.Controls.WebView webView, WebViewNavigationCompletedEventArgs args)
		{
			IsMainPageChild(webView);

			using (InMemoryRandomAccessStream ms = new InMemoryRandomAccessStream())
			{
				System.Diagnostics.Debug.WriteLine("B webView.Size=[" + webView.Width + "," + webView.Height + "] IsOnMainThread=[" + P42.Utils.Environment.IsOnMainThread + "]");
				try
				{
					//await Task.Delay(2000);
					await webView.CapturePreviewToStreamAsync(ms);

					var decoder = await BitmapDecoder.CreateAsync(ms);

					var transform = new BitmapTransform
					{
						ScaledHeight = (uint)decoder.PixelHeight,
						ScaledWidth = (uint)decoder.PixelWidth
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
											(uint)decoder.PixelWidth, (uint)decoder.PixelHeight,
											0, 0, bytes);
						await encoder.FlushAsync();
					}

					var onComplete = (TaskCompletionSource<ToPngResult>)webView.GetValue(TaskCompletionSourceProperty);
					System.Diagnostics.Debug.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerMemberName() + ": Complete[" + file.Path + "]");
					onComplete?.SetResult(new ToPngResult(false, file.Path));
				}
				catch (Exception e)
				{
					var onComplete = (TaskCompletionSource<ToPngResult>)webView.GetValue(TaskCompletionSourceProperty);
					onComplete?.SetResult(new ToPngResult(true, e.InnerException?.Message ?? e.Message));
				}
			}
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