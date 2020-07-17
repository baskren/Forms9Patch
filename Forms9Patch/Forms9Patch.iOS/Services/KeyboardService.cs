using UIKit;
using Foundation;

[assembly: Xamarin.Forms.Dependency(typeof(Forms9Patch.iOS.KeyboardService))]
namespace Forms9Patch.iOS
{
    /// <summary>
    /// Keyboard service.
    /// </summary>
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class KeyboardService : IKeyboardService
    {
        public bool IsHardwareKeyboardActive
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.iOS.KeyboardService"/> class.
        /// </summary>
        public KeyboardService()
        {
            UIKeyboard.Notifications.ObserveWillHide(OnHidden);
            UIKeyboard.Notifications.ObserveWillShow(OnShown);
            UIKeyboard.Notifications.ObserveDidChangeFrame(OnFrameChanged);
        }

        bool _hidden = true;
        /// <summary>
        /// Ons the hidden.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void OnHidden(object sender, UIKeyboardEventArgs e)
        {
            Forms9Patch.KeyboardService.OnVisiblityChange(KeyboardVisibilityChange.Hidden);
            Height = 0;
            _hidden = true;
        }

        /// <summary>
        /// Ons the shown.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void OnShown(object sender, UIKeyboardEventArgs e)
        {
            Forms9Patch.KeyboardService.OnVisiblityChange(KeyboardVisibilityChange.Shown);
            Height = e.FrameEnd.Height;
            _hidden = false;
        }

        void OnFrameChanged(object sender, UIKeyboardEventArgs e)
        {
            //CGSize kbSize = [[info objectForKey: UIKeyboardFrameBeginUserInfoKey] CGRectValue].size;
            var kbSize = e.FrameEnd;
            if (!_hidden)
                Height = kbSize.Height;

        }


        /// <summary>
        /// Hide this instance.
        /// </summary>
        public void Hide()
        {
            UIApplication.SharedApplication.KeyWindow.EndEditing(true);
        }

        public string LanguageRegion
        {
            get
            {
                //var defs = NSUserDefaults.StandardUserDefaults;
                return UITextInputMode.CurrentInputMode.PrimaryLanguage;
                //return null;
            }
        }

        double _height;
        public double Height
        {
            get => _height;
            set
            {
                if (System.Math.Abs(_height - value) > 0.1)
                {
                    _height = value;
                    Forms9Patch.KeyboardService.OnHeightChanged(_height);
                }
                _height = value;
            }
        }
    }
}
