using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using System;


namespace Forms9Patch.Droid
{
    /// <summary>
    /// Forms9Patch Layout renderer.
    /// </summary>
    public class OldLayoutRenderer<TElement> : ViewRenderer<TElement, Android.Widget.RelativeLayout> where TElement : View, IBackgroundImage
    {
        Image _oldImage;
        ImageViewManager _imageViewManager;

        string text
        {
            get
            {
                var contentView = Element as ContentView;
                if (contentView != null)
                {
                    var label = contentView.Content as Xamarin.Forms.Label;
                    if (label != null)
                        return label.Text;
                }
                return "";
            }
        }


        /// <summary>
        /// Raises the element changed event.
        /// </summary>
        /// <param name="e">E.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<TElement> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                if (_imageViewManager == null)
                {
                    SetNativeControl(new Android.Widget.RelativeLayout(Context));
                    _imageViewManager = new ImageViewManager(Control, e.NewElement);
                    _imageViewManager.LayoutComplete += OnBackgroundImageLayoutComplete;
                }
                _oldImage = e.NewElement.BackgroundImage;
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
            //System.Diagnostics.Debug.WriteLine ("LayoutRenderer<>.OnElementPropertyChanged txt=["+text+"] w=["+Element.Width+"] h=["+Element.Height+"] propertyName=["+e.PropertyName+"]");
            base.OnElementPropertyChanged(sender, e);
            if (
                e.PropertyName == ShapeBase.OutlineColorProperty.PropertyName
                || e.PropertyName == ShapeBase.ShadowInvertedProperty.PropertyName
                || e.PropertyName == ShapeBase.OutlineRadiusProperty.PropertyName
                || e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName)
            {
                var box = Element as IShape;
                if (box != null)
                {
                    if (Android.OS.Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.JellyBean)
#pragma warning disable CS0618 // Type or member is obsolete
                        SetBackgroundDrawable(new RoundRectDrawable(box));
#pragma warning restore CS0618 // Type or member is obsolete
                    else
                        Background = new RoundRectDrawable(box);
                }
            }
            else if (e.PropertyName == ContentView.BackgroundImageProperty.PropertyName)
            {
                if (_oldImage != null)
                    _oldImage.PropertyChanged -= OnBackgroundImagePropertyChanged;
                _oldImage = Element.BackgroundImage;
                if (_oldImage != null)
                    _oldImage.PropertyChanged += OnBackgroundImagePropertyChanged;
#pragma warning disable 4014
                _imageViewManager.LayoutImage(_oldImage);
#pragma warning restore 4014
            }
        }

        void LayoutImage()
        {
#pragma warning disable 4014
            _imageViewManager.LayoutImage(_oldImage);
#pragma warning restore 4014
        }

        void OnBackgroundImagePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // No Wait or await here because we want RenderBackgroundImage to run in parallel
            if (e.PropertyName == Xamarin.Forms.Image.SourceProperty.PropertyName)
            {
#pragma warning disable 4014
                _imageViewManager.LayoutImage(null);
                _imageViewManager.LayoutImage(_oldImage);
#pragma warning restore 4014
            }
        }

        void OnBackgroundImageLayoutComplete(bool hasImage)
        {
            if (!hasImage)
            {
                var roundedBoxElement = Element as IShape;
                if (roundedBoxElement != null)
                {
                    if (Android.OS.Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.JellyBean)
#pragma warning disable CS0618 // Type or member is obsolete
                        SetBackgroundDrawable(new RoundRectDrawable(roundedBoxElement));
#pragma warning restore CS0618 // Type or member is obsolete
                    else
                        Background = new RoundRectDrawable(roundedBoxElement);
                }
            }
            else
                SetBackgroundColor(Element.BackgroundColor.ToAndroid());

        }

        protected override void Dispose(bool disposing)
        {
            //System.Diagnostics.Debug.WriteLine("{0}[{1}] disposing=["+disposing+"]", PCL.Utils.ReflectionExtensions.CallerString(), GetType());
            if (disposing)
            {
                //System.Diagnostics.Debug.WriteLine("{0}[{1}] disposing", PCL.Utils.ReflectionExtensions.CallerString(), GetType());
                SetBackgroundColor(Color.Transparent.ToAndroid());
                _oldImage = null;
                _imageViewManager?.Dispose();
                _imageViewManager = null;
                if (Android.OS.Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.JellyBean)
#pragma warning disable CS0618 // Type or member is obsolete
                    SetBackgroundDrawable(null);
#pragma warning restore CS0618 // Type or member is obsolete
                else
                    Background = null;
            }
            base.Dispose(disposing);
        }

        /*
		~LayoutRenderer()
		{
			//System.Diagnostics.Debug.WriteLine("{0}[{1}] ", PCL.Utils.ReflectionExtensions.CallerString(), GetType());
			_oldImage = null;
			_imageViewManager?.Dispose();
			_imageViewManager = null;
			try
			{
				if (Android.OS.Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.JellyBean)
					SetBackgroundDrawable(null);
				else
					Background = null;
			}
			catch (Exception e)
			{
				System.Console.WriteLine("This exception happened because Forms9Patch was being a good citizen and trying to dispose of all of it's objects.  Unfortunately, no good deed good unpunished.  In this case, the object Forms9Patch was trying to disposed had already been disposed.  Would be nice if Microsoft would just ignored this.  You're better - because you can!"); 
			}
		}
		*/
    }

}
