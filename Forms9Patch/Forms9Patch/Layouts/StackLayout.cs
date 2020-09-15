using Xamarin.Forms;
using System.ComponentModel;

namespace Forms9Patch
{
    /// <summary>
    /// Forms9Patch StackLayout.
    /// </summary>
    [Preserve(AllMembers = true)]
    [DesignTimeVisible(true)]
    public class StackLayout : Layout<Xamarin.Forms.StackLayout>, IElementConfiguration<Xamarin.Forms.StackLayout>
    {
        /// <summary>
        /// Backing store for the Orientation property
        /// </summary>
        public static readonly BindableProperty OrientationProperty = BindableProperty.Create(nameof(Orientation), typeof(StackOrientation), typeof(StackLayout), StackOrientation.Vertical,
                                                                                              propertyChanged: (bindable, oldvalue, newvalue) => ((StackLayout)bindable)._layout.SetValue(Xamarin.Forms.StackLayout.OrientationProperty, newvalue));

        /// <summary>
        /// Backing store for the Spacing property
        /// </summary>
        public static readonly BindableProperty SpacingProperty = BindableProperty.Create(nameof(Spacing), typeof(double), typeof(StackLayout), 6d,
                                                                                          propertyChanged: (bindable, oldvalue, newvalue) => ((StackLayout)bindable)._layout.SetValue(Xamarin.Forms.StackLayout.SpacingProperty, newvalue));

        /// <summary>
        /// Gets or sets the orientation of the stack.
        /// </summary>
        /// <value>The orientation.</value>
        public StackOrientation Orientation
        {
            get => _layout.Orientation;
            set => _layout.Orientation = value;
        }

        /// <summary>
        /// Gets or sets the spacing between children.
        /// </summary>
        /// <value>The spacing.</value>
        public double Spacing
        {
            get => _layout.Spacing;
            set => _layout.Spacing = value;
        }


        Xamarin.Forms.StackLayout _layout => _xfLayout as Xamarin.Forms.StackLayout;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.StackLayout"/> class.
        /// </summary>
        public StackLayout()
        {
        }

        /// <summary>
        /// Marker interface for returning platform-specific configuration elements.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IPlatformElementConfiguration<T, Xamarin.Forms.StackLayout> On<T>() where T : IConfigPlatform
        {
            return ((IElementConfiguration<Xamarin.Forms.StackLayout>)_xfLayout).On<T>();
        }
    }
}

