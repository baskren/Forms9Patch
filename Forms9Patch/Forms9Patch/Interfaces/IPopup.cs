using System;

namespace Forms9Patch
{
	interface IPopup
	{
		Action ForceNativeLayout { get; set; }

		void HostSizeChanged();
	}
}

