using System;
using Xamarin.Forms;
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
        public new View Content
        {
            get => _bubbleLayout.Content;
            set => _bubbleLayout.Content = value;
        }
        #endregion

        #region Bubble Properties
        /// <summary>
        /// The target bias property backing Store.
        /// </summary>
        public static readonly BindableProperty TargetBiasProperty = BindableProperty.Create("Forms9Patch.BubblePopup.TargetBias", typeof(double), typeof(BubblePopup), 0.5);
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
        public static readonly BindableProperty PointerLengthProperty = BindableProperty.Create("Forms9Patch.BubblePopup.PointerLength", typeof(float), typeof(BubblePopup), (float)BubbleLayout.PointerLengthProperty.DefaultValue,
        propertyChanged: (b, o, n) =>
         {
             if (b is BubblePopup bubblePopup)
                 bubblePopup._bubbleLayout.PointerLength = bubblePopup.PointerLength;
         });
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
        public static readonly BindableProperty PointerTipRadiusProperty = BindableProperty.Create("PointerTipRadius", typeof(float), typeof(BubblePopup), (float)BubbleLayout.PointerTipRadiusProperty.DefaultValue,
        propertyChanged: (b, o, n) =>
        {
            if (b is BubblePopup bubblePopup)
                bubblePopup._bubbleLayout.PointerTipRadius = bubblePopup.PointerTipRadius;
        });
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
        public static readonly BindableProperty PreferredPointerDirectionProperty = BindableProperty.Create("Forms9Patch.BubblePopup.PreferredPointerDirection", typeof(PointerDirection), typeof(BubblePopup), PointerDirection.None);
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
        public static readonly BindableProperty PointerCornerRadiusProperty = BindableProperty.Create("PointerCornerRadius", typeof(float), typeof(BubblePopup), (float)BubbleLayout.PointerCornerRadiusProperty.DefaultValue,
        propertyChanged: (b, o, n) =>
        {
            if (b is BubblePopup bubblePopup)
                bubblePopup._bubbleLayout.PointerCornerRadius = bubblePopup.PointerCornerRadius;
        });
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

        #region Point property
        /// <summary>
        /// backing store for Point property
        /// </summary>
        public static readonly BindableProperty PointProperty = BindableProperty.Create("Forms9Patch.BubblePopup.Point", typeof(Point), typeof(BubblePopup), new Point(double.NegativeInfinity, double.PositiveInfinity));
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
                PointerTipRadius = PointerTipRadius,
                PointerCornerRadius = PointerCornerRadius
            };
            DecorativeContainerView = _bubbleLayout;

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
            //System.Diagnostics.Debug.WriteLine("propertyNmae=[" + propertyName + "]");
            if (!P42.Utils.Environment.IsOnMainThread)
            {
                Device.BeginInvokeOnMainThread(() => OnPropertyChanged(propertyName));
                return;
            }

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
            if (!P42.Utils.Environment.IsOnMainThread)
            {
                Device.BeginInvokeOnMainThread(() => LayoutChildren(x, y, width, height));
                return;
            }
            //System.Diagnostics.Debug.WriteLine("BubblePopup.LayoutChildren(" + x + ", " + y + ", " + width + ", " + height + ")");
            //System.Diagnostics.Debug.WriteLine("this.Bounds: " + Bounds);

            if (_bubbleLayout?.Content == null)
                return;

            // layout the page overlay
            //base.LayoutChildren(x, y, width, height);

            height -= KeyboardService.Height;
            var bounds = new Rectangle(x, y, width, height);

            var hzModal = width - Margin.HorizontalThickness;
            var vtModal = height - Margin.VerticalThickness;
            double hzAvail = 0, vtAvail = 0;
            SizeRequest sizeRequest = new SizeRequest();

            //System.Diagnostics.Debug.WriteLine("{0}[{1}] bounds=["+RectangleExtensions.ToString(bounds)+"]", P42.Utils.ReflectionExtensions.CallerString(), GetType());
            if (width > 0 && height > 0)
            {
                _bubbleLayout.IsVisible = true;
                _bubbleLayout.Content.IsVisible = true;

                var shadowPadding = ShapeBase.ShadowPadding(_bubbleLayout);

                PointerDirection pointerDir = PointerDirection.None;
                Page targetPage = this;
                Rectangle targetBounds = Rectangle.Zero;

                if (Target != null && PointerDirection != PointerDirection.None)
                {

                    // STEP 1 : WHERE DOES THE CONTENT BEST FIT?

                    targetBounds = Target is PopupBase popup
                        ? DependencyService.Get<IDescendentBounds>().PageDescendentBounds(targetPage, popup.DecorativeContainerView)
                        : DependencyService.Get<IDescendentBounds>().PageDescendentBounds(targetPage, Target);

                    if (targetBounds.Width < 0 && targetBounds.Height < 0 && targetBounds.X < 0 && targetBounds.Y < 0)
                        return;

                    if (targetBounds.Right > targetPage.Bounds.Left && targetBounds.Left < targetPage.Bounds.Right && targetBounds.Bottom > targetPage.Bounds.Top && targetBounds.Top < targetPage.Bounds.Bottom)
                    {
                        //System.Diagnostics.Debug.WriteLine("Available=[" + x + ", " + y + "," + width + "," + height + "]");
                        //System.Diagnostics.Debug.WriteLine("targetBounds=[" + targetBounds + "]");
                        /// System.Diagnostics.Debug.WriteLine("targetPage.Bounds=[" + targetPage.Bounds + "]");
                        //if (Math.Abs(targetBounds.X - 468) < 0.001 && Math.Abs(targetBounds.Y - 675.5) < 0.001)
                        //    System.Diagnostics.Debug.WriteLine("");

                        var availL = targetBounds.Left - Margin.Left - PointerLength;
                        var availR = width - targetBounds.Right - Margin.Right - PointerLength;
                        var availT = targetBounds.Top - Margin.Top - PointerLength;
                        var availB = height - targetBounds.Bottom - Margin.Bottom - PointerLength;

                        if (WidthRequest > 0 && HorizontalOptions.Alignment != LayoutAlignment.Fill)
                        {
                            availL = Math.Min(availL, WidthRequest);
                            availR = Math.Min(availR, WidthRequest);
                            hzModal = Math.Min(hzModal, WidthRequest);
                        }
                        if (HeightRequest > 0 && VerticalOptions.Alignment != LayoutAlignment.Fill)
                        {
                            availT = Math.Min(availT, HeightRequest);
                            availB = Math.Min(availB, HeightRequest);
                            vtModal = Math.Min(vtModal, HeightRequest);
                        }

                        //double hzExtra = 0, vtExtra = 0;
                        double hzExtra = -1, vtExtra = -1;
                        PointerDirection hzPointerDir = PointerDirection.None;
                        PointerDirection vtPointerDir = PointerDirection.None;
                        SizeRequest vtSizeRequest = new SizeRequest();
                        SizeRequest hzSizeRequest = new SizeRequest();
                        if (PreferredPointerDirection != PointerDirection.None)
                        {
                            if (PreferredPointerDirection.DownAllowed() && availT > vtAvail)
                            {
                                vtPointerDir = PointerDirection.Down;
                                vtAvail = availT;
                            }
                            if (PreferredPointerDirection.UpAllowed() && availB > vtAvail)
                            {
                                vtPointerDir = PointerDirection.Up;
                                vtAvail = availB;
                            }
                            if (PreferredPointerDirection.LeftAllowed() && availR > hzAvail)
                            {
                                hzPointerDir = PointerDirection.Left;
                                hzAvail = availR;
                            }
                            if (PreferredPointerDirection.RightAllowed() && availL > hzAvail)
                            {
                                hzPointerDir = PointerDirection.Right;
                                hzAvail = availL;
                            }
                            if (vtAvail > 0)
                            {
                                var hz = hzModal - Padding.HorizontalThickness - shadowPadding.HorizontalThickness;
                                var vt = vtAvail - Padding.VerticalThickness - shadowPadding.VerticalThickness - PointerLength;
                                vtSizeRequest = _bubbleLayout.Content.Measure(hz, vt);
                                var hzx = hz - vtSizeRequest.Request.Width;
                                var vtx = vt - vtSizeRequest.Request.Height;
                                if (hzx > 0 && vtx >= 0)
                                    vtExtra = hzx + vtx;
                            }
                            if (hzAvail > 0)
                            {
                                var hz = hzAvail - Padding.HorizontalThickness - shadowPadding.HorizontalThickness - PointerLength;
                                var vt = vtModal - Padding.VerticalThickness - shadowPadding.VerticalThickness;
                                hzSizeRequest = _bubbleLayout.Content.Measure(hz, vt);
                                var hzx = hz - hzSizeRequest.Request.Width;
                                var vtx = vt - hzSizeRequest.Request.Height;
                                if (hzx > 0 && vtx >= 0)
                                    hzExtra = hzx + vtx;
                            }
                        }

                        if (hzExtra >= 0 || vtExtra >= 0)
                        {
                            if (hzExtra > vtExtra)
                            {
                                sizeRequest = hzSizeRequest;
                                pointerDir = hzPointerDir;
                                vtAvail = vtModal;
                            }
                            else
                            {
                                sizeRequest = vtSizeRequest;
                                pointerDir = vtPointerDir;
                                hzAvail = hzModal;
                            }
                            if (HorizontalOptions.Alignment != LayoutAlignment.Fill)
                                hzAvail = WidthRequest > 0 ? Math.Min(hzModal, WidthRequest) : sizeRequest.Request.Width + Padding.HorizontalThickness + shadowPadding.HorizontalThickness;
                            if (VerticalOptions.Alignment != LayoutAlignment.Fill)
                                vtAvail = HeightRequest > 0 ? Math.Min(vtModal, HeightRequest) : sizeRequest.Request.Height + Padding.VerticalThickness + shadowPadding.VerticalThickness;

                        }
                        else
                        {
                            //failed
                            hzAvail = 0; vtAvail = 0;
                            hzExtra = -1; vtExtra = -1;
                            hzPointerDir = PointerDirection.None;
                            vtPointerDir = PointerDirection.None;
                            vtSizeRequest = new SizeRequest();
                            hzSizeRequest = new SizeRequest();
                            if (PointerDirection.DownAllowed() && availT > vtAvail)
                            {
                                vtPointerDir = PointerDirection.Down;
                                vtAvail = availT;
                            }
                            if (PointerDirection.UpAllowed() && availB > vtAvail)
                            {
                                vtPointerDir = PointerDirection.Up;
                                vtAvail = availB;
                            }
                            if (PointerDirection.LeftAllowed() && availR > hzAvail)
                            {
                                hzPointerDir = PointerDirection.Left;
                                hzAvail = availR;
                            }
                            if (PointerDirection.RightAllowed() && availL > hzAvail)
                            {
                                hzPointerDir = PointerDirection.Right;
                                hzAvail = availL;
                            }
                            if (vtAvail > 0)
                            {
                                var hz = hzModal - Padding.HorizontalThickness - shadowPadding.HorizontalThickness;
                                var vt = vtAvail - Padding.VerticalThickness - shadowPadding.VerticalThickness - PointerLength;
                                vtSizeRequest = _bubbleLayout.Content.Measure(hz, vt);
                                var hzx = hz - vtSizeRequest.Request.Width;
                                var vtx = vt - vtSizeRequest.Request.Height;
                                if (hzx > 0 && vtx >= 0)
                                    vtExtra = hzx + vtx;
                            }
                            if (hzAvail > 0)
                            {
                                var hz = hzAvail - Padding.HorizontalThickness - shadowPadding.HorizontalThickness - PointerLength;
                                var vt = vtModal - Padding.VerticalThickness - shadowPadding.VerticalThickness;
                                hzSizeRequest = _bubbleLayout.Content.Measure(hz, vt);
                                var hzx = hz - hzSizeRequest.Request.Width;
                                var vtx = vt - hzSizeRequest.Request.Height;
                                if (hzx > 0 && vtx >= 0)
                                    hzExtra = hzx + vtx;
                            }
                            if (hzExtra >= 0 || vtExtra >= 0)
                            {
                                if (hzExtra > vtExtra)
                                {
                                    sizeRequest = hzSizeRequest;
                                    pointerDir = hzPointerDir;
                                    vtAvail = vtModal;
                                }
                                else
                                {
                                    sizeRequest = vtSizeRequest;
                                    pointerDir = vtPointerDir;
                                    hzAvail = hzModal;
                                }
                                if (HorizontalOptions.Alignment != LayoutAlignment.Fill)
                                    hzAvail = WidthRequest > 0 ? Math.Min(hzModal, WidthRequest) : sizeRequest.Request.Width + Padding.HorizontalThickness + shadowPadding.HorizontalThickness;
                                if (VerticalOptions.Alignment != LayoutAlignment.Fill)
                                    vtAvail = HeightRequest > 0 ? Math.Min(vtModal, HeightRequest) : sizeRequest.Request.Height + Padding.VerticalThickness + shadowPadding.VerticalThickness;

                            }
                        }
                    }
                }

                // IF WE GOT HERE AND THERE ISN"T A pointerDir, THEN THERE WASN"T A BEST FIT

                _bubbleLayout.PointerDirection = pointerDir;
                if (pointerDir == PointerDirection.None)
                {
                    //System.Diagnostics.Debug.WriteLine("hzModal: " + hzModal + "  vtModal: " + vtModal);

                    sizeRequest = _bubbleLayout.Content.Measure(hzModal - Padding.HorizontalThickness - shadowPadding.HorizontalThickness,
                                                vtModal - Padding.VerticalThickness - shadowPadding.VerticalThickness,
                                                MeasureFlags.None);

                    //System.Diagnostics.Debug.WriteLine("szReq: " + sizeRequest);

                    if (HorizontalOptions.Alignment != LayoutAlignment.Fill)
                        hzModal = WidthRequest > 0 ? Math.Min(hzModal, WidthRequest) : sizeRequest.Request.Width + Padding.HorizontalThickness + shadowPadding.HorizontalThickness;
                    if (VerticalOptions.Alignment != LayoutAlignment.Fill)
                        vtModal = HeightRequest > 0 ? Math.Min(vtModal, HeightRequest) : sizeRequest.Request.Height + Padding.VerticalThickness + shadowPadding.VerticalThickness;


                    double contentX = 0;
                    switch (HorizontalOptions.Alignment)
                    {
                        case LayoutAlignment.Center: contentX = width / 2.0 - hzModal / 2.0; break;
                        case LayoutAlignment.Start: contentX = Margin.Left + shadowPadding.Left; break;
                        case LayoutAlignment.End: contentX = width - Margin.Right - shadowPadding.HorizontalThickness - hzModal; break;
                        case LayoutAlignment.Fill: contentX = Margin.Left + shadowPadding.Left; break;
                    }
                    //var contentX = double.IsNegativeInfinity(Location.X) || HorizontalOptions.Alignment == LayoutAlignment.Fill ? width / 2.0 - rboxSize.Width / 2.0 : Location.X;
                    double contentY = 0;
                    switch (VerticalOptions.Alignment)
                    {
                        case LayoutAlignment.Center: contentY = height / 2.0 - vtModal / 2.0; break;
                        case LayoutAlignment.Start: contentY = Margin.Top + shadowPadding.Top; break;
                        case LayoutAlignment.End: contentY = height - Margin.Bottom - shadowPadding.VerticalThickness - vtModal; break;
                        case LayoutAlignment.Fill: contentY = height / 2.0 - vtModal / 2.0; break;
                    }


                    //var contentX = width / 2.0 - hzModal / 2.0;
                    //var contentY = height / 2.0 - vtModal / 2.0;
                    bounds = new Rectangle(contentX, contentY, hzModal, vtModal);
                    Xamarin.Forms.Layout.LayoutChildIntoBoundingRegion(_bubbleLayout, bounds);
                    //System.Diagnostics.Debug.WriteLine("bounds=" + bounds);
                }
                else
                {
                    if (HorizontalOptions.Alignment == LayoutAlignment.Fill)
                    {
                        if (pointerDir.IsVertical())
                            hzAvail = width - Margin.HorizontalThickness;
                    }
                    if (VerticalOptions.Alignment == LayoutAlignment.Fill)
                    {
                        if (pointerDir.IsHorizontal())
                            vtAvail = height - Margin.VerticalThickness;
                    }
                    //System.Diagnostics.Debug.WriteLine("===============================");
                    Tuple<double, float> tuple;
                    if (pointerDir.IsVertical())
                    {
                        //System.Diagnostics.Debug.WriteLine("\t\t rboxSize=[" + rboxSize + "] targetBounds=[" + targetBounds + "]");
                        if (UsePoint)
                        {
                            tuple = StartAndPointerLocation(hzAvail, Point.X + targetBounds.Left, 0, width);
                            bounds = new Rectangle(
                                new Point(
                                    tuple.Item1 + x,
                                    (pointerDir == PointerDirection.Up ? Point.Y + targetBounds.Top : Point.Y + targetBounds.Top - vtAvail - PointerLength) + y),
                                new Size(hzAvail, vtAvail + PointerLength)
                            );
                        }
                        else
                        {
                            tuple = StartAndPointerLocation(hzAvail, targetBounds.Left, targetBounds.Width, width);
                            bounds = new Rectangle(
                                new Point(
                                    tuple.Item1 + x,
                                    (pointerDir == PointerDirection.Up ? targetBounds.Bottom : targetBounds.Top - vtAvail - PointerLength) + y),
                                new Size(hzAvail, vtAvail + PointerLength)
                            );
                        }
                    }
                    else
                    {
                        //System.Diagnostics.Debug.WriteLine("\t\t rboxSize=[" + rboxSize + "] targetBounds=[" + targetBounds + "]");
                        if (UsePoint)
                        {
                            tuple = StartAndPointerLocation(vtAvail, Point.Y + targetBounds.Top, 0, height);
                            bounds = new Rectangle(
                                new Point(
                                    (pointerDir == PointerDirection.Left ? Point.X + targetBounds.Left : Point.X + targetBounds.Left - hzAvail - PointerLength) + x,
                                    tuple.Item1 + y),
                                new Size(hzAvail + PointerLength, vtAvail)
                            );
                        }
                        else
                        {
                            //System.Diagnostics.Debug.WriteLine("========================================");
                            tuple = StartAndPointerLocation(vtAvail, targetBounds.Top, targetBounds.Height, height);
                            //System.Diagnostics.Debug.WriteLine("PointerDir=[" + pointerDir + "]");
                            //System.Diagnostics.Debug.WriteLine("tuple=[" + tuple + "]");
                            bounds = new Rectangle(
                                new Point(
                                    (pointerDir == PointerDirection.Left ? targetBounds.Right : targetBounds.Left - hzAvail - PointerLength) + x,
                                    tuple.Item1 + y),
                                new Size(hzAvail + PointerLength, vtAvail)
                            );
                            //System.Diagnostics.Debug.WriteLine("bounds=[" + bounds + "]");
                        }
                    }
                    _bubbleLayout.PointerAxialPosition = tuple.Item2;
                    var newBounds = new Rectangle(bounds.X - targetPage.Padding.Left, bounds.Y - targetPage.Padding.Top, bounds.Width, bounds.Height);
                    //System.Diagnostics.Debug.WriteLine("\t\t BubblePopupLayoutChildIntoBoundingRegtion(_bubbleLayout, " + newBounds + ")");
                    Xamarin.Forms.Layout.LayoutChildIntoBoundingRegion(_bubbleLayout, newBounds);
                    //System.Diagnostics.Debug.WriteLine("===============================");
                    _lastLayout = DateTime.Now;
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


        #endregion


    }
}


