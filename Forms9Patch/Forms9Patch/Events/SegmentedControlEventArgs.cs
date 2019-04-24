
namespace Forms9Patch
{
	/// <summary>
	/// Segment tapped event arguments.
	/// </summary>
	public class SegmentedControlEventArgs
	{
		/// <summary>
		/// The index of the tapped segment.
		/// </summary>
		public int Index;

		/// <summary>
		/// The segment that was tapped
		/// </summary>
		public Segment Segment;

		/// <summary>
		/// Initializes a new instance of the <see cref="Forms9Patch.SegmentedControlEventArgs"/> class.
		/// </summary>
		/// <param name="index">Index.</param>
		/// <param name="segment">Segment.</param>
		public SegmentedControlEventArgs (int index, Segment segment)
		{
			Index = index;
			Segment = segment;
		}
	}
}

