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

        /*
    // Dictionary to convert international cmd.Input to their KeyInputs
    // Dictionary<language-Region, Dictionary<cmd.Input, KeyInput>
    Dictionary<string, Dictionary<string, string>> regionalShiftedKeyInputs = new Dictionary<string, Dictionary<string, string>>
    {
        { "EN", new Dictionary<string, string>
            {
                {"~","`"}, { "!","1" }, { "@","2"}, {"#","3"}, {"$","4"}, {"%","5"}, {"^","6"}, {"&","7"}, {"*","8"}, {"(","9"}, {")","0"}, {"_","-"}, {"+","="},
                {"{","["}, {"}","]"}, {"|","\\"},
                {":",";"}, {"\"","'"},
                {"<",","}, {">","."}, {"/","?"}
            }
        },
        { "fr-FR", new Dictionary<string, string>
            {
                {">","<"}, { "1","&" }, { "2","é"}, {"3","\""}, {"4","'"}, {"5","("}, {"6","-"}, {"7","è"}, {"8","_"}, {"9","ç"}, {"0","à"}, {"°",")"},{"_","-"},
                {"¨","^"}, {"*","$"}, {"£","`"},
                {"%","ù"},
                {"?",","}, {".",";"}, {"/",":"}, {"+","="}
            }
        }
    };
    */

        Selector _onKeyPressSelector = new Selector("OnKeyPress:");

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
            string keyInput = cmd.Input.ToString().ToUpper();
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
            else if (cmd.Input == "\b")
                keyInput = HardwareKey.BackspaceDeleteKeyInput;
            else if (cmd.Input == "\0x7F")
                keyInput = HardwareKey.ForwardDeleteKeyInput;
            else if (cmd.Input == "\t")
                keyInput = HardwareKey.TabKeyInput;
            else if (cmd.Input == "\r")
                keyInput = HardwareKey.EnterReturnKeyInput;
            else if (cmd.Input == "UIKeyInputPageUp")
                keyInput = HardwareKey.PageUpKeyInput;
            else if (cmd.Input == "UIKeyInputPageDown")
                keyInput = HardwareKey.PageDownKeyInput;
            else if (cmd.Input == "UIKeyInputHome")
                keyInput = HardwareKey.HomeKeyInput;
            else if (cmd.Input == "UIKeyInputEnd")
                keyInput = HardwareKey.EndKeyInput;
            else if (cmd.Input == "UIKeyInputInsert")
                keyInput = HardwareKey.InsertKeyInput;
            else if (cmd.Input == "UIKeyInputDelete")
                keyInput = HardwareKey.ForwardDeleteKeyInput;

            /*
            else if (cmd.Input == "\\^P")
                input = HardwareKey.F1Label;
            else if (cmd.Input == "UIKeyInputF2")
                input = HardwareKey.F2KeyInput;
            else if (cmd.Input == "UIKeyInputF3")
                input = HardwareKey.F3KeyInput;
            else if (cmd.Input == "UIKeyInputF4")
                input = HardwareKey.F4KeyInput;
            else if (cmd.Input == "UIKeyInputF5")
                input = HardwareKey.F5KeyInput;
            else if (cmd.Input == "UIKeyInputF6")
                input = HardwareKey.F6KeyInput;
            else if (cmd.Input == "UIKeyInputF7")
                input = HardwareKey.F7KeyInput;
            else if (cmd.Input == "UIKeyInputF8")
                input = HardwareKey.F8KeyInput;
            else if (cmd.Input == "UIKeyInputF9")
                input = HardwareKey.F9KeyInput;
            else if (cmd.Input == "UIKeyInputF10")
                input = HardwareKey.F10KeyInput;
            else if (cmd.Input == "UIKeyInputF11")
                input = HardwareKey.F11KeyInput;
            else if (cmd.Input == "UIKeyInputF12")
                input = HardwareKey.F12KeyInput;
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

            /*
            if (!modifiers.HasFlag(HardwareKeyModifierKeys.NumericPadKey))
            {
                switch (Forms9Patch.KeyboardService.LanguageRegion)
                {
                    case "fr-FR":
                        {
                            switch (cmd.Input)
                            {
                                case "1":
                                    keyInput = "&";
                                    break;
                                case "2":
                                    keyInput = "é";
                                    break;
                                case "3":
                                    keyInput = "\"";
                                    break;
                                case "4":
                                    keyInput = "'";
                                    break;
                                case "5":
                                    keyInput = "(";
                                    System.Diagnostics.Debug.WriteLine("fr-FR [ ( ]");
                                    break;
                                case "6":
                                    keyInput = "-";
                                    break;
                                case "7":
                                    keyInput = "è";
                                    break;
                                case "8":
                                    keyInput = "_";
                                    break;
                                case "9":
                                    keyInput = "ç";
                                    break;
                                case "0":
                                    keyInput = "à";
                                    break;
                                case "-":
                                    keyInput = ")";
                                    break;
                            }
                        }
                        break;
                }
            }
            */

            var listeners = element.GetHardwareKeyListeners();
            for (int i = 0; i < listeners.Count; i++)
            {
                var listener = listeners[i];
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

        public override UIKeyCommand[] KeyCommands
        {
            get
            {
                var element = HardwareKeyPage.FocusedElement ?? HardwareKeyPage.DefaultFocusedElement;
                if (element == null)
                    return null;

                var result = new List<UIKeyCommand>();
                var listeners = element.GetHardwareKeyListeners();
                for (int i = 0; i < listeners.Count; i++)
                {
                    var listener = listeners[i];
                    var keyInput = listener.HardwareKey.KeyInput;
                    NSString nsInput = null;
                    //if (keyInput.Length > 1)
                    //{
                    switch (keyInput)
                    {
                        case HardwareKey.UpArrowKeyInput:
                            nsInput = UIKeyCommand.UpArrow;
                            break;
                        case HardwareKey.DownArrowKeyInput:
                            nsInput = UIKeyCommand.DownArrow;
                            break;
                        case HardwareKey.LeftArrowKeyInput:
                            nsInput = UIKeyCommand.LeftArrow;
                            break;
                        case HardwareKey.RightArrowKeyInput:
                            nsInput = UIKeyCommand.RightArrow;
                            break;
                        case HardwareKey.EscapeKeyInput:
                            nsInput = UIKeyCommand.Escape;
                            break;
                        case HardwareKey.BackspaceDeleteKeyInput:
                            nsInput = new NSString("\b");
                            break;
                        case HardwareKey.ForwardDeleteKeyInput:
                            //nsInput = new NSString("\0x7F");
                            nsInput = new NSString("UIKeyInputDelete");
                            break;
                        // there is not an insert key on mac extended keyboard.  In it's place is the Fn?
                        case HardwareKey.TabKeyInput:
                            nsInput = new NSString("\t");
                            break;
                        case HardwareKey.EnterReturnKeyInput:
                            nsInput = new NSString("\r");
                            break;
                        case HardwareKey.PageUpKeyInput:
                            nsInput = new NSString("UIKeyInputPageUp");
                            break;
                        case HardwareKey.PageDownKeyInput:
                            nsInput = new NSString("UIKeyInputPageDown");
                            break;
                        case HardwareKey.HomeKeyInput:
                            nsInput = new NSString("UIKeyInputHome");
                            break;
                        case HardwareKey.EndKeyInput:
                            nsInput = new NSString("UIKeyInputEnd");
                            break;
                        case HardwareKey.InsertKeyInput:
                            nsInput = new NSString("UIKeyInputInsert");
                            break;

                        /* Don't know how to get Function keys on mac!
                        case HardwareKey.F1Label:
                            //nsInput = new NSString("UIKeyInputF1");
                            //nsInput = new NSString("" + (char)58);
                            nsInput = new NSString("\\^P");
                            break;
                        case HardwareKey.F2KeyInput:
                            nsInput = new NSString("UIKeyInputF2");
                            break;
                        case HardwareKey.F3KeyInput:
                            nsInput = new NSString("UIKeyInputF3");
                            break;
                        case HardwareKey.F4KeyInput:
                            nsInput = new NSString("UIKeyInputF4");
                            break;
                        case HardwareKey.F5KeyInput:
                            nsInput = new NSString("UIKeyInputF5");
                            break;
                        case HardwareKey.F6KeyInput:
                            nsInput = new NSString("UIKeyInputF6");
                            break;
                        case HardwareKey.F7KeyInput:
                            nsInput = new NSString("UIKeyInputF7");
                            break;
                        case HardwareKey.F8KeyInput:
                            nsInput = new NSString("UIKeyInputF8");
                            break;
                        case HardwareKey.F9KeyInput:
                            nsInput = new NSString("UIKeyInputF9");
                            break;
                        case HardwareKey.F10KeyInput:
                            nsInput = new NSString("UIKeyInputF10");
                            break;
                        case HardwareKey.F11KeyInput:
                            nsInput = new NSString("UIKeyInputF11");
                            break;
                        case HardwareKey.F12KeyInput:
                            nsInput = new NSString("UIKeyInputF12");
                            break;
                        */

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

                }
                return result.ToArray();
            }
        }

        UIKeyModifierFlags UIKeyModifierFlagsFrom(HardwareKeyModifierKeys modifierKeys)
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

        UIKeyCommand UIKeyCommandFrom(NSString nsInput, HardwareKeyModifierKeys modifier, string descriptiveText)
        {
            var flags = UIKeyModifierFlagsFrom(modifier);
            if (string.IsNullOrWhiteSpace(descriptiveText))
                return UIKeyCommand.Create(nsInput, flags, _onKeyPressSelector);
            else
                return UIKeyCommand.Create(nsInput, flags, _onKeyPressSelector, new NSString(descriptiveText));
        }
    }
}
