using Xamarin.Forms;

namespace Forms9Patch
{
    /// <summary>
    /// FormsDragNDropListView Group header view.
    /// </summary>
    public class GroupHeaderView : Xamarin.Forms.ViewCell
    {

        /// <summary>
        /// Triggered by a changing in the binding context.
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            if (!P42.Utils.Environment.IsOnMainThread)
            {
                Device.BeginInvokeOnMainThread(OnBindingContextChanged);
                return;
            }

            base.OnBindingContextChanged();

            var group = BindingContext as GroupWrapper;
            View.BindingContext = group?.Source;
        }

    }
}

