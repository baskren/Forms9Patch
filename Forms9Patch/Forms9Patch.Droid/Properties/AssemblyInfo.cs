using System.Reflection;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

// Information about this assembly is defined by the following attributes. 
// Change them to the values specific to your project.
using Forms9Patch;

[assembly: AssemblyTitle("Forms9Patch.Droid")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("42nd Parallel")]
[assembly: AssemblyProduct("")]
[assembly: AssemblyCopyright("42nd Parallel, 2015")]
[assembly: AssemblyTrademark("42nd Parallel")]
[assembly: AssemblyCulture("")]

// The assembly version has the format "{Major}.{Minor}.{Build}.{Revision}".
// The form "{Major}.{Minor}.*" will automatically update the build and revision,
// and "{Major}.{Minor}.{Build}.*" will update just the revision.

[assembly: AssemblyVersion("1.0.0")]

[assembly: ResolutionGroupName("Forms9Patch")]

// The following attributes are used to specify the signing key for the assembly, 
// if desired. See the Mono documentation for more information about signing.

//[assembly: AssemblyDelaySign(false)]
//[assembly: AssemblyKeyFile("")]

[assembly: ExportRenderer(typeof(Forms9Patch.Image), typeof(Forms9Patch.Droid.ImageRenderer))]
[assembly: ExportRenderer(typeof(Forms9Patch.ContentView), typeof(Forms9Patch.Droid.ContentViewRenderer))]
[assembly: ExportRenderer(typeof(Forms9Patch.Frame), typeof(Forms9Patch.Droid.FrameRenderer))]
[assembly: ExportRenderer(typeof(Forms9Patch.StackLayout), typeof(Forms9Patch.Droid.StackLayoutRenderer))]
[assembly: ExportRenderer(typeof(Forms9Patch.RelativeLayout), typeof(Forms9Patch.Droid.RelativeLayoutRenderer))]
[assembly: ExportRenderer(typeof(Forms9Patch.Grid), typeof(Forms9Patch.Droid.GridRenderer))]
[assembly: ExportRenderer(typeof(Forms9Patch.AbsoluteLayout), typeof(Forms9Patch.Droid.AbsoluteLayoutRenderer))]
[assembly: ExportRenderer(typeof(Forms9Patch.BubbleLayout), typeof(Forms9Patch.Droid.BubbleLayoutRenderer))]
[assembly: ExportRenderer(typeof(Forms9Patch.ManualLayout), typeof(Forms9Patch.Droid.ManualLayoutRenderer))]
