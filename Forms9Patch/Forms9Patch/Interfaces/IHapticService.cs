using System;
namespace Forms9Patch
{
	public interface IHapticService
	{
		void Feedback(HapticEffect effect, HapticMode mode=HapticMode.ApplicationDefault);

	}
}
