using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace Forms9Patch
{
    /// <summary>
    /// Enabled StepSize control of Xamarin.Forms.Slider element
    /// </summary>
    [Preserve(AllMembers = true)]
    [DesignTimeVisible(true)]
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
                if (System.Math.Abs(value - _stepSize) > (_stepSize * 0.01))
                {
                    _stepSize = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StepSize)));
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

#pragma warning disable CC0057 // Unused parameters
#pragma warning disable IDE0060 // Remove unused parameter
        /// <summary>
        /// Applies SliderStepSizeEffect to a Xamarin.Forms.Slider
        /// </summary>
        /// <returns><c>true</c>, if to was applyed, <c>false</c> otherwise.</returns>
        /// <param name="slider">Slider.</param>
        /// <param name="assembly">Assembly.</param>
        public static bool ApplyTo(Slider slider, Assembly assembly = null)
#pragma warning restore IDE0060 // Remove unused parameter
#pragma warning restore CC0057 // Unused parameters
        {
            if (slider.Effects.Any(e => e is SliderStepSizeEffect))
                return true;
            if (new SliderStepSizeEffect() is SliderStepSizeEffect effect)
            {
                slider.Effects.Add(effect);
                return slider.Effects.Contains(effect);
            }
            return false;
        }


    }
}
