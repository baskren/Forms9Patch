using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Forms9PatchDemo
{
	[ContentProperty ("Source")]
	public class ImageMultiResourceExtension : IMarkupExtension
	{
		public string Source { get; set; }

		public object ProvideValue (IServiceProvider serviceProvider)
		{
			return Source == null ? null : Forms9Patch.ImageSource.FromMultiResource (Source);
		}
	}

	[ContentProperty ("Source")]
	public class ImageResourceExtension : IMarkupExtension
	{
		public string Source { get; set; }

		public object ProvideValue (IServiceProvider serviceProvider)
		{
			return Source == null ? null : Xamarin.Forms.ImageSource.FromResource (Source);
		}
	}
}

