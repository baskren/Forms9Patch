using System;
namespace Forms9Patch
{
	public class SelectedItemChangedEventArgs : Xamarin.Forms.SelectedItemChangedEventArgs
	{
		public object Group
		{
			get;
			private set;
		}


		public SelectedItemChangedEventArgs(object group, object selectedItem) : base (selectedItem)
		{
			Group = group;
		}
	}
}

