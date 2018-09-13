using System;
using Xamarin.Forms;
using static Xamarin.Forms.RelativeLayout;

namespace Forms9Patch
{
    /// <summary>
    /// Forms9Patch RelativeLayout.
    /// </summary>
    public class RelativeLayout : Layout<Xamarin.Forms.RelativeLayout>
    {
        public static readonly BindableProperty XConstraintProperty = Xamarin.Forms.RelativeLayout.XConstraintProperty;

        public static readonly BindableProperty YConstraintProperty = Xamarin.Forms.RelativeLayout.YConstraintProperty;

        public static readonly BindableProperty WidthConstraintProperty = Xamarin.Forms.RelativeLayout.WidthConstraintProperty;

        public static readonly BindableProperty HeightConstraintProperty = Xamarin.Forms.RelativeLayout.HeightConstraintProperty;

        public static readonly BindableProperty BoundsConstraintProperty = Xamarin.Forms.RelativeLayout.BoundsConstraintProperty;

        Xamarin.Forms.RelativeLayout _layout;

        public RelativeLayout()
        {
            VerticalOptions = HorizontalOptions = LayoutOptions.FillAndExpand;
            _layout = _xfLayout as Xamarin.Forms.RelativeLayout;
        }

        public new IRelativeList<View> Children => _layout.Children;

        public static BoundsConstraint GetBoundsConstraint(BindableObject bindable) => Xamarin.Forms.RelativeLayout.GetBoundsConstraint(bindable);

        public static Constraint GetHeightConstraint(BindableObject bindable) => Xamarin.Forms.RelativeLayout.GetHeightConstraint(bindable);

        public static Constraint GetWidthConstraint(BindableObject bindable) => Xamarin.Forms.RelativeLayout.GetWidthConstraint(bindable);

        public static Constraint GetXConstraint(BindableObject bindable) => Xamarin.Forms.RelativeLayout.GetXConstraint(bindable);

        public static Constraint GetYConstraint(BindableObject bindable) => Xamarin.Forms.RelativeLayout.GetYConstraint(bindable);

        public static void SetBoundsConstraint(BindableObject bindable, BoundsConstraint value) => Xamarin.Forms.RelativeLayout.SetBoundsConstraint(bindable, value);

        public static void SetHeightConstraint(BindableObject bindable, Constraint value) => Xamarin.Forms.RelativeLayout.SetHeightConstraint(bindable, value);

        public static void SetWidthConstraint(BindableObject bindable, Constraint value) => Xamarin.Forms.RelativeLayout.SetWidthConstraint(bindable, value);

        public static void SetXConstraint(BindableObject bindable, Constraint value) => Xamarin.Forms.RelativeLayout.SetXConstraint(bindable, value);

        public static void SetYConstraint(BindableObject bindable, Constraint value) => Xamarin.Forms.RelativeLayout.SetYConstraint(bindable, value);


    }
}

