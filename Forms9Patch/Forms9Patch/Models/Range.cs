
#if __IOS__
namespace Forms9Patch.iOS
#elif __DROID__
namespace Forms9Patch.Droid
#elif WINDOWS_UWP
namespace Forms9Patch.UWP
#else
namespace Forms9Patch
#endif
{
    internal class Range
	{
        /// <summary>
        /// First pixel in stretchable range (the previous pixel will not be stretchable)
        /// </summary>
		public double Start=-1;
        /// <summary>
        /// Last pixel in stretchable range (the next pixel will not be stretchable)
        /// </summary>
		public double End=double.MaxValue;

        public double Width => End - Start + 1;
	}
}

