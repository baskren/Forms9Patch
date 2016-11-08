using System;
using Xamarin.Forms;

namespace Forms9Patch
{
	/// <summary>
	/// Describes a Swipe action button that appears when a cell is swiped 
	/// </summary>
	public class SwipeAction : BindableObject
	{
		#region Properties
		/// <summary>
		/// The icon image source property backing store;
		/// </summary>
		public static readonly BindableProperty IconImageSourceProperty = BindableProperty.Create("IconImageSource", typeof(Forms9Patch.ImageSource), typeof(SwipeAction), default(Forms9Patch.ImageSource));
		/// <summary>
		/// Gets or sets the icon image source - an alternative to IconText.
		/// </summary>
		/// <value>The icon image source.</value>
		public Forms9Patch.ImageSource IconImageSource
		{
			get { return (Forms9Patch.ImageSource)GetValue(IconImageSourceProperty); }
			set { SetValue(IconImageSourceProperty, value); }
		}

		/// <summary>
		/// The icon text property backing store.
		/// </summary>
		public static readonly BindableProperty IconTextProperty = BindableProperty.Create("IconText", typeof(string), typeof(SwipeAction), default(string));
		/// <summary>
		/// Gets or sets the icon text - an alternative to IconImageSource.
		/// </summary>
		/// <value>The icon text.</value>
		public string IconText
		{
			get { return (string)GetValue(IconTextProperty); }
			set { SetValue(IconTextProperty, value); }
		}

		/// <summary>
		/// The text property backing store.
		/// </summary>
		public static readonly BindableProperty TextProperty = BindableProperty.Create("Text", typeof(string), typeof(SwipeAction), default(string));
		/// <summary>
		/// Gets or sets the text (can be in HTML format.
		/// </summary>
		/// <value>The html text.</value>
		public string Text
		{
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}

		/// <summary>
		/// The text color property.
		/// </summary>
		public static readonly BindableProperty TextColorProperty = BindableProperty.Create("TextColor", typeof(Color), typeof(SwipeAction), Color.White);
		/// <summary>
		/// Gets or sets the color of the text and icon (image).
		/// </summary>
		/// <value>The color of the text.</value>
		public Color TextColor
		{
			get { return (Color)GetValue(TextColorProperty); }
			set { SetValue(TextColorProperty, value); }
		}

		/// <summary>
		/// The swipe executable property backing store.
		/// </summary>
		public static readonly BindableProperty SwipeExecutableProperty = BindableProperty.Create("SwipeExecutable", typeof(bool), typeof(SwipeAction), default(bool));
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:Forms9Patch.SwipeAction"/> swipe executable.
		/// Only used if this SwipeAction is at the top of the stack of SwipeActions.
		/// </summary>
		/// <value><c>true</c> if swipe executable; otherwise, <c>false</c>.</value>
		public bool SwipeExecutable
		{
			get { return (bool)GetValue(SwipeExecutableProperty); }
			set { SetValue(SwipeExecutableProperty, value); }
		}

		/// <summary>
		/// The background color property backing store.
		/// </summary>
		public static readonly BindableProperty BackgroundColorProperty = BindableProperty.Create("BackgroundColor", typeof(Color), typeof(SwipeAction), default(Color));
		/// <summary>
		/// Gets or sets the color of the background.
		/// </summary>
		/// <value>The color of the background.</value>
		public Color BackgroundColor
		{
			get { return (Color)GetValue(BackgroundColorProperty); }
			set { SetValue(BackgroundColorProperty, value); }
		}

		/// <summary>
		/// The action property backing store.
		/// </summary>
		public static readonly BindableProperty ActionProperty = BindableProperty.Create("Action", typeof(Action<object,object>), typeof(SwipeAction), default(Action<object,object>));
		/// <summary>
		/// Gets or sets the action to be performed if activated.
		/// </summary>
		/// <value>The action.</value>
		public Action<object,object> Action
		{
			get { return (Action<object,object>)GetValue(ActionProperty); }
			set { SetValue(ActionProperty, value); }
		}

		#endregion

		public SwipeAction()
		{
		}


		#region Change management
		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);
			if (propertyName == IconTextProperty.PropertyName && IconTextProperty != null)
				IconImageSource = null;
			else if (propertyName == IconImageSourceProperty.PropertyName && IconImageSource != null)
				IconText = null;
		}
		#endregion
	}
}
