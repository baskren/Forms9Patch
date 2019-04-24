using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;

[assembly: ExportEffect(typeof(Forms9Patch.iOS.ListViewSelectionDisabledEffect), "ListViewSelectionDisabledEffect")]
namespace Forms9Patch.iOS
{
	public class ListViewSelectionDisabledEffect : PlatformEffect
	{
		protected override void OnAttached()
		{
			((UITableView)Control).AllowsSelection = false;
			//((UITableView)Control.
		}

		protected override void OnDetached()
		{
		}
	}
}

