using Xamarin.Forms;

[assembly: ExportRenderer(typeof(Forms9Patch.ContentView), typeof(Forms9Patch.iOS.ContentViewRenderer))]
namespace Forms9Patch.iOS
{
	/// <summary>
	/// Forms9Patch Content view renderer.
	/// </summary>
	public class ContentViewRenderer : LayoutRenderer<ContentView> {}
}