using Xamarin.Forms;
using static Xamarin.Forms.RelativeLayout;
using System.ComponentModel;

namespace Forms9Patch
{
    /// <summary>
    /// Forms9Patch RelativeLayout.
    /// </summary>
    [Preserve(AllMembers = true)]
    [DesignTimeVisible(true)]
    public class RelativeLayout : Layout<Xamarin.Forms.RelativeLayout>, ILayout, IElementConfiguration<Xamarin.Forms.RelativeLayout>
    {

        Xamarin.Forms.RelativeLayout _layout => _xfLayout as Xamarin.Forms.RelativeLayout;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.RelativeLayout"/> class.
        /// </summary>
        public RelativeLayout()
        {
            VerticalOptions = HorizontalOptions = LayoutOptions.FillAndExpand;
        }

        /// <summary>
        /// List of Views that are children of this RelativeLayout.
        /// </summary>
        /// <value>The children.</value>
        public new IRelativeList<View> Children => _layout.Children;

        /// <summary>
        /// Gets the bounds constraint of element in RelativeLayout
        /// </summary>
        /// <returns>The bounds constraint.</returns>
        /// <param name="bindable">Bindable.</param>
        public static BoundsConstraint GetBoundsConstraint(BindableObject bindable) => Xamarin.Forms.RelativeLayout.GetBoundsConstraint(bindable);

        /// <summary>
        /// Gets the height constraint of element in RelativeLayout
        /// </summary>
        /// <returns>The height constraint.</returns>
        /// <param name="bindable">Bindable.</param>
        public static Constraint GetHeightConstraint(BindableObject bindable) => Xamarin.Forms.RelativeLayout.GetHeightConstraint(bindable);

        /// <summary>
        /// Gets the width constraint of element in RelativeLayout
        /// </summary>
        /// <returns>The width constraint.</returns>
        /// <param name="bindable">Bindable.</param>
        public static Constraint GetWidthConstraint(BindableObject bindable) => Xamarin.Forms.RelativeLayout.GetWidthConstraint(bindable);

        /// <summary>
        /// Gets the X constraint of element in RelativeLayout
        /// </summary>
        /// <returns>The XC onstraint.</returns>
        /// <param name="bindable">Bindable.</param>
        public static Constraint GetXConstraint(BindableObject bindable) => Xamarin.Forms.RelativeLayout.GetXConstraint(bindable);

        /// <summary>
        /// Gets the Y constraint of element in RelativeLayout
        /// </summary>
        /// <returns>The YC onstraint.</returns>
        /// <param name="bindable">Bindable.</param>
        public static Constraint GetYConstraint(BindableObject bindable) => Xamarin.Forms.RelativeLayout.GetYConstraint(bindable);

        /// <summary>
        /// Sets the bounds constraint of element in RelativeLayout
        /// </summary>
        /// <param name="bindable">Bindable.</param>
        /// <param name="value">Value.</param>
        public static void SetBoundsConstraint(BindableObject bindable, BoundsConstraint value) => Xamarin.Forms.RelativeLayout.SetBoundsConstraint(bindable, value);

        /// <summary>
        /// Sets  the height constraint of element in RelativeLayout
        /// </summary>
        /// <param name="bindable">Bindable.</param>
        /// <param name="value">Value.</param>
        public static void SetHeightConstraint(BindableObject bindable, Constraint value) => Xamarin.Forms.RelativeLayout.SetHeightConstraint(bindable, value);

        /// <summary>
        /// Sets the width constraint of element in RelativeLayout
        /// </summary>
        /// <param name="bindable">Bindable.</param>
        /// <param name="value">Value.</param>
        public static void SetWidthConstraint(BindableObject bindable, Constraint value) => Xamarin.Forms.RelativeLayout.SetWidthConstraint(bindable, value);

        /// <summary>
        /// Sets the X constraint of element in RelativeLayout
        /// </summary>
        /// <param name="bindable">Bindable.</param>
        /// <param name="value">Value.</param>
        public static void SetXConstraint(BindableObject bindable, Constraint value) => Xamarin.Forms.RelativeLayout.SetXConstraint(bindable, value);

        /// <summary>
        /// Sets the Y constraint of element in RelativeLayout
        /// </summary>
        /// <param name="bindable">Bindable.</param>
        /// <param name="value">Value.</param>
        public static void SetYConstraint(BindableObject bindable, Constraint value) => Xamarin.Forms.RelativeLayout.SetYConstraint(bindable, value);

        /// <summary>
        /// Marker interface for returning platform-specific configuration elements.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IPlatformElementConfiguration<T, Xamarin.Forms.RelativeLayout> On<T>() where T : IConfigPlatform
        {
            return ((IElementConfiguration<Xamarin.Forms.RelativeLayout>)_xfLayout).On<T>();

        }
    }
}

