using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Forms9PatchDemo
{
    [ContentProperty("EmbeddedResourceId")]
    class FontResourceExtension : IMarkupExtension
    {
        public string EmbeddedResourceId { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (EmbeddedResourceId == null)
                return null;
            var fontSource = Forms9Patch.FontSource.FromEmbeddedResource(EmbeddedResourceId);
            return fontSource;
        }
    }
}
