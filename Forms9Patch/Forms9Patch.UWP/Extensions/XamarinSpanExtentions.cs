using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Documents;
using Xamarin.Forms;
using Forms9Patch;

namespace Forms9Patch.UWP
{
    static class XamarinSpanExtentions
    {
        public static Run ToRun(this Xamarin.Forms.Span span)
        {
            var run = new Run { Text = span.Text };

            if (span.ForegroundColor != Color.Default)
                run.Foreground = span.ForegroundColor.ToBrush();

            if (!span.HasDefaultFont())
            {
                #pragma warning disable 618
                run.ApplyFont(span.Font);
                #pragma warning restore 618
            }

            return run;
        }
    }
}
