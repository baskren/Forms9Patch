using System;
using Forms9Patch.Interfaces;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(Forms9Patch.iOS.HapticService))]
namespace Forms9Patch.iOS
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class HapticService : IHapticsService
    {
        static readonly AudioToolbox.SystemSound vibrate = new AudioToolbox.SystemSound(4095);

        public void Feedback(HapticEffect effect, FeedbackMode mode = FeedbackMode.Default)
        {
            if (mode == FeedbackMode.Off
                || effect == HapticEffect.None
                || (mode == FeedbackMode.Default && Forms9Patch.Feedback.HapticMode == FeedbackMode.Off)
                || !UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
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
                case HapticEffect.Long:
                    vibrate.PlaySystemSound();
                    break;

            }

        }

    }
}
