using System.ComponentModel;
using System.Reflection;
using Xamarin.Forms;

namespace Forms9Patch
{
    /// <summary>
    /// Enabled StepSize control of Xamarin.Forms.Slider element
    /// </summary>
    public class SliderStepSizeEffect : RoutingEffect, INotifyPropertyChanged
    {
        static SliderStepSizeEffect()
        {
            Settings.ConfirmInitialization();
        }

        /// <summary>
        /// PropertyChanged event handler
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        double _stepSize;

        /// <summary>
        /// StepSize property
        /// </summary>
        public double StepSize
        {
            get => _stepSize; 
            set
            {
                if (value != _stepSize)
                {
                    _stepSize = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("StepSize"));
                }
            }
        }

        /// <summary>
        /// Constructor for SliderStepSizeEffect
        /// </summary>
        protected SliderStepSizeEffect() : base("Forms9Patch.SliderStepSizeEffect")
        {
        }

        /// <summary>
        /// Convenience constructor for SliderStepSizeEffect
        /// </summary>
        /// <param name="stepSize"></param>
        public SliderStepSizeEffect(double stepSize) : base("Forms9Patch.SliderStepSizeEffect")
        {
            _stepSize = stepSize;
        }

        /// <summary>
        /// Applies SliderStepSizeEffect to a Xamarin.Forms.Slider
        /// </summary>
        /// <returns><c>true</c>, if to was applyed, <c>false</c> otherwise.</returns>
        /// <param name="slider">Slider.</param>
        /// <param name="assembly">Assembly.</param>
        public static bool ApplyTo(Slider slider, Assembly assembly = null)
        {
            var effect = new SliderStepSizeEffect();
            if (effect != null)
            {
                slider.Effects.Add(effect);
                return slider.Effects.Contains(effect);
            }
            return false;
        }


    }
}
