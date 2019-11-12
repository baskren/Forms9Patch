using System;
using Forms9Patch.Interfaces;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(Forms9Patch.iOS.HapticService))]
namespace Forms9Patch.iOS
{
    public class HapticService : IHapticsService
    {
        public void Feedback(HapticEffect effect, EffectMode mode = EffectMode.Default)
        {
            if (mode == EffectMode.Off
                || effect == HapticEffect.None
                || !UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
                return;
            if (mode == EffectMode.Default && Forms9Patch.Settings.HapticEffectMode == EffectMode.Off)
                return;
            switch (effect)
            {
                case HapticEffect.Selection:
                    {
                        using (var selection = new UISelectionFeedbackGenerator())
                        {
                            selection.Prepare();
                            selection.SelectionChanged();
                        }
                    }
                    break;
                case HapticEffect.LightImpact:
                    {
                        using (var impact = new UIImpactFeedbackGenerator(UIImpactFeedbackStyle.Light))
                        {
                            impact.Prepare();
                            impact.ImpactOccurred();
                        }
                    }
                    break;
                case HapticEffect.MediumImpact:
                    {
                        using (var impact = new UIImpactFeedbackGenerator(UIImpactFeedbackStyle.Medium))
                        {
                            impact.Prepare();
                            impact.ImpactOccurred();
                        }
                    }
                    break;
                case HapticEffect.HeavyImpact:
                    {
                        using (var impact = new UIImpactFeedbackGenerator(UIImpactFeedbackStyle.Heavy))
                        {
                            impact.Prepare();
                            impact.ImpactOccurred();
                        }
                    }
                    break;
                case HapticEffect.ErrorNotification:
                    {
                        // Initialize feedback
                        using (var notification = new UINotificationFeedbackGenerator())
                        {
                            notification.Prepare();
                            notification.NotificationOccurred(UINotificationFeedbackType.Error);
                        }
                    }
                    break;
                case HapticEffect.WarningNotification:
                    {
                        // Initialize feedback
                        using (var notification = new UINotificationFeedbackGenerator())
                        {
                            notification.Prepare();
                            notification.NotificationOccurred(UINotificationFeedbackType.Warning);
                        }
                    }
                    break;
                case HapticEffect.SuccessNotification:
                    {
                        // Initialize feedback
                        using (var notification = new UINotificationFeedbackGenerator())
                        {
                            notification.Prepare();
                            notification.NotificationOccurred(UINotificationFeedbackType.Success);
                        }
                    }
                    break;
            }

        }

    }
}
