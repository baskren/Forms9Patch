using Xamarin.Forms;
namespace Forms9Patch
{
    internal class TextCellViewContent : Forms9Patch.Label // Xamarin.Forms.Label //, ICellHeight
    {

        public TextCellViewContent()
        {
            TextColor = Color.Black;
            Margin = new Thickness(5, 1, 5, 1);
            VerticalTextAlignment = TextAlignment.Center;
            HorizontalTextAlignment = TextAlignment.Start;
            LineBreakMode = LineBreakMode.TailTruncation;
            Lines = 1;
            AutoFit = AutoFit.Width;
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

