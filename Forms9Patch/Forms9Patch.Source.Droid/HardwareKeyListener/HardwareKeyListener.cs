using System;

namespace Forms9Patch.Droid
{
    public static class HardwareKeyListener
    {
        public static bool OnKeyDown(Android.Views.Keycode keyCode, Android.Views.KeyEvent e)
        {
            System.Diagnostics.Debug.WriteLine("HardwareKeyPageListener.OnKeyUp[" + keyCode + "] dispLabel=[" + (e.DisplayLabel == 0 ? "" : ""+e.DisplayLabel) + "] number=[" + (e.Number == 0 ? "" : ""+e.Number) + "] [" + (char)e.UnicodeChar == null + "] [" + e + "] ");

            var element = HardwareKeyPage.FocusedElement ?? HardwareKeyPage.DefaultFocusedElement;
            if (element == null)
                return false;

            var keyLabel = ("" + (char)e.DisplayLabel).ToUpper();
            var modifiers = HardwareKeyModifierKeys.None;
            switch (keyCode)
            {
                case Android.Views.Keycode.Numpad0:
                    keyLabel = "0";
                    modifiers = HardwareKeyModifierKeys.NumericPadKey;
                    break;
                case Android.Views.Keycode.Numpad1:
                    keyLabel = "1";
                    modifiers = HardwareKeyModifierKeys.NumericPadKey;
                    break;
                case Android.Views.Keycode.Numpad2:
                    keyLabel = "2";
                    modifiers = HardwareKeyModifierKeys.NumericPadKey;
                    break;
                case Android.Views.Keycode.Numpad3:
                    keyLabel = "3";
                    modifiers = HardwareKeyModifierKeys.NumericPadKey;
                    break;
                case Android.Views.Keycode.Numpad4:
                    keyLabel = "4";
                    modifiers = HardwareKeyModifierKeys.NumericPadKey;
                    break;
                case Android.Views.Keycode.Numpad5:
                    keyLabel = "5";
                    modifiers = HardwareKeyModifierKeys.NumericPadKey;
                    break;
                case Android.Views.Keycode.Numpad6:
                    keyLabel = "6";
                    modifiers = HardwareKeyModifierKeys.NumericPadKey;
                    break;
                case Android.Views.Keycode.Numpad7:
                    keyLabel = "7";
                    modifiers = HardwareKeyModifierKeys.NumericPadKey;
                    break;
                case Android.Views.Keycode.Numpad8:
                    keyLabel = "8";
                    modifiers = HardwareKeyModifierKeys.NumericPadKey;
                    break;
                case Android.Views.Keycode.Numpad9:
                    keyLabel = "9";
                    modifiers = HardwareKeyModifierKeys.NumericPadKey;
                    break;
                case Android.Views.Keycode.NumpadAdd:
                    keyLabel = "+";
                    modifiers = HardwareKeyModifierKeys.NumericPadKey;
                    break;
                case Android.Views.Keycode.NumpadComma:
                    keyLabel = ",";
                    modifiers = HardwareKeyModifierKeys.NumericPadKey;
                    break;
                case Android.Views.Keycode.NumpadDivide:
                    keyLabel = "/";
                    modifiers = HardwareKeyModifierKeys.NumericPadKey;
                    break;
                case Android.Views.Keycode.NumpadDot:
                    keyLabel = ".";
                    modifiers = HardwareKeyModifierKeys.NumericPadKey;
                    break;
                case Android.Views.Keycode.NumpadEnter:
                    keyLabel = HardwareKey.EnterReturnKeyLabel;
                    modifiers = HardwareKeyModifierKeys.NumericPadKey;
                    break;
                case Android.Views.Keycode.NumpadEquals:
                    keyLabel = "=";
                    modifiers = HardwareKeyModifierKeys.NumericPadKey;
                    break;
                case Android.Views.Keycode.NumpadLeftParen:
                    keyLabel = "(";
                    modifiers = HardwareKeyModifierKeys.NumericPadKey;
                    break;
                case Android.Views.Keycode.NumpadRightParen:
                    keyLabel = ")";
                    modifiers = HardwareKeyModifierKeys.NumericPadKey;
                    break;
                case Android.Views.Keycode.NumpadSubtract:
                    keyLabel = "-";
                    modifiers = HardwareKeyModifierKeys.NumericPadKey;
                    break;
                case Android.Views.Keycode.DpadDown:
                    keyLabel = HardwareKey.DownArrowKeyLabel;
                    break;
                case Android.Views.Keycode.DpadUp:
                    keyLabel = HardwareKey.UpArrowKeyLabel;
                    break;
                case Android.Views.Keycode.DpadLeft:
                    keyLabel = HardwareKey.LeftArrowKeyLabel;
                    break;
                case Android.Views.Keycode.DpadRight:
                    keyLabel = HardwareKey.RightArrowKeyLabel;
                    break;
                case Android.Views.Keycode.Escape:
                    keyLabel = HardwareKey.EscapeKeyLabel;
                    break;

                case Android.Views.Keycode.F1:
                    keyLabel = HardwareKey.F1KeyLabel;
                    break;
                case Android.Views.Keycode.F2:
                    keyLabel = HardwareKey.F2KeyLabel;
                    break;
                case Android.Views.Keycode.F3:
                    keyLabel = HardwareKey.F3KeyLabel;
                    break;
                case Android.Views.Keycode.F4:
                    keyLabel = HardwareKey.F4KeyLabel;
                    break;
                case Android.Views.Keycode.F5:
                    keyLabel = HardwareKey.F5KeyLabel;
                    break;
                case Android.Views.Keycode.F6:
                    keyLabel = HardwareKey.F6KeyLabel;
                    break;
                case Android.Views.Keycode.F7:
                    keyLabel = HardwareKey.F7KeyLabel;
                    break;
                case Android.Views.Keycode.F8:
                    keyLabel = HardwareKey.F8KeyLabel;
                    break;
                case Android.Views.Keycode.F9:
                    keyLabel = HardwareKey.F9KeyLabel;
                    break;
                case Android.Views.Keycode.F10:
                    keyLabel = HardwareKey.F10KeyLabel;
                    break;
                case Android.Views.Keycode.F11:
                    keyLabel = HardwareKey.F11KeyLabel;
                    break;
                case Android.Views.Keycode.F12:
                    keyLabel = HardwareKey.F12KeyLabel;
                    break;

                case Android.Views.Keycode.PageUp:
                    keyLabel = HardwareKey.PageUpKeyLabel;
                    break;
                case Android.Views.Keycode.PageDown:
                    keyLabel = HardwareKey.PageDownKeyLabel;
                    break;
                case Android.Views.Keycode.Insert:
                    keyLabel = HardwareKey.InsertKeyLabel;
                    break;
                case Android.Views.Keycode.Del:
                    keyLabel = HardwareKey.BackspaceDeleteKeyLabel;
                    break;
                case Android.Views.Keycode.ForwardDel:
                    keyLabel = HardwareKey.ForwardDeleteKeyLabel;
                    break;
                case Android.Views.Keycode.Home:
                    keyLabel = HardwareKey.HomeKeyLabel;
                    break;
                case Android.Views.Keycode.Endcall:
                    keyLabel = HardwareKey.EndKeyLabel;
                    break;

                case Android.Views.Keycode.Num0:
                    keyLabel = "0";
                    break;
                case Android.Views.Keycode.Num1:
                    keyLabel = "1";
                    break;
                case Android.Views.Keycode.Num2:
                    keyLabel = "2";
                    break;
                case Android.Views.Keycode.Num3:
                    keyLabel = "3";
                    break;
                case Android.Views.Keycode.Num4:
                    keyLabel = "4";
                    break;
                case Android.Views.Keycode.Num5:
                    keyLabel = "5";
                    break;
                case Android.Views.Keycode.Num6:
                    keyLabel = "6";
                    break;
                case Android.Views.Keycode.Num7:
                    keyLabel = "7";
                    break;
                case Android.Views.Keycode.Num8:
                    keyLabel = "8";
                    break;
                case Android.Views.Keycode.Num9:
                    keyLabel = "9";
                    break;

                default:
                    if (e.UnicodeChar == 0)
                        return false;
                    break;
            }

            if ((e.Modifiers & Android.Views.MetaKeyStates.CapsLockOn) > 0)
                modifiers |= HardwareKeyModifierKeys.CapsLock;
            if ((e.Modifiers & Android.Views.MetaKeyStates.AltMask) > 0)
                modifiers |= HardwareKeyModifierKeys.Alternate;
            if ((e.Modifiers & Android.Views.MetaKeyStates.CtrlMask) > 0)
                modifiers |= HardwareKeyModifierKeys.Control;
            if ((e.Modifiers & Android.Views.MetaKeyStates.FunctionOn) > 0)
                modifiers |= HardwareKeyModifierKeys.FunctionKey;
            if ((e.Modifiers & Android.Views.MetaKeyStates.SymOn) > 0)
                modifiers |= HardwareKeyModifierKeys.PlatformKey;
            if ((e.Modifiers & Android.Views.MetaKeyStates.ShiftMask) > 0)
                modifiers |= HardwareKeyModifierKeys.Shift;

            var listeners = element.GetHardwareKeyListeners();
            for (int i = 0; i < listeners.Count; i++)
            {
                var listener = listeners[i];
                if (listener.HardwareKey.KeyLabel == keyLabel && listener.HardwareKey.ModifierKeys == modifiers)
                {
                    if (listener.Command != null && listener.Command.CanExecute(listener.CommandParameter))
                        listener.Command.Execute(listener.CommandParameter);
                    listener.Pressed?.Invoke(element, new HardwareKeyEventArgs(listener.HardwareKey, element));
                    //System.Diagnostics.Debug.WriteLine("SUCCESS!!!");
                    return true;
                }
            }
            return false;
        }
    }
}