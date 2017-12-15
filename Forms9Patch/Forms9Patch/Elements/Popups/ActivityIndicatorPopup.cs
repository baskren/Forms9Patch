using System;
using Xamarin.Forms;
namespace Forms9Patch
{
	/// <summary>
	/// Activity indicator full page overlay.
	/// </summary>
	public class ActivityIndicatorPopup : ModalPopup
	{
		/// <summary>
		/// The color property.
		/// </summary>
		public static readonly BindableProperty ColorProperty = BindableProperty.Create("Color", typeof(Color), typeof(ActivityIndicatorPopup), Color.Blue);
		/// <summary>
		/// Gets or sets the color.
		/// </summary>
		/// <value>The color.</value>
		public Color Color
		{
			get { return (Color)GetValue(ColorProperty); }
			set { SetValue(ColorProperty, value); }
		}

		/// <summary>
		/// Create this instance.
		/// </summary>
		public static ActivityIndicatorPopup Create()
		{
			var indicator = new ActivityIndicatorPopup();
			indicator.IsVisible = true;
			return indicator;
		}


		#region Fields
		ActivityIndicator _indicator = new ActivityIndicator
		{
			BackgroundColor = Color.Transparent,
			Color = Color.Blue,
           
		};
		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Forms9Patch.ActivityIndicatorPopup"/> class.
		/// </summary>
		public ActivityIndicatorPopup() 
		{
            if (Device.RuntimePlatform == Device.UWP)
                _indicator.WidthRequest = 300;
			Content = _indicator;

			CancelOnPageOverlayTouch = false;
			BackgroundColor = Color.FromRgba(0,0,0,1);
			_indicator.IsVisible = true;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Forms9Patch.ActivityIndicatorPopup"/> class.
		/// </summary>
		/// <param name="target">Target.</param>
		[Obsolete]
		public ActivityIndicatorPopup(VisualElement target) : base(target)
		{
			throw new NotSupportedException(PCL.Utils.ReflectionExtensions.CallerMemberName() + " is obsolete.");
		}

		/// <summary>
		/// Ons the property changed.
		/// </summary>
		/// <param name="propertyName">Property name.</param>
		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);
			if (propertyName == ColorProperty.PropertyName)
				_indicator.Color = Color;
			if (propertyName == IsVisibleProperty.PropertyName)
			{
				_indicator.IsVisible = IsVisible;
				_indicator.IsRunning = IsVisible;
			}
		}
	}
}
