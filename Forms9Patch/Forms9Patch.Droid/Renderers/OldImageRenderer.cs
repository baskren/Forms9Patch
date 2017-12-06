using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Views;

namespace Forms9Patch.Droid
{
    /// <summary>
    /// Forms9Patch Image renderer.
    /// </summary>
    public class ImageRenderer : ViewRenderer<Image, Android.Widget.ImageView>//global::Android.Views.View>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Forms9Patch.Droid.ImageRenderer"/> class.
        /// </summary>
        public ImageRenderer()
        {
            AutoPackage = false;
        }

        Image _oldImage;
        ImageViewManager _imageViewManager;

        /// <summary>
        /// Raises the element changed event.
        /// </summary>
        /// <param name="e">E.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                if (_imageViewManager == null)
                {
                    SetNativeControl(new global::Android.Widget.ImageView(Context));
                    _imageViewManager = new ImageViewManager(Control, e.NewElement);
                    _imageViewManager.LayoutComplete += OnBackgroundImageLayoutComplete;
                }
                _oldImage = e.NewElement;//.BackgroundImage;
#pragma warning disable 4014
                _imageViewManager.LayoutImage(_oldImage);
#pragma warning restore 4014
            }
            else
            {
                _imageViewManager.LayoutComplete -= OnBackgroundImageLayoutComplete;
                _imageViewManager.Dispose();
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
            if (e.PropertyName == ShapeBase.OutlineColorProperty.PropertyName ||
                e.PropertyName == ShapeBase.HasShadowProperty.PropertyName ||
                e.PropertyName == ShapeBase.OutlineWidthProperty.PropertyName ||
                e.PropertyName == ShapeBase.OutlineRadiusProperty.PropertyName ||
                e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName)
            {
#pragma warning disable 4014
                _imageViewManager.LayoutImage(_oldImage);
#pragma warning restore 4014
            }
            else if (e.PropertyName == VisualElement.WidthProperty.PropertyName)
            {
                //SetNeedsDisplay ();
            }
            else if (e.PropertyName == Image.TintColorProperty.PropertyName)
            {
                //_imageViewManager.RelayoutImage (null);  // this line causes image to not reappear after tint change on Android (ImageButton examples)
                _imageViewManager.RelayoutImage(_oldImage);
            }
            else if (e.PropertyName == ContentView.BackgroundImageProperty.PropertyName)
            {
                if (_oldImage != null)
                    _oldImage.PropertyChanged -= OnBackgroundImagePropertyChanged;
                _oldImage = Element;//.BackgroundImage;
                if (_oldImage != null)
                    _oldImage.PropertyChanged += OnBackgroundImagePropertyChanged;
            }
        }

        void OnBackgroundImagePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // No Wait or await here because we want RenderBackgroundImage to run in parallel
#pragma warning disable 4014
            _imageViewManager.LayoutImage(_oldImage);
#pragma warning restore 4014
        }

        void OnBackgroundImageLayoutComplete(bool hasImage)
        {
            //if (!hasImage)
            //	Background = new RoundRectViewManager (Element);
            //else
            SetBackgroundColor(Element.BackgroundColor.ToAndroid());
        }
    }
}
