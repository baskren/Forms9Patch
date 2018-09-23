using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using static Xamarin.Forms.RelativeLayout;

namespace Forms9Patch
{
    /// <summary>
    /// Forms9Patch RelativeLayout.
    /// </summary>
    public class RelativeLayout : Xamarin.Forms.RelativeLayout, ILayout
    {
        /*
        public static readonly BindableProperty XConstraintProperty = Xamarin.Forms.RelativeLayout.XConstraintProperty;

        public static readonly BindableProperty YConstraintProperty = Xamarin.Forms.RelativeLayout.YConstraintProperty;

        public static readonly BindableProperty WidthConstraintProperty = Xamarin.Forms.RelativeLayout.WidthConstraintProperty;

        public static readonly BindableProperty HeightConstraintProperty = Xamarin.Forms.RelativeLayout.HeightConstraintProperty;

        public static readonly BindableProperty BoundsConstraintProperty = Xamarin.Forms.RelativeLayout.BoundsConstraintProperty;

        Xamarin.Forms.RelativeLayout _layout;

        public RelativeLayout()
        {
            VerticalOptions = HorizontalOptions = LayoutOptions.FillAndExpand;
            _layout = _xfLayout as Xamarin.Forms.RelativeLayout;
        }

        public new IRelativeList<View> Children => _layout.Children;

        public static BoundsConstraint GetBoundsConstraint(BindableObject bindable) => Xamarin.Forms.RelativeLayout.GetBoundsConstraint(bindable);

        public static Constraint GetHeightConstraint(BindableObject bindable) => Xamarin.Forms.RelativeLayout.GetHeightConstraint(bindable);

        public static Constraint GetWidthConstraint(BindableObject bindable) => Xamarin.Forms.RelativeLayout.GetWidthConstraint(bindable);

        public static Constraint GetXConstraint(BindableObject bindable) => Xamarin.Forms.RelativeLayout.GetXConstraint(bindable);

        public static Constraint GetYConstraint(BindableObject bindable) => Xamarin.Forms.RelativeLayout.GetYConstraint(bindable);

        public static void SetBoundsConstraint(BindableObject bindable, BoundsConstraint value) => Xamarin.Forms.RelativeLayout.SetBoundsConstraint(bindable, value);

        public static void SetHeightConstraint(BindableObject bindable, Constraint value) => Xamarin.Forms.RelativeLayout.SetHeightConstraint(bindable, value);

        public static void SetWidthConstraint(BindableObject bindable, Constraint value) => Xamarin.Forms.RelativeLayout.SetWidthConstraint(bindable, value);

        public static void SetXConstraint(BindableObject bindable, Constraint value) => Xamarin.Forms.RelativeLayout.SetXConstraint(bindable, value);

        public static void SetYConstraint(BindableObject bindable, Constraint value) => Xamarin.Forms.RelativeLayout.SetYConstraint(bindable, value);
        */

        #region Properties

        #region ILayout Properties

        #region IgnoreChildren
        /// <summary>
        /// The ignore children property.
        /// </summary>
        public static readonly BindableProperty IgnoreChildrenProperty = BindableProperty.Create("IgnoreChildren", typeof(bool), typeof(RelativeLayout), default(bool));
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Forms9Patch.RelativeLayout"/> ignore children.
        /// </summary>
        /// <value><c>true</c> if ignore children; otherwise, <c>false</c>.</value>
        public bool IgnoreChildren
        {
            get => (bool)GetValue(IgnoreChildrenProperty);
            set => SetValue(IgnoreChildrenProperty, value);
        }
        #endregion IgnoreChildren

        #region IBackground

        #region BackgroundImage property
        public static readonly BindableProperty BackgroundImageProperty = ShapeBase.BackgroundImageProperty;
        public Image BackgroundImage
        {
            get => (Image)GetValue(BackgroundImageProperty);
            set => SetValue(BackgroundImageProperty, value);
        }
        #endregion BackgroundImage property

        #region LimitMinSizeToBackgroundImageSize property
        public static readonly BindableProperty LimitMinSizeToBackgroundImageSizeProperty = BindableProperty.Create("Forms9Patch.RelativeLayout.LimitMinSizeToBackgroundImageSize", typeof(bool), typeof(RelativeLayout), default(bool));
        public bool LimitMinSizeToBackgroundImageSize
        {
            get => (bool)GetValue(LimitMinSizeToBackgroundImageSizeProperty);
            set => SetValue(LimitMinSizeToBackgroundImageSizeProperty, value);
        }
        #endregion LimitMinSizeToBackgroundImageSize property

        #region IShape

        #region BackgroundColor property
        public static readonly new BindableProperty BackgroundColorProperty = BindableProperty.Create("Forms9Patch.RelativeLayout.BackgroundColor", typeof(Color), typeof(RelativeLayout), default(Color));

        public new Color BackgroundColor
        {
            get => (Color)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }
        #endregion BackgroundColor property

        #region HasShadow property
        public static readonly BindableProperty HasShadowProperty = BindableProperty.Create("Forms9Patch.RelativeLayout.HasShadow", typeof(bool), typeof(RelativeLayout), default(bool));

        public bool HasShadow
        {
            get => (bool)GetValue(HasShadowProperty);
            set => SetValue(HasShadowProperty, value);
        }
        #endregion HasShadow property

        #region ShadowInverted property
        public static readonly BindableProperty ShadowInvertedProperty = BindableProperty.Create("Forms9Patch.RelativeLayout.ShadowInverted", typeof(bool), typeof(RelativeLayout), default(bool));
        public bool ShadowInverted
        {
            get => (bool)GetValue(ShadowInvertedProperty);
            set => SetValue(ShadowInvertedProperty, value);
        }
        #endregion ShadowInverted property

        #region OutlineColor property
        public static readonly BindableProperty OutlineColorProperty = BindableProperty.Create("Forms9Patch.RelativeLayout.OutlineColor", typeof(Color), typeof(RelativeLayout), default(Color));
        public Color OutlineColor
        {
            get => (Color)GetValue(OutlineColorProperty);
            set => SetValue(OutlineColorProperty, value);
        }
        #endregion OutlineColor property

        #region OutlineRadius property
        public static readonly BindableProperty OutlineRadiusProperty = BindableProperty.Create("Forms9Patch.RelativeLayout.OutlineRadius", typeof(float), typeof(RelativeLayout), default(float));
        public float OutlineRadius
        {
            get => (float)GetValue(OutlineRadiusProperty);
            set => SetValue(OutlineRadiusProperty, value);
        }
        #endregion OutlineRadius property

        #region OutlineWidth property
        public static readonly BindableProperty OutlineWidthProperty = BindableProperty.Create("Forms9Patch.RelativeLayout.OutlineWidth", typeof(float), typeof(RelativeLayout), default(float));
        public float OutlineWidth
        {
            get => (float)GetValue(OutlineWidthProperty);
            set => SetValue(OutlineWidthProperty, value);
        }
        #endregion OutlineWidth property

        #region ElementShape property
        public static readonly BindableProperty ElementShapeProperty = BindableProperty.Create("Forms9Patch.RelativeLayout.ElementShape", typeof(ElementShape), typeof(RelativeLayout), default(ElementShape));
        public ElementShape ElementShape
        {
            get => (ElementShape)GetValue(ElementShapeProperty);
            set => SetValue(ElementShapeProperty, value);
        }
        #endregion ElementShape property

        #region IElement properties
        public int InstanceId => _f9pId;
        #endregion IElement properties

        #endregion IShape properties

        #endregion IBackground properties

        #endregion ILayout properties


        #endregion


        #region Private Fields and Properties
        static int _instances;
        protected readonly int _f9pId;

        readonly Image _fallbackBackgroundImage = new Image
        {
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill,
        };

        Image CurrentBackgroundImage => BackgroundImage ?? _fallbackBackgroundImage;

        /*
        ObservableCollection<Element> _baseInternalChildren;
        ObservableCollection<Element> BaseInternalChildren
        {
            get
            {
                if (_baseInternalChildren == null)
                {
                    _baseInternalChildren = (ObservableCollection<Element>)P42.Utils.ReflectionExtensions.GetPropertyValue(this, "InternalChildren");
                    _baseInternalChildren?.Insert(0, CurrentBackgroundImage);
                }
                return _baseInternalChildren;
            }
        }
        */
        #endregion


        #region Constructors
        static RelativeLayout()
        {
            Settings.ConfirmInitialization();

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.ContentView"/> class.  Forms9Patch.ContentView is same as Forms9Patch.Frame - but with different default values.
        /// </summary>
        public RelativeLayout()
        {
            _f9pId = _instances++;
            SetXConstraint(_fallbackBackgroundImage, Constraint.RelativeToParent((p) => 0));
            SetYConstraint(_fallbackBackgroundImage, Constraint.RelativeToParent((p) => 0));
            SetWidthConstraint(_fallbackBackgroundImage, Constraint.RelativeToParent((p) => p.Width));
            SetHeightConstraint(_fallbackBackgroundImage, Constraint.RelativeToParent((p) => p.Height));


        }
        #endregion


        #region Description
        /// <summary>
        /// Returns a <see cref="System.String"/> that describes the current <see cref="Forms9Patch.Frame"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that describes the current <see cref="Forms9Patch.Frame"/>.</returns>
        public string Description() { return string.Format("[{0}.{1}]", GetType(), _f9pId); }

        /// <summary>
        /// Returns a <see cref="System.String"/> that describes the current <see cref="Forms9Patch.Frame"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Description();
        #endregion


        #region IShape Methods
        //Xamarin.Forms.Thickness IShape.ShadowPadding() => ((IShape)CurrentBackgroundImage).ShadowPadding();
        #endregion


        #region Property Change Handlers
        protected override void OnPropertyChanging([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanging(propertyName);
            if (propertyName == BackgroundImageProperty.PropertyName)
            {
                //CurrentBackgroundImage.SizeChanged -= OnCurrentBackgroundSizeChanged;

                //BaseInternalChildren.Remove(BackgroundImage);
                //BaseInternalChildren.Remove(_fallbackBackgroundImage);
                if (BackgroundImage != null && Children.Contains(BackgroundImage))
                    Children.Remove(BackgroundImage);
                if (_fallbackBackgroundImage != null && Children.Contains(_fallbackBackgroundImage))
                    Children.Remove(_fallbackBackgroundImage);
            }

        }



        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == BackgroundImageProperty.PropertyName)
            {
                SetXConstraint(CurrentBackgroundImage, Constraint.Constant(0));
                SetYConstraint(CurrentBackgroundImage, Constraint.Constant(0));
                //SetWidthConstraint(CurrentBackgroundImage, Constraint.RelativeToParent((p) => 200));
                //SetHeightConstraint(CurrentBackgroundImage, Constraint.RelativeToParent((p) => 200));
                SetWidthConstraint(CurrentBackgroundImage, Constraint.FromExpression(() => Width));
                SetHeightConstraint(CurrentBackgroundImage, Constraint.FromExpression(() => Height));
                //BaseInternalChildren.Insert(0, CurrentBackgroundImage);
                Children.Insert(0, CurrentBackgroundImage);

                //CurrentBackgroundImage.SizeChanged += OnCurrentBackgroundSizeChanged;

            }
            else if (propertyName == BackgroundColorProperty.PropertyName)
                CurrentBackgroundImage.BackgroundColor = _fallbackBackgroundImage.BackgroundColor = BackgroundColor;
            else if (propertyName == HasShadowProperty.PropertyName)
                CurrentBackgroundImage.HasShadow = _fallbackBackgroundImage.HasShadow = HasShadow;
            else if (propertyName == ShadowInvertedProperty.PropertyName)
                CurrentBackgroundImage.ShadowInverted = _fallbackBackgroundImage.ShadowInverted = ShadowInverted;
            else if (propertyName == OutlineColorProperty.PropertyName)
                CurrentBackgroundImage.OutlineColor = _fallbackBackgroundImage.OutlineColor = OutlineColor;
            else if (propertyName == OutlineRadiusProperty.PropertyName)
                CurrentBackgroundImage.OutlineRadius = _fallbackBackgroundImage.OutlineRadius = OutlineRadius;
            else if (propertyName == OutlineWidthProperty.PropertyName)
                CurrentBackgroundImage.OutlineWidth = _fallbackBackgroundImage.OutlineWidth = OutlineWidth;
            else if (propertyName == ElementShapeProperty.PropertyName)
                CurrentBackgroundImage.ElementShape = _fallbackBackgroundImage.ElementShape = ElementShape;
        }

        private void OnCurrentBackgroundSizeChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("CurrentBackgroundImage.Bounds:" + CurrentBackgroundImage.Bounds);
        }
        #endregion


        #region IgnoreChildren handlers
        /// <summary>
        /// Shoulds the invalidate on child added.
        /// </summary>
        /// <returns><c>true</c>, if invalidate on child added was shoulded, <c>false</c> otherwise.</returns>
        /// <param name="child">Child.</param>
        protected override bool ShouldInvalidateOnChildAdded(View child)
        {
            return !IgnoreChildren; // stop pestering me
        }

        /// <summary>
        /// Shoulds the invalidate on child removed.
        /// </summary>
        /// <returns><c>true</c>, if invalidate on child removed was shoulded, <c>false</c> otherwise.</returns>
        /// <param name="child">Child.</param>
        protected override bool ShouldInvalidateOnChildRemoved(View child)
        {
            return !IgnoreChildren; // go away and leave me alone
        }

        /// <summary>
        /// Ons the child measure invalidated.
        /// </summary>
        protected override void OnChildMeasureInvalidated()
        {
            // I'm ignoring you.  You'll take whatever size I want to give
            // you.  And you'll like it.
            if (!IgnoreChildren)
                base.OnChildMeasureInvalidated();
        }
        #endregion IgnoreChildren handlers

    }
}

