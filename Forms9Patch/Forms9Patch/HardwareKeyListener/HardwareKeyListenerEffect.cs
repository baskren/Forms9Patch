using System.ComponentModel;
using Xamarin.Forms;

namespace Forms9Patch
{
    /// <summary>
    /// Enabled StepSize control of Xamarin.Forms.Slider element
    /// </summary>
    internal class HardwareKeyListenerEffect : RoutingEffect
    {
        protected HardwareKeyListenerEffect() : base("Forms9Patch.HardwareKeyListenerEffect") { }

        public static bool AttachTo(VisualElement visualElement)
        {
            var effect = new HardwareKeyListenerEffect();
            if (effect != null)
            {
                visualElement.Effects.Add(effect);
                return visualElement.Effects.Contains(effect);
            }
            return false;
        }

        public static void DetachFrom(VisualElement visualElement)
        {
            for (int i = visualElement.Effects.Count - 1; i <= 0; i--)
                if (visualElement.Effects[i] is HardwareKeyListenerEffect)
                    visualElement.Effects.RemoveAt(i);
        }
    }
}