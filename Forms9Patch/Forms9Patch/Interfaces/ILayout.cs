
namespace Forms9Patch
{
    /// <summary>
    /// Interface for Forms9Patch layout elements
    /// </summary>
    public interface ILayout : IBackground
    {
        /// <summary>
        /// Padding
        /// </summary>
        Xamarin.Forms.Thickness Padding { get; set; }
        /// <summary>
        /// Don't relayout layout if child's layout/size changes
        /// </summary>
        bool IgnoreChildren { get; set; }
    }
}
