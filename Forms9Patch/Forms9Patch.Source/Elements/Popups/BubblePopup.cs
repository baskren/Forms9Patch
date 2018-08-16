using System;
using Xamarin.Forms;
using FormsGestures;
using System.Runtime.CompilerServices;

namespace Forms9Patch
{
    /// <summary>
    /// Forms9Patch Bubble pop-up.
    /// </summary>
    public class BubblePopup : PopupBase
    {
        #region Properties

        #region Content 
        /// <summary>
        /// Gets or sets the content of the FormsPopup.Modal.
        /// </summary>
        /// <value>The content.</value>
        public View Content
        {
            get => _bubbleLayout.Content;
            set => _bubbleLayout.Content = value;
        }
        #endregion

        #region Bubble Properties
        /// <summary>
        /// The target bias property backing Store.
        /// </summary>
        public static readonly BindableProperty TargetBiasProperty = BindableProperty.Create("TargetBias", typeof(double), typeof(BubblePopup), 0.5);
        /// <summary>
        /// Gets or sets the bias (0.0 is start; 0.5 is center;  1.0 is end; greater than 1.0 is pixels from start; less than 0.0 is pixels from end)of the pointer relative to the chosen face on the target.
        /// </summary>
        /// <value>The target bias.</value>
        public double TargetBias
        {
            get => (double)GetValue(TargetBiasProperty);
            set => SetValue(TargetBiasProperty, value);
        }

        #endregion

        #region Pointer Properties

        #region PointerLength
        /// <summary>
        /// Backing store for pointer length property.
        /// </summary>
        public static readonly BindableProperty PointerLengthProperty = BindableProperty.Create("PointerLength", typeof(float), typeof(BubblePopup), (float)BubbleLayout.PointerLengthProperty.DefaultValue);
        /// <summary>
        /// Gets or sets the length of the bubble layout's pointer.
        /// </summary>
        /// <value>The length of the pointer.</value>
        public float PointerLength
        {
            get => (float)GetValue(PointerLengthProperty);
            set => SetValue(PointerLengthProperty, value);
        }
        #endregion

        #region PointerTipRadius
        /// <summary>
        /// Backing store for pointer tip radius property.
        /// </summary>
        public static readonly BindableProperty PointerTipRadiusProperty = BindableProperty.Create("PointerTipRadius", typeof(float), typeof(BubblePopup), (float)BubbleLayout.PointerTipRadiusProperty.DefaultValue);
        /// <summary>
        /// Gets or sets the radius of the bubble's pointer tip.
        /// </summary>
        /// <value>The pointer tip radius.</value>
        public float PointerTipRadius
        {
            get => (float)GetValue(PointerTipRadiusProperty);
            set => SetValue(PointerTipRadiusProperty, value);
        }
        #endregion

        #region PointerDirection
        /// <summary>
        /// Backing store for pointer direction property.
        /// </summary>
        public static readonly BindableProperty PointerDirectionProperty = BindableProperty.Create("PointerDirection", typeof(PointerDirection), typeof(BubblePopup), (PointerDirection)BubbleLayout.PointerDirectionProperty.DefaultValue);
        /// <summary>
        /// Gets or sets the direction in which the pointer pointing.
        /// </summary>
        /// <value>The pointer direction.</value>
        public PointerDirection PointerDirection
        {
            get => (PointerDirection)GetValue(PointerDirectionProperty);
            set => SetValue(PointerDirectionProperty, value);
        }
        #endregion

        #region PreferredPointerDirection property
        /// <summary>
        /// backing store for PreferredPointerDirection property
        /// </summary>
        public static readonly BindableProperty PreferredPointerDirectionProperty = BindableProperty.Create("PreferredPointerDirection", typeof(PointerDirection), typeof(BubblePopup), PointerDirection.None);
        /// <summary>
        /// Gets/Sets the PreferredPointerDirection property
        /// </summary>
        public PointerDirection PreferredPointerDirection
        {
            get => (PointerDirection)GetValue(PreferredPointerDirectionProperty);
            set => SetValue(PreferredPointerDirectionProperty, value);
        }
        #endregion PreferredPointerDirection property

        #region PointerCornerRadius
        /// <summary>
        /// The pointer corner radius property.  Defaults to OutlineCornerRadius if not set.
        /// </summary>
        public static readonly BindableProperty PointerCornerRadiusProperty = BindableProperty.Create("PointerCornerRadius", typeof(float), typeof(BubblePopup), (float)BubbleLayout.PointerCornerRadiusProperty.DefaultValue);
        /// <summary>
        /// Gets or sets the pointer corner radius.
        /// </summary>
        /// <value>The pointer corner radius.</value>
        public float PointerCornerRadius
        {
            get => (float)GetValue(PointerCornerRadiusProperty);
            set => SetValue(PointerCornerRadiusProperty, value);
        }
        #endregion

        #region PointerAngle property
        /// <summary>
        /// backing store for PointerAngle property
        /// </summary>
        internal static readonly BindableProperty PointerAngleProperty = BindableProperty.Create("PointerAngle", typeof(float), typeof(BubblePopup), (float)BubbleLayout.PointerAngleProperty.DefaultValue);
        /// <summary>
        /// Gets/Sets the PointerAngle property
        /// </summary>
        internal float PointerAngle
        {
            get => (float)GetValue(PointerAngleProperty);
            set => SetValue(PointerAngleProperty, value);
        }
        #endregion PointerAngle property

        #region Point property
        /// <summary>
        /// backing store for Point property
        /// </summary>
        public static readonly BindableProperty PointProperty = BindableProperty.Create("Point", typeof(Point), typeof(BubblePopup), new Point(double.NegativeInfinity, double.PositiveInfinity));
        /// <summary>
        /// Gets/Sets the Point property
        /// </summary>
        public Point Point
        {
            get => (Point)GetValue(PointProperty);
            set => SetValue(PointProperty, value);
        }
        #endregion Point property


        #endregion

        #endregion


        #region Fields
        BubbleLayout _bubbleLayout;
        #endregion


        #region Constructor / Destructor

        void Init()
        {
            _bubbleLayout = new BubbleLayout
            {
                Padding = Padding,
                HasShadow = HasShadow,
                OutlineColor = OutlineColor,
                OutlineWidth = OutlineWidth,
                OutlineRadius = OutlineRadius,
                BackgroundColor = BackgroundColor,

                PointerLength = PointerLength,
                PointerAngle = PointerAngle,
                PointerTipRadius = PointerTipRadius,
                PointerCornerRadius = PointerCornerRadius
            };
            DecorativeContainerView = _bubbleLayout;

            Margin = 0;
            Padding = 10;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.BubblePopup"/> class.
        /// </summary>
        /// <param name="target">Target.</param>
        /// <param name="retain">If set to <c>true</c> retain.</param>
        public BubblePopup(VisualElement target, bool retain = false) : base(target, retain) => Init();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.BubblePopup"/> class that targets a point inside of the target VisualElement
        /// </summary>
        /// <param name="target">Target.</param>
        /// <param name="point">Point.</param>
        /// <param name="retain">If set to <c>true</c> retain.</param>
        public BubblePopup(VisualElement target, Point point, bool retain = false) : base(target, retain)
        {
            Init();
            Point = point;
        }

        #endregion


        #region Change management

        /// <param name="propertyName">The name of the property that changed.</param>
        /// <summary>
        /// Call this method from a child class to notify that a change happened on a property.
        /// </summary>
        protected override void OnPropertyChanged(string propertyName = null)
        {
            if (propertyName == TranslationXProperty.PropertyName)
            {
                Content.TranslationX = TranslationX;
                return;
            }
            if (propertyName == TranslationYProperty.PropertyName)
            {
                Content.TranslationY = TranslationY;
                return;
            }
            if (propertyName == RotationProperty.PropertyName)
            {
                Content.Rotation = Rotation;
                return;
            }
            if (propertyName == RotationXProperty.PropertyName)
            {
                Content.RotationX = RotationX;
                return;
            }
            if (propertyName == RotationYProperty.PropertyName)
            {
                Content.RotationY = RotationY;
                return;
            }

            base.OnPropertyChanged(propertyName);

            if (_bubbleLayout == null)
                return;

            if (propertyName == PointerLengthProperty.PropertyName)
                _bubbleLayout.PointerLength = PointerLength;
            else if (propertyName == PointerAngleProperty.PropertyName)
            {
                if (propertyName == PointerAngleProperty.PropertyName)
                {
                    if (PointerAngle < 1)
                    {
                        PointerAngle = 1;
                        return;
                    }
                    if (PointerAngle > 89)
                    {
                        PointerAngle = 89;
                        return;
                    }
                }
                _bubbleLayout.PointerAngle = PointerAngle;
            }
            else if (propertyName == PointerTipRadiusProperty.PropertyName)
                _bubbleLayout.PointerTipRadius = PointerTipRadius;
            else if (propertyName == PointerCornerRadiusProperty.PropertyName)
                _bubbleLayout.PointerCornerRadius = PointerCornerRadius;
            /*
            else if (propertyName == "Parent")
            {
                if (Parent is RootPage rootPage)
                    rootPage.SizeChanged += OnParentSizeChanged;
            }
            */

            else if (propertyName == TargetProperty.PropertyName)
            {
                // we need to determine if the target is inside of a scroll view because the scroll view's ScrollOffset is set to zero on orientation changes.
                if (Target.Parent<ScrollView>() is ScrollView scrollView)
                    scrollView.Scrolled += OnScrollWrappingTargetScrolled;
            }
        }

        protected override void OnPropertyChanging([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanging(propertyName);
            if (propertyName == TargetProperty.PropertyName)
            {
                if (Target.Parent<ScrollView>() is ScrollView scrollView)
                    scrollView.Scrolled -= OnScrollWrappingTargetScrolled;
            }
        }

        void OnScrollWrappingTargetScrolled(object sender, ScrolledEventArgs e)
        {
            if (IsVisible)
                LayoutChildren(RootPage.X, RootPage.Y, RootPage.Bounds.Size.Width, RootPage.Bounds.Height - KeyboardService.Height);
        }

        #endregion


        #region Layout
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Forms9Patch.BubblePopup"/> will target Point (a point in the target) vs. the border of the target.
        /// </summary>
        /// <value><c>true</c> if use point; otherwise, <c>false</c>.</value>
        public bool UsePoint
        {
            get => !double.IsNegativeInfinity(Point.X) && !double.IsPositiveInfinity(Point.Y);
            set => Point = new Point(double.NegativeInfinity, double.PositiveInfinity);
        }

        //Using the below check seems to fail to render correct size on some pop-ups!
        //Rectangle _lastBounds = Rectangle.Zero;  
        //Rectangle _lastTargetBounds = Rectangle.Zero;
        /// <param name="x">A value representing the x coordinate of the child region bounding box.</param>
        /// <param name="y">A value representing the y coordinate of the child region bounding box.</param>
        /// <param name="width">A value representing the width of the child region bounding box.</param>
        /// <param name="height">A value representing the height of the child region bounding box.</param>
        /// <summary>
        /// Positions and sizes the children of a Layout.
        /// </summary>
        /// <remarks>Implementors wishing to change the default behavior of a Layout should override this method. It is suggested to
        /// still call the base method and modify its calculated results.</remarks>
        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            //System.Diagnostics.Debug.WriteLine("BubblePopup.LayoutChildren(" + x + ", " + y + ", " + width + ", " + height + ")");

            if (_bubbleLayout?.Content == null)
                return;
            var bounds = new Rectangle(x, y, width, height);

            // layout the background
            base.LayoutChildren(x, y, width, height);
            //System.Diagnostics.Debug.WriteLine("{0}[{1}] bounds=["+RectangleExtensions.ToString(bounds)+"]", P42.Utils.ReflectionExtensions.CallerString(), GetType());
            if (width > 0 && height > 0)
            {
                _bubbleLayout.IsVisible = true;
                _bubbleLayout.Content.IsVisible = true;

                var availWidth = width - (Margin.HorizontalThickness + _bubbleLayout.Padding.HorizontalThickness); // _bubbleLayout.Padding.HorizontalThickness + shadow.HorizontalThickness);
                var availHeight = height - (Margin.VerticalThickness + _bubbleLayout.Padding.VerticalThickness); // _bubbleLayout.Padding.VerticalThickness + shadow.VerticalThickness);
                if (_bubbleLayout.Content.WidthRequest > 0)
                    availWidth = _bubbleLayout.Content.WidthRequest;
                if (_bubbleLayout.Content.HeightRequest > 0)
                    availHeight = _bubbleLayout.Content.HeightRequest;
                var request = _bubbleLayout.Content.Measure(availWidth, availHeight, MeasureFlags.None);  //

                var decoration = ShapeBase.ShadowPadding(_bubbleLayout, HasShadow);// _bubbleLayout.DecorativePadding();

                var rBoxWidth = HorizontalOptions.Alignment == LayoutAlignment.Fill ? availWidth : Math.Min(request.Request.Width, availWidth); // _bubbleLayout.Padding.HorizontalThickness + shadow.HorizontalThickness);
                rBoxWidth += _bubbleLayout.Padding.HorizontalThickness;
                rBoxWidth += decoration.HorizontalThickness;
                var rBoxHeight = VerticalOptions.Alignment == LayoutAlignment.Fill ? availHeight : Math.Min(request.Request.Height, availHeight); // _bubbleLayout.Padding.VerticalThickness + shadow.VerticalThickness);
                rBoxHeight += _bubbleLayout.Padding.VerticalThickness;
                rBoxHeight += decoration.VerticalThickness;
                var rboxSize = new Size(rBoxWidth, rBoxHeight);

                //System.Diagnostics.Debug.WriteLine("\tBubblePopup.LayoutChildren _bubbleLayout size=[{0}, {1}]",rboxSize.Width, rboxSize.Height);
                PointerDirection pointerDir = PointerDirection.None;


                var targetPage = Application.Current.MainPage;
                var hostingPage = this.HostingPage();
                foreach (var page in Application.Current.MainPage.Navigation.ModalStack)
                {
                    if (page == hostingPage)
                    {
                        targetPage = hostingPage;
                        break;
                    }
                }

                //Rectangle bounds;
                Rectangle targetBounds = Rectangle.Zero;
                if (Target != null)
                {

                    // NOTE: Use of PageDescentBounds is deliberate.  It has different behavior on UWP in that it ignores the target's location (assumes 0,0) for the transformation.
                    //       FormsGestures coordinate transform methods can't be used because popup.ContentView will most likely have a non-zero X and Y value.

                    //System.Diagnostics.Debug.WriteLine("\t\t Target.Bounds=[" + Target.Bounds + "]");
                    //System.Diagnostics.Debug.WriteLine("\t\t targetPage.Bounds=[" + targetPage.Bounds + "]");
                    if (Target is PopupBase popup)
                    {
                        targetBounds = DependencyService.Get<IDescendentBounds>().PageDescendentBounds(targetPage, popup.DecorativeContainerView);
                        //targetBounds = popup.ContentView.BoundsToEleCoord(targetPage);
                        //System.Diagnostics.Debug.WriteLine("\t\t targetBounds=[" + targetBounds + "]");
                        //targetBounds = targetPage.GetRelativeBounds(popup.DecorativeContainerView);
                        //System.Diagnostics.Debug.WriteLine("targetBounds=[" + targetBounds + "]");
                    }
                    else
                    {
                        targetBounds = DependencyService.Get<IDescendentBounds>().PageDescendentBounds(targetPage, Target);
                        //targetBounds = Target.BoundsToEleCoord(targetPage);
                        //System.Diagnostics.Debug.WriteLine("\t\t targetBounds=[" + targetBounds + "]");
                        //targetBounds = targetPage.GetRelativeBounds(Target);
                        //System.Diagnostics.Debug.WriteLine("targetBounds=[" + targetBounds + "]");
                    }



                    var reqSpaceToLeft = (UsePoint ? Point.X + targetBounds.Left : targetBounds.Left) - rboxSize.Width - PointerLength - Margin.Left;
                    var reqSpaceToRight = width - (UsePoint ? Point.X + targetBounds.Left : targetBounds.Right) - rboxSize.Width - PointerLength - Margin.Right;
                    var reqSpaceAbove = (UsePoint ? Point.Y + targetBounds.Top : targetBounds.Top) - rboxSize.Height - PointerLength - Margin.Top;
                    var reqSpaceBelow = height - (UsePoint ? Point.Y + targetBounds.Top : targetBounds.Bottom) - rboxSize.Height - PointerLength - Margin.Bottom;
                    var reqHzSpace = width - rboxSize.Width - Margin.HorizontalThickness;
                    var reqVtSpace = height - rboxSize.Height - Margin.VerticalThickness;


                    double space = 0;

                    if (PreferredPointerDirection != PointerDirection.None)
                    {
                        if (PreferredPointerDirection.DownAllowed() && PointerDirection.DownAllowed() && Math.Min(reqSpaceAbove, reqHzSpace) > space)
                        {
                            pointerDir = PointerDirection.Down;
                            space = Math.Min(reqSpaceAbove, reqHzSpace);
                        }
                        if (PreferredPointerDirection.UpAllowed() && PointerDirection.UpAllowed() && Math.Min(reqSpaceBelow, reqHzSpace) > space)
                        {
                            pointerDir = PointerDirection.Up;
                            space = Math.Min(reqSpaceBelow, reqHzSpace);
                        }
                        if (PreferredPointerDirection.LeftAllowed() && PointerDirection.LeftAllowed() && Math.Min(reqSpaceToRight, reqVtSpace) > space)
                        {
                            pointerDir = PointerDirection.Left;
                            space = Math.Min(reqSpaceToRight, reqVtSpace);
                        }
                        if (PreferredPointerDirection.RightAllowed() && PointerDirection.RightAllowed() && Math.Min(reqSpaceToLeft, reqVtSpace) > space)
                        {
                            pointerDir = PointerDirection.Right;
                            space = Math.Min(reqSpaceToLeft, reqVtSpace);
                        }
                    }

                    if (space < 1)
                    {
                        if (PointerDirection.DownAllowed() && Math.Min(reqSpaceAbove, reqHzSpace) > space)
                        {
                            pointerDir = PointerDirection.Down;
                            space = Math.Min(reqSpaceAbove, reqHzSpace);
                        }
                        if (PointerDirection.UpAllowed() && Math.Min(reqSpaceBelow, reqHzSpace) > space)
                        {
                            pointerDir = PointerDirection.Up;
                            space = Math.Min(reqSpaceBelow, reqHzSpace);
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
                    }

                    if (space < 1)
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
                    //if (_lastBounds == bounds &&  _lastTargetBounds == Rectangle.Zero)
                    //	return;
                    //_lastBounds = bounds;
                    //_lastTargetBounds = Rectangle.Zero;

                }
                _bubbleLayout.PointerDirection = pointerDir;
                //_bubbleLayout.IsVisible = true;
                if (pointerDir == PointerDirection.None)
                {
                    var contentX = width / 2.0 - rboxSize.Width / 2.0;
                    var contentY = height / 2.0 - rboxSize.Height / 2.0;

                    bounds = new Rectangle(contentX, contentY, rboxSize.Width, rboxSize.Height);
                    //var rect = new Rectangle(Margin.Left + width / 2.0 - rboxSize.Width / 2.0, Margin.Top + height / 2.0 - rboxSize.Height / 2.0, rboxSize.Width, rboxSize.Height);
                    LayoutChildIntoBoundingRegion(_bubbleLayout, bounds);
                }
                else
                {
                    Tuple<double, float> tuple;
                    if (pointerDir.IsVertical())
                    {
                        //System.Diagnostics.Debug.WriteLine("\t\t rboxSize=[" + rboxSize + "] targetBounds=[" + targetBounds + "]");
                        if (UsePoint)
                        {
                            tuple = StartAndPointerLocation(rboxSize.Width, Point.X + targetBounds.Left, 0, width);
                            bounds = new Rectangle(
                                new Point(
                                    tuple.Item1 + x,
                                    (pointerDir == PointerDirection.Up ? Point.Y + targetBounds.Top : Point.Y + targetBounds.Top - rboxSize.Height - PointerLength) + y),
                                new Size(rboxSize.Width, rboxSize.Height + PointerLength)
                            );
                        }
                        else
                        {
                            tuple = StartAndPointerLocation(rboxSize.Width, targetBounds.Left, targetBounds.Width, width);
                            bounds = new Rectangle(
                                new Point(
                                    tuple.Item1 + x,
                                    (pointerDir == PointerDirection.Up ? targetBounds.Bottom : targetBounds.Top - rboxSize.Height - PointerLength) + y),
                                new Size(rboxSize.Width, rboxSize.Height + PointerLength)
                            );
                        }
                    }
                    else
                    {
                        //System.Diagnostics.Debug.WriteLine("\t\t rboxSize=[" + rboxSize + "] targetBounds=[" + targetBounds + "]");
                        if (UsePoint)
                        {
                            tuple = StartAndPointerLocation(rboxSize.Height, Point.Y + targetBounds.Top, 0, height);
                            bounds = new Rectangle(
                                new Point(
                                    (pointerDir == PointerDirection.Left ? Point.X + targetBounds.Left : Point.X + targetBounds.Left - rboxSize.Width - PointerLength) + x,
                                    tuple.Item1 + y),
                                new Size(rboxSize.Width + PointerLength, rboxSize.Height)
                            );
                        }
                        else
                        {
                            tuple = StartAndPointerLocation(rboxSize.Height, targetBounds.Top, targetBounds.Height, height);
                            bounds = new Rectangle(
                                new Point(
                                    (pointerDir == PointerDirection.Left ? targetBounds.Right : targetBounds.Left - rboxSize.Width - PointerLength) + x,
                                    tuple.Item1 + y),
                                new Size(rboxSize.Width + PointerLength, rboxSize.Height)
                            );
                        }
                    }
                    _bubbleLayout.PointerAxialPosition = tuple.Item2;
                    var newBounds = new Rectangle(bounds.X - targetPage.Padding.Left, bounds.Y - targetPage.Padding.Top, bounds.Width, bounds.Height);
                    LayoutChildIntoBoundingRegion(_bubbleLayout, newBounds);
                    //System.Diagnostics.Debug.WriteLine("\t\t LayoutChildIntoBoundingRegtion(_bubbleLayout, " + newBounds + ")");
                    //System.Diagnostics.Debug.WriteLine("");
                }
            }
        }



        #region Layout Support 
        double _pwfStart, _pwfWidth, _pwfTargetStart, _pwfTargetWidth, _pwfAvailableWidth;
        double PositionWeightingFunction(double start)
        {
            // how far apart is the pop-up center from the target?
            double err = 0;
            if (TargetBias < 0)
                err = Math.Abs((start + _pwfWidth / 2.0) - (_pwfTargetStart + _pwfTargetWidth + TargetBias));
            else if (TargetBias > 1)
                err = Math.Abs((start + _pwfWidth / 2.0) - (_pwfTargetStart + TargetBias));
            else
                err = Math.Abs((start + _pwfWidth / 2.0) - (_pwfTargetStart + _pwfTargetWidth * TargetBias));
            //double err = Math.Abs((start + _pwfWidth / 2.0) - (_pwfTargetStart + _pwfTargetWidth / 2.0));

            // does the pop-up and the target overlap?
            err += (start + _pwfWidth >= _pwfTargetStart ? 0 : 100 * _pwfTargetStart - start - _pwfWidth);
            err += (start <= _pwfTargetStart + _pwfTargetWidth ? 0 : 100 * start - (_pwfTargetStart + _pwfTargetWidth));

            // are we close to the edges?
            err += (start < 20 ? 20 * (20 - start) : 0);
            err += (start + _pwfWidth > _pwfAvailableWidth - 20 ? 20 * (start + _pwfWidth - _pwfAvailableWidth + 20) : 0);

            // are we off the screen?
            err += (start < 0 ? 1000 * -start : 0);
            err += (start + _pwfWidth > _pwfAvailableWidth ? 1000 * (start + _pwfWidth - _pwfAvailableWidth) : 0);
            //System.Diagnostics.Debug.WriteLine ("\t\t\tstart="+start+" err=" + err);
            return err;
        }

        double PointerWeightingFunction(double offset)
        {
            // how far is the offset from the center of the target?
            double err = 0;
            if (TargetBias < -1)
                err = Math.Abs((_pwfStart + offset) - (_pwfTargetStart + _pwfTargetWidth + TargetBias));
            else if (TargetBias < 0)
                err = Math.Abs((_pwfStart + offset) - (_pwfTargetStart + _pwfTargetWidth * (1 + TargetBias)));
            else if (TargetBias > 1)
                err = Math.Abs((_pwfStart + offset) - (_pwfTargetStart + TargetBias));
            else
                err = Math.Abs((_pwfStart + offset) - (_pwfTargetStart + _pwfTargetWidth * TargetBias));
            //double err = Math.Abs((_pwfStart + offset) - (_pwfTargetStart + _pwfTargetWidth / 2.0));

            // does the pointer overlap the target?
            err += (_pwfStart + offset >= _pwfTargetStart ? 0 : 100 * _pwfTargetStart - _pwfStart - offset);
            err += (_pwfStart + offset <= _pwfTargetStart + _pwfTargetWidth ? 0 : 100 * _pwfStart + offset - (_pwfTargetStart + _pwfTargetWidth));

            return err;
        }

        Tuple<double, float> StartAndPointerLocation(double width, double targetStart, double targetWidth, double availableWidth)
        {
            //System.Diagnostics.Debug.WriteLine("StartAndPointerLocation("+width+","+targetStart+","+targetWidth+","+availableWidth+")");
            _pwfWidth = width;
            _pwfTargetStart = targetStart;
            _pwfTargetWidth = targetWidth;
            _pwfAvailableWidth = availableWidth;
            double optimalStart;
            P42.NumericalMethods.Search1D.BrentMin(
                0,
                targetStart + targetWidth / 2.0,
                availableWidth - width,
                PositionWeightingFunction, 0.0001, out optimalStart);

            _pwfStart = optimalStart;

            double optimalPointerLoc;
            P42.NumericalMethods.Search1D.BrentMin(
                0,
                width / 2.0,
                width,
                PointerWeightingFunction, 0.0001, out optimalPointerLoc);

            var pointerOffset = (float)(optimalPointerLoc / width);
            return new Tuple<double, float>(optimalStart, pointerOffset);
        }
        #endregion

        #region Sizing
        /*
		protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
		{
		//	System.Diagnostics.Debug.WriteLine("{0}[{1}] w=[" + widthConstraint + "] h=[" + heightConstraint + "]", P42.Utils.ReflectionExtensions.CallerString(), GetType());
			return base.OnMeasure(widthConstraint, heightConstraint);
		}
		*/

        /*
		//double _lastWidthAllocated, _lastHeightAllocated;
		protected override void OnSizeAllocated(double width, double height)
		{
		//	if (Math.Abs(_lastWidthAllocated - width) < 0.001 && Math.Abs(_lastHeightAllocated - height) < 0.001)
		//		return;
		//	_lastWidthAllocated = width;
		//	_lastHeightAllocated = height;
		//	System.Diagnostics.Debug.WriteLine("{0}[{1}] w=[" + width + "] h=[" + height + "]", P42.Utils.ReflectionExtensions.CallerString(), GetType());
			base.OnSizeAllocated(width, height);
		}
		*/
        #endregion

        #endregion


    }
}


