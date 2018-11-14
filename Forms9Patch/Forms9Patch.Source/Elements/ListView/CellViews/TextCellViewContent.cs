using Xamarin.Forms;
namespace Forms9Patch
{
    internal class TextCellViewContent : Xamarin.Forms.Label //, ICellHeight
    {

        public TextCellViewContent()
        {
            TextColor = Color.Black;
            Margin = new Thickness(5, 1, 5, 1);
            VerticalTextAlignment = TextAlignment.Center;
            HorizontalTextAlignment = TextAlignment.Start;
        }

        //public int CellHeight => (int) System.Math.Ceiling(Bounds.Height);

        #region change management
        #endregion

        protected override void OnBindingContextChanged()
        {
            if (!P42.Utils.Environment.IsOnMainThread)
            {
                Device.BeginInvokeOnMainThread(OnBindingContextChanged);
                return;
            }

            base.OnBindingContextChanged();
            Text = BindingContext?.ToString();
        }
    }
}

