using Xamarin.Forms;
using System;
using P42.Utils;
using Xamarin.Forms.Internals;
using FormsGestures;

namespace Forms9Patch
{
    /// <summary>
    /// Forms9Patch.Label
    /// </summary>
    [ContentProperty(nameof(HtmlText))]
    public class Label : Xamarin.Forms.Label, ILabel, IElement //View, IFontElement
    {


        #region Obsolete Properties
        /// <summary>
        /// backing store for Fit property
        /// </summary>
        [Obsolete("FitProperty is obsolete.  Use AutoFitProperty instead.")]
        public static readonly BindableProperty FitProperty = BindableProperty.Create(nameof(Fit), typeof(LabelFit), typeof(Label), LabelFit.None, propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is Label label && newValue is LabelFit fit)
            {
                switch (fit)
                {
                    case LabelFit.None: label.AutoFit = AutoFit.None; break;
                    case LabelFit.Width: label.AutoFit = AutoFit.Width; break;
                    case LabelFit.Lines: label.AutoFit = AutoFit.Lines; break;
                }
            }
        });

        /// <summary>
        /// Gets/Sets the Fit property
        /// </summary>
        [Obsolete("Fit property is obsolete.  Use AutoFit property instead.")]
        public LabelFit Fit
        {
            get { return (LabelFit)GetValue(FitProperty); }
            set { SetValue(FitProperty, value); }
        }

        /// <summary>
        /// OBSOLETE: Use FittedFontSizeProperty instead
        /// </summary>
        [Obsolete("Use FittedFontSizeProperty instead.", true)]
        public static readonly BindableProperty ActualFontSizeProperty = BindableProperty.Create(nameof(ActualFontSize), typeof(double), typeof(Label), default(double));
        /// <summary>
        /// OBSOLETE: Use FittedFontSize property instead
        /// </summary>
        [Obsolete("Use FittedFontSize property instead.", true)]
        public double ActualFontSize
        {
            get { return (double)GetValue(ActualFontSizeProperty); }
            set { SetValue(ActualFontSizeProperty, value); }
        }
        #endregion


        #region Static Properties
        /// <summary>
        /// The default size of labels rendered on the current platform
        /// </summary>
        public static double DefaultFontSize { get; internal set; }
        #endregion


        #region Properties

        #region HtmlText property
        /// <summary>
        /// Backing store for the formatted text property.
        /// </summary>
        public static readonly BindableProperty HtmlTextProperty = BindableProperty.Create(nameof(HtmlText), typeof(string), typeof(Label));
        /// <summary>
        /// Gets or sets the formatted text.
        /// </summary>
        /// <value>The formatted text.</value>
        public string HtmlText
        {
            get => (string)GetValue(HtmlTextProperty);
            set => SetValue(HtmlTextProperty, value);
        }
        #endregion

        #region F9PFormattedString
        // not public, so not bindable!
        internal static readonly BindableProperty F9PFormattedStringProperty = BindableProperty.Create(nameof(F9PFormattedString), typeof(F9PFormattedString), typeof(Label), null);
        internal F9PFormattedString F9PFormattedString
        {
            get => (F9PFormattedString)GetValue(F9PFormattedStringProperty);
            set => SetValue(F9PFormattedStringProperty, value);
        }
        #endregion

        #region AutoFit property
        /// <summary>
        /// The backing store for the AutoFit property.
        /// </summary>
        public static readonly BindableProperty AutoFitProperty = BindableProperty.Create(nameof(AutoFit), typeof(AutoFit), typeof(Label), AutoFit.None);
        /// <summary>
        /// Gets or sets the fit method.  Ignored if the Width and Height is not fixed by a parent, HeightRequest, and/or WidthRequest.
        /// </summary>
        /// <value>The fit.</value>
        public AutoFit AutoFit
        {
            get => (AutoFit)GetValue(AutoFitProperty);
            set => SetValue(AutoFitProperty, value);
        }
        #endregion

        #region Lines property
        /// <summary>
        /// The backing store for the lines property.
        /// </summary>
        public static readonly BindableProperty LinesProperty = BindableProperty.Create(nameof(Lines), typeof(int), typeof(Label), 0);
        /// <summary>
        /// Gets or sets the number of lines used in a fit.  If zero and fit is not AutoFit.None or ignored, will maximize the font size to best width and height with minimum number of lines.
        /// </summary>
        /// <value>The lines.</value>
        public int Lines
        {
            get => (int)GetValue(LinesProperty);
            set => SetValue(LinesProperty, value);
        }
        #endregion

        #region MinFontSize
        /// <summary>
        /// The backing store for the minimum font size property.
        /// </summary>
        public static readonly BindableProperty MinFontSizeProperty = BindableProperty.Create(nameof(MinFontSize), typeof(double), typeof(Label), -1.0);
        /// <summary>
        /// Gets or sets the minimum size of the font allowed during an autofit. 
        /// </summary>
        /// <value>The minimum size of the font.  Default=4</value>
        public double MinFontSize
        {
            get => (double)GetValue(MinFontSizeProperty);
            set => SetValue(MinFontSizeProperty, value);
        }
        #endregion

        #region IsDynamicallySize property
        /// <summary>
        /// The backing store for fixed size property.
        /// </summary>
        public static readonly BindableProperty IsDynamicallySizedProperty = BindableProperty.Create(nameof(IsDynamicallySized), typeof(bool), typeof(Label), true, BindingMode.TwoWay);
        /// <summary>
        /// Gets if the size of the label has been fixed by a parent element, the HeightRequest and/or the WidthRequest properties.
        /// </summary>
        /// <value>Is the label fixed in size.</value>
        public bool IsDynamicallySized
        {
            get => (bool)GetValue(IsDynamicallySizedProperty);
            private set => SetValue(IsDynamicallySizedProperty, value);
        }
        #endregion

        #region FittedFontSize property
        DateTime _lastTimeFittedFontSizeSet = DateTime.MinValue;

        internal static readonly BindablePropertyKey FittedFontSizePropertyKey = BindableProperty.CreateReadOnly(nameof(FittedFontSize), typeof(double), typeof(Label), -1.0);
        /// <summary>
        /// Backing store for the actual font size property after fitting.
        /// </summary>
        public static readonly BindableProperty FittedFontSizeProperty = FittedFontSizePropertyKey.BindableProperty;
        /// <summary>
        /// Gets the actual size of the font (after fitting).
        /// </summary>
        /// <value>The actual size of the font.</value>
        public double FittedFontSize
        {
            get => (double)GetValue(FittedFontSizeProperty);
            internal set
            {
                if (Math.Abs(value - FittedFontSize) > 0.01)
                {
                    SetValue(FittedFontSizePropertyKey, value);
                    _lastTimeFittedFontSizeSet = DateTime.Now;
                    Device.StartTimer(TimeSpan.FromMilliseconds(25), () =>
                    {
                        if (DateTime.Now - _lastTimeFittedFontSizeSet < TimeSpan.FromMilliseconds(50))
                            return true;
                        FittedFontSizeChanged?.Invoke(this, value);
                        return false;
                    });
                }
            }
        }
        #endregion

        #region SynchronizedFontSize property

        /// <summary>
        /// backing store for SynchronizedFontSize property
        /// </summary>
        public static readonly BindableProperty SynchronizedFontSizeProperty = BindableProperty.Create(nameof(SynchronizedFontSize), typeof(double), typeof(Label), -1.0);
        /// <summary>
        /// Gets/Sets the SynchronizedFontSize property
        /// </summary>
        public double SynchronizedFontSize
        {
            get => (double)GetValue(SynchronizedFontSizeProperty);
            set => SetValue(SynchronizedFontSizeProperty, value);
        }
        #endregion SynchronizedFontSize property

        #region IElement
        /// <summary>
        /// Incremental identity (starting at 0) of instance of this class
        /// </summary>
        public int InstanceId => _id;
        #endregion

        #endregion


        #region events
        /// <summary>
        /// Occurs when HtmlText wrapped with an action (&lt;a&gt;) tag is tapped.
        /// </summary>
        public event EventHandler<ActionTagEventArgs> ActionTagTapped;

        /// <summary>
        /// Occurs when label has performed fitting algorithm.  A value of -1 indicates that value of the FontSize property was used.
        /// </summary>
        public event EventHandler<double> FittedFontSizeChanged;
        #endregion


        #region Constructor & Fields
        static Label()
        {
            Settings.ConfirmInitialization();
            /*
            P42.Utils.Debug.ConditionFunc = (obj) =>
            {
                if (obj is string str && str.ToLower().Trim().StartsWith("lateral"))
                    return true;
                if (obj is Label label)
                    return P42.Utils.Debug.ConditionFunc(label.HtmlText ?? label.Text);
                if (obj is IText textObj)
                    return P42.Utils.Debug.ConditionFunc(textObj.Text);
                return false;
            };
            */
        }



        //bool cancelEvents;
        static int instances;
        readonly internal int _id;
        FormsGestures.Listener _listener;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.Label"/> class.
        /// </summary>
        public Label()
        {
            _id = instances++;
        }

        /// <summary>
        /// Convenience constructor for Forms9Patch.Label instance
        /// </summary>
        /// <param name="text"></param>
        public Label(string text) : this()
        {
            HtmlText = text;
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
            if (!P42.Utils.Environment.IsOnMainThread)
            {
                Device.BeginInvokeOnMainThread(() => OnPropertyChanged(propertyName));
                return;
            }

            if (propertyName == FittedFontSizeProperty.PropertyName)
                // required to keep the layout system calm.  
                return;

            if (propertyName == HtmlTextProperty.PropertyName)
            {
                if (HtmlText != null)
                {
                    Text = null;
                    F9PFormattedString = new HTMLMarkupString(HtmlText);
                }
                else
                    F9PFormattedString = null;
                if (F9PFormattedString != null && F9PFormattedString.ContainsActionSpan)
                {
                    if (_listener == null)
                    {
                        _listener = FormsGestures.Listener.For(this);
                        _listener.Tapped += OnTapped;
                        _listener.Down += OnDown;
                    }
                }
                else if (_listener != null)
                {
                    _listener.Tapped -= OnTapped;
                    _listener.Down -= OnDown;
                    _listener.Dispose();
                    _listener = null;
                }

            }
            else if (propertyName == TextProperty.PropertyName && Text != null)
                HtmlText = null;

            base.OnPropertyChanged(propertyName);

            if (propertyName == LinesProperty.PropertyName
                || propertyName == AutoFitProperty.PropertyName
                || propertyName == HtmlTextProperty.PropertyName
                || propertyName == TextProperty.PropertyName
               )
                InvalidateMeasure();
        }


        internal void InternalInvalidateMeasure()
            => InvalidateMeasure();

        #endregion


        #region Operators
        /// <param name="label">Label.</param>
        public static explicit operator string(Label label)
            => label?.HtmlText ?? label?.Text;

        #endregion


        #region for use by Button and Autofit
        internal bool MinimizeHeight;

        //internal Action SizeAndAlign;
        internal Func<double, double, Size> RendererSizeForWidthAndFontSize;
        internal Func<double, double, SizeRequest> Draw;

        /// <summary>
        /// Sizes the size of the for width and font.
        /// </summary>
        /// <returns>The for width and font size.</returns>
        /// <param name="width">Width.</param>
        /// <param name="fontSize">Font size.</param>
        public Size SizeForWidthAndFontSize(double width, double fontSize)
        {
            //P42.Utils.Debug.Message(this, "ENTER width=[" + width + "] fontSize=[" + fontSize + "]");
            var result = (RendererSizeForWidthAndFontSize?.Invoke(width, fontSize) ?? Size.Zero);
            //P42.Utils.Debug.Message(this, "EXIT result=[" + result + "]");
            return result;
        }

        /*
        /// <param name="widthConstraint">The available width that a parent element can allocated to a child. Value will be between 0 and double.PositiveInfinity.</param>
        /// <param name="heightConstraint">The available height that a parent element can allocated to a child. Value will be between 0 and double.PositiveInfinity.</param>
        /// <summary>
        /// Gets the size request.
        /// </summary>
        /// <returns>The size request.</returns>
        [Obsolete("Use OnMeasure")]
        public override SizeRequest GetSizeRequest(double widthConstraint, double heightConstraint)
        {
            //P42.Utils.Debug.Message(this, "ENTER (" + widthConstraint + ", " + heightConstraint + ")");

            IsDynamicallySized = true;
            SizeRequest result;
            //if (!_sizeAllocated && Draw != null)
            //    result = Draw.Invoke(widthConstraint, heightConstraint);
            //else
            result = base.GetSizeRequest(widthConstraint, heightConstraint);
            //P42.Utils.Debug.Message(this, "EXIT  result=[" + result + "]");
            return result;
        }


        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            //P42.Utils.Debug.Message(this, "ENTER  args(" + widthConstraint + ", " + heightConstraint + ")");
            var result = base.OnMeasure(widthConstraint, heightConstraint);
            //P42.Utils.Debug.Message(this, "EXIT  result=[" + result + "]");
            return result;
        }
        */


        //bool _sizeAllocated;
        protected override void OnSizeAllocated(double width, double height)
        {
            //P42.Utils.Debug.Message(this, "ENTER width=[" + width + "] height=[" + height + "]");
            base.OnSizeAllocated(width, height);
            //_sizeAllocated = true;
            Draw?.Invoke(width, height);
            //P42.Utils.Debug.Message(this, "EXIT Width=[" + Width + "] Height=[" + Height + "]");
        }

        /*
#pragma warning disable CS0672 // Member overrides obsolete member
        protected override SizeRequest OnSizeRequest(double widthConstraint, double heightConstraint)
#pragma warning restore CS0672 // Member overrides obsolete member
        {
            //P42.Utils.Debug.Message(this, "ENTER  args(" + widthConstraint + ", " + heightConstraint + ")");
            var result = base.OnSizeRequest(widthConstraint, heightConstraint);
            //P42.Utils.Debug.Message(this, "EXIT  result=[" + result + "]");
            return result;
        }

        private void OnSizeChanged(object sender, EventArgs e)
        {
            if (Width < 0 || Height < 0)
                _sizeAllocated = false;
            //P42.Utils.Debug.Message(this, "Width=[" + Width + "] Height=[" + Height + "]");

        }

        public void HardForceLayout()
            => Draw?.Invoke(Width, Height);
        */
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

        private void OnDown(object sender, DownUpEventArgs e)
        {
            if (Device.RuntimePlatform == Device.Android && e.NumberOfTouches == 1)
            {
                var index = IndexAtPoint(e.Touches[0]);
                foreach (var span in F9PFormattedString._spans)
                {
                    if (span is ActionSpan actionSpan)
                    {
                        if (index >= actionSpan.Start && index <= actionSpan.End)
                        {
                            e.Handled = true;
                            return;
                        }
                    }
                }
                e.Handled = false;
            }
        }


        void OnTapped(object sender, FormsGestures.TapEventArgs e)
        {
            // we're going to handle windows via the Window's Hyperlink Span element
            if (Device.RuntimePlatform == Device.UWP)
                return;
            if (e.NumberOfTouches == 1)
            {
                var index = IndexAtPoint(e.Touches[0]);
                foreach (var span in F9PFormattedString._spans)
                {
                    if (span is ActionSpan actionSpan)
                    {
                        if (index >= actionSpan.Start && index <= actionSpan.End)
                        {
                            Tap(actionSpan);
                            e.Handled = true;
                            return;
                        }
                    }
                }
            }
        }

        internal void Tap(string id, string href)
        {
            if (id == ActionSpan.NullId)
                id = null;
            ActionTagTapped?.Invoke(this, new ActionTagEventArgs(id, href));
        }

        internal void Tap(ActionSpan actionSpan)
            => Tap(actionSpan?.Id, actionSpan?.Href);

        #endregion



    }

    /// <summary>
    /// Action tag event arguments.
    /// </summary>
    public class ActionTagEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the href.
        /// </summary>
        /// <value>The href.</value>
        public string Href { get; private set; }
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.ActionTagEventArgs"/> class.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="href">Href.</param>
        public ActionTagEventArgs(string id, string href)
        {
            Href = href;
            Id = id;
        }
    }
}

