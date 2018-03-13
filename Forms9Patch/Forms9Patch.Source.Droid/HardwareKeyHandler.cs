using System;

namespace Forms9Patch.Droid
{
    public static class HardwareKeyHandler
    {
        public static bool OnKeyUp(Android.Views.Keycode keyCode, Android.Views.KeyEvent e)
        {
            var input = "" + (char)e.UnicodeChar;
            var modifiers = HardwareKeyModifierKeys.None;

            if (keyCode == Android.Views.Keycode.Numpad0)
            {
                input = "0";
                modifiers = HardwareKeyModifierKeys.NumericPadKey;
            }
            else if (keyCode == Android.Views.Keycode.Numpad1)
            {
                input = "1";
                modifiers = HardwareKeyModifierKeys.NumericPadKey;
            }
            else if (keyCode == Android.Views.Keycode.Numpad2)
            {
                input = "2";
                modifiers = HardwareKeyModifierKeys.NumericPadKey;
            }
            else if (keyCode == Android.Views.Keycode.Numpad3)
            {
                input = "3";
                modifiers = HardwareKeyModifierKeys.NumericPadKey;
            }
            else if (keyCode == Android.Views.Keycode.Numpad4)
            {
                input = "4";
                modifiers = HardwareKeyModifierKeys.NumericPadKey;
            }
            else if (keyCode == Android.Views.Keycode.Numpad5)
            {
                input = "5";
                modifiers = HardwareKeyModifierKeys.NumericPadKey;
            }
            else if (keyCode == Android.Views.Keycode.Numpad6)
            {
                input = "6";
                modifiers = HardwareKeyModifierKeys.NumericPadKey;
            }
            else if (keyCode == Android.Views.Keycode.Numpad7)
            {
                input = "7";
                modifiers = HardwareKeyModifierKeys.NumericPadKey;
            }
            else if (keyCode == Android.Views.Keycode.Numpad8)
            {
                input = "8";
                modifiers = HardwareKeyModifierKeys.NumericPadKey;
            }
            else if (keyCode == Android.Views.Keycode.Numpad9)
            {
                input = "9";
                modifiers = HardwareKeyModifierKeys.NumericPadKey;
            }
            else if (keyCode == Android.Views.Keycode.NumpadAdd)
            {
                input = "+";
                modifiers = HardwareKeyModifierKeys.NumericPadKey;
            }
            else if (keyCode == Android.Views.Keycode.NumpadComma)
            {
                input = ",";
                modifiers = HardwareKeyModifierKeys.NumericPadKey;
            }
            else if (keyCode == Android.Views.Keycode.NumpadDivide)
            {
                input = "/";
                modifiers = HardwareKeyModifierKeys.NumericPadKey;
            }
            else if (keyCode == Android.Views.Keycode.NumpadDot)
            {
                input = ".";
                modifiers = HardwareKeyModifierKeys.NumericPadKey;
            }
            else if (keyCode == Android.Views.Keycode.NumpadEnter)
            {
                input = HardwareKey.EnterReturnInput;
                modifiers = HardwareKeyModifierKeys.NumericPadKey;
            }
            else if (keyCode == Android.Views.Keycode.NumpadEquals)
            {
                input = "=";
                modifiers = HardwareKeyModifierKeys.NumericPadKey;
            }
            else if (keyCode == Android.Views.Keycode.NumpadLeftParen)
            {
                input = "(";
                modifiers = HardwareKeyModifierKeys.NumericPadKey;
            }
            else if (keyCode == Android.Views.Keycode.NumpadRightParen)
            {
                input = ")";
                modifiers = HardwareKeyModifierKeys.NumericPadKey;
            }
            else if (keyCode == Android.Views.Keycode.NumpadSubtract)
            {
                input = "-";
                modifiers = HardwareKeyModifierKeys.NumericPadKey;
            }
            else if (keyCode == Android.Views.Keycode.Numpad9)
            {
                input = "9";
                modifiers = HardwareKeyModifierKeys.NumericPadKey;
            }
            else if (keyCode == Android.Views.Keycode.DpadDown)
                input = HardwareKey.DownArrowInput;
            else if (keyCode == Android.Views.Keycode.DpadUp)
                input = HardwareKey.UpArrowInput;
            else if (keyCode == Android.Views.Keycode.DpadLeft)
                input = HardwareKey.LeftArrowInput;
            else if (keyCode == Android.Views.Keycode.DpadRight)
                input = HardwareKey.RightArrowInput;
            else if (keyCode == Android.Views.Keycode.Escape)
                input = HardwareKey.EscapeInput;
            else if (keyCode == Android.Views.Keycode.F1)
                input = HardwareKey.F1Input;
            else if (keyCode == Android.Views.Keycode.F2)
                input = HardwareKey.F1Input;
            else if (keyCode == Android.Views.Keycode.F3)
                input = HardwareKey.F1Input;
            else if (keyCode == Android.Views.Keycode.F4)
                input = HardwareKey.F1Input;
            else if (keyCode == Android.Views.Keycode.F5)
                input = HardwareKey.F1Input;
            else if (keyCode == Android.Views.Keycode.F6)
                input = HardwareKey.F1Input;
            else if (keyCode == Android.Views.Keycode.F7)
                input = HardwareKey.F1Input;
            else if (keyCode == Android.Views.Keycode.F8)
                input = HardwareKey.F1Input;
            else if (keyCode == Android.Views.Keycode.F9)
                input = HardwareKey.F1Input;
            else if (keyCode == Android.Views.Keycode.F10)
                input = HardwareKey.F1Input;
            else if (keyCode == Android.Views.Keycode.F11)
                input = HardwareKey.F1Input;
            else if (keyCode == Android.Views.Keycode.F12)
                input = HardwareKey.F1Input;
            else if (keyCode == Android.Views.Keycode.PageUp)
                input = HardwareKey.PageUpInput;
            else if (keyCode == Android.Views.Keycode.PageDown)
                input = HardwareKey.PageDownInput;
            else if (keyCode == Android.Views.Keycode.Insert)
                input = HardwareKey.InsertInput;
            else if (keyCode == Android.Views.Keycode.Del)
                input = HardwareKey.BackspaceDeleteInput;
            else if (keyCode == Android.Views.Keycode.ForwardDel)
                input = HardwareKey.ForwardDeleteInput;
            else if (keyCode == Android.Views.Keycode.Home)
                input = HardwareKey.HomeInput;
            else if (keyCode == Android.Views.Keycode.Endcall)
                input = HardwareKey.EndInput;

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

            var listeners = RootPage.HardwareKeyFocusElement.GetHardwareKeyListeners();
            for (int i = 0; i < listeners.Count; i++)
            {
                var listener = listeners[i];
                if (listener.HardwareKey.Input == input && listener.HardwareKey.ModifierKeys == modifiers)
                {
                    if (listener.Command != null && listener.Command.CanExecute(listener.CommandParameter))
                        listener.Command.Execute(listener.CommandParameter);
                    listener.Pressed?.Invoke(RootPage.HardwareKeyFocusElement, new HardwareKeyEventArgs(listener.HardwareKey, RootPage.HardwareKeyFocusElement));
                    System.Diagnostics.Debug.WriteLine("SUCCESS!!!");
                    return true;
                }
            }
            return false;
        }
    }
}