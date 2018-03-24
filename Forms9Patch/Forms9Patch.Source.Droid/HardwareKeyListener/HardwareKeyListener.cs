using System;

namespace Forms9Patch.Droid
{
    public static class HardwareKeyListener
    {
        public static bool OnKeyDown(Android.Views.Keycode keyCode, Android.Views.KeyEvent e)
        {
            //System.Diagnostics.Debug.WriteLine("HardwareKeyPageListener.OnKeyDown[" + keyCode + "] dispLabel=[" + (e.DisplayLabel == 0 ? "" : "" + e.DisplayLabel) + "] number=[" + (e.Number == 0 ? "" : "" + e.Number) + "] [" + (char)e.UnicodeChar == null + "] [" + e + "] ");
            System.Diagnostics.Debug.WriteLine("HardwareKeyPageListener.OnKeyDown:");
            System.Diagnostics.Debug.WriteLine("\t\t keyCode=[" + keyCode + "]");
            System.Diagnostics.Debug.WriteLine("\t\t DisplayLabel=[" + (e.DisplayLabel == 0 ? "" : "" + e.DisplayLabel) + "]");
            System.Diagnostics.Debug.WriteLine("\t\t Number=[" + (e.Number == 0 ? "" : "" + e.Number) + "]");
            System.Diagnostics.Debug.WriteLine("\t\t UnicodeChar=[" + (e.UnicodeChar == 0 ? "" : ((char)e.UnicodeChar).ToString()) + "]");

            var element = HardwareKeyPage.FocusedElement ?? HardwareKeyPage.DefaultFocusedElement;
            if (element == null)
                return false;

            var keyInput = ("" + Convert.ToChar(e.UnicodeChar)).ToUpper();
            var modifiers = HardwareKeyModifierKeys.None;
            bool useShift = true;
            switch (keyCode)
            {
                case Android.Views.Keycode.Numpad0:
                    keyInput = "0";
                    modifiers = HardwareKeyModifierKeys.NumericPadKey;
                    useShift = false;
                    break;
                case Android.Views.Keycode.Numpad1:
                    keyInput = "1";
                    modifiers = HardwareKeyModifierKeys.NumericPadKey;
                    useShift = false;
                    break;
                case Android.Views.Keycode.Numpad2:
                    keyInput = "2";
                    modifiers = HardwareKeyModifierKeys.NumericPadKey;
                    useShift = false;
                    break;
                case Android.Views.Keycode.Numpad3:
                    keyInput = "3";
                    modifiers = HardwareKeyModifierKeys.NumericPadKey;
                    useShift = false;
                    break;
                case Android.Views.Keycode.Numpad4:
                    keyInput = "4";
                    modifiers = HardwareKeyModifierKeys.NumericPadKey;
                    useShift = false;
                    break;
                case Android.Views.Keycode.Numpad5:
                    keyInput = "5";
                    modifiers = HardwareKeyModifierKeys.NumericPadKey;
                    useShift = false;
                    break;
                case Android.Views.Keycode.Numpad6:
                    keyInput = "6";
                    modifiers = HardwareKeyModifierKeys.NumericPadKey;
                    useShift = false;
                    break;
                case Android.Views.Keycode.Numpad7:
                    keyInput = "7";
                    modifiers = HardwareKeyModifierKeys.NumericPadKey;
                    useShift = false;
                    break;
                case Android.Views.Keycode.Numpad8:
                    keyInput = "8";
                    modifiers = HardwareKeyModifierKeys.NumericPadKey;
                    useShift = false;
                    break;
                case Android.Views.Keycode.Numpad9:
                    keyInput = "9";
                    modifiers = HardwareKeyModifierKeys.NumericPadKey;
                    useShift = false;
                    break;
                case Android.Views.Keycode.NumpadAdd:
                    keyInput = "+";
                    useShift = false;
                    break;
                case Android.Views.Keycode.NumpadComma:
                    keyInput = ",";
                    useShift = false;
                    break;
                case Android.Views.Keycode.NumpadDivide:
                    keyInput = "/";
                    useShift = false;
                    break;
                case Android.Views.Keycode.NumpadDot:
                    keyInput = ".";
                    useShift = false;
                    break;
                case Android.Views.Keycode.NumpadEnter:
                    keyInput = HardwareKey.EnterReturnKeyInput;
                    useShift = false;
                    break;
                case Android.Views.Keycode.NumpadEquals:
                    keyInput = "=";
                    useShift = false;
                    break;
                case Android.Views.Keycode.NumpadLeftParen:
                    keyInput = "(";
                    useShift = false;
                    break;
                case Android.Views.Keycode.NumpadRightParen:
                    keyInput = ")";
                    useShift = false;
                    break;
                case Android.Views.Keycode.NumpadSubtract:
                    keyInput = "-";
                    useShift = false;
                    break;
                case Android.Views.Keycode.DpadDown:
                    keyInput = HardwareKey.DownArrowKeyInput;
                    break;
                case Android.Views.Keycode.DpadUp:
                    keyInput = HardwareKey.UpArrowKeyInput;
                    break;
                case Android.Views.Keycode.DpadLeft:
                    keyInput = HardwareKey.LeftArrowKeyInput;
                    break;
                case Android.Views.Keycode.DpadRight:
                    keyInput = HardwareKey.RightArrowKeyInput;
                    break;
                case Android.Views.Keycode.Escape:
                    keyInput = HardwareKey.EscapeKeyInput;
                    break;

                case Android.Views.Keycode.F1:
                    keyInput = HardwareKey.F1KeyInput;
                    break;
                case Android.Views.Keycode.F2:
                    keyInput = HardwareKey.F2KeyInput;
                    break;
                case Android.Views.Keycode.F3:
                    keyInput = HardwareKey.F3KeyInput;
                    break;
                case Android.Views.Keycode.F4:
                    keyInput = HardwareKey.F4KeyInput;
                    break;
                case Android.Views.Keycode.F5:
                    keyInput = HardwareKey.F5KeyInput;
                    break;
                case Android.Views.Keycode.F6:
                    keyInput = HardwareKey.F6KeyInput;
                    break;
                case Android.Views.Keycode.F7:
                    keyInput = HardwareKey.F7KeyInput;
                    break;
                case Android.Views.Keycode.F8:
                    keyInput = HardwareKey.F8KeyInput;
                    break;
                case Android.Views.Keycode.F9:
                    keyInput = HardwareKey.F9KeyInput;
                    break;
                case Android.Views.Keycode.F10:
                    keyInput = HardwareKey.F10KeyInput;
                    break;
                case Android.Views.Keycode.F11:
                    keyInput = HardwareKey.F11KeyInput;
                    break;
                case Android.Views.Keycode.F12:
                    keyInput = HardwareKey.F12KeyInput;
                    break;

                case Android.Views.Keycode.PageUp:
                    keyInput = HardwareKey.PageUpKeyInput;
                    break;
                case Android.Views.Keycode.PageDown:
                    keyInput = HardwareKey.PageDownKeyInput;
                    break;
                case Android.Views.Keycode.Insert:
                    keyInput = HardwareKey.InsertKeyInput;
                    break;
                case Android.Views.Keycode.Del:
                    keyInput = HardwareKey.BackspaceDeleteKeyInput;
                    break;
                case Android.Views.Keycode.ForwardDel:
                    keyInput = HardwareKey.ForwardDeleteKeyInput;
                    break;
                case Android.Views.Keycode.Home:
                    keyInput = HardwareKey.HomeKeyInput;
                    break;
                case Android.Views.Keycode.Endcall:
                    keyInput = HardwareKey.EndKeyInput;
                    break;
                /*
            case Android.Views.Keycode.Num0:
                keyInput = "0";
                break;
            case Android.Views.Keycode.Num1:
                keyInput = "1";
                break;
            case Android.Views.Keycode.Num2:
                keyInput = "2";
                break;
            case Android.Views.Keycode.Num3:
                keyInput = "3";
                break;
            case Android.Views.Keycode.Num4:
                keyInput = "4";
                break;
            case Android.Views.Keycode.Num5:
                keyInput = "5";
                break;
            case Android.Views.Keycode.Num6:
                keyInput = "6";
                break;
            case Android.Views.Keycode.Num7:
                keyInput = "7";
                break;
            case Android.Views.Keycode.Num8:
                keyInput = "8";
                break;
            case Android.Views.Keycode.Num9:
                keyInput = "9";
                break;
*/
                default:
                    if (e.UnicodeChar == 0)
                        return false;
                    useShift = keyInput.Length > 0 && char.IsLetter(keyInput[0]);
                    break;
            }

            /*
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
            if (keyInput.Length > 0 && char.IsLetter(keyInput[0]) && (e.Modifiers & Android.Views.MetaKeyStates.ShiftMask) > 0)
                modifiers |= HardwareKeyModifierKeys.Shift;
                */

            modifiers |= GetModifierKeys(e.Modifiers, useShift);

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
                    return true;
                }
            }
            return false;
        }

        static Forms9Patch.HardwareKeyModifierKeys GetModifierKeys(Android.Views.MetaKeyStates metaKeyStates, bool includeShift)
        {
            var modifiers = HardwareKeyModifierKeys.None;
            if ((metaKeyStates & Android.Views.MetaKeyStates.CapsLockOn) > 0)
                modifiers |= HardwareKeyModifierKeys.CapsLock;
            if ((metaKeyStates & Android.Views.MetaKeyStates.AltMask) > 0)
                modifiers |= HardwareKeyModifierKeys.Alternate;
            if ((metaKeyStates & Android.Views.MetaKeyStates.CtrlMask) > 0)
                modifiers |= HardwareKeyModifierKeys.Control;
            if ((metaKeyStates & Android.Views.MetaKeyStates.FunctionOn) > 0)
                modifiers |= HardwareKeyModifierKeys.FunctionKey;
            if ((metaKeyStates & Android.Views.MetaKeyStates.SymOn) > 0)
                modifiers |= HardwareKeyModifierKeys.PlatformKey;
            if (includeShift && (metaKeyStates & Android.Views.MetaKeyStates.ShiftMask) > 0)
                modifiers |= HardwareKeyModifierKeys.Shift;
            return modifiers;
        }
    }
}