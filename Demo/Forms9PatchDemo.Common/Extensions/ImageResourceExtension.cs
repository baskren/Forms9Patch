using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Reflection;

namespace Forms9PatchDemo
{
    [ContentProperty("Source")]
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class ImageMultiResourceExtension : IMarkupExtension
    {
        public string Source { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            var result = Source == null ? null : Forms9Patch.ImageSource.FromMultiResource(Source, GetType().GetTypeInfo().Assembly);
            return result;
        }
    }

    [ContentProperty("Source")]
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class ImageResourceExtension : IMarkupExtension
    {
        public string Source { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            var result = Source == null ? null : Xamarin.Forms.ImageSource.FromResource(Source, GetType().GetTypeInfo().Assembly);
            return result;
        }
    }
}

