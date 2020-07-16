// /*******************************************************************
//  *
//  * ExternalEmbeddedResourceImage.cs copyright 2016 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using Xamarin.Forms;

namespace Forms9PatchDemo
{
	[Xamarin.Forms.Internals.Preserve(AllMembers = true)]
	public class ExternalEmbeddedResourceImage : ContentPage
	{
		public ExternalEmbeddedResourceImage()
		{
			Content = new StackLayout
			{
				
				Children = {
					new Forms9Patch.Image
                    {
					    Source = Forms9Patch.ImageSource.FromMultiResource("FormsGestures.Resources.rocket"),
                        Fill = Forms9Patch.Fill.AspectFit
					}
				}
			};
		}

	}
}


