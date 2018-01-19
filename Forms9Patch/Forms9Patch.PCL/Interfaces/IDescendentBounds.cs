using System;
using Xamarin.Forms;

namespace Forms9Patch
{
	interface IDescendentBounds
	{
		Rectangle PageDescendentBounds(Page page, VisualElement descendent);
	}
}

