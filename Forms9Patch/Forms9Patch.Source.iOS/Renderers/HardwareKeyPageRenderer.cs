using System;
using System.Collections.Generic;
using Foundation;
using UIKit;
using ObjCRuntime;
using Xamarin.Forms;
using P42.Utils;

//[assembly: ExportRenderer(typeof(Forms9Patch.RootPage), typeof(Forms9Patch.iOS.PageRenderer))]
//[assembly: ExportRenderer(typeof(Xamarin.Forms.Page), typeof(Forms9Patch.iOS.PageRenderer))]
[assembly: ExportRenderer(typeof(Forms9Patch.HardwareKeyPage), typeof(Forms9Patch.iOS.HardwareKeyPageRenderer))]
namespace Forms9Patch.iOS
{
    public class HardwareKeyPageRenderer : Xamarin.Forms.Platform.iOS.PageRenderer
    {
        public override bool CanBecomeFirstResponder => true;

        /* These have no impact
        public override void PressesBegan(NSSet<UIPress> presses, UIPressesEvent evt)
        {
            base.PressesBegan(presses, evt);
        }

        public override void PressesEnded(NSSet<UIPress> presses, UIPressesEvent evt)
        {
            base.PressesEnded(presses, evt);
        }

        public override void PressesChanged(NSSet<UIPress> presses, UIPressesEvent evt)
        {
            base.PressesChanged(presses, evt);
        }

        public override void PressesCancelled(NSSet<UIPress> presses, UIPressesEvent evt)
        {
            base.PressesCancelled(presses, evt);
        }

        public override bool CanPerform(Selector action, NSObject withSender)
        {
            System.Diagnostics.Debug.WriteLine("CanPerform: " + action.Name);
            return base.CanPerform(action, withSender);
        }

        public override NSObject GetTargetForAction(Selector action, NSObject sender)
        {
            System.Diagnostics.Debug.WriteLine("GetTargetForAction: " + action.Name);
            return base.GetTargetForAction(action, sender);
        }

        public override UIViewController GetTargetViewControllerForAction(Selector action, NSObject sender)
        {
            return base.GetTargetViewControllerForAction(action, sender);
        }

        public override IntPtr GetMethodForSelector(Selector sel)
        {
            System.Diagnostics.Debug.WriteLine("GetMethodForSelector: " + sel.Name);
            return base.GetMethodForSelector(sel);
        }

        public override bool RespondsToSelector(Selector sel)
        {
            System.Diagnostics.Debug.WriteLine("RespondsToSelector: " + sel.Name);
            return base.RespondsToSelector(sel);
        }
        */


        [Export("OnKeyPress:")]
        void OnKeyPress(UIKeyCommand cmd)
        {
            var element = HardwareKeyPage.FocusedElement ?? HardwareKeyPage.DefaultFocusedElement;
            if (element == null)
                return;
            //System.Diagnostics.Debug.WriteLine("Forms9Patch.PageRenderer: cmd.Input=[" + cmd.Input + "] cmd.ModifierFlags[" + cmd.ModifierFlags + "] ");

            //var isFirstResponder = CanBecomeFirstResponder;
            //var directBinding = (bool)this.GetPropertyValue("IsDirectBinding");

            var modifiers = HardwareKeyModifierKeys.None;
            string keyLabel = cmd.Input.ToString().ToUpper();
            if (cmd.Input == UIKeyCommand.DownArrow)
                keyLabel = HardwareKey.DownArrowKeyLabel;
            else if (cmd.Input == UIKeyCommand.UpArrow)
                keyLabel = HardwareKey.UpArrowKeyLabel;
            else if (cmd.Input == UIKeyCommand.LeftArrow)
                keyLabel = HardwareKey.LeftArrowKeyLabel;
            else if (cmd.Input == UIKeyCommand.RightArrow)
                keyLabel = HardwareKey.RightArrowKeyLabel;
            else if (cmd.Input == UIKeyCommand.Escape)
                keyLabel = HardwareKey.EscapeKeyLabel;
            else if (cmd.Input == "\b")
                keyLabel = HardwareKey.BackspaceDeleteKeyLabel;
            else if (cmd.Input == "\0x7F")
                keyLabel = HardwareKey.ForwardDeleteKeyLabel;
            else if (cmd.Input == "\t")
                keyLabel = HardwareKey.TabKeyLabel;
            else if (cmd.Input == "\r")
                keyLabel = HardwareKey.EnterReturnKeyLabel;
            else if (cmd.Input == "UIKeyInputPageUp")
                keyLabel = HardwareKey.PageUpKeyLabel;
            else if (cmd.Input == "UIKeyInputPageDown")
                keyLabel = HardwareKey.PageDownKeyLabel;
            else if (cmd.Input == "UIKeyInputHome")
                keyLabel = HardwareKey.HomeKeyLabel;
            else if (cmd.Input == "UIKeyInputEnd")
                keyLabel = HardwareKey.EndKeyLabel;

            /*
            else if (cmd.Input == "\\^P")
                input = HardwareKey.F1Label;
            else if (cmd.Input == "UIKeyInputF2")
                input = HardwareKey.F2KeyLabel;
            else if (cmd.Input == "UIKeyInputF3")
                input = HardwareKey.F3KeyLabel;
            else if (cmd.Input == "UIKeyInputF4")
                input = HardwareKey.F4KeyLabel;
            else if (cmd.Input == "UIKeyInputF5")
                input = HardwareKey.F5KeyLabel;
            else if (cmd.Input == "UIKeyInputF6")
                input = HardwareKey.F6KeyLabel;
            else if (cmd.Input == "UIKeyInputF7")
                input = HardwareKey.F7KeyLabel;
            else if (cmd.Input == "UIKeyInputF8")
                input = HardwareKey.F8KeyLabel;
            else if (cmd.Input == "UIKeyInputF9")
                input = HardwareKey.F9KeyLabel;
            else if (cmd.Input == "UIKeyInputF10")
                input = HardwareKey.F10KeyLabel;
            else if (cmd.Input == "UIKeyInputF11")
                input = HardwareKey.F11KeyLabel;
            else if (cmd.Input == "UIKeyInputF12")
                input = HardwareKey.F12KeyLabel;
            */

            if ((cmd.ModifierFlags & UIKeyModifierFlags.AlphaShift) > 0)
                modifiers |= HardwareKeyModifierKeys.CapsLock;
            if ((cmd.ModifierFlags & UIKeyModifierFlags.Alternate) > 0)
                modifiers |= HardwareKeyModifierKeys.Alternate;
            if ((cmd.ModifierFlags & UIKeyModifierFlags.Control) > 0)
                modifiers |= HardwareKeyModifierKeys.Control;
            if ((cmd.ModifierFlags & UIKeyModifierFlags.NumericPad) > 0)
                modifiers |= HardwareKeyModifierKeys.NumericPadKey;
            if ((cmd.ModifierFlags & UIKeyModifierFlags.Command) > 0)
                modifiers |= HardwareKeyModifierKeys.PlatformKey;
            if ((cmd.ModifierFlags & UIKeyModifierFlags.Shift) > 0)
                modifiers |= HardwareKeyModifierKeys.Shift;

            var listeners = element.GetHardwareKeyListeners();
            for (int i = 0; i < listeners.Count; i++)
            {
                var listener = listeners[i];
                if (listener.HardwareKey.KeyLabel == keyLabel.ToUpper() && listener.HardwareKey.ModifierKeys == modifiers)
                {
                    if (listener.Command != null && listener.Command.CanExecute(listener.CommandParameter))
                        listener.Command.Execute(listener.CommandParameter);
                    listener.Pressed?.Invoke(element, new HardwareKeyEventArgs(listener.HardwareKey, element));
                    //System.Diagnostics.Debug.WriteLine("SUCCESS!!!");
                    return;
                }
            }
        }

        public override UIKeyCommand[] KeyCommands
        {
            get
            {
                var element = HardwareKeyPage.FocusedElement ?? HardwareKeyPage.DefaultFocusedElement;
                if (element == null)
                    return null;

                var sel = new Selector("OnKeyPress:");
                var result = new List<UIKeyCommand>();
                var listeners = element.GetHardwareKeyListeners();
                for (int i = 0; i < listeners.Count; i++)
                {
                    var listener = listeners[i];
                    var keyLabel = listener.HardwareKey.KeyLabel;
                    UIKeyModifierFlags flags = (UIKeyModifierFlags)0;
                    NSString nsInput = null;
                    if (keyLabel.Length > 1)
                    {
                        switch (keyLabel)
                        {
                            case HardwareKey.UpArrowKeyLabel:
                                nsInput = UIKeyCommand.UpArrow;
                                break;
                            case HardwareKey.DownArrowKeyLabel:
                                nsInput = UIKeyCommand.DownArrow;
                                break;
                            case HardwareKey.LeftArrowKeyLabel:
                                nsInput = UIKeyCommand.LeftArrow;
                                break;
                            case HardwareKey.RightArrowKeyLabel:
                                nsInput = UIKeyCommand.RightArrow;
                                break;
                            case HardwareKey.EscapeKeyLabel:
                                nsInput = UIKeyCommand.Escape;
                                break;
                            case HardwareKey.BackspaceDeleteKeyLabel:
                                nsInput = new NSString("\b");
                                break;
                            case HardwareKey.ForwardDeleteKeyLabel:
                                nsInput = new NSString("\0x7F");
                                break;
                            // there is not an insert key on mac extended keyboard.  In it's place is the Fn?
                            case HardwareKey.TabKeyLabel:
                                nsInput = new NSString("\t");
                                break;
                            case HardwareKey.EnterReturnKeyLabel:
                                nsInput = new NSString("\r");
                                break;
                            case HardwareKey.PageUpKeyLabel:
                                nsInput = new NSString("UIKeyInputPageUp");
                                break;
                            case HardwareKey.PageDownKeyLabel:
                                nsInput = new NSString("UIKeyInputPageDown");
                                break;
                            case HardwareKey.HomeKeyLabel:
                                nsInput = new NSString("UIKeyInputHome");
                                break;
                            case HardwareKey.EndKeyLabel:
                                nsInput = new NSString("UIKeyInputEnd");
                                break;

                            /* Don't know how to get Function keys on mac!
                            case HardwareKey.F1Label:
                                //nsInput = new NSString("UIKeyInputF1");
                                //nsInput = new NSString("" + (char)58);
                                nsInput = new NSString("\\^P");
                                break;
                            case HardwareKey.F2KeyLabel:
                                nsInput = new NSString("UIKeyInputF2");
                                break;
                            case HardwareKey.F3KeyLabel:
                                nsInput = new NSString("UIKeyInputF3");
                                break;
                            case HardwareKey.F4KeyLabel:
                                nsInput = new NSString("UIKeyInputF4");
                                break;
                            case HardwareKey.F5KeyLabel:
                                nsInput = new NSString("UIKeyInputF5");
                                break;
                            case HardwareKey.F6KeyLabel:
                                nsInput = new NSString("UIKeyInputF6");
                                break;
                            case HardwareKey.F7KeyLabel:
                                nsInput = new NSString("UIKeyInputF7");
                                break;
                            case HardwareKey.F8KeyLabel:
                                nsInput = new NSString("UIKeyInputF8");
                                break;
                            case HardwareKey.F9KeyLabel:
                                nsInput = new NSString("UIKeyInputF9");
                                break;
                            case HardwareKey.F10KeyLabel:
                                nsInput = new NSString("UIKeyInputF10");
                                break;
                            case HardwareKey.F11KeyLabel:
                                nsInput = new NSString("UIKeyInputF11");
                                break;
                            case HardwareKey.F12KeyLabel:
                                nsInput = new NSString("UIKeyInputF12");
                                break;
                            */

                            default:
                                continue;
                        }
                    }
                    else
                        nsInput = new NSString(keyLabel.ToLower());

                    var modifier = listener.HardwareKey.ModifierKeys;
                    if ((modifier & HardwareKeyModifierKeys.Alternate) > 0)
                        flags |= UIKeyModifierFlags.Alternate;
                    if ((modifier & HardwareKeyModifierKeys.CapsLock) > 0)
                        flags |= UIKeyModifierFlags.AlphaShift;
                    if ((modifier & HardwareKeyModifierKeys.Control) > 0)
                        flags |= UIKeyModifierFlags.Control;
                    if ((modifier & HardwareKeyModifierKeys.NumericPadKey) > 0)
                        flags |= UIKeyModifierFlags.NumericPad;
                    if ((modifier & HardwareKeyModifierKeys.PlatformKey) > 0)
                        flags |= UIKeyModifierFlags.Command;
                    if ((modifier & HardwareKeyModifierKeys.Shift) > 0)
                        flags |= UIKeyModifierFlags.Shift;


                    var descriptiveText = listener.HardwareKey.DiscoverableTitle;
                    if (descriptiveText != null)
                        result.Add(UIKeyCommand.Create(nsInput, flags, sel, new NSString(descriptiveText)));
                    else
                        result.Add(UIKeyCommand.Create(nsInput, flags, sel));

                }
                return result.ToArray();
            }
        }
    }
}
