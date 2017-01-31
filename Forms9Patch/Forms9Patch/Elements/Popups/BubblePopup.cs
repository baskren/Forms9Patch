using System;
using Xamarin.Forms;

namespace Forms9Patch
{
	/// <summary>
	/// Forms9Patch Bubble popup.
	/// </summary>
	public class BubblePopup : PopupBase 
	{

		#region Content 
		/// <summary>
		/// Gets or sets the content of the FormsPopup.Modal.
		/// </summary>
		/// <value>The content.</value>
		public View Content {
			get { return _bubbleLayout.Content; }
			set { _bubbleLayout.Content = value; }
		}
		#endregion


		#region Bubble Properties
		/// <summary>
		/// The target bias property backing Store.
		/// </summary>
		public static readonly BindableProperty TargetBiasProperty = BindableProperty.Create("BpuTargetBias", typeof(double), typeof(BubblePopup), 0.5);
		/// <summary>
		/// Gets or sets the bias (0.0 is start; 0.5 is center;  1.0 is end; greater than 1.0 is pixels from start; less than 0.0 is pixels from end)of the pointer relative to the chosen face on the target.
		/// </summary>
		/// <value>The target bias.</value>
		public double TargetBias
		{
			get { return (double)GetValue(TargetBiasProperty); }
			set { 
				SetValue(TargetBiasProperty, value); 
			}
		}

		#endregion


		#region Pointer Properties
		/// <summary>
		/// Backing store for pointer length property.
		/// </summary>
		public static readonly BindableProperty PointerLengthProperty = BindableProperty.Create("BpuPointerLength",typeof(float), typeof(BubblePopup), (float)BubbleLayout.PointerLengthProperty.DefaultValue);
		/// <summary>
		/// Gets or sets the length of the bubble layout's pointer.
		/// </summary>
		/// <value>The length of the pointer.</value>
		public float PointerLength {
			get { return (float) GetValue (PointerLengthProperty); }
			set { SetValue (PointerLengthProperty, value); }
		}

		/// <summary>
		/// Backing store for pointer tip radius property.
		/// </summary>
		public static readonly BindableProperty PointerTipRadiusProperty = BindableProperty.Create("BpuPointerTipRadius", typeof(float), typeof(BubblePopup), (float)BubbleLayout.PointerTipRadiusProperty.DefaultValue);
		/// <summary>
		/// Gets or sets the radius of the bubble's pointer tip.
		/// </summary>
		/// <value>The pointer tip radius.</value>
		public float PointerTipRadius {
			get { return (float)GetValue (PointerTipRadiusProperty); }
			set { SetValue (PointerTipRadiusProperty, value); }
		}

		/*
		/// <summary>
		/// Backing store for pointer axial position property.
		/// </summary>
		public static readonly BindableProperty PointerAxialPositionProperty = BindableProperty.Create("PointerAxialPosition", typeof(float), typeof(BubblePopup), (float)BubbleLayout.PointerAxialPositionProperty.DefaultValue);
		/// <summary>
		/// Gets or sets the position of the bubble's pointer along the face it's on.
		/// </summary>
		/// <value>The pointer axial position (left/top is zero).</value>
		public float PointerAxialPosition {
			get { return (float)GetValue (PointerAxialPositionProperty); }
			set { SetValue (PointerAxialPositionProperty, value); }
		}
		*/

		/// <summary>
		/// Backing store for pointer direction property.
		/// </summary>
		public static readonly BindableProperty PointerDirectionProperty = BindableProperty.Create("BpuPointerDirection", typeof(PointerDirection), typeof(BubblePopup), (PointerDirection)BubbleLayout.PointerDirectionProperty.DefaultValue);
		/// <summary>
		/// Gets or sets the direction in which the pointer pointing.
		/// </summary>
		/// <value>The pointer direction.</value>
		public PointerDirection PointerDirection {
			get { return (PointerDirection)GetValue (PointerDirectionProperty); }
			set { SetValue (PointerDirectionProperty, value); }
		}

		/// <summary>
		/// The pointer corner radius property.  Defaults to OutlineCornerRadius if not set.
		/// </summary>
		public static readonly BindableProperty PointerCornerRadiusProperty = BindableProperty.Create("BpuPointerCornerRadius", typeof(float), typeof(BubblePopup), (float)BubbleLayout.PointerCornerRadiusProperty.DefaultValue);
		/// <summary>
		/// Gets or sets the pointer corner radius.
		/// </summary>
		/// <value>The pointer corner radius.</value>
		public float PointerCornerRadius {
			get { return (float) GetValue (PointerCornerRadiusProperty); }
			set { SetValue (PointerCornerRadiusProperty, value); }
		}
		#endregion


		#region Fields
		readonly BubbleLayout _bubbleLayout;
		#endregion


		#region Constructor / Destructor
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Forms9Patch.BubblePopup"/> class.
		/// </summary>
		/// <param name="target">Target.</param>
		/// <param name="retain">If set to <c>true</c> retain.</param>
		public BubblePopup (VisualElement target,bool retain=false) : base (target,retain) {
			_bubbleLayout = new BubbleLayout {
				//HorizontalOptions = LayoutOptions.Center,
				//VerticalOptions = LayoutOptions.Center,
				Padding = Padding,
				HasShadow = HasShadow,
				OutlineColor = OutlineColor,
				OutlineWidth = OutlineWidth,
				OutlineRadius = OutlineRadius,
				BackgroundColor = BackgroundColor,
				PointerLength = PointerLength,
				PointerTipRadius = PointerTipRadius,
				PointerCornerRadius = PointerCornerRadius
			};
			ContentView = _bubbleLayout;
			/*
			target.SizeChanged += (sender, e) =>
			{
				System.Diagnostics.Debug.WriteLine("BubblePopup target.SizeChanged ("+target.Bounds+")");
				var b = Application.Current.MainPage.Bounds;
				LayoutChildren(b.X,b.Y, b.Width, b.Height);
			};
*/
		}



		#endregion



		#region Change management

		void OnParentSizeChanged(object sender, EventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("BubblePopup ParentSizeChanged (" + Application.Current.MainPage.Bounds + ")");
			if (Device.OS == TargetPlatform.Android)
			{
				Device.StartTimer(TimeSpan.FromMilliseconds(100), () =>
				 {
					 var b = Application.Current.MainPage.Bounds;
					 LayoutChildren(b.X, b.Y, b.Width, b.Height);
					 return false;
				 });
			}
			else
			{
				var b = Application.Current.MainPage.Bounds;
				LayoutChildren(b.X, b.Y, b.Width, b.Height);
			}
		}

		/// <summary>
		/// Ons the property changing.
		/// </summary>
		/// <param name="propertyName">Property name.</param>
		protected override void OnPropertyChanging(string propertyName = null)
		{
			base.OnPropertyChanging(propertyName);
			if (propertyName == "Parent")
			{
				var rootPage = Parent as RootPage;
				if (rootPage != null)
					rootPage.SizeChanged -= OnParentSizeChanged;
			}
		}


		/// <param name="propertyName">The name of the property that changed.</param>
		/// <summary>
		/// Call this method from a child class to notify that a change happened on a property.
		/// </summary>
		protected override void OnPropertyChanged (string propertyName = null) {
			//System.Diagnostics.Debug.WriteLine ($"{this.GetType().FullName}.OnPropertyChanged property={propertyName}");
			//if (propertyName == IsPresentedProperty.PropertyName) {
			base.OnPropertyChanged (propertyName);
			if (_bubbleLayout == null)
				return;
			else if (propertyName == PointerLengthProperty.PropertyName)
				_bubbleLayout.PointerLength = PointerLength;
			else if (propertyName == PointerTipRadiusProperty.PropertyName)
				_bubbleLayout.PointerTipRadius = PointerTipRadius;
			else if (propertyName == PointerCornerRadiusProperty.PropertyName)
				_bubbleLayout.PointerCornerRadius = PointerCornerRadius;
			else if (propertyName == "Parent")
			{
				var rootPage = Parent as RootPage;
				if (rootPage != null)
					rootPage.SizeChanged += OnParentSizeChanged;
			}
		}


		double _pwfStart, _pwfWidth, _pwfTargetStart, _pwfTargetWidth, _pwfAvailableWidth;
		double positionWeightingFunction(double start) {
			// how far apart is the popup center from the target?
			double err=0;
			if (TargetBias < 0)
				err = Math.Abs((start + _pwfWidth / 2.0) - (_pwfTargetStart + _pwfTargetWidth + TargetBias));
			else if (TargetBias > 1)
				err = Math.Abs((start + _pwfWidth / 2.0) - (_pwfTargetStart + TargetBias));
			else
				err = Math.Abs((start + _pwfWidth/2.0) - (_pwfTargetStart + _pwfTargetWidth * TargetBias));
			//double err = Math.Abs((start + _pwfWidth / 2.0) - (_pwfTargetStart + _pwfTargetWidth / 2.0));

			// does the popup and the target overlap?
			err += (start + _pwfWidth >= _pwfTargetStart ? 0 : 100 * _pwfTargetStart - start - _pwfWidth);
			err += (start <= _pwfTargetStart + _pwfTargetWidth ? 0 : 100 * start - (_pwfTargetStart + _pwfTargetWidth));

			// are we close to the edges?
			err += (start < 20 ? 20 * (20-start) : 0);
			err += (start + _pwfWidth > _pwfAvailableWidth - 20 ? 20 * (start + _pwfWidth - _pwfAvailableWidth + 20) : 0);

			// are we off the screen?
			err += (start < 0 ? 1000 * -start : 0);
			err += (start + _pwfWidth > _pwfAvailableWidth ? 1000 * (start + _pwfWidth - _pwfAvailableWidth) : 0);
			//System.Diagnostics.Debug.WriteLine ("\t\t\tstart="+start+" err=" + err);
			return err;
		}

		double pointerWeightingFunction(double offset) {
			// how far is the offset from the center of the target?
			double err = 0;
			if (TargetBias < -1)
				err = Math.Abs((_pwfStart + offset) - (_pwfTargetStart + _pwfTargetWidth + TargetBias));
			else if (TargetBias < 0)	
				err = Math.Abs((_pwfStart + offset) - (_pwfTargetStart + _pwfTargetWidth * (1+TargetBias)));
			else if (TargetBias > 1)
				err = Math.Abs((_pwfStart + offset) - (_pwfTargetStart + TargetBias));
			else
				err = Math.Abs((_pwfStart + offset) - (_pwfTargetStart + _pwfTargetWidth * TargetBias));
			//double err = Math.Abs((_pwfStart + offset) - (_pwfTargetStart + _pwfTargetWidth / 2.0));

			// does the pointer overlap the target?
			err += (_pwfStart + offset >= _pwfTargetStart ? 0 : 100 * _pwfTargetStart - _pwfStart - offset);
			err += (_pwfStart + offset <= _pwfTargetStart + _pwfTargetWidth ? 0 : 100 *  _pwfStart + offset - (_pwfTargetStart + _pwfTargetWidth));

			return err;
		}

		Tuple<double,float> StartAndPointerLocation(double width, double targetStart, double targetWidth, double availableWidth) {
			System.Diagnostics.Debug.WriteLine("StartAndPointerLocation("+width+","+targetStart+","+targetWidth+","+availableWidth+")");
			_pwfWidth = width;
			_pwfTargetStart = targetStart;
			_pwfTargetWidth = targetWidth;
			_pwfAvailableWidth = availableWidth;
			double optimalStart;
			NumericalMethods.Search1D.BrentMin (
				0, 
				targetStart + targetWidth / 2.0, 
				availableWidth - width, 
				positionWeightingFunction, 0.0001, out optimalStart);
		
			_pwfStart = optimalStart;

			double optimalPointerLoc;
			NumericalMethods.Search1D.BrentMin (
				0, 
				width/2.0, 
				width,
				pointerWeightingFunction, 0.0001, out optimalPointerLoc);

			var pointerOffset = (float)(optimalPointerLoc / width);
			return new Tuple<double, float> (optimalStart, pointerOffset );
		}

		Rectangle _lastBounds = Rectangle.Zero;
		Rectangle _lastTargetBounds = Rectangle.Zero;
		/// <param name="x">A value representing the x coordinate of the child region bounding box.</param>
		/// <param name="y">A value representing the y coordinate of the child region bounding box.</param>
		/// <param name="width">A value representing the width of the child region bounding box.</param>
		/// <param name="height">A value representing the height of the child region bounding box.</param>
		/// <summary>
		/// Positions and sizes the children of a Layout.
		/// </summary>
		/// <remarks>Implementors wishing to change the default behavior of a Layout should override this method. It is suggested to
		/// still call the base method and modify its calculated results.</remarks>
		protected override void LayoutChildren (double x, double y, double width, double height)
		{
			if (_bubbleLayout?.Content == null)
				return;
			var bounds = new Rectangle(x, y, width, height);
			base.LayoutChildren(x, y, width, height);
			System.Diagnostics.Debug.WriteLine("{0}[{1}] ", PCL.Utils.ReflectionExtensions.CallerString(), GetType());
			if (width > 0 && height > 0) {

				var shadow = BubbleLayout.ShadowPadding (_bubbleLayout);
				SizeRequest request;
				if (_bubbleLayout.Content.WidthRequest > 0 && _bubbleLayout.Content.HeightRequest > 0)
					request = new SizeRequest(new Size(_bubbleLayout.Content.WidthRequest, _bubbleLayout.Content.HeightRequest));
				else
					request = _bubbleLayout.Content.Measure (width, height);
				//var request = _bubbleLayout.Content.Measure(Host.Bounds.Width, Host.Bounds.Height);
				var rboxSize = new Size (request.Request.Width + _bubbleLayout.Padding.HorizontalThickness + shadow.HorizontalThickness, request.Request.Height + _bubbleLayout.Padding.VerticalThickness + shadow.VerticalThickness);

				//System.Diagnostics.Debug.WriteLine("\tBubblePopup.LayoutChildren _bubbleLayout size=[{0}, {1}]",rboxSize.Width, rboxSize.Height);
				PointerDirection pointerDir = PointerDirection.None;

				//Rectangle bounds;
				Rectangle targetBounds=Rectangle.Zero;
				if (Target != null)
				{
					//targetBounds = DependencyService.Get<IDescendentBounds> ().PageDescendentBounds (HostPage, Target);
					targetBounds = DependencyService.Get<IDescendentBounds>().PageDescendentBounds(Application.Current.MainPage, Target);

					if (_lastBounds == bounds && targetBounds == _lastTargetBounds)
						return;
					_lastBounds = bounds;
					_lastTargetBounds = targetBounds;



					var reqSpaceToLeft = targetBounds.Left - rboxSize.Width - PointerLength;
					var reqSpaceToRight = width - targetBounds.Right - rboxSize.Width - PointerLength;
					var reqSpaceAbove = targetBounds.Top - rboxSize.Height - PointerLength;
					var reqSpaceBelow = height - targetBounds.Bottom - rboxSize.Height - PointerLength;
					var reqHzSpace = width - rboxSize.Width;
					var reqVtSpace = height - rboxSize.Height;


					double space = 0;
					if (PointerDirection.UpAllowed() && Math.Min(reqSpaceBelow, reqHzSpace) > space)
					{
						pointerDir = PointerDirection.Up;
						space = Math.Min(reqSpaceBelow, reqHzSpace);
					}
					if (PointerDirection.DownAllowed() && Math.Min(reqSpaceAbove, reqHzSpace) > space)
					{
						pointerDir = PointerDirection.Down;
						space = Math.Min(reqSpaceAbove, reqHzSpace);
					}
					if (PointerDirection.LeftAllowed() && Math.Min(reqSpaceToRight, reqVtSpace) > space)
					{
						pointerDir = PointerDirection.Left;
						space = Math.Min(reqSpaceToRight, reqVtSpace);
					}
					if (PointerDirection.RightAllowed() && Math.Min(reqSpaceToLeft, reqVtSpace) > space)
					{
						pointerDir = PointerDirection.Right;
						space = Math.Min(reqSpaceToLeft, reqVtSpace);
					}
					if (space < 0.01)
					{
						// it doesn't fit ... what's the closest fit?
						space = int.MaxValue;
						if (PointerDirection.UpAllowed() && Math.Abs(Math.Min(reqSpaceBelow, reqHzSpace)) < space)
						{
							pointerDir = PointerDirection.Up;
							space = Math.Abs(Math.Min(reqSpaceBelow, reqHzSpace));
						}
						if (PointerDirection.DownAllowed() && Math.Abs(Math.Min(reqSpaceAbove, reqHzSpace)) < space)
						{
							pointerDir = PointerDirection.Down;
							space = Math.Abs(Math.Min(reqSpaceAbove, reqHzSpace));
						}
						if (PointerDirection.LeftAllowed() && Math.Abs(Math.Min(reqSpaceToRight, reqVtSpace)) < space)
						{
							pointerDir = PointerDirection.Left;
							space = Math.Abs(Math.Min(reqSpaceToRight, reqVtSpace));
						}
						if (PointerDirection.RightAllowed() && Math.Abs(Math.Min(reqSpaceToLeft, reqVtSpace)) < space)
						{
							pointerDir = PointerDirection.Right;
						}
					}
				}
				else
				{
					if (_lastBounds == bounds &&  _lastTargetBounds == Rectangle.Zero)
						return;
					_lastBounds = bounds;
					_lastTargetBounds = Rectangle.Zero;

				}
				_bubbleLayout.PointerDirection = pointerDir;
				//_bubbleLayout.IsVisible = true;
				if (pointerDir == PointerDirection.None) {
					LayoutChildIntoBoundingRegion (_bubbleLayout, new Rectangle (width / 2.0 - rboxSize.Width / 2.0, height / 2.0 - rboxSize.Height / 2.0, rboxSize.Width, rboxSize.Height));
				} else {
					Tuple<double,float> tuple;
					if (pointerDir.IsVertical ()) {
						tuple = StartAndPointerLocation (rboxSize.Width, targetBounds.Left, targetBounds.Width, width);
						bounds = new Rectangle (
							new Point (
								tuple.Item1 + x,
								(pointerDir == PointerDirection.Up ? targetBounds.Bottom : targetBounds.Top - rboxSize.Height - PointerLength) + y),
							new Size (rboxSize.Width, rboxSize.Height + PointerLength)
						);
					} else {
						tuple = StartAndPointerLocation (rboxSize.Height, targetBounds.Top, targetBounds.Height, height);
						bounds = new Rectangle (
							new Point (
								(pointerDir == PointerDirection.Left ? targetBounds.Right : targetBounds.Left - rboxSize.Width - PointerLength) + x, 
								tuple.Item1 + y),
							new Size (rboxSize.Width + PointerLength, rboxSize.Height)
						);
					}
					_bubbleLayout.PointerAxialPosition = tuple.Item2;	
					LayoutChildIntoBoundingRegion (_bubbleLayout, bounds);
				}
			} 
		}
		#endregion


	}
}


