
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
		public double Start=-1;
		public double End=double.MaxValue;
	}
}

