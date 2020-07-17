using System;
using Xamarin.Forms;

namespace Forms9Patch
{
	[Xamarin.Forms.Internals.Preserve(AllMembers = true)]
	interface IDescendentBounds
	{
		Rectangle PageDescendentBounds(Page page, VisualElement descendent);
	}
}

