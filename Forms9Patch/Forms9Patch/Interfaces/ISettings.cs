
using System.Collections.Generic;
using System.Reflection;

namespace Forms9Patch
{
    /// <summary>
    /// Interface for platform settings
    /// </summary>
    [Preserve(AllMembers = true)]
    public interface ISettings
    {
        /// <summary>
        /// Needed by UWP implemenation to assure Xamarin.Forms works AND EmbeddedResource loading works
        /// </summary>
        List<Assembly> IncludedAssemblies { get; }


        /// <summary>
        /// Lazy initializes the Forms9Patch native code for use by Xamarin Previewer
        /// </summary>
        void LazyInit();

    }
}

