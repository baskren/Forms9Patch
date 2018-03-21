using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(Forms9Patch.HardwareKeyPage), typeof(Forms9Patch.UWP.HardwareKeyPageRenderer))]
//[assembly: ExportRenderer(typeof(Forms9Patch.ContentPage), typeof(Forms9Patch.UWP.PageRenderer))]
namespace Forms9Patch.UWP
{
    class HardwareKeyPageRenderer  : Xamarin.Forms.Platform.UWP.PageRenderer
    {
        void Connect()
        {
            Window.Current.CoreWindow.CharacterReceived += CoreWindow_CharacterReceived;
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            //Window.Current.CoreWindow.KeyUp += CoreWindow_KeyUp;
        }

        void Disconnect()
        {
            Window.Current.CoreWindow.CharacterReceived -= CoreWindow_CharacterReceived;
            Window.Current.CoreWindow.KeyDown -= CoreWindow_KeyDown;
            //Window.Current.CoreWindow.KeyUp -= CoreWindow_KeyUp;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null)
                Disconnect();
            if (e.NewElement != null)
                Connect();
        }

        public HardwareKeyPageRenderer() : base ()
        {
            //KeyUp += OnKeyUp;
            //KeyDown += OnKeyDown;

            //Loaded += PageRenderer_Loaded;


            

            
            /*
            Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(4), () =>
             {
                 var element = Windows.UI.Xaml.Input.FocusManager.GetFocusedElement();
                 if (element is Windows.UI.Xaml.FrameworkElement frameworkElement)
                 {
                     //System.Diagnostics.Debug.WriteLine("\tName=[" + frameworkElement.Name + "]");
                     //System.Diagnostics.Debug.WriteLine("\tParent=[" + frameworkElement.Parent + "]");
                 }
                 //System.Diagnostics.Debug.WriteLine("Focus: ["+element+"]");
                 if (element is Windows.UI.Xaml.Controls.ScrollViewer scrollViewer && scrollViewer.Parent==null)
                 {
                     
                     System.Diagnostics.Debug.WriteLine("\tHorizontalScrollBarVisibility=["+ scrollViewer.HorizontalScrollBarVisibility + "]");
                     System.Diagnostics.Debug.WriteLine("\tVerticalScrollBarVisibility=[" + scrollViewer.VerticalScrollBarVisibility + "]");
                     System.Diagnostics.Debug.WriteLine("\tIsDeferredScrollingEnabled=[" + scrollViewer.IsDeferredScrollingEnabled + "]");
                     System.Diagnostics.Debug.WriteLine("\tHorizontalScrollMode=[" + scrollViewer.HorizontalScrollMode + "]");
                     System.Diagnostics.Debug.WriteLine("\tIsTabStop=[" + scrollViewer.IsTabStop + "]");
                     System.Diagnostics.Debug.WriteLine("\tIsVerticalRailEnabled=[" + scrollViewer.IsVerticalRailEnabled + "]");
                     System.Diagnostics.Debug.WriteLine("\tMargin=[" + scrollViewer.Margin + "]");
                     System.Diagnostics.Debug.WriteLine("\tVerticalScrollMode=[" + scrollViewer.VerticalScrollMode + "]");
                     System.Diagnostics.Debug.WriteLine("\tZoomMode=[" + scrollViewer.ZoomMode + "]");
                     //if (scrollViewer.HorizontalScrollBarVisibility== Windows.UI.Xaml.Controls.ScrollBarVisibility.Hidden  && scrollViewer.VerticalScrollBarVisibility == Windows.UI.Xaml.Controls.ScrollBarVisibility.Hidden && !scrollViewer.IsDeferredScrollingEnabled)
                     scrollViewer.IsTabStop = false;
                     Windows.UI.Xaml.Input.FocusManager.TryMoveFocus(Windows.UI.Xaml.Input.FocusNavigationDirection.Next);
                     
                     
                     scrollViewer.IsTabStop = false;
                     Windows.UI.Xaml.Input.FocusManager.TryMoveFocus(Windows.UI.Xaml.Input.FocusNavigationDirection.Next);
                     System.Diagnostics.Debug.WriteLine("NEUTRALIZED");
                     return false;
                     

                     System.Diagnostics.Debug.WriteLine("=================Hierarchy ============== \n"+ scrollViewer.GenerateHeirachry());
                     System.Diagnostics.Debug.WriteLine("=========================================");

                     return false;
                 }
                 return true;
             });
             */
        }

        /*
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int GetKeyNameText(int lParam, StringBuilder lpString, int cchSize);

        string KeyInput(uint keycode)
        {
            var buffer = new StringBuilder(64);
            //int lParam = ((int)keycode) << 16;
            int lParam = (int)keycode;
            GetKeyNameText(lParam, buffer, buffer.Capacity);
            return buffer.ToString();
        }
        */

        bool keyDownCaptured = false;
        private void CoreWindow_KeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            keyDownCaptured = false;
            var keyInput = "";
            switch(args.VirtualKey)
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



                default:
                    return;
            }

            var modifiers = GetModifierKeys();
            //var result = new Forms9Patch.HardwareKey(keyInput, GetModifierKeys());

            var listeners = Element.GetHardwareKeyListeners();
            for (int i = 0; i < listeners.Count; i++)
            {
                var listener = listeners[i];
                if (listener.HardwareKey.KeyInput == keyInput && listener.HardwareKey.ModifierKeys == modifiers)
                {
                    if (listener.Command != null && listener.Command.CanExecute(listener.CommandParameter))
                        listener.Command.Execute(listener.CommandParameter);
                    listener.Pressed?.Invoke(Element, new HardwareKeyEventArgs(listener.HardwareKey, Element));
                    //System.Diagnostics.Debug.WriteLine("SUCCESS!!!");
                    args.Handled = true;
                    return;
                }
            }


            //System.Diagnostics.Debug.WriteLine("CoreWindow.KeyDown ["+args.VirtualKey+"] dev:["+args.DeviceId+"] handled["+args.Handled+"]" + KeyStatusString(args.KeyStatus));


        }

        /*
        private void CoreWindow_KeyUp(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            System.Diagnostics.Debug.WriteLine("CoreWindow.KeyUp [" + args.VirtualKey + "] dev:[" + args.DeviceId + "] handled[" + args.Handled + "]" + KeyStatusString(args.KeyStatus));

        }
        */

        private void CoreWindow_CharacterReceived(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.CharacterReceivedEventArgs args)
        {
            if (keyDownCaptured)
            {
                keyDownCaptured = false;
                return;
            }

            var keyInput = ("" + (char)args.KeyCode).ToUpper();

            var modifiers = GetModifierKeys();
            //var result = new Forms9Patch.HardwareKey(keyInput, GetModifierKeys());

            var listeners = Element.GetHardwareKeyListeners();
            for (int i = 0; i < listeners.Count; i++)
            {
                var listener = listeners[i];
                if (listener.HardwareKey.KeyInput == keyInput && listener.HardwareKey.ModifierKeys == modifiers)
                {
                    if (listener.Command != null && listener.Command.CanExecute(listener.CommandParameter))
                        listener.Command.Execute(listener.CommandParameter);
                    listener.Pressed?.Invoke(Element, new HardwareKeyEventArgs(listener.HardwareKey, Element));
                    //System.Diagnostics.Debug.WriteLine("SUCCESS!!!");
                    args.Handled = true;
                    return;
                }
            }


            //System.Diagnostics.Debug.WriteLine("CharRecv[" + (char)args.KeyCode + "]" + KeyStatusString(args.KeyStatus));
            //System.Diagnostics.Debug.WriteLine("");
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



            return "shft=["+shiftState+"] ctrl=["+ctrlState+"] altState=["+altState+"] pltf=["+platformState+"] capl=["+capsState+"] numl=["+numLock+"] ext:[" + KeyStatus.IsExtendedKey + "] rel:[" + KeyStatus.IsKeyReleased + "] men:[" + KeyStatus.IsMenuKeyDown + "]  rep:[" + KeyStatus.RepeatCount + "] cod:[" + KeyStatus.ScanCode + "] wasDown:[" + KeyStatus.WasKeyDown + "]";

        }

        Forms9Patch.HardwareKeyModifierKeys GetModifierKeys()
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

        /*
        protected override void OnProcessKeyboardAccelerators(ProcessKeyboardAcceleratorEventArgs args)
        {
            System.Diagnostics.Debug.WriteLine("PageRenderer.OnProcessKeyboardAccelerators VirtualKey["+args.Key+"] modifiers["+args.Modifiers+"] handled["+args.Handled+"]");
            base.OnProcessKeyboardAccelerators(args);
        }
        */
        /*
        private void PageRenderer_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var root = this.GetFurthestAncestor<Windows.UI.Xaml.FrameworkElement>();
            if (root is Windows.UI.Xaml.Controls.ScrollViewer scrollViewer)
                scrollViewer.IsTabStop = false;
        }

        private void OnKeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("OnKeyDown[" + e.Key + "]["+e.OriginalKey+"]  Element=[" + Element + "] Parent=[" + Element.Parent + "]  \te.Handled=[" + e.Handled + "] \te.KeyStatus=[" + e.KeyStatus + "] ");

            // This is a french keyboard?  The quick brown fox jumped over the lazy dog? 
            
        }

        private void OnKeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("OnKeyUp[" + e.Key + "][" + e.OriginalKey + "]   Element=[" + Element + "] Parent=[" + Element.Parent + "]  \te.Handled=[" + e.Handled + "] \te.KeyStatus=[" + e.KeyStatus + "]");
        }
        */

    }
}
