using System;
using System.Collections.Generic;
using Foundation;
using UIKit;
using ObjCRuntime;
using Xamarin.Forms;
using P42.Utils;

//[assembly: ExportRenderer(typeof(Forms9Patch.RootPage), typeof(Forms9Patch.iOS.PageRenderer))]
[assembly: ExportRenderer(typeof(Xamarin.Forms.Page), typeof(Forms9Patch.iOS.PageRenderer))]
//[assembly: ExportRenderer(typeof(Xamarin.Forms.ContentPage), typeof(Forms9Patch.iOS.PageRenderer))]
namespace Forms9Patch.iOS
{
    public class PageRenderer : Xamarin.Forms.Platform.iOS.PageRenderer
    {
        public override bool CanBecomeFirstResponder => true;

        [Export("OnKeyPress:")]
        void OnKeyPress(UIKeyCommand cmd)
        {
            var element = HardwareKeyFocus.Element ?? HardwareKeyFocus.DefaultElement;
            if (element == null)
                return;
            //System.Diagnostics.Debug.WriteLine("Forms9Patch.PageRenderer: cmd.Input=[" + cmd.Input + "] cmd.ModifierFlags[" + cmd.ModifierFlags + "] ");

            var isFirstResponder = CanBecomeFirstResponder;
            var directBinding = (bool)this.GetPropertyValue("IsDirectBinding");

            var modifiers = HardwareKeyModifierKeys.None;
            string input = cmd.Input.ToString();
            if (cmd.Input == UIKeyCommand.DownArrow)
                input = HardwareKey.DownArrowInput;
            else if (cmd.Input == UIKeyCommand.UpArrow)
                input = HardwareKey.UpArrowInput;
            else if (cmd.Input == UIKeyCommand.LeftArrow)
                input = HardwareKey.LeftArrowInput;
            else if (cmd.Input == UIKeyCommand.RightArrow)
                input = HardwareKey.RightArrowInput;
            else if (cmd.Input == UIKeyCommand.Escape)
                input = HardwareKey.EscapeInput;
            else if (cmd.Input == "\b")
                input = HardwareKey.BackspaceDeleteInput;
            else if (cmd.Input == "\0x7F")
                input = HardwareKey.ForwardDeleteInput;
            else if (cmd.Input == "\t")
                input = HardwareKey.TabInput;
            else if (cmd.Input == "\r")
                input = HardwareKey.EnterReturnInput;
            else if (cmd.Input == "UIKeyInputPageUp")
                input = HardwareKey.PageUpInput;
            else if (cmd.Input == "UIKeyInputPageDown")
                input = HardwareKey.PageDownInput;
            else if (cmd.Input == "UIKeyInputHome")
                input = HardwareKey.HomeInput;
            else if (cmd.Input == "UIKeyInputEnd")
                input = HardwareKey.EndInput;


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
                if (listener.HardwareKey.Input.ToLower() == input && listener.HardwareKey.ModifierKeys == modifiers)
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
                var element = HardwareKeyFocus.Element ?? HardwareKeyFocus.DefaultElement;
                if (element == null)
                    return null;

                var sel = new Selector("OnKeyPress:");
                var result = new List<UIKeyCommand>();
                var listeners = element.GetHardwareKeyListeners();
                for (int i = 0; i < listeners.Count; i++)
                {
                    var listener = listeners[i];
                    var input = listener.HardwareKey.Input;
                    UIKeyModifierFlags flags = (UIKeyModifierFlags)0;
                    NSString nsInput = null;
                    if (input.Length > 1)
                    {
                        switch (input)
                        {
                            case HardwareKey.UpArrowInput:
                                nsInput = UIKeyCommand.UpArrow;
                                break;
                            case HardwareKey.DownArrowInput:
                                nsInput = UIKeyCommand.DownArrow;
                                break;
                            case HardwareKey.LeftArrowInput:
                                nsInput = UIKeyCommand.LeftArrow;
                                break;
                            case HardwareKey.RightArrowInput:
                                nsInput = UIKeyCommand.RightArrow;
                                break;
                            case HardwareKey.EscapeInput:
                                nsInput = UIKeyCommand.Escape;
                                break;
                            case HardwareKey.BackspaceDeleteInput:
                                nsInput = new NSString("\b");
                                break;
                            case HardwareKey.ForwardDeleteInput:
                                nsInput = new NSString("\0x7F");
                                break;
                            // there is not an insert key on macs.  Maps to Fn key?
                            case HardwareKey.TabInput:
                                nsInput = new NSString("\t");
                                break;
                            case HardwareKey.EnterReturnInput:
                                nsInput = new NSString("\r");
                                break;
                            case HardwareKey.PageUpInput:
                                nsInput = new NSString("UIKeyInputPageUp");
                                break;
                            case HardwareKey.PageDownInput:
                                nsInput = new NSString("UIKeyInputPageDown");
                                break;
                            case HardwareKey.HomeInput:
                                nsInput = new NSString("UIKeyInputHome");
                                break;
                            case HardwareKey.EndInput:
                                nsInput = new NSString("UIKeyInputEnd");
                                break;
                            default:
                                continue;
                        }
                    }
                    else
                        nsInput = new NSString(input.ToLower());

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
