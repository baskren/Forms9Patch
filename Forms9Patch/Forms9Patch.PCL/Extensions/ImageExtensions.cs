using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Forms9Patch
{
    static class ImageExtensions
    {
        public static bool HasDefaultAspectAndLayoutOptions(this Xamarin.Forms.Image image)
        {
            var hasDefaultAspect = image.Aspect == (Aspect)Xamarin.Forms.Image.AspectProperty.DefaultValue;
            var hasDefaultHorizontalLayout = image.HorizontalOptions.IsEqualTo((LayoutOptions)Xamarin.Forms.Image.HorizontalOptionsProperty.DefaultValue);
            var hasDefaultVerticalLayout = image.VerticalOptions.IsEqualTo((LayoutOptions)Xamarin.Forms.Image.VerticalOptionsProperty.DefaultValue);

            return hasDefaultAspect && hasDefaultHorizontalLayout && hasDefaultVerticalLayout;
        }
    }
}
