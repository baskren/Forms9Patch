
namespace Forms9Patch
{
    /// <summary>
    /// The foundation of Forms9Patch visual elements
    /// </summary>
    public interface IElement
    {
        /// <summary>
        /// Incremental instance id (starting at zero, increasing by one for each new instance)
        /// </summary>
        int InstanceId { get; }
    }
}
