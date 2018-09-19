using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using static Xamarin.Forms.Grid;

namespace Forms9Patch
{
    /// <summary>
    /// Forms9Patch Grid layout.
    /// </summary>
    public class Grid : Xamarin.Forms.Grid, ILayout
    {
        /* NG
        public static readonly BindableProperty RowProperty = Xamarin.Forms.Grid.RowProperty;

        public static readonly BindableProperty RowSpanProperty = Xamarin.Forms.Grid.RowSpanProperty;

        public static readonly BindableProperty ColumnProperty = Xamarin.Forms.Grid.ColumnProperty;

        public static readonly BindableProperty ColumnSpanProperty = Xamarin.Forms.Grid.ColumnSpanProperty;

        public static readonly BindableProperty RowSpacingProperty = BindableProperty.Create("RowSpacing", typeof(double), typeof(Grid), 6d,
                                                                                             propertyChanged: (bindable, oldValue, newValue) => ((Grid)bindable)._grid.SetValue(Xamarin.Forms.Grid.RowSpacingProperty, newValue));

        public static readonly BindableProperty ColumnSpacingProperty = BindableProperty.Create("ColumnSpacing", typeof(double), typeof(Grid), 6d,
                                                                                                propertyChanged: (bindable, oldValue, newValue) => ((Grid)bindable)._grid.SetValue(Xamarin.Forms.Grid.ColumnSpacingProperty, newValue));

        public static readonly BindableProperty ColumnDefinitionsProperty = BindableProperty.Create("ColumnDefinitions", typeof(ColumnDefinitionCollection), typeof(Grid), null,
            validateValue: (bindable, value) => value != null, propertyChanged: (bindable, oldvalue, newvalue) =>
            {
                ((Grid)bindable)._grid.SetValue(Xamarin.Forms.Grid.ColumnDefinitionsProperty, newvalue);
            });

        public static readonly BindableProperty RowDefinitionsProperty = BindableProperty.Create("RowDefinitions", typeof(RowDefinitionCollection), typeof(Grid), null,
            validateValue: (bindable, value) => value != null, propertyChanged: (bindable, oldvalue, newvalue) =>
            {
                ((Grid)bindable)._grid.SetValue(Xamarin.Forms.Grid.RowDefinitionsProperty, newvalue);
            });

        public new IGridList<View> Children => _grid.Children;

        public ColumnDefinitionCollection ColumnDefinitions
        {
            get => _grid.ColumnDefinitions;
            set => _grid.ColumnDefinitions = value;
        }

        public double ColumnSpacing
        {
            get => _grid.ColumnSpacing;
            set => _grid.ColumnSpacing = value;
        }

        public RowDefinitionCollection RowDefinitions
        {
            get => _grid.RowDefinitions;
            set => _grid.RowDefinitions = value;
        }

        public double RowSpacing
        {
            get => _grid.RowSpacing;
            set => _grid.RowSpacing = value;
        }

        public static int GetColumn(BindableObject bindable) => Xamarin.Forms.Grid.GetColumn(bindable);

        public static int GetColumnSpan(BindableObject bindable) => Xamarin.Forms.Grid.GetColumnSpan(bindable);

        public static int GetRow(BindableObject bindable) => Xamarin.Forms.Grid.GetRow(bindable);

        public static int GetRowSpan(BindableObject bindable) => Xamarin.Forms.Grid.GetRowSpan(bindable);

        public static void SetColumn(BindableObject bindable, int value) => Xamarin.Forms.Grid.SetColumn(bindable, value);

        public static void SetColumnSpan(BindableObject bindable, int value) => Xamarin.Forms.Grid.SetColumnSpan(bindable, value);

        public static void SetRow(BindableObject bindable, int value) => Xamarin.Forms.Grid.SetRow(bindable, value);

        public static void SetRowSpan(BindableObject bindable, int value) => Xamarin.Forms.Grid.SetRowSpan(bindable, value);

        public void InvalidateMeasureInernalNonVirtual(InvalidationTrigger trigger) => _grid.InvalidateMeasureInernalNonVirtual(trigger);

        readonly Xamarin.Forms.Grid _grid;

        public Grid()
        {
            _grid = _xfLayout as Xamarin.Forms.Grid;
        }
        */

        #region Properties

        #region ILayout Properties

        #region IgnoreChildren
        /// <summary>
        /// The ignore children property.
        /// </summary>
        public static readonly BindableProperty IgnoreChildrenProperty = BindableProperty.Create("IgnoreChildren", typeof(bool), typeof(Grid), default(bool));
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Forms9Patch.Grid"/> ignore children.
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
        public static readonly BindableProperty LimitMinSizeToBackgroundImageSizeProperty = BindableProperty.Create("Forms9Patch.Grid.LimitMinSizeToBackgroundImageSize", typeof(bool), typeof(Grid), default(bool));
        public bool LimitMinSizeToBackgroundImageSize
        {
            get => (bool)GetValue(LimitMinSizeToBackgroundImageSizeProperty);
            set => SetValue(LimitMinSizeToBackgroundImageSizeProperty, value);
        }
        #endregion LimitMinSizeToBackgroundImageSize property

        #region IShape

        #region BackgroundColor property
        public static readonly new BindableProperty BackgroundColorProperty = BindableProperty.Create("Forms9Patch.Grid.BackgroundColor", typeof(Color), typeof(Grid), default(Color));

        public new Color BackgroundColor
        {
            get => (Color)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }
        #endregion BackgroundColor property

        #region HasShadow property
        public static readonly BindableProperty HasShadowProperty = BindableProperty.Create("Forms9Patch.Grid.HasShadow", typeof(bool), typeof(Grid), default(bool));

        public bool HasShadow
        {
            get => (bool)GetValue(HasShadowProperty);
            set => SetValue(HasShadowProperty, value);
        }
        #endregion HasShadow property

        #region ShadowInverted property
        public static readonly BindableProperty ShadowInvertedProperty = BindableProperty.Create("Forms9Patch.Grid.ShadowInverted", typeof(bool), typeof(Grid), default(bool));
        public bool ShadowInverted
        {
            get => (bool)GetValue(ShadowInvertedProperty);
            set => SetValue(ShadowInvertedProperty, value);
        }
        #endregion ShadowInverted property

        #region OutlineColor property
        public static readonly BindableProperty OutlineColorProperty = BindableProperty.Create("Forms9Patch.Grid.OutlineColor", typeof(Color), typeof(Grid), default(Color));
        public Color OutlineColor
        {
            get => (Color)GetValue(OutlineColorProperty);
            set => SetValue(OutlineColorProperty, value);
        }
        #endregion OutlineColor property

        #region OutlineRadius property
        public static readonly BindableProperty OutlineRadiusProperty = BindableProperty.Create("Forms9Patch.Grid.OutlineRadius", typeof(float), typeof(Grid), default(float));
        public float OutlineRadius
        {
            get => (float)GetValue(OutlineRadiusProperty);
            set => SetValue(OutlineRadiusProperty, value);
        }
        #endregion OutlineRadius property

        #region OutlineWidth property
        public static readonly BindableProperty OutlineWidthProperty = BindableProperty.Create("Forms9Patch.Grid.OutlineWidth", typeof(float), typeof(Grid), default(float));
        public float OutlineWidth
        {
            get => (float)GetValue(OutlineWidthProperty);
            set => SetValue(OutlineWidthProperty, value);
        }
        #endregion OutlineWidth property

        #region ElementShape property
        public static readonly BindableProperty ElementShapeProperty = BindableProperty.Create("Forms9Patch.Grid.ElementShape", typeof(ElementShape), typeof(Grid), default(ElementShape));
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
        static Grid()
        {
            Settings.ConfirmInitialization();

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.ContentView"/> class.  Forms9Patch.ContentView is same as Forms9Patch.Frame - but with different default values.
        /// </summary>
        public Grid()
        {
            _f9pId = _instances++;
            RowDefinitions.ItemSizeChanged += (sender, e) => SetRowSpan(CurrentBackgroundImage, RowDefinitions.Count);
            ColumnDefinitions.ItemSizeChanged += (sender, e) => SetColumnSpan(CurrentBackgroundImage, ColumnDefinitions.Count);
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
                SetRowSpan(CurrentBackgroundImage, RowDefinitions.Count);
                SetColumnSpan(CurrentBackgroundImage, ColumnDefinitions.Count);
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

