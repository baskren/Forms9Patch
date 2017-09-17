using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace Forms9Patch.UWP
{
    static class F9PImageElementExtensions
    {
        public static Stretch ToStretch(this Forms9Patch.Fill fill)
        {
            switch (fill)
            {
                case Fill.Fill:
                    return Stretch.Fill;

                case Fill.AspectFill:
                    return Stretch.UniformToFill;

                case Fill.AspectFit:
                    return Stretch.Uniform;

                default:
                    return Stretch.None;
            }
        }
    }
}
