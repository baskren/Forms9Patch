using System;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using P42.Utils;

[assembly: ExportEffect(typeof(Forms9Patch.UWP.HardwareKeyListenerEffect), "HardwareKeyListenerEffect")]
namespace Forms9Patch.UWP
{
    public class HardwareKeyListenerEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            //if (Device.RuntimePlatform == Device.WinPhone || Device.RuntimePlatform == Device.WinRT)
            //    return;
            if (Element is HardwareKeyPage)
                return;
            //var type = Control?.GetType(); 

            var success = false;
            if (Control != null)
            {
                try
                {
                    Control.PreviewKeyDown += OnKeyDown;
                    Control.CharacterReceived += OnCharacterReceived;
                    success = true;
                }
                catch (Exception) { }
                if (!success) try
                    {

                        Control.KeyDown += OnKeyDown;
                        Control.CharacterReceived += OnCharacterReceived;
                        success = true;
                    }
                    catch (Exception) { }
            }
            if (!success && Container != null)
            {
                try
                {
                    Container.PreviewKeyDown += OnKeyDown;
                    Container.CharacterReceived += OnCharacterReceived;
                    success = true;
                }
                catch (Exception) { }
                //else if (Container?.GetType().GetMethodInfo("KeyDown") is System.Reflection.MethodInfo)
                if (!success) try
                    {
                        Container.KeyDown += OnKeyDown;
                        Container.CharacterReceived += OnCharacterReceived;
                    }
                    catch (Exception) { }
            }
        }

        protected override void OnDetached()
        {
            if (Control?.GetType() is Type controlType)
            {
                if (controlType.GetMethodInfo("PreviewKeyDown") is System.Reflection.MethodInfo)
                    Control.PreviewKeyDown -= OnKeyDown;
                else if (controlType.GetMethodInfo("KeyDown") is System.Reflection.MethodInfo)
                    Control.KeyDown -= OnKeyDown;
                Control.CharacterReceived -= OnCharacterReceived;
            }
            else if (Container?.GetType() is Type containerType)
            {
                if (containerType.GetMethodInfo("PreviewKeyDown") is System.Reflection.MethodInfo)
                    Container.PreviewKeyDown -= OnKeyDown;
                else if (containerType.GetMethodInfo("KeyDown") is System.Reflection.MethodInfo)
                    Container.KeyDown -= OnKeyDown;
                Container.CharacterReceived -= OnCharacterReceived;

            }
        }

        private void OnCharacterReceived(UIElement sender, CharacterReceivedRoutedEventArgs args)
        {
            //System.Diagnostics.Debug.WriteLine("HardwareKeyListenerEffect.OnCharacterReceived("+Element+")");
            if (Element is VisualElement element)
                args.Handled = HardwareKeyPageRenderer.ProcessCharacter(element, args.Character);

        }

        private void OnKeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (Element is VisualElement element)
                e.Handled = HardwareKeyPageRenderer.ProcessVirualKey(element, e.Key);

            //System.Diagnostics.Debug.WriteLine("HardwareKeyListenerEffect.OnControlKeyPress [" + e.Event + "][" + e.Handled + "][" + e.KeyCode + "]");
        }


    }
}