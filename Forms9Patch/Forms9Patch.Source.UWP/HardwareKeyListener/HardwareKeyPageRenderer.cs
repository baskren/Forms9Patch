using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(Forms9Patch.HardwareKeyPage), typeof(Forms9Patch.UWP.HardwareKeyPageRenderer))]
//[assembly: ExportRenderer(typeof(Forms9Patch.ContentPage), typeof(Forms9Patch.UWP.PageRenderer))]
namespace Forms9Patch.UWP
{
    public class HardwareKeyPageRenderer : Xamarin.Forms.Platform.UWP.PageRenderer
    {
        void Connect()
        {
            Window.Current.CoreWindow.CharacterReceived += OnCharacterReceived;
            Window.Current.CoreWindow.KeyDown += OnKeyDown;
            //Window.Current.CoreWindow.KeyUp += CoreWindow_KeyUp;

            //var interceptor = Windows.UI.Input.KeyboardDeliveryInterceptor.GetForCurrentView();
            //interceptor.KeyDown += OnInterceptorKeyDown;
            //interceptor.IsInterceptionEnabledWhenInForeground = true;
        }

        void Disconnect()
        {
            Window.Current.CoreWindow.CharacterReceived -= OnCharacterReceived;
            Window.Current.CoreWindow.KeyDown -= OnKeyDown;
            //Window.Current.CoreWindow.KeyUp -= CoreWindow_KeyUp;

            //var interceptor = Windows.UI.Input.KeyboardDeliveryInterceptor.GetForCurrentView();
            //interceptor.KeyDown -= OnInterceptorKeyDown;
            //interceptor.IsInterceptionEnabledWhenInForeground = false;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null)
                Disconnect();
            if (e.NewElement != null)
                Connect();
        }

        static bool keyDownCaptured = false;

        private void OnInterceptorKeyDown(KeyboardDeliveryInterceptor sender, KeyEventArgs args)
        {
            args.Handled = ProcessVirualKey(HardwareKeyPage.FocusedElement ?? HardwareKeyPage.DefaultFocusedElement, args.VirtualKey);
        }

        public void OnKeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            args.Handled = ProcessVirualKey(HardwareKeyPage.FocusedElement ?? HardwareKeyPage.DefaultFocusedElement, args.VirtualKey);
        }

        public static bool ProcessVirualKey(VisualElement element, Windows.System.VirtualKey virtualKey)
        {
            keyDownCaptured = false;
            var keyInput = "";
            switch (virtualKey)
            {
                case Windows.System.VirtualKey.Back:
                    keyInput = Forms9Patch.HardwareKey.BackspaceDeleteKeyInput;
                    break;
                case Windows.System.VirtualKey.Delete:
                    keyInput = Forms9Patch.HardwareKey.ForwardDeleteKeyInput;
                    break;
                case Windows.System.VirtualKey.Down:
                    keyInput = Forms9Patch.HardwareKey.DownArrowKeyInput;
                    break;
                case Windows.System.VirtualKey.End:
                    keyInput = Forms9Patch.HardwareKey.EndKeyInput;
                    break;
                case Windows.System.VirtualKey.Escape:
                    keyInput = Forms9Patch.HardwareKey.EscapeKeyInput;
                    break;
                case Windows.System.VirtualKey.F1:
                    keyInput = Forms9Patch.HardwareKey.F1KeyInput;
                    break;
                case Windows.System.VirtualKey.F2:
                    keyInput = Forms9Patch.HardwareKey.F2KeyInput;
                    break;
                case Windows.System.VirtualKey.F3:
                    keyInput = Forms9Patch.HardwareKey.F3KeyInput;
                    break;
                case Windows.System.VirtualKey.F4:
                    keyInput = Forms9Patch.HardwareKey.F4KeyInput;
                    break;
                case Windows.System.VirtualKey.F5:
                    keyInput = Forms9Patch.HardwareKey.F5KeyInput;
                    break;
                case Windows.System.VirtualKey.F6:
                    keyInput = Forms9Patch.HardwareKey.F6KeyInput;
                    break;
                case Windows.System.VirtualKey.F7:
                    keyInput = Forms9Patch.HardwareKey.F7KeyInput;
                    break;
                case Windows.System.VirtualKey.F8:
                    keyInput = Forms9Patch.HardwareKey.F8KeyInput;
                    break;
                case Windows.System.VirtualKey.F9:
                    keyInput = Forms9Patch.HardwareKey.F9KeyInput;
                    break;
                case Windows.System.VirtualKey.F10:
                    keyInput = Forms9Patch.HardwareKey.F10KeyInput;
                    break;
                case Windows.System.VirtualKey.F11:
                    keyInput = Forms9Patch.HardwareKey.F11KeyInput;
                    break;
                case Windows.System.VirtualKey.F12:
                    keyInput = Forms9Patch.HardwareKey.F12KeyInput;
                    break;
                case Windows.System.VirtualKey.Home:
                    keyInput = Forms9Patch.HardwareKey.HomeKeyInput;
                    break;
                case Windows.System.VirtualKey.Insert:
                    keyInput = Forms9Patch.HardwareKey.InsertKeyInput;
                    break;
                case Windows.System.VirtualKey.Left:
                    keyInput = Forms9Patch.HardwareKey.LeftArrowKeyInput;
                    break;
                case Windows.System.VirtualKey.PageDown:
                    keyInput = Forms9Patch.HardwareKey.PageDownKeyInput;
                    break;
                case Windows.System.VirtualKey.PageUp:
                    keyInput = Forms9Patch.HardwareKey.PageUpKeyInput;
                    break;
                case Windows.System.VirtualKey.Right:
                    keyInput = Forms9Patch.HardwareKey.RightArrowKeyInput;
                    break;
                case Windows.System.VirtualKey.Up:
                    keyInput = Forms9Patch.HardwareKey.UpArrowKeyInput;
                    break;

                /*
            case Windows.System.VirtualKey.NumberPad0:
                keyInput = Forms9Patch.HardwareKey.Numpad0;
                keyDownCaptured = true;
                break;
            case Windows.System.VirtualKey.NumberPad1:
                keyInput = Forms9Patch.HardwareKey.Numpad1;
                keyDownCaptured = true;
                break;
            case Windows.System.VirtualKey.NumberPad2:
                keyInput = Forms9Patch.HardwareKey.Numpad2;
                keyDownCaptured = true;
                break;
            case Windows.System.VirtualKey.NumberPad3:
                keyInput = Forms9Patch.HardwareKey.Numpad3;
                keyDownCaptured = true;
                break;
            case Windows.System.VirtualKey.NumberPad4:
                keyInput = Forms9Patch.HardwareKey.Numpad4;
                keyDownCaptured = true;
                break;
            case Windows.System.VirtualKey.NumberPad5:
                keyInput = Forms9Patch.HardwareKey.Numpad5;
                keyDownCaptured = true;
                break;
            case Windows.System.VirtualKey.NumberPad6:
                keyInput = Forms9Patch.HardwareKey.Numpad6;
                keyDownCaptured = true;
                break;
            case Windows.System.VirtualKey.NumberPad7:
                keyInput = Forms9Patch.HardwareKey.Numpad7;
                keyDownCaptured = true;
                break;
            case Windows.System.VirtualKey.NumberPad8:
                keyInput = Forms9Patch.HardwareKey.Numpad8;
                keyDownCaptured = true;
                break;
            case Windows.System.VirtualKey.NumberPad9:
                keyInput = Forms9Patch.HardwareKey.Numpad9;
                keyDownCaptured = true;
                break;

            case Windows.System.VirtualKey.Number0:
                keyInput = "0";
                keyDownCaptured = true;
                break;
            case Windows.System.VirtualKey.Number1:
                keyInput = "1";
                keyDownCaptured = true;
                break;
            case Windows.System.VirtualKey.Number2:
                keyInput = "2";
                keyDownCaptured = true;
                break;
            case Windows.System.VirtualKey.Number3:
                keyInput = "3";
                keyDownCaptured = true;
                break;
            case Windows.System.VirtualKey.Number4:
                keyInput = "4";
                keyDownCaptured = true;
                break;
            case Windows.System.VirtualKey.Number5:
                keyInput = "5";
                keyDownCaptured = true;
                break;
            case Windows.System.VirtualKey.Number6:
                keyInput = "6";
                keyDownCaptured = true;
                break;
            case Windows.System.VirtualKey.Number7:
                keyInput = "7";
                keyDownCaptured = true;
                break;
            case Windows.System.VirtualKey.Number8:
                keyInput = "8";
                keyDownCaptured = true;
                break;
            case Windows.System.VirtualKey.Number9:
                keyInput = "9";
                keyDownCaptured = true;
                break;
                */


                default:
                    return false;
            }

            var modifiers = GetModifierKeys();
            //var result = new Forms9Patch.HardwareKey(keyInput, GetModifierKeys());

            var listeners = element.GetHardwareKeyListeners();
            for (int i = 0; i < listeners.Count; i++)
            {
                var listener = listeners[i];
                if (string.IsNullOrEmpty(listener.HardwareKey.KeyInput))
                    continue;
                if (listener.HardwareKey.KeyInput == keyInput && listener.HardwareKey.ModifierKeys == modifiers)
                {
                    if (listener.Command != null && listener.Command.CanExecute(listener.CommandParameter))
                        listener.Command.Execute(listener.CommandParameter);
                    listener.Pressed?.Invoke(element, new HardwareKeyEventArgs(listener.HardwareKey, element));
                    //System.Diagnostics.Debug.WriteLine("SUCCESS!!!");
                    return true;
                }
            }


            //System.Diagnostics.Debug.WriteLine("CoreWindow.KeyDown ["+args.VirtualKey+"] dev:["+args.DeviceId+"] handled["+args.Handled+"]" + KeyStatusString(args.KeyStatus));

            return false;
        }

        public void OnCharacterReceived(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.CharacterReceivedEventArgs args)
        {
            ProcessCharacter(HardwareKeyPage.FocusedElement ?? HardwareKeyPage.DefaultFocusedElement, (char)args.KeyCode);
        }

        public static bool ProcessCharacter(VisualElement element, char keyCode)
        {
            if (keyDownCaptured)
            {
                keyDownCaptured = false;
                return false;
            }

            var keyInput = ("" + keyCode).ToUpper();
            System.Diagnostics.Debug.WriteLine("CoreWindow_CharacterReceived keyInput=[" + keyInput + "]");

            var modifiers = GetModifierKeys();
            //var result = new Forms9Patch.HardwareKey(keyInput, GetModifierKeys());

            var listeners = element.GetHardwareKeyListeners();
            for (int i = 0; i < listeners.Count; i++)
            {
                var listener = listeners[i];
                if (string.IsNullOrEmpty(listener.HardwareKey.KeyInput))
                    continue;
                if (listener.HardwareKey.KeyInput == keyInput.ToUpper() && (listener.HardwareKey.ModifierKeys == modifiers || listener.HardwareKey.ModifierKeys.HasFlag(HardwareKeyModifierKeys.Any)))
                {
                    if (listener.Command != null && listener.Command.CanExecute(listener.CommandParameter))
                        listener.Command.Execute(listener.CommandParameter);
                    var observedHardwareKey = new HardwareKey(keyInput.ToUpper(), modifiers);
                    listener.Pressed?.Invoke(element, new HardwareKeyEventArgs(observedHardwareKey, element));
                    return true;
                }
            }
            return false;
        }

        string KeyStatusString(Windows.UI.Core.CorePhysicalKeyStatus KeyStatus)
        {
            //var shiftState = Window.Current.CoreWindow.GetForCurrentThread().GetKeyState(VirtualKey.Shift);
            //var ctrlState = Window.Current.CoreWindow.GetForCurrentThread().GetKeyState(VirtualKey.Control);

            var shiftState = Window.Current.CoreWindow.GetKeyState(Windows.System.VirtualKey.Shift).HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down);
            var ctrlState = Window.Current.CoreWindow.GetKeyState(Windows.System.VirtualKey.Control).HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down);
            var altState = Window.Current.CoreWindow.GetKeyState(Windows.System.VirtualKey.Menu).HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down);
            var platformState = Window.Current.CoreWindow.GetKeyState(Windows.System.VirtualKey.LeftWindows).HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down);
            platformState |= Window.Current.CoreWindow.GetKeyState(Windows.System.VirtualKey.RightWindows).HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down);
            var capsState = Window.Current.CoreWindow.GetKeyState(Windows.System.VirtualKey.CapitalLock).HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down);
            var numLock = Window.Current.CoreWindow.GetKeyState(Windows.System.VirtualKey.NumberKeyLock).HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down);



            return "shft=[" + shiftState + "] ctrl=[" + ctrlState + "] altState=[" + altState + "] pltf=[" + platformState + "] capl=[" + capsState + "] numl=[" + numLock + "] ext:[" + KeyStatus.IsExtendedKey + "] rel:[" + KeyStatus.IsKeyReleased + "] men:[" + KeyStatus.IsMenuKeyDown + "]  rep:[" + KeyStatus.RepeatCount + "] cod:[" + KeyStatus.ScanCode + "] wasDown:[" + KeyStatus.WasKeyDown + "]";

        }

        static Forms9Patch.HardwareKeyModifierKeys GetModifierKeys()
        {
            var shiftState = Window.Current.CoreWindow.GetKeyState(Windows.System.VirtualKey.Shift).HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down);
            var ctrlState = Window.Current.CoreWindow.GetKeyState(Windows.System.VirtualKey.Control).HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down);
            var altState = Window.Current.CoreWindow.GetKeyState(Windows.System.VirtualKey.Menu).HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down);
            var platformState = Window.Current.CoreWindow.GetKeyState(Windows.System.VirtualKey.LeftWindows).HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down);
            platformState |= Window.Current.CoreWindow.GetKeyState(Windows.System.VirtualKey.RightWindows).HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down);
            var capsState = Window.Current.CoreWindow.GetKeyState(Windows.System.VirtualKey.CapitalLock).HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down);
            var numLock = Window.Current.CoreWindow.GetKeyState(Windows.System.VirtualKey.NumberKeyLock).HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down);

            Forms9Patch.HardwareKeyModifierKeys result = HardwareKeyModifierKeys.None;
            if (shiftState)
                result |= HardwareKeyModifierKeys.Shift;
            if (ctrlState)
                result |= HardwareKeyModifierKeys.Control;
            if (altState)
                result |= HardwareKeyModifierKeys.Alternate;
            if (platformState)
                result |= HardwareKeyModifierKeys.PlatformKey;
            if (capsState)
                result |= HardwareKeyModifierKeys.CapsLock;

            return result;
        }

    }
}
