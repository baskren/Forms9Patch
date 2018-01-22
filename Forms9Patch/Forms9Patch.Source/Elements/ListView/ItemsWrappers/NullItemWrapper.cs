using Xamarin.Forms;

namespace Forms9Patch
{
	class NullItemWrapper : ItemWrapper
	{
		#region Properties
		public static readonly BindableProperty RequestedHeightProperty = BindableProperty.Create("RequestedHeight", typeof(double), typeof(NullItemWrapper), 0.0);
		public double RequestedHeight {
			get { return (double)GetValue (RequestedHeightProperty); }
			set { SetValue (RequestedHeightProperty, value); }
		}
		#endregion

		public NullItemWrapper()
		{
			RemoveBinding(CellBackgroundColorProperty);
		}
	}
}

