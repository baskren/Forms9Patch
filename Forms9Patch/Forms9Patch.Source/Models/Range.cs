
namespace Forms9Patch
{
    internal class Range
    {
        /// <summary>
        /// First pixel in stretchable range (the previous pixel will not be stretchable)
        /// </summary>
		public double Start = -1;
        /// <summary>
        /// Last pixel in stretchable range (the next pixel will not be stretchable)
        /// </summary>
		public double End = double.MaxValue;

        public double Width => End - Start + 1;

        public Range(double start, double end)
        {
            Start = start;
            End = end;
        }

        public Range() { }
    }
}

