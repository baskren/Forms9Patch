using System;
using Xamarin.Forms;
namespace Forms9Patch.Extensions
{
    public static class LayoutOptionsExtensions
    {
        public static TextAlignment ToTextAlignment(this LayoutOptions options)
        {
            switch (options.Alignment)
            {
                case LayoutAlignment.Start:
                    return TextAlignment.Start;
                case LayoutAlignment.Center:
                    return TextAlignment.Center;
                case LayoutAlignment.End:
                    return TextAlignment.End;
            }
            return TextAlignment.Start;
        }
    }
}
