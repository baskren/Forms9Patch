using System;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

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
            if (Control != null)
            {
                Control.PreviewKeyDown += OnKeyDown;
                Control.CharacterReceived += OnCharacterReceived;
            }
            else if (Container != null)
            {
                Control.PreviewKeyDown += OnKeyDown;
                Container.CharacterReceived += OnCharacterReceived;
            }
        }

        protected override void OnDetached()
        {
            if (Control != null)
            {
                Control.PreviewKeyDown -= OnKeyDown;
                Control.CharacterReceived -= OnCharacterReceived;
            }
            else if (Container != null)
            {
                Control.PreviewKeyDown -= OnKeyDown;
                Container.CharacterReceived -= OnCharacterReceived;
            }
        }

        private void OnCharacterReceived(UIElement sender, CharacterReceivedRoutedEventArgs args)
        {
            System.Diagnostics.Debug.WriteLine("HardwareKeyListenerEffect.OnCharacterReceived("+Element+")");
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