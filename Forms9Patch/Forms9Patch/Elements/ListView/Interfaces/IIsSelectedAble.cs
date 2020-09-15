using System;
using System.ComponentModel;

namespace Forms9Patch
{
    /// <summary>
    /// Assures a class has the IsSelected property.
    /// </summary>
    [Preserve(AllMembers = true)]
    [DesignTimeVisible(true)]
    public interface IIsSelectedAble
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Forms9Patch.IIsSelectedAble"/> is selected.
        /// </summary>
        /// <value><c>true</c> if is selected; otherwise, <c>false</c>.</value>
        bool IsSelected { get; set; }
    }
}

