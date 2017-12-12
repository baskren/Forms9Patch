using System;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using ObjCRuntime;

[assembly: ExportEffect(typeof(Forms9Patch.iOS.SliderStepSizeEffect), "SliderStepSizeEffect")]
namespace Forms9Patch.iOS
{
    public class SliderStepSizeEffect : PlatformEffect
    {
        static int instances;
        Forms9Patch.SliderStepSizeEffect _stepSizeEffect;

        public SliderStepSizeEffect()
        {
        }

        protected override void OnAttached()
        {
            instances++;
            _stepSizeEffect = (Forms9Patch.SliderStepSizeEffect)Element.Effects.FirstOrDefault(e => e is Forms9Patch.SliderStepSizeEffect);

            if (_stepSizeEffect != null && Element is Slider slider)
                slider.ValueChanged += EditingEvent;
        }

        protected override void OnDetached()
        {
            if (Element is Slider slider)
                slider.ValueChanged -= EditingEvent;
            _stepSizeEffect = null;
        }

        void EditingEvent(object sender, EventArgs e)
        {
            if (Element is Xamarin.Forms.Slider slider)
                slider.Value = (float)(Math.Round(slider.Value / _stepSizeEffect.StepSize) * _stepSizeEffect.StepSize);
        }

    }
}
