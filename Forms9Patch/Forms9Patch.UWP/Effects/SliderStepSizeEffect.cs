using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportEffect(typeof(Forms9Patch.UWP.SliderStepSizeEffect), "SliderStepSizeEffect")]
namespace Forms9Patch.UWP
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    class SliderStepSizeEffect : PlatformEffect
    {
        static int instances;
        Forms9Patch.SliderStepSizeEffect _stepSizeEffect;

        protected override void OnAttached()
        {
            instances++;
            _stepSizeEffect = (Forms9Patch.SliderStepSizeEffect)Element.Effects.FirstOrDefault(e => e is Forms9Patch.SliderStepSizeEffect);

            if (_stepSizeEffect != null && Control is Windows.UI.Xaml.Controls.Slider slider)
            {
                slider.StepFrequency = _stepSizeEffect.StepSize;
                _stepSizeEffect.PropertyChanged += OnEffectPropertyChanged;
            }


        }

        protected override void OnDetached()
        {
            if (_stepSizeEffect != null)
                _stepSizeEffect.PropertyChanged -= OnEffectPropertyChanged;
            _stepSizeEffect = null;
        }

        private void OnEffectPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Forms9Patch.SliderStepSizeEffect.StepSize) /*"StepSize"*/ && _stepSizeEffect != null && Control is Windows.UI.Xaml.Controls.Slider slider)
                slider.StepFrequency = _stepSizeEffect.StepSize;
        }

        /// <summary>
        /// Called when a property is changed
        /// </summary>
        /// <param name="e"></param>
        protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
        {
            /*
            if (e.PropertyName == Slider.ValueProperty.PropertyName && Element is Xamarin.Forms.Slider slider)
            {
                var value = slider.Value;
                var steppedValue = Math.Round(value / _stepSizeEffect.StepSize) * _stepSizeEffect.StepSize;
                if (Math.Abs(value - steppedValue) > double.Epsilon * 2)
                {
                    slider.Value = steppedValue;
                    return;
                }
            }
            */
            base.OnElementPropertyChanged(e);
        }

    }
}
