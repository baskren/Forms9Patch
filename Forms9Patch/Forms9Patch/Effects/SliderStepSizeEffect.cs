using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forms9Patch
{
    public class SliderStepSizeEffect : Xamarin.Forms.RoutingEffect, INotifyPropertyChanged
    {
        double _stepSize;
        public double StepSize
        {
            get { return _stepSize; }
            set
            {
                if (value != _stepSize)
                {
                    _stepSize = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("StepSize"));
                }
            }
        }

        protected SliderStepSizeEffect() : base("Forms9Patch.SliderStepSizeEffect")
        {
        }

        public SliderStepSizeEffect(double stepSize) : base("Forms9Patch.SliderStepSizeEffect")
        {
            _stepSize = stepSize;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
