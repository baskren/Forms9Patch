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
renditions of an image, Forms9Patch prescribes how to name each rendition of 
your image files (and thus their EmbeddedResourceId).

*File Name*: `base_image_name[resolution_modifier][device_modifier].fextension`

*EmbeddedResourceId*: `project_assembly_name.project_path.base_image_name[resolution_modifier][device_modifier].fextension`

Notice the addition of the optional `[resolution_modifier]` and 
`[device_modifier]` modifiers.  These modifiers will be familiar to iOS 
developers.  Likewise, Android developers can see analogs in the `Resources` folder naming conventions.
 Valid values for the optional modifiers are:

### Resolution Modifiers

|**Forms9Patch** | **_notes_** | **iOS analog** | **Android analog** |
|---|---|---|---|
|*none* | *fallback image if no other is available* | *none (default)* | *none (default)*|
|`@¾x` | *low density screens (~120 dpi)* | *n/a* | `-ldpi`|
|`@1x` | *explicit medium density (~160 dpi)* | *none (default)* | `-mdpi`|
|`@1½x` | *(~240 dpi)* | *n/a* | `-hdpi`|
|`@2x` | *most common (~320)* | `@2x` | `-xhdpi`|
|`@3x` | *rare (~480 dpi)* | `@3x` | `-xxhdpi`|
|`@4x` | *future proof (~640 dpi)* |  `@4x` | `-xxxhdpi`|

### Device Modifiers

|**Forms9Patch** | **_notes_** | **iOS analog** | **Android analog**|
|---|---|---|---|
|*none* | *fallback image if no other is available* | *none (default)* | *none (default)*|
|`~phone` | *maps to Xamarin Forms'* `TargetIdiom.Phone` | `~iPhone` | *none (default)*|
|`~tablet` | *maps to Xamarin Forms'* `TargetIdiom.Tablet` |  `~iPad` | `sw600dp`|

## Forms9Patch.ImageSource

Now that you've saved your image files for each device type and resolution 
rendition you want to support, next comes using those images in your app.  This
is where `Forms9Patch.ImageSource` comes in.  `Forms9Patch.ImageSource` extends 
`Xamarin.Forms.ImageSource` by adding the `FromMultiResource` static method, 
adding the following functionality to Xamarin.Forms.ImageSource.FromResource: 

 - Finds the best fit image among the EmbeddedResource image renditions
 - Eliminates the need for explicitly specifying the image file's extension 

This is not without compromise.  Because `Xamarin.Forms.ImageSource.FromResource` 
is intended to be multi-platform, it only supports the following image file
formats: NinePatch (`.9.png`), `.png`, `.jpg`, `.jpeg`, `.gif`, `.bmp`, `.bmpf` 
and `.svg`.  Note that, unlike the other image file formats, NinePatch and `.svg` 
images can only be used with Forms9Patch image elements.

Note that a Forms9Patch license is not required to use `Forms9Patch.ImageSource`.

## Code Example

The following is a very simple app to demonstrate how `Forms9Patch.ImageSource` 
uses Forms9Patch's Embedded Resource ResourceID naming convention to provide the 
best Embeded Resource image to Xamarin.Forms.Image or Forms9Patch.Image.

First, we start with a set of multi-device / multi-resolution images:

|**50x50** | **100x100** | **150x150**|
|---|---|---|
|image.png ![image.png](../images/Guides/ImageSource/image.png) | image@2x.png ![image@2x.png](../images/Guides/ImageSource/image@2x.png) | image@3x.png ![image@3x.png](../images/Guides/ImageSource/image@3x.png) |
|image~tablet.png ![image~tablet.png](../images/Guides/ImageSource/image~tablet.png) | image@2x~tablet.png ![image@2x~tablet.png](../images/Guides/ImageSource/image@2x~tablet.png) | image@3x~tablet.png ![image@3x~tablet.png](../images/Guides/ImageSource/image@3x~tablet.png) |

Next, we create our app:

 - Create a new Xamarin Forms cross-platform (.Netstandard or PCL) application named `MyDemoApp` with the `MyDemoApp` assembly namespace
 - Create a Resources directory in the app's cross-platform project (the .NetStandard or PCL project)
 - Save the above six images in the cross-platform project's `Resources` directory
 - Set the `Build Action` to `EmbeddedResource` for those images
 - Modify your app code `MyDemoApp.cs` as shown, below:

![MyDemoApp.cs screen shot](../images/Guides/ImageSource/resource-schema-tree.png)

`Forms9Patch.ImageSource.FromMultiResource` will choose among the available 
embedded resource renditions in the cross-platform assembly and select the 
rendition that works best with the current device (tablet or phone; low / medium / high resolution).

## XAML Example

In Xamarin.Forms, access to embedded resources from XAML requires some 
addtional work.  Unfortunately, Forms9Patch is no different.  As with Xamarin.Forms, 
you will need (in the same assembly as your embedded resource images) a simple 
custom XAML markup extension to load images using their ResourceID.

``` csharp
[ContentPropert ("Source")]
public class ImageMultiResourceExtension : IMarkupExtension
{
    public string Source { get; set; }

    public object ProvideValue (IServiceProvider serviceProvider)
    {
        if (Source == null)
            return null;

        // Do your translation lookup here, using whatever method you require
        var imageSource  = Forms9Patch.ImageSource.FromMultiResource(Source);

        return imageSource;
    }
}
```

Once you have the above, you can load your embedded resource images as shown in 
the below example.  Be sure to add a namespace to your XAML for the assembly that contains 
both the above MarkupExtension and your EmbeddedResources (named `local` in the below example).

``` XAML
<?xml version="1.0" encoding="UTF-8"?>
<ContentPage
    xmlns="http://xamarin.com/schemas/2014/forms"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:local="clr-namespace:MyXamlDemo;assembly=MyXamlDemo"
    x:Class="MyXamlDemo.MyPage"
    Padding="5, 20, 5, 5">
    <ScrollView>
        <ScrollView.Content>
            <StackLayout>
                <Label Text="Xamarin.Image"/>
                <Image Source="{local:ImageMultiResource Forms9PatchDemo.Resources.image}"/>
            </StackLayout>
        </ScrollView.Content>
    </ScrollView>
</ContentPage>
```

