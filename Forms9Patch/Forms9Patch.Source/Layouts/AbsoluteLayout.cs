using System;
using Xamarin.Forms;
using static Xamarin.Forms.AbsoluteLayout;

namespace Forms9Patch
{
    public class AbsoluteLayout : Layout<Xamarin.Forms.AbsoluteLayout>
    {
        public static readonly BindableProperty LayoutFlagsProperty = Xamarin.Forms.AbsoluteLayout.LayoutFlagsProperty;

        public static readonly BindableProperty LayoutBoundsProperty = Xamarin.Forms.AbsoluteLayout.LayoutBoundsProperty;

        public new IAbsoluteList<View> Children => ((Xamarin.Forms.AbsoluteLayout)_xfLayout).Children;


        [TypeConverter(typeof(BoundsTypeConverter))]
        public static Rectangle GetLayoutBounds(BindableObject bindable) => Xamarin.Forms.AbsoluteLayout.GetLayoutBounds(bindable);

        public static AbsoluteLayoutFlags GetLayoutFlags(BindableObject bindable) => Xamarin.Forms.AbsoluteLayout.GetLayoutFlags(bindable);

        public static void SetLayoutBounds(BindableObject bindable, Rectangle bounds) => Xamarin.Forms.AbsoluteLayout.SetLayoutBounds(bindable, bounds);

        public static void SetLayoutFlags(BindableObject bindable, AbsoluteLayoutFlags flags) => Xamarin.Forms.AbsoluteLayout.SetLayoutFlags(bindable, flags);

    }
}

