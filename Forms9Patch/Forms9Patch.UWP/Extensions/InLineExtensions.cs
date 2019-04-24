using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Documents;

namespace Forms9Patch.UWP
{
    static class InLineExtensions
    {
        public static bool ExceedsFontSize(this InlineCollection inlineCollection, double fontSize)
        {
            foreach(var inline in inlineCollection)
            {
                if (inline is Windows.UI.Xaml.Documents.Span span)
                {
                    if (span.Inlines.ExceedsFontSize(fontSize))
                        return true;
                }
                if (inline.FontSize > fontSize)
                    return true;
            }
            return false;
        }

    }
}
