using Xamarin.Forms;

namespace Forms9Patch
{
	/// <summary>
	/// FormsDragNDropListView Group header view.
	/// </summary>
	public class GroupHeaderView : Xamarin.Forms.ViewCell {

		/// <summary>
		/// Triggered by a changing in the binding context.
		/// </summary>
		protected override void OnBindingContextChanged ()
		{
			var group = BindingContext as GroupWrapper;
			View.BindingContext = group?.Source;
			base.OnBindingContextChanged ();
		}

	}
}

