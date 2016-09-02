using Xamarin.Forms;

[assembly: ExportRenderer(typeof(Forms9Patch.RelativeLayout), typeof(Forms9Patch.iOS.RelativeLayoutRenderer))]
namespace Forms9Patch.iOS
{
	/// <summary>
	/// Forms9Patch Relative layout renderer.
	/// </summary>
	public class RelativeLayoutRenderer : LayoutRenderer<RelativeLayout> {}
}