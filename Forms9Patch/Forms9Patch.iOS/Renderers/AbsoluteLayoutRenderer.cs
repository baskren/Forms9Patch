using Xamarin.Forms;

[assembly: ExportRenderer(typeof(Forms9Patch.AbsoluteLayout), typeof(Forms9Patch.iOS.AbsoluteLayoutRenderer))]
namespace Forms9Patch.iOS
{
	/// <summary>
	/// Forms9Patch Absolute layout renderer.
	/// </summary>
	public class AbsoluteLayoutRenderer : LayoutRenderer<AbsoluteLayout> { }
}

