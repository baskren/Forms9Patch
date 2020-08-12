using Xamarin.Forms;
using System;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Forms9Patch
{
    /// <summary>
    /// Permission popup.
    /// </summary>
    [Preserve(AllMembers = true)]
    [DesignTimeVisible(true)]
    public class PermissionPopup : Alert
    {

        #region Factories
        /// <summary>
        /// Create the specified title, text, okText, cancelText, okButtonColor, cancelButtonColor, okTextColor and cancelTextColor.
        /// </summary>
        /// <param name="title">Title.</param>
        /// <param name="text">Text.</param>
        /// <param name="okText">Ok text.</param>
        /// <param name="cancelText">Cancel text.</param>
        /// <param name="okButtonColor">Ok button color.</param>
        /// <param name="cancelButtonColor">Cancel button color.</param>
        /// <param name="okTextColor">Ok text color.</param>
        /// <param name="cancelTextColor">Cancel text color.</param>
        public static PermissionPopup Create(string title, string text, string okText = "OK", string cancelText = "Cancel", Color okButtonColor = default, Color cancelButtonColor = default, Color okTextColor = default, Color cancelTextColor = default)
        {
            var popup = new PermissionPopup { Title = title, Text = text, OkText = okText, CancelText = cancelText };
            if (okTextColor != default)
                popup.OkTextColor = okTextColor;
            if (okButtonColor != default)
                popup.OkButtonColor = okButtonColor;
            if (cancelTextColor != default)
                popup.CancelTextColor = cancelTextColor;
            if (cancelButtonColor != default)
                popup.CancelButtonColor = cancelButtonColor;
            popup.IsVisible = true;
            return popup;
        }

        /// <summary>
        /// Create the specified target, title, text, okText, cancelText, okButtonColor, cancelButtonColor, okTextColor and cancelTextColor.
        /// </summary>
        /// <returns>The create.</returns>
        /// <param name="target">Target.</param>
        /// <param name="title">Title.</param>
        /// <param name="text">Text.</param>
        /// <param name="okText">Ok text.</param>
        /// <param name="cancelText">Cancel text.</param>
        /// <param name="okButtonColor">Ok button color.</param>
        /// <param name="cancelButtonColor">Cancel button color.</param>
        /// <param name="okTextColor">Ok text color.</param>
        /// <param name="cancelTextColor">Cancel text color.</param>
        public static PermissionPopup Create(VisualElement target, string title, string text, string okText = "OK", string cancelText = "Cancel", Color okButtonColor = default, Color cancelButtonColor = default, Color okTextColor = default, Color cancelTextColor = default)
        {
            var popup = new PermissionPopup(target) { Title = title, Text = text, OkText = okText, CancelText = cancelText };
            if (okTextColor != default)
                popup.OkTextColor = okTextColor;
            if (okButtonColor != default)
                popup.OkButtonColor = okButtonColor;
            if (cancelTextColor != default)
                popup.CancelTextColor = cancelTextColor;
            if (cancelButtonColor != default)
                popup.CancelButtonColor = cancelButtonColor;
            popup.IsVisible = true;
            return popup;
        }
        #endregion


        #region Properties

        /// <summary>
        /// The cancel text property.
        /// </summary>
        public static readonly BindableProperty CancelTextProperty = BindableProperty.Create(nameof(CancelText), typeof(string), typeof(PermissionPopup), default(string));
        /// <summary>
        /// Gets or sets the cancel text.
        /// </summary>
        /// <value>The cancel text.</value>
        public string CancelText
        {
            get => (string)GetValue(CancelTextProperty);
            set => SetValue(CancelTextProperty, value);
        }

        /// <summary>
        /// The cancel button color property.
        /// </summary>
        public static readonly BindableProperty CancelButtonColorProperty = BindableProperty.Create(nameof(CancelButtonColor), typeof(Color), typeof(PermissionPopup), default(Color));
        /// <summary>
        /// Gets or sets the color of the cancel button.
        /// </summary>
        /// <value>The color of the cancel button.</value>
        public Color CancelButtonColor
        {
            get => (Color)GetValue(CancelButtonColorProperty);
            set => SetValue(CancelButtonColorProperty, value);
        }

        /// <summary>
        /// The cancel text color property.
        /// </summary>
        public static readonly BindableProperty CancelTextColorProperty = BindableProperty.Create(nameof(CancelTextColor), typeof(Color), typeof(PermissionPopup), default(Color));
        /// <summary>
        /// Gets or sets the color of the cancel text.
        /// </summary>
        /// <value>The color of the cancel text.</value>
        public Color CancelTextColor
        {
            get => (Color)GetValue(CancelTextColorProperty);
            set => SetValue(CancelTextColorProperty, value);
        }


        #endregion



        #region Visual Elements
        readonly Button _cancelButton = new Button
        {
            HorizontalOptions = LayoutOptions.FillAndExpand
        };
        #endregion


        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.PermissionPopup"/> class.
        /// </summary>
        public PermissionPopup(VisualElement target = null) : base(target)
        {
            _cancelButton.BackgroundColor = CancelButtonColor;
            _cancelButton.TextColor = CancelTextColor;
            _cancelButton.HtmlText = CancelText;

            _cancelButton.Tapped += OnCancelButtonTappedAsync;

            var stack = Content as Xamarin.Forms.StackLayout;
            stack.Children.Remove(_okButton);
            stack.Children.Add(new Xamarin.Forms.StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children = { _cancelButton, _okButton }
            });
            CancelOnBackButtonClick = true;
            CancelOnPageOverlayTouch = true;
        }

        bool _disposed;
        /// <summary>
        /// Dispose instance
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _disposed = true;
                _cancelButton.Tapped -= OnCancelButtonTappedAsync;
                _cancelButton.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion


        #region Event Handlers
        async void OnCancelButtonTappedAsync(object sender, EventArgs e)
        {
            P42.Utils.BreadCrumbs.Add(GetType(), "cancel");
            PermissionState = PermissionState.Rejected;
            await CancelAsync(_cancelButton);
        }

        /// <summary>
        /// Cancel the PermissionPopup
        /// </summary>
        /// <param name="trigger"></param>
        /// <returns></returns>
        public override Task CancelAsync(object trigger = null)
        {
            if (PermissionState == PermissionState.Pending)
                PermissionState = PermissionState.Cancelled;
            return base.CancelAsync(trigger);
        }

        #endregion


        #region Property Change Management
        /// <summary>
        /// Ons the property changed.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        protected override void OnPropertyChanged(string propertyName = null)
        {
            Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() =>
            {

                base.OnPropertyChanged(propertyName);

                if (propertyName == CancelTextProperty.PropertyName)
                    _cancelButton.HtmlText = CancelText;
                else if (propertyName == CancelTextColorProperty.PropertyName)
                    _cancelButton.TextColor = CancelTextColor;
                else if (propertyName == CancelButtonColorProperty.PropertyName)
                    _cancelButton.BackgroundColor = CancelButtonColor;
            });
        }
        #endregion
    }
}
