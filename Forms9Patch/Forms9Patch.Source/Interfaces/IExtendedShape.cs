using System;
using Xamarin.Forms;
namespace Forms9Patch
{
    public interface IExtendedShape : IShape
    {
        /// <summary>
        /// For internal use only.  Goes beyond ElementShape to allow setting of segment shapes
        /// </summary>
        ExtendedElementShape ExtendedElementShape { get; set; }

        /// <summary>
        /// Gets or sets the orientation of the elments outline shape.
        /// </summary>
        /// <value>The shape orientation.</value>
        Xamarin.Forms.StackOrientation ExtendedElementShapeOrientation { get; set; }

        float ExtendedElementSeparatorWidth { get; set; }

        //StackOrientation ParentSegmentsOrientation { get; set; }

    }
}
