using Xamarin.Forms;
using System;

namespace Forms9Patch
{
    /// <summary>
    /// Forms9Patch StackLayout.
    /// </summary>
    public class StackLayout : Layout<Xamarin.Forms.StackLayout>
    {
        public static readonly BindableProperty OrientationProperty = BindableProperty.Create("Orientation", typeof(StackOrientation), typeof(StackLayout), StackOrientation.Vertical,
                                                                                              propertyChanged: (bindable, oldvalue, newvalue) => ((StackLayout)bindable)._layout.SetValue(Xamarin.Forms.StackLayout.OrientationProperty, newvalue));

        public static readonly BindableProperty SpacingProperty = BindableProperty.Create("Spacing", typeof(double), typeof(StackLayout), 6d,
                                                                                          propertyChanged: (bindable, oldvalue, newvalue) => ((StackLayout)bindable)._layout.SetValue(Xamarin.Forms.StackLayout.SpacingProperty, newvalue));

        public StackOrientation Orientation
        {
            get => _layout.Orientation;
            set => _layout.Orientation = value;
        }

        public double Spacing
        {
            get => _layout.Spacing;
            set => _layout.Spacing = value;
        }


        readonly Xamarin.Forms.StackLayout _layout;

        public StackLayout()
        {
            _layout = _xfLayout as Xamarin.Forms.StackLayout;
        }



    }
}

