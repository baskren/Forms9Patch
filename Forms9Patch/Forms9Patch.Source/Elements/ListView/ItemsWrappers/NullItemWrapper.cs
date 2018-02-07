using Xamarin.Forms;

namespace Forms9Patch
{
	class NullItemWrapper : ItemWrapper
	{
		public NullItemWrapper()
		{
			RemoveBinding(CellBackgroundColorProperty);
		}
	}
    
}

