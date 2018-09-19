using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using static Xamarin.Forms.AbsoluteLayout;

namespace Forms9Patch
{
    /*  NG!!
    public class AbsoluteLayout : Layout<Xamarin.Forms.AbsoluteLayout>
    {
        public static readonly BindableProperty LayoutFlagsProperty = Xamarin.Forms.AbsoluteLayout.LayoutFlagsProperty;

        public static readonly BindableProperty LayoutBoundsProperty = Xamarin.Forms.AbsoluteLayout.LayoutBoundsProperty;

        public new IAbsoluteList<View> Children => ((Xamarin.Forms.AbsoluteLayout)_xfLayout).Children;


        [TypeConverter(typeof(BoundsTypeConverter))]
        public static Rectangle GetLayoutBounds(BindableObject bindable) => Xamarin.Forms.AbsoluteLayout.GetLayoutBounds(bindable);

        public static AbsoluteLayoutFlags GetLayoutFlags(BindableObject bindable) => Xamarin.Forms.AbsoluteLayout.GetLayoutFlags(bindable);

        public static void SetLayoutBounds(BindableObject bindable, Rectangle bounds) => Xamarin.Forms.AbsoluteLayout.SetLayoutBounds(bindable, bounds);

        public static void SetLayoutFlags(BindableObject bindable, AbsoluteLayoutFlags flags) => Xamarin.Forms.AbsoluteLayout.SetLayoutFlags(bindable, flags);

    }
    */

    public class AbsoluteLayout : Xamarin.Forms.AbsoluteLayout, ILayout
    {
        #region Properties

        #region ILayout Properties

        #region IgnoreChildren
        /// <summary>
        /// The ignore children property.
        /// </summary>
        public static readonly BindableProperty IgnoreChildrenProperty = BindableProperty.Create("IgnoreChildren", typeof(bool), typeof(AbsoluteLayout), default(bool));
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Forms9Patch.AbsoluteLayout"/> ignore children.
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
        public static readonly BindableProperty LimitMinSizeToBackgroundImageSizeProperty = BindableProperty.Create("Forms9Patch.AbsoluteLayout.LimitMinSizeToBackgroundImageSize", typeof(bool), typeof(AbsoluteLayout), default(bool));
        public bool LimitMinSizeToBackgroundImageSize
        {
            get => (bool)GetValue(LimitMinSizeToBackgroundImageSizeProperty);
            set => SetValue(LimitMinSizeToBackgroundImageSizeProperty, value);
        }
        #endregion LimitMinSizeToBackgroundImageSize property

        #region IShape

        #region BackgroundColor property
        public static readonly new BindableProperty BackgroundColorProperty = BindableProperty.Create("Forms9Patch.AbsoluteLayout.BackgroundColor", typeof(Color), typeof(AbsoluteLayout), default(Color));

        public new Color BackgroundColor
        {
            get => (Color)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }
        #endregion BackgroundColor property

        #region HasShadow property
        public static readonly BindableProperty HasShadowProperty = BindableProperty.Create("Forms9Patch.AbsoluteLayout.HasShadow", typeof(bool), typeof(AbsoluteLayout), default(bool));

        public bool HasShadow
        {
            get => (bool)GetValue(HasShadowProperty);
            set => SetValue(HasShadowProperty, value);
        }
        #endregion HasShadow property

        #region ShadowInverted property
        public static readonly BindableProperty ShadowInvertedProperty = BindableProperty.Create("Forms9Patch.AbsoluteLayout.ShadowInverted", typeof(bool), typeof(AbsoluteLayout), default(bool));
        public bool ShadowInverted
        {
            get => (bool)GetValue(ShadowInvertedProperty);
            set => SetValue(ShadowInvertedProperty, value);
        }
        #endregion ShadowInverted property

        #region OutlineColor property
        public static readonly BindableProperty OutlineColorProperty = BindableProperty.Create("Forms9Patch.AbsoluteLayout.OutlineColor", typeof(Color), typeof(AbsoluteLayout), default(Color));
        public Color OutlineColor
        {
            get => (Color)GetValue(OutlineColorProperty);
            set => SetValue(OutlineColorProperty, value);
        }
        #endregion OutlineColor property

        #region OutlineRadius property
        public static readonly BindableProperty OutlineRadiusProperty = BindableProperty.Create("Forms9Patch.AbsoluteLayout.OutlineRadius", typeof(float), typeof(AbsoluteLayout), default(float));
        public float OutlineRadius
        {
            get => (float)GetValue(OutlineRadiusProperty);
            set => SetValue(OutlineRadiusProperty, value);
        }
        #endregion OutlineRadius property

        #region OutlineWidth property
        public static readonly BindableProperty OutlineWidthProperty = BindableProperty.Create("Forms9Patch.AbsoluteLayout.OutlineWidth", typeof(float), typeof(AbsoluteLayout), default(float));
        public float OutlineWidth
        {
            get => (float)GetValue(OutlineWidthProperty);
            set => SetValue(OutlineWidthProperty, value);
        }
        #endregion OutlineWidth property

        #region ElementShape property
        public static readonly BindableProperty ElementShapeProperty = BindableProperty.Create("Forms9Patch.AbsoluteLayout.ElementShape", typeof(ElementShape), typeof(AbsoluteLayout), default(ElementShape));
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
        static AbsoluteLayout()
        {
            Settings.ConfirmInitialization();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.ContentView"/> class.  Forms9Patch.ContentView is same as Forms9Patch.Frame - but with different default values.
        /// </summary>
        public AbsoluteLayout()
        {
            _f9pId = _instances++;
            SetLayoutBounds(_fallbackBackgroundImage, new Rectangle(0, 0, 1, 1));
            SetLayoutFlags(_fallbackBackgroundImage, AbsoluteLayoutFlags.All);
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
        Xamarin.Forms.Thickness IShape.ShadowPadding() => ((IShape)CurrentBackgroundImage).ShadowPadding();
        #endregion


        #region Property Change Handlers
        protected override void OnPropertyChanging([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanging(propertyName);
            if (propertyName == BackgroundImageProperty.PropertyName)
            {
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
                SetLayoutBounds(CurrentBackgroundImage, new Rectangle(0, 0, 1, 1));
                SetLayoutFlags(CurrentBackgroundImage, AbsoluteLayoutFlags.All);
                Children.Insert(0, CurrentBackgroundImage);
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

