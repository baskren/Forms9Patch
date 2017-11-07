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
    class SliderStepSizeEffect : PlatformEffect
    {
        static int instances;
        Forms9Patch.SliderStepSizeEffect stepSizeEffect;

        protected override void OnAttached()
        {
            instances++;
            var stepSizeEffect = (Forms9Patch.SliderStepSizeEffect)Element.Effects.FirstOrDefault(e => e is Forms9Patch.SliderStepSizeEffect);

            if (stepSizeEffect != null && Control is Windows.UI.Xaml.Controls.Slider slider)
            {
                slider.StepFrequency = stepSizeEffect.StepSize;
                stepSizeEffect.PropertyChanged += OnEffectPropertyChanged;
            }


        }

        protected override void OnDetached()
        {
            if (stepSizeEffect != null)
                stepSizeEffect.PropertyChanged -= OnEffectPropertyChanged;
            stepSizeEffect = null;
        }

        private void OnEffectPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "StepSize" && stepSizeEffect!=null && Control is Windows.UI.Xaml.Controls.Slider slider)
                slider.StepFrequency = stepSizeEffect.StepSize;
        }

        /// <param name="args">To be added.</param>
        /// <summary>
        /// Called when a property is changed
        /// </summary>
        /// <remarks>To be added.</remarks>
        protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);
        }

    }
}
