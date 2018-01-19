using System.ComponentModel;
using CoreGraphics;
using Forms9Patch;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;


[assembly: ExportRenderer(typeof(Forms9Patch.Image), typeof(Forms9Patch.iOS.ImageRenderer))]
namespace Forms9Patch.iOS
{
	/// <summary>
	/// Forms9Patch Image renderer.
	/// </summary>
	public class ImageRenderer : ViewRenderer<Image, UIImageView>
	{
		Image _oldImage;
		ImageViewManager _imageViewManager;

		/// <summary>
		/// Raises the element changed event.
		/// </summary>
		/// <param name="e">E.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
		{
			if (Control == null) {
				SetNativeControl(new UIImageView(CGRect.Empty) {
					ContentMode = UIViewContentMode.ScaleToFill,
					ClipsToBounds = true
				});
			}
			if (e.OldElement != null) {
				_imageViewManager.LayoutComplete -= OnImageLayoutComplete;
				_imageViewManager.Dispose ();
				_imageViewManager = null;
			}
			base.OnElementChanged(e);
			if (e.NewElement != null) {
				if (_imageViewManager == null) {
					_imageViewManager = new ImageViewManager (Control, e.NewElement);
					_imageViewManager.LayoutComplete += OnImageLayoutComplete;
				}
				_imageViewManager.Element = e.NewElement;
				_oldImage = e.NewElement;
#pragma warning disable 4014
				_imageViewManager.LayoutImage (_oldImage);
#pragma warning restore 4014
			} else if (_imageViewManager!=null) {
				_imageViewManager.LayoutComplete -= OnImageLayoutComplete;
				_imageViewManager.Dispose ();
				_imageViewManager = null;
			}
		}

		/// <summary>
		/// Raises the element property changed event.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			if (e.PropertyName == Xamarin.Forms.Image.SourceProperty.PropertyName ||
			    e.PropertyName == Image.CapInsetsProperty.PropertyName ||
			    e.PropertyName == Image.FillProperty.PropertyName) {
#pragma warning disable 4014
				_imageViewManager.LayoutImage(_oldImage);
#pragma warning restore 4014
			} else if (e.PropertyName == Xamarin.Forms.Image.IsOpaqueProperty.PropertyName) {
				Control.Opaque = Element.IsOpaque;
			} else if (e.PropertyName == Image.TintColorProperty.PropertyName) {
				if (_imageViewManager.Tintable && Element.TintColor != Color.Default)
					Control.TintColor = Element.TintColor.ToUIColor();
				else 
					_imageViewManager.RelayoutImage (_oldImage);
			} else if (e.PropertyName == VisualElement.WidthProperty.PropertyName) {
				SetNeedsDisplay ();
			}

		}
			
		/// <summary>
		/// Draw the specified rect.
		/// </summary>
		/// <param name="rect">Rect.</param>
		public override void Draw (CGRect rect)
		{
			_imageViewManager.Draw (rect);
		}

		void OnImageLayoutComplete(bool hasImage) {
			BackgroundColor = Element.BackgroundColor.ToUIColor ();
			SetNeedsDisplay ();
		}

		/// <summary>
		/// The is disposed.
		/// </summary>
		bool isDisposed;

		/// <summary>
		/// Dispose the specified disposing.
		/// </summary>
		/// <param name="disposing">If set to <c>true</c> disposing.</param>
		protected override void Dispose (bool disposing)
		{
			if (isDisposed) {
				return;
			}
			UIImage image;
			if (disposing && Control != null && (image = Control.Image) != null) {
				image.Dispose ();
			}
			isDisposed = true;
			base.Dispose (disposing);
		}

	}
}
