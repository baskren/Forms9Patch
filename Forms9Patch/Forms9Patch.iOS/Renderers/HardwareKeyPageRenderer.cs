using System;
using System.Collections.Generic;
using Foundation;
using UIKit;
using ObjCRuntime;
using Xamarin.Forms;
using P42.Utils;

[assembly: ExportRenderer(typeof(Forms9Patch.HardwareKeyPage), typeof(Forms9Patch.iOS.HardwareKeyPageRenderer))]
namespace Forms9Patch.iOS
{
    public class HardwareKeyPageRenderer : Xamarin.Forms.Platform.iOS.PageRenderer
    {
        static readonly NSString UIKeyCommand_BackspaceDeleteKeyInput = new NSString("\b");
        static readonly NSString UIKeyCommand_ForwardDeleteKeyInput = new NSString("\0x7F");
        static readonly NSString UIKeyCommand_TabKeyInput = new NSString("\t");
        static readonly NSString UIKeyCommand_EnterReturnKeyInput = new NSString("\r");
        static readonly NSString UIKeyCommand_PageUpKeyInput = new NSString("UIKeyInputPageUp");
        static readonly NSString UIKeyCommand_PageDownKeyInput = new NSString("UIKeyInputPageDown");
        static readonly NSString UIKeyCommand_HomeKeyInput = new NSString("UIKeyInputHome");
        static readonly NSString UIKeyCommand_EndKeyInput = new NSString("UIKeyInputEnd");
        static readonly NSString UIKeyCommand_InsertKeyInput = new NSString("UIKeyInputInsert");
        static readonly NSString UIKeyCommand_InputDeleteKeyInput = new NSString("UIKeyInputDelete");
        //static readonly NSString UIKeyCommand_ = new NSString();

        public override bool CanBecomeFirstResponder => true;

        [Export("OnKeyPress:")]
        void OnKeyPress(UIKeyCommand cmd) => ProcessKeyPress(cmd);

        UIKeyCommand[] _keyCommands;
        public override UIKeyCommand[] KeyCommands
        {
            get
            {
                DisposeKeyCommands();
                return _keyCommands = GetKeyCommands();
            }
        }

        void DisposeKeyCommands()
        {
            if (_keyCommands != null)
            {
                foreach (var keyCommand in _keyCommands)
                {
                    keyCommand.DiscoverabilityTitle?.Dispose();
                    if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
                    {
                        keyCommand.Image?.Dispose();  // https://appcenter.ms/orgs/AWC/apps/SpanCalc.iOS/crashes/errors/3982867855u/overview, SIGABRT: Objective-C exception thrown. Name: NSInvalidArgumentException Reason: -[UIKeyCommand image]: unrecognized selector sent to instance
                        keyCommand.PropertyList?.Dispose();
                    }
                    keyCommand.Input?.Dispose();
                    keyCommand.Dispose();
                }
            }
            _keyCommands = null;
        }

        private bool _disposed;
        protected override void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _disposed = true;
                DisposeKeyCommands();
            }
            base.Dispose(disposing);
        }


        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            BecomeFirstResponder();
        }

        #region static methods
        static UIKeyCommand _lastKeyCommand;
        internal static void ProcessKeyPress(UIKeyCommand cmd)
        {
            if (cmd == _lastKeyCommand)
                return;
            _lastKeyCommand = cmd;
            var element = HardwareKeyPage.FocusedElement ?? HardwareKeyPage.DefaultFocusedElement;
            if (element == null)
                return;

            //System.Diagnostics.Debug.WriteLine("Forms9Patch.PageRenderer: cmd.Input=[" + cmd.Input + "] cmd.ModifierFlags[" + cmd.ModifierFlags + "] ");

            //var isFirstResponder = CanBecomeFirstResponder;
            //var directBinding = (bool)this.GetPropertyValue("IsDirectBinding");

            string keyInput = cmd.Input.ToString().ToUpper();
            bool useShift = true;
            if (cmd.Input == UIKeyCommand.DownArrow)
                keyInput = HardwareKey.DownArrowKeyInput;
            else if (cmd.Input == UIKeyCommand.UpArrow)
                keyInput = HardwareKey.UpArrowKeyInput;
            else if (cmd.Input == UIKeyCommand.LeftArrow)
                keyInput = HardwareKey.LeftArrowKeyInput;
            else if (cmd.Input == UIKeyCommand.RightArrow)
                keyInput = HardwareKey.RightArrowKeyInput;
            else if (cmd.Input == UIKeyCommand.Escape)
                keyInput = HardwareKey.EscapeKeyInput;
            else if (cmd.Input == UIKeyCommand_BackspaceDeleteKeyInput)
                keyInput = HardwareKey.BackspaceDeleteKeyInput;
            else if (cmd.Input == UIKeyCommand_ForwardDeleteKeyInput)
                keyInput = HardwareKey.ForwardDeleteKeyInput;
            else if (cmd.Input == UIKeyCommand_TabKeyInput)
                keyInput = HardwareKey.TabKeyInput;
            else if (cmd.Input == UIKeyCommand_EnterReturnKeyInput)
                keyInput = HardwareKey.EnterReturnKeyInput;
            else if (cmd.Input == UIKeyCommand_PageUpKeyInput)
                keyInput = HardwareKey.PageUpKeyInput;
            else if (cmd.Input == UIKeyCommand_PageDownKeyInput)
                keyInput = HardwareKey.PageDownKeyInput;
            else if (cmd.Input == UIKeyCommand_HomeKeyInput)
                keyInput = HardwareKey.HomeKeyInput;
            else if (cmd.Input == UIKeyCommand_EndKeyInput)
                keyInput = HardwareKey.EndKeyInput;
            else if (cmd.Input == UIKeyCommand_InsertKeyInput)
                keyInput = HardwareKey.InsertKeyInput;
            else if (cmd.Input == UIKeyCommand_InputDeleteKeyInput)
                keyInput = HardwareKey.ForwardDeleteKeyInput;
            else
                useShift = keyInput.Length > 0 && char.IsLetter(keyInput[0]);

            var modifiers = GetModifierKeys(cmd.ModifierFlags, useShift);

            var listeners = element.GetHardwareKeyListeners();
            for (int i = 0; i < listeners.Count; i++)
            {
                var listener = listeners[i];
                if (string.IsNullOrEmpty(listener?.HardwareKey?.KeyInput))
                    continue;
                if (listener.HardwareKey.KeyInput == keyInput.ToUpper() && (listener.HardwareKey.ModifierKeys == modifiers || listener.HardwareKey.ModifierKeys.HasFlag(HardwareKeyModifierKeys.Any)))
                {
                    if (listener.Command != null && listener.Command.CanExecute(listener.CommandParameter))
                        listener.Command.Execute(listener.CommandParameter);
                    var observedHardwareKey = new HardwareKey(keyInput.ToUpper(), modifiers);
                    listener.Pressed?.Invoke(element, new HardwareKeyEventArgs(observedHardwareKey, element));
                    //System.Diagnostics.Debug.WriteLine("SUCCESS!!!");
                    return;
                }
            }
        }

        static Forms9Patch.HardwareKeyModifierKeys GetModifierKeys(UIKeyModifierFlags uIKeyModifierFlags, bool includeShift)
        {
            var modifiers = Forms9Patch.HardwareKeyModifierKeys.None;
            if ((uIKeyModifierFlags & UIKeyModifierFlags.AlphaShift) > 0)
                modifiers |= HardwareKeyModifierKeys.CapsLock;
            if ((uIKeyModifierFlags & UIKeyModifierFlags.Alternate) > 0)
                modifiers |= HardwareKeyModifierKeys.Alternate;
            if ((uIKeyModifierFlags & UIKeyModifierFlags.Control) > 0)
                modifiers |= HardwareKeyModifierKeys.Control;
            if ((uIKeyModifierFlags & UIKeyModifierFlags.NumericPad) > 0)
                modifiers |= HardwareKeyModifierKeys.NumericPadKey;
            if ((uIKeyModifierFlags & UIKeyModifierFlags.Command) > 0)
                modifiers |= HardwareKeyModifierKeys.PlatformKey;
            if (includeShift && (uIKeyModifierFlags & UIKeyModifierFlags.Shift) > 0)
                modifiers |= HardwareKeyModifierKeys.Shift;
            return modifiers;
        }


        internal static UIKeyCommand[] GetKeyCommands()
        {
            var element = HardwareKeyPage.FocusedElement ?? HardwareKeyPage.DefaultFocusedElement;
            if (element == null)
                return null;

            var result = new List<UIKeyCommand>();
            var listeners = element.GetHardwareKeyListeners();
            for (int i = 0; i < listeners.Count; i++)
            {
                var listener = listeners[i];
                var keyInput = listener?.HardwareKey?.KeyInput;
                if (string.IsNullOrEmpty(keyInput))
                    continue;
                NSString nsInput;
                switch (keyInput)
                {
                    case HardwareKey.UpArrowKeyInput:
                        nsInput = new NSString(UIKeyCommand.UpArrow);
                        break;
                    case HardwareKey.DownArrowKeyInput:
                        nsInput = new NSString(UIKeyCommand.DownArrow);
                        break;
                    case HardwareKey.LeftArrowKeyInput:
                        nsInput = new NSString(UIKeyCommand.LeftArrow);
                        break;
                    case HardwareKey.RightArrowKeyInput:
                        nsInput = new NSString(UIKeyCommand.RightArrow);
                        break;
                    case HardwareKey.EscapeKeyInput:
                        nsInput = new NSString(UIKeyCommand.Escape);
                        break;
                    case HardwareKey.BackspaceDeleteKeyInput:
                        nsInput = new NSString(UIKeyCommand_BackspaceDeleteKeyInput);
                        break;
                    case HardwareKey.ForwardDeleteKeyInput:
                        //nsInput = new NSString("\0x7F");
                        nsInput = new NSString(UIKeyCommand_InputDeleteKeyInput);
                        break;
                    // there is not an insert key on mac extended keyboard.  In it's place is the Fn?
                    case HardwareKey.TabKeyInput:
                        nsInput = new NSString(UIKeyCommand_TabKeyInput);
                        break;
                    case HardwareKey.EnterReturnKeyInput:
                        nsInput = new NSString(UIKeyCommand_EnterReturnKeyInput);
                        break;
                    case HardwareKey.PageUpKeyInput:
                        nsInput = new NSString(UIKeyCommand_PageUpKeyInput);
                        break;
                    case HardwareKey.PageDownKeyInput:
                        nsInput = new NSString(UIKeyCommand_PageDownKeyInput);
                        break;
                    case HardwareKey.HomeKeyInput:
                        nsInput = new NSString(UIKeyCommand_HomeKeyInput);
                        break;
                    case HardwareKey.EndKeyInput:
                        nsInput = new NSString(UIKeyCommand_EndKeyInput);
                        break;
                    case HardwareKey.InsertKeyInput:
                        nsInput = new NSString(UIKeyCommand_InsertKeyInput);
                        break;


                    default:
                        nsInput = new NSString(keyInput.ToLower());
                        break;
                }

                var modifier = listener.HardwareKey.ModifierKeys;
                if (modifier.HasFlag(HardwareKeyModifierKeys.Any))
                    for (uint m = 0; m < (int)HardwareKeyModifierKeys.Any; m++)
                        result.Add(UIKeyCommandFrom(nsInput, (HardwareKeyModifierKeys)m, listener.HardwareKey.DiscoverableTitle));
                else
                    result.Add(UIKeyCommandFrom(nsInput, modifier, listener.HardwareKey.DiscoverableTitle));

                nsInput.Dispose();

            }
            return result.ToArray();
        }


        static UIKeyModifierFlags UIKeyModifierFlagsFrom(HardwareKeyModifierKeys modifierKeys)
        {
            var flags = (UIKeyModifierFlags)0;
            if (((HardwareKeyModifierKeys)modifierKeys & HardwareKeyModifierKeys.Alternate) > 0)
                flags |= UIKeyModifierFlags.Alternate;
            if (((HardwareKeyModifierKeys)modifierKeys & HardwareKeyModifierKeys.CapsLock) > 0)
                flags |= UIKeyModifierFlags.AlphaShift;
            if (((HardwareKeyModifierKeys)modifierKeys & HardwareKeyModifierKeys.Control) > 0)
                flags |= UIKeyModifierFlags.Control;
            if (((HardwareKeyModifierKeys)modifierKeys & HardwareKeyModifierKeys.NumericPadKey) > 0)
                flags |= UIKeyModifierFlags.NumericPad;
            if (((HardwareKeyModifierKeys)modifierKeys & HardwareKeyModifierKeys.PlatformKey) > 0)
                flags |= UIKeyModifierFlags.Command;
            if (((HardwareKeyModifierKeys)modifierKeys & HardwareKeyModifierKeys.Shift) > 0)
                flags |= UIKeyModifierFlags.Shift;
            return flags;
        }

        static internal readonly Selector _onKeyPressSelector = new Selector("OnKeyPress:");

        static UIKeyCommand UIKeyCommandFrom(NSString nsInput, HardwareKeyModifierKeys modifier, string descriptiveText)
        {
            var flags = UIKeyModifierFlagsFrom(modifier);
            return string.IsNullOrWhiteSpace(descriptiveText)
                ? UIKeyCommand.Create(nsInput, flags, _onKeyPressSelector)
                : UIKeyCommand.Create(nsInput, flags, _onKeyPressSelector, new NSString(descriptiveText));
        }
        #endregion
    }
}
