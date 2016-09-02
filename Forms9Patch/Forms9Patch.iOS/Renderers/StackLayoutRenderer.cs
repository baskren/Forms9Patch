using Xamarin.Forms;

[assembly: ExportRenderer(typeof(Forms9Patch.StackLayout), typeof(Forms9Patch.iOS.StackLayoutRenderer))]
namespace Forms9Patch.iOS
{
	/// <summary>
	/// Forms9Patch Stack layout renderer.
	/// </summary>
	public class StackLayoutRenderer : LayoutRenderer<StackLayout> {}
}