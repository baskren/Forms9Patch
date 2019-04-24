
namespace Forms9Patch
{
    internal class Range
    {
        /// <summary>
        /// First pixel in stretchable range (the previous pixel will not be stretchable)
        /// </summary>
		public float Start = -1;
        /// <summary>
        /// Last pixel in stretchable range (the next pixel will not be stretchable)
        /// </summary>
		public float End = float.MaxValue;

        public bool Stretchable;

        public float Width => End - Start + 1;

        public Range(float start, float end, bool strechable = false)
        {
            Start = start;
            End = end;
            Stretchable = strechable;
        }

        public Range() { }
    }
}

