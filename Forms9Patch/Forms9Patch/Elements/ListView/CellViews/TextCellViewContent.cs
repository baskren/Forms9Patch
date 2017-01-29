using Xamarin.Forms;
namespace Forms9Patch
{
	internal class TextCellViewContent : Label
	{

		public TextCellViewContent()
		{
			TextColor = Color.Black;
			//VerticalOptions = LayoutOptions.FillAndExpand;
			//HorizontalOptions = LayoutOptions.FillAndExpand;
			Margin = new Thickness(5, 1, 5, 1);
			VerticalTextAlignment = TextAlignment.Center;
			HorizontalTextAlignment = TextAlignment.Start;
		}

		#region change management
		#endregion

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			if (BindingContext != null)
				Text = BindingContext.ToString();
			else
				Text = null;
		}
	}
}

