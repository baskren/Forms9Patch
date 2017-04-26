using System;
namespace Forms9Binding
{
	public class MaterialSegmentedControl : Forms9Patch.MaterialSegmentedControl
	{
		public MaterialSegmentedControl()
		{
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			foreach (var segment in Segments)
				segment.BindingContext = BindingContext;
		}
	}
}