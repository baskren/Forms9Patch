using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Forms9Patch
{
	[ContentProperty ("Source")]
	public class ImageResourceExtension : IMarkupExtension
	{
		public string Source { get; set; }

		public object ProvideValue (IServiceProvider serviceProvider) {
			if (Source == null)
				return null;
			var imageSource = Forms9Patch.ImageSource.FromResource (Source);
			return imageSource;
		}
	}
}

