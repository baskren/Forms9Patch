using Xamarin.Forms;
using System.ComponentModel;

namespace Forms9Patch
{
    [DesignTimeVisible(true)]
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
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
            Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() =>
            {
                base.OnBindingContextChanged();
                HtmlText = BindingContext?.ToString();
            });
        }
    }
}

