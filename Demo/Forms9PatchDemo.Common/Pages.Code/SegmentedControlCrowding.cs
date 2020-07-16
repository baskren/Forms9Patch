using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace Forms9PatchDemo
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class SegmentedCrowdingPage : ContentPage
    {
        Forms9Patch.SegmentedControl control = new Forms9Patch.SegmentedControl
        {
            Segments =
            {
                new Forms9Patch.Segment("alpha"),
                new Forms9Patch.Segment("beta"),
                new Forms9Patch.Segment("gama"),
                new Forms9Patch.Segment("delta"),
                new Forms9Patch.Segment("epsilon"),
                new Forms9Patch.Segment("zeta"),
                new Forms9Patch.Segment("eta"),
                new Forms9Patch.Segment("theta"),
                new Forms9Patch.Segment("iota"),
                new Forms9Patch.Segment("kappa"),
                new Forms9Patch.Segment("lamda"),
                new Forms9Patch.Segment("mu"),
                new Forms9Patch.Segment("nu"),
                new Forms9Patch.Segment("xi"),
                new Forms9Patch.Segment("omicron"),
                new Forms9Patch.Segment("pi"),
                new Forms9Patch.Segment("rho"),
                new Forms9Patch.Segment("sigma"),
                new Forms9Patch.Segment("tua"),
                new Forms9Patch.Segment("upsilon"),
                new Forms9Patch.Segment("phi"),
                new Forms9Patch.Segment("chi"),
                new Forms9Patch.Segment("psi"),
                new Forms9Patch.Segment("omega"),
            },
            VerticalOptions = LayoutOptions.CenterAndExpand
        };

        public SegmentedCrowdingPage()
        {
            Content = control;

            control.PropertyChanged += OnControlPropertyChanged;
        }

        private void OnControlPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Forms9Patch.SegmentedControl.IsClippedProperty.PropertyName)
                System.Diagnostics.Debug.WriteLine("IsClipped=" + control.IsClipped);
        }
    }
}