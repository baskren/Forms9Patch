using Xamarin.Forms;
namespace Forms9Patch
{
	internal class TextCellViewContent : Label
	{

		public TextCellViewContent()
		{
			//HeightRequest = 30;
			//BackgroundColor = Color.Pink;
			TextColor = Color.Default;
			VerticalOptions = LayoutOptions.Center;
			HorizontalOptions = LayoutOptions.Start;
			Margin = new Thickness(5, 1, 5, 1);
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

