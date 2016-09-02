using Xamarin.Forms;

[assembly: ExportRenderer(typeof(Forms9Patch.Frame), typeof(Forms9Patch.iOS.FrameRenderer))]
namespace Forms9Patch.iOS
{
	/// <summary>
	/// Forms9Patch Frame renderer.
	/// </summary>
	public class FrameRenderer : LayoutRenderer<Frame> {}
}