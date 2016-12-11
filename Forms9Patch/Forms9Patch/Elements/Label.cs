using Xamarin.Forms;
using System;
using PCL.Utils;
using Xamarin.Forms.Internals;

namespace Forms9Patch
{
	/// <summary>
	/// Forms9Patch.Label
	/// </summary>
	[ContentProperty("HtmlText")]
	public class Label : View, IFontElement
	{

		#region Properties
		internal static readonly BindableProperty FontProperty = BindableProperty.Create ("Font", typeof(Font), typeof(Label), default(Font), BindingMode.OneWay, null, new BindableProperty.BindingPropertyChangedDelegate (FontStructPropertyChanged), null, null, null);
		internal Font Font {
			get { return (Font)GetValue (FontProperty); }
			set { SetValue (FontProperty, value); }
		}

		/// <summary>
		/// Backing store for the font family property.
		/// </summary>
		public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create ("FontFamily", typeof(string), typeof(Label), null, BindingMode.OneWay, null, new BindableProperty.BindingPropertyChangedDelegate (OnFontFamilyChanged), null, null, null);
		/// <summary>
		/// Gets or sets the font family.
		/// </summary>
		/// <value>The font family.</value>
		public string FontFamily {
			get { return (string)GetValue (FontFamilyProperty); }
			set { SetValue (FontFamilyProperty, value); }
		}

		/// <summary>
		/// Backing store for the font size property.
		/// </summary>
		public static readonly BindableProperty FontSizeProperty = BindableProperty.Create ("FontSize", typeof(double), typeof(Label), -1.0, BindingMode.OneWay, null, new BindableProperty.BindingPropertyChangedDelegate (OnFontSizeChanged));
		/// <summary>
		/// Gets or sets the size of the font.
		/// </summary>
		/// <value>The size of the font.</value>
		[TypeConverter (typeof(FontSizeConverter))]
		public double FontSize {
			get { return (double)GetValue (FontSizeProperty); }
			set { SetValue (FontSizeProperty, value); }
		}

		/// <summary>
		/// Backing store for the font attributes property.
		/// </summary>
		public static readonly BindableProperty FontAttributesProperty = BindableProperty.Create ("FontAttributes", typeof(FontAttributes), typeof(Label), FontAttributes.None, BindingMode.OneWay, null, new BindableProperty.BindingPropertyChangedDelegate (OnFontAttributesChanged), null, null, null);
		/// <summary>
		/// Gets or sets the font attributes.
		/// </summary>
		/// <value>The font attributes.</value>
		public FontAttributes FontAttributes {
			get { return (FontAttributes)GetValue (FontAttributesProperty); }
			set { SetValue (FontAttributesProperty, value); }
		}

		/// <summary>
		/// Backing store for the text color property.
		/// </summary>
		public static readonly BindableProperty TextColorProperty = Xamarin.Forms.Label.TextColorProperty;
		/// <summary>
		/// Gets or sets the color of the text.
		/// </summary>
		/// <value>The color of the text.</value>
		public Color TextColor {
			get { return (Color)GetValue (TextColorProperty); }
			set { SetValue (TextColorProperty, value); }
		}

		/// <summary>
		/// Backing store for the line break mode property.
		/// </summary>
		public static readonly BindableProperty LineBreakModeProperty = BindableProperty.Create ("LineBreakMode", typeof(LineBreakMode), typeof(Label), LineBreakMode.WordWrap, BindingMode.OneWay, null, 
			(bindable, oldvalue, newvalue) => { ((Label)bindable).InvalidateMeasure (); }
		);
		/// <summary>
		/// Gets or sets the line break mode.
		/// </summary>
		/// <value>The line break mode.</value>
		public LineBreakMode LineBreakMode {
			get { return (LineBreakMode)GetValue (LineBreakModeProperty); }
			set { SetValue (LineBreakModeProperty, value); }
		}

		/// <summary>
		/// Backing store for the text property.
		/// </summary>
		public static readonly BindableProperty TextProperty = BindableProperty.Create ("Text", typeof(string), typeof(Label));
		/// <summary>
		/// Gets or sets the unmarked-up text.
		/// </summary>
		/// <value>The text.</value>
		public string Text {
			get { return (string)GetValue (TextProperty); }
			set { 
				SetValue (TextProperty, value); 
			}
		}

		/// <summary>
		/// Backing store for the formatted text property.
		/// </summary>
		public static readonly BindableProperty HtmlTextProperty = BindableProperty.Create ("HtmlText", typeof(string), typeof(Label));
		/// <summary>
		/// Gets or sets the formatted text.
		/// </summary>
		/// <value>The formatted text.</value>
		public string HtmlText {
			get { 
				return (string)GetValue (HtmlTextProperty); 
			}
			set { 
				SetValue (HtmlTextProperty, value); 
			}
		}

		// not public, so not bindable!
		internal static readonly BindableProperty F9PFormattedStringProperty = BindableProperty.Create("F9PFormattedString", typeof(F9PFormattedString), typeof(Label), null);
		internal F9PFormattedString F9PFormattedString
		{
			get { return (F9PFormattedString)GetValue(F9PFormattedStringProperty); }
			set { SetValue(F9PFormattedStringProperty, value); }
		}


		/// <summary>
		/// Backing store for the horizontal text alignment property.
		/// </summary>
		public static readonly BindableProperty HorizontalTextAlignmentProperty = BindableProperty.Create ("HorizontalTextAlignment", typeof(TextAlignment), typeof(Label), TextAlignment.Start, BindingMode.OneWay, null, new BindableProperty.BindingPropertyChangedDelegate (OnHorizontalTextAlignmentPropertyChanged), null, null, null);
		/// <summary>
		/// Gets or sets the horizontal text alignment.
		/// </summary>
		/// <value>The horizontal text alignment.</value>
		public TextAlignment HorizontalTextAlignment {
			get { return (TextAlignment)GetValue (HorizontalTextAlignmentProperty); }
			set { SetValue (HorizontalTextAlignmentProperty, value); }
		}

		/// <summary>
		/// Backing store for the vertical text alignment property.
		/// </summary>
		public static readonly BindableProperty VerticalTextAlignmentProperty = BindableProperty.Create ("VerticalTextAlignment", typeof(TextAlignment), typeof(Label), TextAlignment.Start, BindingMode.OneWay, null, new BindableProperty.BindingPropertyChangedDelegate (OnVerticalTextAlignmentPropertyChanged), null, null, null);
		/// <summary>
		/// Gets or sets the vertical text alignment.
		/// </summary>
		/// <value>The vertical text alignment.</value>
		public TextAlignment VerticalTextAlignment {
			get { return (TextAlignment)GetValue (VerticalTextAlignmentProperty); }
			set { SetValue (VerticalTextAlignmentProperty, value); }
		}

		/// <summary>
		/// The backing store for the Fit property.
		/// </summary>
		public static readonly BindableProperty FitProperty = BindableProperty.Create("Fit", typeof(LabelFit), typeof(Label), LabelFit.None);
		/// <summary>
		/// Gets or sets the fit method.  Ignored if the Width and Height is not fixed by a parent, HeightRequest, and/or WidthRequest.
		/// </summary>
		/// <value>The fit.</value>
		public LabelFit Fit
		{
			get { return (LabelFit)GetValue(FitProperty); }
			set { SetValue(FitProperty, value); }
		}

		/// <summary>
		/// The backing store for the lines property.
		/// </summary>
		public static readonly BindableProperty LinesProperty = BindableProperty.Create("Lines", typeof(int), typeof(Label), 0);
		/// <summary>
		/// Gets or sets the number of lines used in a fit.  If zero and fit is not LabelFit.None or ignored, will maximize the font size to best width and height with minimum number of lines.
		/// </summary>
		/// <value>The lines.</value>
		public int Lines
		{
			get { return (int)GetValue(LinesProperty); }
			set { SetValue(LinesProperty, value); }
		}

		/// <summary>
		/// The backing store for the minimum font size property.
		/// </summary>
		public static readonly BindableProperty MinFontSizeProperty = BindableProperty.Create("MinFontSize", typeof(double), typeof(Label), -1.0);
		/// <summary>
		/// Gets or sets the minimum size of the font allowed during an autofit. 
		/// </summary>
		/// <value>The minimum size of the font.  Default=4</value>
		public double MinFontSize
		{
			get { return (double)GetValue(MinFontSizeProperty); }
			set { SetValue(MinFontSizeProperty, value); }
		}

		/// <summary>
		/// The backing store for the max font size property.
		/// </summary>
		public static readonly BindableProperty MaxFontSizeProperty = BindableProperty.Create("MaxFontSize", typeof(double), typeof(Label), -1.0);
		/// <summary>
		/// Gets or sets the maximum size of the font allowed during an autofit.
		/// </summary>
		/// <value>The size of the max font.</value>
		public double MaxFontSize
		{
			get { return (double)GetValue(MaxFontSizeProperty); }
			set { SetValue(MaxFontSizeProperty, value); }
		}

		/// <summary>
		/// The backing store for fixed size property.
		/// </summary>
		public static readonly BindableProperty IsDynamicallySizedProperty = BindableProperty.Create("IsDynamicallySized", typeof(bool), typeof(Label), true, BindingMode.TwoWay);
		/// <summary>
		/// Gets if the size of the label has been fixed by a parent element, the HeightRequest and/or the WidthRequest properties.
		/// </summary>
		/// <value>Is the label fixed in size.</value>
		public bool IsDynamicallySized
		{
			get { 
				return (bool)GetValue(IsDynamicallySizedProperty); 
			}
			private set { 
				SetValue(IsDynamicallySizedProperty, value); 
			}
		}

		internal static readonly BindablePropertyKey ActualFontSizePropertyKey = BindableProperty.CreateReadOnly("ActualFontSize", typeof(double), typeof(Label), -1.0);
		/// <summary>
		/// Backing store for the actual font size property after fitting.
		/// </summary>
		public static readonly BindableProperty ActualFontSizeProperty = ActualFontSizePropertyKey.BindableProperty;
		/// <summary>
		/// Gets the actual size of the font (after fitting).
		/// </summary>
		/// <value>The actual size of the font.</value>
		public double ActualFontSize
		{
			get { return (double)GetValue(ActualFontSizeProperty); }
			internal set { SetValue(ActualFontSizePropertyKey, value); }
		}


		#endregion


		#region Static Methods
		static void FontStructPropertyChanged (BindableObject bindable, object oldValue, object newValue)
		{
			//System.Diagnostics.Debug.WriteLine("FontStructPropertyChanged enter");
			var label = (Label)bindable;
			if (label.cancelEvents) {
				return;
			}
			label.cancelEvents = true;
			var newFont = (Font)newValue;
			if (newFont == Font.Default) {
				label.FontFamily = null;
				label.FontSize = Device.GetNamedSize (NamedSize.Default, label);
				label.FontAttributes = FontAttributes.None;
			}
			else {
				label.FontFamily = newFont.FontFamily;
				label.FontSize = newFont.UseNamedSize ? Device.GetNamedSize (newFont.NamedSize, label.GetType ()) : newFont.FontSize;
				label.FontAttributes = newFont.FontAttributes;
			}
			label.cancelEvents = false;
			//System.Diagnostics.Debug.WriteLine("FontStructPropertyChanged label.InvalidateMeasure()");
			label.InvalidateMeasure ();
			//System.Diagnostics.Debug.WriteLine("FontStructPropertyChanged exit");
		}

		static void OnFontAttributesChanged (BindableObject bindable, object oldValue, object newValue)
		{
			var label = (Label)bindable;
			if (label.cancelEvents) 
				return;
			label.cancelEvents = true;
			var fontAttributes = (FontAttributes)newValue;
			var text = (string) label.GetValue(FontFamilyProperty);
			var size = (double)label.GetValue (FontSizeProperty);
			label.Font = text != null ? Font.OfSize (text, size).WithAttributes (fontAttributes) : Font.SystemFontOfSize (size, fontAttributes);
			label.cancelEvents = false;
			label.InvalidateMeasure ();
		}

		static void OnFontFamilyChanged (BindableObject bindable, object oldValue, object newValue)
		{
			var label = (Label)bindable;
			if (label.cancelEvents) 
				return;
			label.cancelEvents = true;
			var size = (double)label.GetValue (FontSizeProperty);
			var attr = (FontAttributes)label.GetValue (FontAttributesProperty);
			var text = (string)newValue;
			label.Font = text != null ? Font.OfSize (text, size).WithAttributes (attr) : Font.SystemFontOfSize (size, attr);
			label.cancelEvents = false;
			label.InvalidateMeasure ();
		}

		static void OnFontSizeChanged (BindableObject bindable, object oldValue, object newValue)
		{
			var label = (Label)bindable;
			if (label.cancelEvents) 
				return;
			label.cancelEvents = true;
			var size = (double)newValue;
			var text = (string) label.GetValue(FontFamilyProperty);
			var attr = (FontAttributes)label.GetValue (FontAttributesProperty);
			label.Font = text != null ? Font.OfSize (text, size).WithAttributes (attr) : Font.SystemFontOfSize (size, attr);
			label.cancelEvents = false;
			label.InvalidateMeasure ();
		}

		static void OnHorizontalTextAlignmentPropertyChanged (BindableObject bindable, object oldValue, object newValue)
		{
			((Label)bindable).OnPropertyChanged ("XAlign");
		}
		static void OnVerticalTextAlignmentPropertyChanged (BindableObject bindable, object oldValue, object newValue)
		{
			((Label)bindable).OnPropertyChanged ("YAlign");
			//((Label)bindable).InvalidateMeasure();
		}

		#endregion


		#region Constructor & Fields
		bool cancelEvents;
		static int instances;
		readonly internal int _id;
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Forms9Patch.Label"/> class.
		/// </summary>
		public Label() {
			_id=instances++;


		}
		#endregion


		#region PropertyChange 
		/// <summary>
		/// Ons the property changed.
		/// </summary>
		/// <returns>The property changed.</returns>
		/// <param name="propertyName">Property name.</param>
		protected override void OnPropertyChanged(string propertyName = null)
		{
			//System.Diagnostics.Debug.WriteLine("["+(HtmlText ?? Text)+"]Forms9Patch.Label.OnPropertyChanged("+propertyName+")");
			if (propertyName == HtmlTextProperty.PropertyName)
			{
				if (HtmlText != null)
				{
					F9PFormattedString = new HTMLMarkupString(HtmlText);
					Text = null;
				}
				else
					F9PFormattedString = null;
			}
			else if (propertyName == TextProperty.PropertyName && Text!=null)
			{
				F9PFormattedString = null;
				HtmlText = null;
			}
			base.OnPropertyChanged(propertyName);
			if (propertyName == LinesProperty.PropertyName 
				|| propertyName == FitProperty.PropertyName 
				|| propertyName == HtmlTextProperty.PropertyName 
				||propertyName == TextProperty.PropertyName 
			    //|| propertyName == WidthProperty.PropertyName 
			    //|| propertyName == HeightProperty.PropertyName
			   )
			{
				//System.Diagnostics.Debug.WriteLine("\t["+(HtmlText ?? Text)+"]Forms9Patch.Label.OnPropertyChanged(" + propertyName + ")");
				InvalidateMeasure();
			}

			//System.Diagnostics.Debug.WriteLine("");
			//System.Diagnostics.Debug.WriteLine("\t["+(HtmlText ?? Text)+"]Forms9Patch.Label.OnPropertyChanged(" + propertyName + ")");
		}

		/// <summary>
		/// Invalidates the measure.
		/// </summary>
		/// <returns>The measure.</returns>
		protected override void InvalidateMeasure()
		{
			//System.Diagnostics.Debug.WriteLine("["+(HtmlText ?? Text)+"]>>InvalidateMeasure");
			IsDynamicallySized = false;
			//Sized = false;
			base.InvalidateMeasure();

			/*
			if (!IsDynamicallySized && Width > 0 && Height > 0)// || !Sized )
			{
				if (HtmlText != null || Text != null)
				//System.Diagnostics.Debug.WriteLine("\t["+(HtmlText ?? Text)+"]InvalidateMeasure.OnMeasure()");
					OnMeasure(Width, Height);
			}
			*/
			//System.Diagnostics.Debug.WriteLine("\t["+(HtmlText ?? Text)+"]InvalidateMeasure>>");

		}

		internal void InternalInvalidateMeasure()
		{
			InvalidateMeasure();
		}

		#endregion


		#region Operators
		/// <param name="label">Label.</param>
		public static explicit operator string (Label label) {
			return label?.HtmlText ?? label?.Text;
		}
		#endregion


		#region for use by MaterialButton and Autofit
		internal bool MinimizeHeight;

		internal Action SizeAndAlign;

		/// <summary>
		/// Ons the size allocated.
		/// </summary>
		/// <returns>The size allocated.</returns>
		/// <param name="width">Width.</param>
		/// <param name="height">Height.</param>
		protected override void OnSizeAllocated(double width, double height)
		{
			//System.Diagnostics.Debug.WriteLine("["+(HtmlText ?? Text)+"]OnSizeAllocated("+width+","+height+")");
			base.OnSizeAllocated(width, height);
			//InvalidateMeasure();
			//System.Diagnostics.Debug.WriteLine("OnSizeAllocated");
			//System.Diagnostics.Debug.WriteLine("{0}[{1}] SizeAndAlign?.Invoke()", ReflectionExtensions.CallerMemberName(), ReflectionExtensions.CallerLineNumber());
			//SizeAndAlign?.Invoke();
		}


		/// <param name="widthConstraint">The available width that a parent element can allocated to a child. Value will be between 0 and double.PositiveInfinity.</param>
		/// <param name="heightConstraint">The available height that a parent element can allocated to a child. Value will be between 0 and double.PositiveInfinity.</param>
		/// <summary>
		/// Gets the size request.
		/// </summary>
		/// <returns>The size request.</returns>
		[Obsolete("Use OnMeasure")]
		public override SizeRequest GetSizeRequest (double widthConstraint, double heightConstraint)
		{
			IsDynamicallySized = true;
			// this is not called if the parent sets this element's size (ex: putting it into a frame)
			//System.Diagnostics.Debug.WriteLine("["+(HtmlText ?? Text)+"]GetSizeRequest("+widthConstraint+","+heightConstraint+") enter");
			var result = base.GetSizeRequest (widthConstraint, heightConstraint);
			//IsDynamicallySized = double.IsPositiveInfinity(widthConstraint) || heightConstraint > result.Request.Height;
			//_widthImposedByParent = !double.IsPositiveInfinity(widthConstraint);
			//_heightImposedByParent = !double.IsPositiveInfinity(heightConstraint);
			//_widthImposedByParent = widthConstraint <= result.Request.Width;
			//var _heightImposedByParent = heightConstraint <= result.Request.Height;
			//HasImposedSize = HasImposedWidth && HasImposedHeight;
			//System.Diagnostics.Debug.WriteLine("\tText=["+Text+"]");
			//System.Diagnostics.Debug.WriteLine("\timposedWidth=["+_widthImposedByParent+"] imposedHeight=["+_heightImposedByParent+"]");
			//System.Diagnostics.Debug.WriteLine("\t["+(HtmlText ?? Text)+"]GetSizeRequest req=[" + result.Request.Width + "," + result.Request.Height + "] min=[" + result.Minimum.Width + "," + result.Minimum.Height + "]");
			////System.Diagnostics.Debug.WriteLine("["+_id+"]HasImposedSize["+HasImposedSize+"]");
#if __IOS__
			if (MinimizeHeight) {
			var size = new Size (result.Request.Width, Math.Min(result.Minimum.Height, FontSize));
			result = new SizeRequest (size, size);
			}
#endif
			////System.Diagnostics.Debug.WriteLine("\t\tVtC=["+VerticallyConstrained+"] HzC=["+HorizontallyConstrained+"]");
			//System.Diagnostics.Debug.WriteLine("GetSizeRequest(" + widthConstraint + "," + heightConstraint + ") exit");
			return result;
		}

		/// <param name="widthConstraint">The available width for the element to use.</param>
		/// <param name="heightConstraint">The available height for the element to use.</param>
		/// <summary>
		/// This method is called during the measure pass of a layout cycle to get the desired size of an element.
		/// </summary>
		[Obsolete("Use OnMeasure")]
		protected override SizeRequest OnSizeRequest (double widthConstraint, double heightConstraint)
		{
			//System.Diagnostics.Debug.WriteLine("["+(HtmlText ?? Text)+"]OnSizeRequest(" + widthConstraint + "," + heightConstraint + ") enter");
			// result is from Platform.GetNativeSize(Element, widthConstraint, heightConstraint)
			var result = base.OnSizeRequest (widthConstraint, heightConstraint);
#if __IOS__
			if (MinimizeHeight) {
			var size = new Size (result.Request.Width, Math.Min(result.Minimum.Height, FontSize));
			result = new SizeRequest (size, size);
			}
#endif
			//System.Diagnostics.Debug.WriteLine("\t["+(HtmlText ?? Text)+"]OnSizeRequest req=[" + result.Request.Width + "," + result.Request.Height + "] min=[" + result.Minimum.Width + "," + result.Minimum.Height + "]");
			//System.Diagnostics.Debug.WriteLine("\tOnSizeRequest(" + widthConstraint + "," + heightConstraint + ") exit");
			return result;
		}

		#endregion


		#region HTML link support
		internal Func<Point, int> RendererIndexAtPoint;
		/// <summary>
		/// Gets unmarked string index at touch point.
		/// </summary>
		/// <returns>The at point.</returns>
		/// <param name="point">Point.</param>
		public int IndexAtPoint(Point point)
		{
			return RendererIndexAtPoint != null ? RendererIndexAtPoint(point) : -1;
		}
		#endregion

	}

}

