using System;
namespace Forms9Binding
{
	public class ImageButton : Forms9Patch.ImageButton
	{
		public ImageButton()
		{
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			DefaultState.BindingContext = BindingContext;
		}


	}
}
