using Xamarin.Forms;

namespace Forms9Patch
{
	class NullItem : Item
	{
		#region Properties
		public static readonly BindableProperty RequestedHeightProperty = BindableProperty.Create("RequestedHeight", typeof(double), typeof(NullItem), 0.0);
		public double RequestedHeight {
			get { return (double)GetValue (RequestedHeightProperty); }
			set { SetValue (RequestedHeightProperty, value); }
		}
		#endregion

		public NullItem()
		{
			RemoveBinding(CellBackgroundColorProperty);
		}
	}
}

