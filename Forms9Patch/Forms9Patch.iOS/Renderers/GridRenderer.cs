using Xamarin.Forms;

[assembly: ExportRenderer(typeof(Forms9Patch.Grid), typeof(Forms9Patch.iOS.GridRenderer))]
namespace Forms9Patch.iOS
{
	/// <summary>
	/// Forms9Patch Grid renderer.
	/// </summary>
	public class GridRenderer : LayoutRenderer<Grid> {}
}