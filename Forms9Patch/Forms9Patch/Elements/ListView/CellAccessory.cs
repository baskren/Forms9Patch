using System;
using Xamarin.Forms;

namespace Forms9Patch
{
	#region CellAccessory class
	/// <summary>
	/// Cell accessory.
	/// </summary>
	public class CellAccessory : BindableObject
	{
		#region Properties
		/// <summary>
		/// The text function property.
		/// </summary>
		public static readonly BindableProperty TextFunctionProperty = BindableProperty.Create("TextFunction", typeof(Func<Item, string>), typeof(CellAccessory), null);
		/// <summary>
		/// Gets or sets the text function.
		/// </summary>
		/// <value>The text function.</value>
		public Func<IItem, string> TextFunction
		{
			get { return (Func<IItem, string>)GetValue(TextFunctionProperty); }
			set { SetValue(TextFunctionProperty, value); }
		}

		/// <summary>
		/// The width property.
		/// </summary>
		public static readonly BindableProperty WidthProperty = BindableProperty.Create("Width", typeof(double), typeof(CellAccessory), 20.0);
		/// <summary>
		/// Gets or sets the width.
		/// </summary>
		/// <value>The width.</value>
		public double Width
		{
			get { return (double)GetValue(WidthProperty); }
			set { SetValue(WidthProperty, value); }
		}

		/// <summary>
		/// The horizonatal alignment property.
		/// </summary>
		public static readonly BindableProperty HorizonatalAlignmentProperty = BindableProperty.Create("HorizontalAlignment", typeof(TextAlignment), typeof(CellAccessory), TextAlignment.Center);
		/// <summary>
		/// Gets or sets the horizontal alignment.
		/// </summary>
		/// <value>The horizontal alignment.</value>
		public TextAlignment HorizontalAlignment
		{
			get { return (TextAlignment)GetValue(HorizonatalAlignmentProperty); }
			set { SetValue(HorizonatalAlignmentProperty, value); }
		}

		/// <summary>
		/// The vertical alignment property.
		/// </summary>
		public static readonly BindableProperty VerticalAlignmentProperty = BindableProperty.Create("VerticalAlignment", typeof(TextAlignment), typeof(CellAccessory), TextAlignment.Center);
		/// <summary>
		/// Gets or sets the vertical alignment.
		/// </summary>
		/// <value>The vertical alignment.</value>
		public TextAlignment VerticalAlignment
		{
			get { return (TextAlignment)GetValue(VerticalAlignmentProperty); }
			set { SetValue(VerticalAlignmentProperty, value); }
		}
		#endregion


		#region Events
		/// <summary>
		/// Occurs when tapped.
		/// </summary>
		public event EventHandler<CellAccessoryEventArgs> Tapped;
		#endregion

	}
	#endregion


	#region CellAccessoryTappedArgs
	/// <summary>
	/// Cell accessory event arguments.
	/// </summary>
	public class CellAccessoryEventArgs : EventArgs
	{
		/// <summary>
		/// Gets the Forms9Patch.IItem.
		/// </summary>
		/// <value>The item.</value>
		public IItem Item { get; private set; }
		/// <summary>
		/// Gets the current accessory text.
		/// </summary>
		/// <value>The text.</value>
		public string Text { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Forms9Patch.CellAccessoryEventArgs"/> class.
		/// </summary>
		/// <param name="item">Item.</param>
		/// <param name="text">Text.</param>
		public CellAccessoryEventArgs(IItem item, string text)
		{
			Item = item;
			Text = text;
		}
	}
	#endregion
}
