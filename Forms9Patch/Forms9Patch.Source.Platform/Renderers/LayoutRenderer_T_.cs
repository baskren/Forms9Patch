using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

#if __IOS__
using CoreGraphics;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using SkiaSharp;
using ObjCRuntime;
using Foundation;
namespace Forms9Patch.iOS
#elif __DROID__
using Android.Runtime;
using Android.Views;
using Xamarin.Forms.Platform.Android;
namespace Forms9Patch.Droid
#elif WINDOWS_UWP
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Media;
using Xamarin.Forms.Platform.UWP;
namespace Forms9Patch.UWP
#else 
namespace Forms9Patch
#endif

{
    class F9pLayoutRenderer<TElement> : ViewRenderer<TElement, SkiaRoundedBoxAndImageView> /*, IUIKeyInput*/ where TElement : Layout, ILayout  // works but renders SkiaRoundedBoxAndImageView over children views
                                                                                                                                               //class F9pLayoutRenderer<TElement> : VisualElementRenderer<TElement> where TElement : View, ILayout // VisualElement, IBackgroundImage
    {
        #region Fields
        static int _instances;
        int _instance;
        #endregion


        #region Constructor / Disposer
        public F9pLayoutRenderer()
        {
            _instance = _instances++;

#if __IOS__




#elif __DROID__

            KeyPress += (sender, e) => System.Diagnostics.Debug.WriteLine("KeyPress [" + e.KeyCode + "] [" + e.Event + "] [" + e.Handled + "]"); ;

#elif WINDOWS_UWP
            KeyUp += OnKeyUp;
            KeyDown += OnKeyDown;
#endif

        }

        #endregion

#if __IOS__
        /*
        public override void DidUpdateFocus(UIFocusUpdateContext context, UIFocusAnimationCoordinator coordinator)
        {
            base.DidUpdateFocus(context, coordinator);
        }

        public override bool ShouldUpdateFocus(UIFocusUpdateContext context)
        {
            return base.ShouldUpdateFocus(context);
        }

        public override void UpdateFocusIfNeeded()
        {
            base.UpdateFocusIfNeeded();
        }

        public override void SetNeedsFocusUpdate()
        {
            base.SetNeedsFocusUpdate();
        }

        public override UIView PreferredFocusedView
        {
            get
            {
                return this;
                //return base.PreferredFocusedView;
            }
        }
        public override bool Focused
        {
            get
            {
                return true;
                //return base.Focused;
            }
        }
        public override bool CanBecomeFocused
        {
            get
            {
                //return base.CanBecomeFocused;
                return true;
            }
        }

        [Export("OnKeyPress:")]
        void OnKeyPress(UIKeyCommand cmd)
        {
            System.Diagnostics.Debug.WriteLine("cmd.Input=[" + cmd.Input + "] cmd.ModifierFlags[" + cmd.ModifierFlags + "]");
        }

        public override UIKeyCommand[] KeyCommands
        {
            get
            {
                var baseCommands = base.KeyCommands;
                var sel = new Selector("OnKeyPress:");

                var c1 = UIKeyCommand.Create(new NSString("1"), (UIKeyModifierFlags.Command), sel, new NSString("1 one"));
                var c2 = UIKeyCommand.Create(new NSString("2"), (UIKeyModifierFlags.Command), sel, new NSString("2 one"));
                var c3 = UIKeyCommand.Create(new NSString("3"), (UIKeyModifierFlags.Command), sel, new NSString("3 one"));
                var c4 = UIKeyCommand.Create(new NSString("4"), (UIKeyModifierFlags.Command), sel, new NSString("4 one"));
                var c5 = UIKeyCommand.Create(new NSString("5"), (UIKeyModifierFlags)0, sel);
                var c6 = UIKeyCommand.Create(new NSString("6"), (UIKeyModifierFlags)0, sel);
                var c7 = UIKeyCommand.Create(new NSString("7"), (UIKeyModifierFlags)0, sel);
                var c8 = UIKeyCommand.Create(new NSString("8"), (UIKeyModifierFlags)0, sel);
                var c9 = UIKeyCommand.Create(new NSString("9"), (UIKeyModifierFlags)0, sel);
                var c0 = UIKeyCommand.Create(new NSString("0"), (UIKeyModifierFlags)0, sel);
                var cPlus = UIKeyCommand.Create(new NSString("+"), (UIKeyModifierFlags)0, sel);
                var cMinus = UIKeyCommand.Create(new NSString("-"), (UIKeyModifierFlags)0, sel);
                var cMult = UIKeyCommand.Create(new NSString("*"), (UIKeyModifierFlags)0, sel);
                var cDiv = UIKeyCommand.Create(new NSString("/"), (UIKeyModifierFlags)0, sel);
                var cEql = UIKeyCommand.Create(new NSString("="), (UIKeyModifierFlags)0, sel);

                var commandList = new List<UIKeyCommand>();
                commandList.AddRange(baseCommands);
                commandList.Add(c1);
                commandList.Add(c2);
                commandList.Add(c3);
                commandList.Add(c4);
                commandList.Add(c5);
                commandList.Add(c6);
                commandList.Add(c7);
                commandList.Add(c8);
                commandList.Add(c9);
                commandList.Add(c0);
                commandList.Add(cPlus);
                commandList.Add(cMinus);
                commandList.Add(cMult);
                commandList.Add(cDiv);
                commandList.Add(cEql);

                return base.KeyCommands;
            }
        }

        public bool HasText => true;

        public UITextAutocapitalizationType AutocapitalizationType { get; set; } = UITextAutocapitalizationType.None;
        public UITextAutocorrectionType AutocorrectionType { get; set; } = UITextAutocorrectionType.No;
        public UIKeyboardType KeyboardType { get; set; } = UIKeyboardType.AsciiCapableNumberPad;
        public UIKeyboardAppearance KeyboardAppearance { get; set; } = UIKeyboardAppearance.Alert;
        public UIReturnKeyType ReturnKeyType { get; set; } = UIReturnKeyType.Continue;
        public bool EnablesReturnKeyAutomatically { get; set; } = true;
        public bool SecureTextEntry { get; set; } = false;
        public UITextSpellCheckingType SpellCheckingType { get; set; } = UITextSpellCheckingType.No;
        */
#elif __DROID__
        public override bool OnKeyUp([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            System.Diagnostics.Debug.WriteLine("OnKeyDown[" + keyCode + "] Element=[" + Element + "] Parent=[" + Element.Parent + "]  \te.UnicodeChar=[" + (char)e.UnicodeChar + "]");
            return base.OnKeyUp(keyCode, e);
        }

        public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
        {
            System.Diagnostics.Debug.WriteLine("OnKeyDown[" + keyCode + "] Element=[" + Element + "] Parent=[" + Element.Parent + "]  \te.UnicodeChar=[" + (char)e.UnicodeChar + "]");
            return base.OnKeyDown(keyCode, e);
        }
#elif WINDOWS_UWP

        private void OnKeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("OnKeyDown["+e.Key+"] Element=[" + Element + "] Parent=[" + Element.Parent + "]  \te.Handled=[" + e.Handled + "] \te.KeyStatus=[" + e.KeyStatus+"]");
        }

        private void OnKeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("OnKeyUp["+e.Key+"]   Element=["+Element+ "] Parent=[" + Element.Parent + "]  \te.Handled=["+e.Handled+"] \te.KeyStatus=["+e.KeyStatus + "]");
        }

#endif

        #region Change management
        protected override void OnElementChanged(ElementChangedEventArgs<TElement> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
#if WINDOWS_UWP
                SizeChanged -= OnSizeChanged;
#endif
                SetAutomationId(null);
            }

            if (e.NewElement != null)
            {
                if (Control == null)
                    SetNativeControl(new SkiaRoundedBoxAndImageView(e.NewElement as IShape));
#if __IOS__

#endif
#if WINDOWS_UWP
                SizeChanged += OnSizeChanged;
#endif

                if (!string.IsNullOrEmpty(Element.AutomationId))
                    SetAutomationId(Element.AutomationId);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName)
                return;
            base.OnElementPropertyChanged(sender, e);
        }





#if __IOS__

        public override void SubviewAdded(UIView uiview)
        {
            base.SubviewAdded(uiview);

            if (uiview is SkiaRoundedBoxAndImageView)
                SendSubviewToBack(uiview);
            else
                BringSubviewToFront(uiview);
        }

        public void InsertText(string text)
        {
            throw new NotImplementedException();
        }

        public void DeleteBackward()
        {
            throw new NotImplementedException();
        }

#elif __DROID__

#elif WINDOWS_UWP
        protected override void UpdateBackgroundColor()
        {
            base.UpdateBackgroundColor();
            Background = new SolidColorBrush(Colors.Transparent);
        }

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            // Since layouts in Forms can be interacted with, we need to create automation peers
            // for them so we can interact with them in automated tests
            return new FrameworkElementAutomationPeer(this);
        }

        void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Element.ExtendedElementShape.IsSegment())
            {
                Control.Height = ActualHeight + 1;
                Control.Width = ActualWidth + 1;
            }
        }

#endif
        #endregion
    }
}
