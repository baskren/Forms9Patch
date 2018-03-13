using System;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace Forms9Patch
{
    internal class HardwareKeyListenerBehavior : Behavior<VisualElement>
    {

        ObservableCollection<HardwareKeyListener> _hardwareKeyListeners = new ObservableCollection<HardwareKeyListener>();
        public ObservableCollection<HardwareKeyListener> HardwareKeyListeners
        {
            get => _hardwareKeyListeners;
        }

        protected override void OnAttachedTo(VisualElement bindable)
        {
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(VisualElement bindable)
        {
            if (HardwareKeyFocus.Element == bindable)
                HardwareKeyFocus.Element = null;
            HardwareKeyListeners.Clear();
            base.OnDetachingFrom(bindable);
        }

        public static HardwareKeyListenerBehavior GetFor(VisualElement visualElement)
        {
            foreach (var behavior in visualElement.Behaviors)
                if (behavior is HardwareKeyListenerBehavior hklBehavior)
                    return hklBehavior;

            var result = new HardwareKeyListenerBehavior();
            visualElement.Behaviors.Add(result);
            return result;
        }
    }
}