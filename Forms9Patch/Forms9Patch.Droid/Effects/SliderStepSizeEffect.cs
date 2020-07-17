using System;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Widget;

[assembly: ExportEffect(typeof(Forms9Patch.Droid.SliderStepSizeEffect), "SliderStepSizeEffect")]
namespace Forms9Patch.Droid
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class SliderStepSizeEffect : PlatformEffect
    {
        static int instances;
        Forms9Patch.SliderStepSizeEffect _stepSizeEffect;

        SeekBarListener _listener;


        public SliderStepSizeEffect()
        {
        }

        protected override void OnAttached()
        {
            instances++;
            _stepSizeEffect = (Forms9Patch.SliderStepSizeEffect)Element.Effects.FirstOrDefault(e => e is Forms9Patch.SliderStepSizeEffect);

            //if (_stepSizeEffect != null && Element is Slider slider)
            //    slider.ValueChanged += EditingEvent;
            if (Control is SeekBar seekbar && Element is Slider slider)
            {
                _listener = new SeekBarListener(slider, _stepSizeEffect.StepSize);
                seekbar.SetOnSeekBarChangeListener(_listener);
            }

            _stepSizeEffect.PropertyChanged += OnEffectPropertyChanged;
        }

        private void OnEffectPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Forms9Patch.SliderStepSizeEffect.StepSize) /*"StepSize"*/ && _listener != null && _stepSizeEffect != null)
                _listener.StepSize = _stepSizeEffect.StepSize;
        }

        protected override void OnDetached()
        {
            //if (Element is Slider slider)
            //    slider.ValueChanged -= EditingEvent;
            if (Control is SeekBar seekbar)
            {
                bool success = false;
                if (Element is Slider slider)
                {
                    var renderer = Platform.GetRenderer(slider);
                    if (renderer is Xamarin.Forms.Platform.Android.SliderRenderer sliderRenderer)
                    {
                        seekbar.SetOnSeekBarChangeListener(sliderRenderer);
                        success = true;
                    }
                }
                if (!success)
                    seekbar.SetOnSeekBarChangeListener(null);
            }
            _listener = null;
            if (_stepSizeEffect != null)
                _stepSizeEffect.PropertyChanged -= OnEffectPropertyChanged;
            _stepSizeEffect = null;
        }

        void EditingEvent(object sender, EventArgs e)
        {
            //if (Element is Xamarin.Forms.Slider slider)
            //    slider.Value = (float)(Math.Round(slider.Value / _stepSizeEffect.StepSize) * _stepSizeEffect.StepSize);
        }

    }

    class SeekBarListener : Java.Lang.Object, Android.Widget.SeekBar.IOnSeekBarChangeListener
    {
        public double StepSize;
        bool _progressChangedOnce;
        readonly Slider Slider;

        double GetValue(SeekBar seekBar)
        {
            return Slider.Minimum + (Slider.Maximum - Slider.Minimum) * (seekBar.Progress / 1000.0);
        }

        void SetValue(SeekBar seekBar, double value)
        {
            seekBar.Progress = (int)((value - Slider.Minimum) / (Slider.Maximum - Slider.Minimum) * 1000.0);
        }

        public SeekBarListener(Slider slider, double stepSize)
        {
            Slider = slider;
            StepSize = stepSize;
        }

        public void OnProgressChanged(SeekBar seekBar, int progress, bool fromUser)
        {
            if (!_progressChangedOnce)
            {
                _progressChangedOnce = true;
                return;
            }

            var value = Math.Round(GetValue(seekBar) / StepSize) * StepSize;

            ((IElementController)Slider).SetValueFromRenderer(Slider.ValueProperty, value);
        }

        public void OnStartTrackingTouch(SeekBar seekBar)
        {
        }

        public void OnStopTrackingTouch(SeekBar seekBar)
        {
        }
    }
}
