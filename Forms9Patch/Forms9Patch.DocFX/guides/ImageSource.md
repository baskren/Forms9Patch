# Using `Forms9Patch.ImageSource`

Xamarin Forms relies upon native APIs and schema for iOS, Android, and Windows UWP multi-screen image 
management (described here)[http://developer.xamarin.com/guides/cross-platform/xamarin-forms/working-with/images/].  
This requires storing your iOS images in your iOS 
platform project using the native iOS schema, storing your Android images in 
your Android platform project using the Android native schema, and storing your UWP 
images in your UWP platform project using the UWP native schema.  In other 
words, duplicative efforts to get the same results on both Android, iOS, and UWP.
Forms9Patch.ImageSource simplifies Xamarin.Forms.ImageSource capabilities by bringing
multi-screen image management to your Shared Library, PCL Assemblies, and 
.NetStandard projects - so you only have to generate and configure your app's image 
resources once. 

## Embedded Resource Id Naming Convention

To use Forms9Patch multi-platform image management, you will need to store your 
image resource files as Embedded Resources.  You can do this by:

*VisualStudio Mac*: Right clicking on image file and selecting "Build Action / EmbeddedResource"

*VisualStudio 2017*: Right clicking on the image file and selecting "Properties" and then, in the "Properties" panel, modify the "&&&&&&&&&" field to "EmbeddedResource"

Each embedded resources has an EmbeddedResourceId string that is used to 
reference it at runtime.  An EmbeddedResourceId is a sequence of strings joined by 
a period (.) as a separator.  This series of strings starts with the assembly name 
for the project in which the embedded resource is in, appended by the names of 
each folder in that project's folder structure for the embedded 
resource file, and ends with the embedded resource's file name.   

    project_assembly_name.project_path.base_image_name.extension

For example, if your project's assembly name is `PizzaMaker` and has an image file named
`slice.png` in its `Resources/Images` folder, the EmbeddedResourceId
of that file would be `PizzaMaker.Resources.Images.slice.png`.  If you're using 
VisualStudio Mac, don't worry too much about this because the EmbeddedResourceId 
is in the "Properties" panel in the "ResourceID" field.   You can find the "Properties" panel
 by right clicking on a file and selecting "Properties".


To facilitate runtime selection (for device resolution and type) between multiple
renditions of an image, Forms9Patch asks for a modification to how you 
name your files (and thus their EmbeddedResourceId).  

*File Name*: `base_image_name[resolution_modifier][device_modifier].fextension`

*EmbeddedResourceId*: `project_assembly_name.project_path.base_image_name[resolution_modifier][device_modifier].fextension`

Notice the addition of the optional `[resolution_modifier]` and 
`[device_modifier]` modifiers.  These modifiers will be familiar to iOS 
developers.  Likewise, Android developers can see analogs in the `Resources` folder naming conventions.
 Valid values for the optional modifiers are:

### Resolution Modifiers

**Forms9Patch** | *notes* | iOS analog | Android analog
---|---|---|---
*none* | *fallback image if no other is available* | *none (default)* | *none (default)*
`@¾x` | *low density screens (~120 dpi)* | *n/a* | `-ldpi`
`@1x` | explicit medium density (~160 dpi)  none (default)  -mdpi
`@1½x` |   (~240 dpi)  n/a -hdpi
`@2x` | most common (~320)  @2x -xhdpi
`@3x` | rare (~480 dpi) @3x -xxhdpi
`@4x` future proof (~640 dpi) @4x -xxxhdpi