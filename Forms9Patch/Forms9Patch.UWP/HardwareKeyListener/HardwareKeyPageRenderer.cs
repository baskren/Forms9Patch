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

        #region Static Implementation
        //static List<HardwareKeyPageRenderer> ActiveInstances = new List<HardwareKeyPageRenderer>();

        static HardwareKeyPageRenderer()
        {
            //Window.Current.Activated += OnActivated;
            //Window.Current.VisibilityChanged += OnVisibilityChanged;
            HardwareKeyPage.RemoveNativeFocus = OnRemoveNativeFocus;
        }
        /*
        private static void OnVisibilityChanged(object sender, VisibilityChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("VISIBILITY: "+e.Visible);
        }

        private static void OnActivated(object sender, WindowActivatedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ACTIVATED: "+e.WindowActivationState);
        }
        */
        #endregion


        #region Fields
        //Windows.UI.Xaml.Controls.Button _removeFocusControl;
        #endregion


        #region Connect / Disconnect
        void Connect(HardwareKeyPage element)
        {
            //ActiveInstances.Add(this);
            if (element != null)
            {
                Window.Current.CoreWindow.CharacterReceived += OnCharacterReceived;
                Window.Current.CoreWindow.KeyDown += OnKeyDown;
                //Window.Current.CoreWindow.KeyUp += CoreWindow_KeyUp;

                //var interceptor = Windows.UI.Input.KeyboardDeliveryInterceptor.GetForCurrentView();
                //interceptor.KeyDown += OnInterceptorKeyDown;
                //interceptor.IsInterceptionEnabledWhenInForeground = true;

                /*
                //HardwareKeyPage.SetNativeFocused = OnSetNativeFocused;
                HardwareKeyPage.GetNativeFocused = OnGetNativeFocused;

                if (_removeFocusControl == null)
                {
                    _removeFocusControl = new Windows.UI.Xaml.Controls.Button();
                    Children.Add((_removeFocusControl));
                }
                */
            }
        }

        void Disconnect(HardwareKeyPage element)
        {
            //ActiveInstances.Remove(this);
            if (element != null)
            {
                /*
                if (ActiveInstances.Count > 0)
                    HardwareKeyPage.RemoveNativeFocus = ActiveInstances.Last().OnRemoveNativeFocus;
                else
                    HardwareKeyPage.RemoveNativeFocus = null;
                _removeFocusControl = null;
                */

                Window.Current.CoreWindow.CharacterReceived -= OnCharacterReceived;
                Window.Current.CoreWindow.KeyDown -= OnKeyDown;
                //Window.Current.CoreWindow.KeyUp -= CoreWindow_KeyUp;

                //var interceptor = Windows.UI.Input.KeyboardDeliveryInterceptor.GetForCurrentView();
                //interceptor.KeyDown -= OnInterceptorKeyDown;
                //interceptor.IsInterceptionEnabledWhenInForeground = false;

            }
        }
        #endregion

        /*
        private void OnRemoveNativeFocus()
        {
            System.Diagnostics.Debug.Write("OnRemoveNativeFocus ");

            if (_removeFocusControl is Windows.UI.Xaml.Controls.ContentControl logicalFocus)
            {
                logicalFocus.Focus(FocusState.Programmatic);
                System.Diagnostics.Debug.Write(" focus set to [" + logicalFocus + "]");
            }
            System.Diagnostics.Debug.WriteLine("");
        }

        static object OnGetNativeFocusedControl()
        {
            return FocusManager.GetFocusedElement();
        }

        */

        /*
        private static void OnSetNativeFocused(object nativeControl)
        {
         
            if (nativeControl is Windows.UI.Xaml.Controls.ContentControl contentControl)
            //if (_logicalFocus is Xamarin.Forms.Platform.UWP.PageControl pageControl)
                contentControl?.Focus(FocusState.Programmatic);
        //var pageControl = this.GetFurthestAncestor<Xamarin.Forms.Platform.UWP.PageControl>();
        //pageControl?.Focus(FocusState.Programmatic);
        }
        */

        static void OnRemoveNativeFocus()
        {
            UnfocusNativeControl(FocusManager.GetFocusedElement());
        }

        static void UnfocusNativeControl(object obj)
        {
            if (obj is Windows.UI.Xaml.Controls.Control control)
                UnfocusNativeControl(control);
        }

        static void UnfocusNativeControl(Windows.UI.Xaml.Controls.Control control)
        {
            if (control == null)
                return;

            // "Unfocusing" doesn't really make sense on Windows; for accessibility reasons,
            // something always has focus. So forcing the unfocusing of a control would normally 
            // just move focus to the next control, or leave it on the current control if no other
            // focus targets are available. This is what happens if you use the "disable/enable"
            // hack. What we *can* do is set the focus to the Page which contains Control;
            // this will cause Control to lose focus without shifting focus to, say, the next Entry 

            // Work our way up the tree to find the containing Page
            DependencyObject parent = control as Windows.UI.Xaml.Controls.Control;
            while (parent != null && !(parent is Windows.UI.Xaml.Controls.Page))
                parent = Windows.UI.Xaml.Media.VisualTreeHelper.GetParent(parent);

            if (parent is Windows.UI.Xaml.Controls.Page _containingPage)
            {
                // Cache the tabstop setting
                // var wasTabStop = _containingPage.IsTabStop;

                // Controls can only get focus if they're a tabstop
                _containingPage.IsTabStop = true;
                _containingPage.Focus(FocusState.Programmatic);

                // Restore the tabstop setting; that may cause the Page to lose focus,
                // but it won't restore the focus to Control
                //_containingPage.IsTabStop = wasTabStop;
            }
        }



        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);
            Disconnect(e.OldElement as HardwareKeyPage);
            Connect(e.NewElement as HardwareKeyPage);
        }

        static bool keyDownCaptured = false;

#pragma warning disable CC0057 // Unused parameters
#pragma warning disable IDE0060 // Remove unused parameter
        static void OnInterceptorKeyDown(KeyboardDeliveryInterceptor sender, KeyEventArgs args)
#pragma warning restore IDE0060 // Remove unused parameter
#pragma warning restore CC0057 // Unused parameters
        {
            //var logicalFocus = FocusManager.GetFocusedElement();
            //System.Diagnostics.Debug.WriteLine("=========== Windows.UI.Xaml.Input.FocusManager.FocusedElement=" + logicalFocus);
            //if (logicalFocus == this)
            //    System.Diagnostics.Debug.WriteLine("    THIS     ");

            args.Handled = ProcessVirualKey(HardwareKeyPage.FocusedElement ?? HardwareKeyPage.DefaultFocusedElement, args.VirtualKey);
        }

#pragma warning disable CC0057 // Unused parameters
        public static void OnKeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
#pragma warning restore CC0057 // Unused parameters
        {
            //var logicalFocus = FocusManager.GetFocusedElement();
            //System.Diagnostics.Debug.WriteLine("=========== Windows.UI.Xaml.Input.FocusManager.FocusedElement=" + logicalFocus);
            //if (logicalFocus == this)
            //    System.Diagnostics.Debug.WriteLine("    THIS     ");

            args.Handled = ProcessVirualKey(HardwareKeyPage.FocusedElement ?? HardwareKeyPage.DefaultFocusedElement, args.VirtualKey);
        }

        public static bool ProcessVirualKey(VisualElement element, Windows.System.VirtualKey virtualKey)
        {



            if (element == null)
                return false;
            keyDownCaptured = false;
            string keyInput;
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
                case Windows.System.VirtualKey.Enter:
                    keyInput = Forms9Patch.HardwareKey.EnterReturnKeyInput;
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

            var modifiers = GetModifierKeys(true);
            //var result = new Forms9Patch.HardwareKey(keyInput, GetModifierKeys());

            var listeners = element.GetHardwareKeyListeners();
#pragma warning disable CC0006 // Use foreach
            for (int i = 0; i < listeners.Count; i++)
#pragma warning restore CC0006 // Use foreach
            {
                var listener = listeners[i];
                if (string.IsNullOrEmpty(listener?.HardwareKey?.KeyInput))
                    continue;
                if (listener.HardwareKey.KeyInput == keyInput && (listener.HardwareKey.ModifierKeys == modifiers || listener.HardwareKey.ModifierKeys == HardwareKeyModifierKeys.Any))
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

#pragma warning disable CC0091 // Use static method
#pragma warning disable CC0057 // Unused parameters
        public static void OnCharacterReceived(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.CharacterReceivedEventArgs args)
#pragma warning restore CC0057 // Unused parameters
#pragma warning restore CC0091 // Use static method
        {
            //System.Diagnostics.Debug.WriteLine("HardwareKeyPageRenderer.OnCharacterReceived");
            ProcessCharacter(HardwareKeyPage.FocusedElement ?? HardwareKeyPage.DefaultFocusedElement, (char)args.KeyCode);
        }

        public static bool ProcessCharacter(VisualElement element, char keyCode)
        {
            if (element == null)
                return false;
            if (keyDownCaptured)
            {
                keyDownCaptured = false;
                return false;
            }

            var keyInput = ("" + keyCode).ToUpper();
            //System.Diagnostics.Debug.WriteLine("ProcessCharacter keyInput=[" + keyInput + "]");

            var modifiers = GetModifierKeys(char.IsLetter(keyCode));
            //var result = new Forms9Patch.HardwareKey(keyInput, GetModifierKeys());

            var listeners = element.GetHardwareKeyListeners();
#pragma warning disable CC0006 // Use foreach
            for (int i = 0; i < listeners.Count; i++)
#pragma warning restore CC0006 // Use foreach
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
                    return true;
                }
            }
            return false;
        }

        static string KeyStatusString(Windows.UI.Core.CorePhysicalKeyStatus KeyStatus)
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

        static Forms9Patch.HardwareKeyModifierKeys GetModifierKeys(bool includeShift)
        {
            var shiftState = Window.Current.CoreWindow.GetKeyState(Windows.System.VirtualKey.Shift).HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down);
            var ctrlState = Window.Current.CoreWindow.GetKeyState(Windows.System.VirtualKey.Control).HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down);
            var altState = Window.Current.CoreWindow.GetKeyState(Windows.System.VirtualKey.Menu).HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down);
            var platformState = Window.Current.CoreWindow.GetKeyState(Windows.System.VirtualKey.LeftWindows).HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down);
            platformState |= Window.Current.CoreWindow.GetKeyState(Windows.System.VirtualKey.RightWindows).HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down);
            var capsState = Window.Current.CoreWindow.GetKeyState(Windows.System.VirtualKey.CapitalLock).HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down);
            //var numLock = Window.Current.CoreWindow.GetKeyState(Windows.System.VirtualKey.NumberKeyLock).HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down);

            var result = HardwareKeyModifierKeys.None;
            if (shiftState && includeShift)
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
