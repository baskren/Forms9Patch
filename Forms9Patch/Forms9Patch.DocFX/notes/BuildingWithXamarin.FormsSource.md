---
uid: BuildingWithXamarin.FormsSource.md
title: Build with Xamarin.Forms source
---
# Building with Xamarin.Forms source

## Project Configuration

#### Notes for those starting from scratch (as a August 16, 2018) 

1.  Switch to the Xamarin.Forms_Official/3.1.0 branch
2.  Xamarin.Forms projects used:
    -  Xamarin.Flex  
    -  Xamarin.Forms.Core
    -  Xamarin.Forms.Platform
    -  Xamarin.Forms.Platform.Android
    -  Xamarin.Forms.Platform.Android.FormsViewGroup
    -  Xamarin.Forms.Platform.iOS
    -  Xamarin.Forms.Platform.UAP
    -  Xamarin.Forms.Xaml
    -  Stubs/Xamarin.Forms.Platform.Android
    -  Stubs/Xamarin.Forms.Platform.iOS
3.  Xamarin.Forms.Android:
    1.  needs to be built with Android 8.1 (Project Properties / Application / Compile using Android version)
    2.  Minimum Android Version needs to be 4.1 (API16) to support HTML copy / paste 
4.  Set up ```packages``` symbolic links (see below)
5.  Use .netstandard 2.0 for all of your cross platform projects
6.  Reference the following packages in your .netstandard cross platform projects:
    -  Xamarin.Flex  
    -  Xamarin.Forms.Core
    -  Xamarin.Forms.Platform
    -  Xamarin.Forms.Xaml
7.  Reference the following packages in your Android projects:
    -  Xamarin.Flex  
    -  Xamarin.Forms.Core
    -  Xamarin.Forms.Platform.Android
    -  Xamarin.Forms.Platform.Android.FormsViewGroup
    -  Xamarin.Forms.Xaml
    -  Stubs/Xamarin.Forms.Platform.Android
8.  Reference the following packages in your iOS projects:  
    -  Xamarin.Flex  
    -  Xamarin.Forms.Core
    -  Xamarin.Forms.Platform.iOS
    -  Xamarin.Forms.Platform.UAP
    -  Xamarin.Forms.Xaml
    -  Stubs/Xamarin.Forms.Platform.iOS
9.  Reference the following packages in your UWP projects:
    -  Xamarin.Flex  
    -  Xamarin.Forms.Core
    -  Xamarin.Forms.Platform.UAP
    -  Xamarin.Forms.Xaml


#### Every submodule needs a ```packages``` folder

Because of the way VisualStudio, Nuget, and Git (don't) work together, relative paths are not valid for Nuget packages referenced in project files (that are from VisualStudio solutions) that have been added as git submodules.  To work around for this, go to the following directories (below) and symbolically link the ```packages``` subdirectory to the solution's working Nuget packages directory:

 - Forms9Patch/Forms9PatchDemo
 - Forms9Patch/FormsGestures
 - Forms9Patch/P42
 - Forms9Patch/Xamarin.Forms

Below are example commands of how to symbolically link to your VisualStudio solution's (and git root module) Nuget packages folder.  For the case of building Forms9Patch source and demo code using just this git module, the below command lines are meant to be executed in the above directories.

 - **Windows:** ``` c:> mklink /D packages ..\packages ```
 - **OSX:** ``` $ ln -s ../packages packages ```

If you are using Forms9Patch source as a submodule in another git module, modify the above command lines to cause the symbolic links to point to the ```packages``` directory in the root directory of the VisualStudio solution associated with that git module.   


#### Address error:  ``` Project '..\Xamarin.Forms ... .csproj' targets 'netstandard2.0' ```
Be sure all of your shared projects use .netStandard 2.0


#### Changes to Forms9PatchDemo, Forms9Patch, or FormsGestures source code isn't updating

This may likely not be a problem in the future but currently VisualStudio doesn't always show changes to ```.target``` files.  The extreme version of this can be seen in how ```.target``` file references are not seen at all in ```.PCL```, ```.iOS```, ```.Droid``` and ```.UWP``` projects.  Why am I using ```.target``` files then?  Because it saves a lot of work in keeping the source code for various permutations of builds in sync.



#### Forms9PatchDemo.PCL.Droid won't run when built and deployed from VisualStudio 2017 (Windows) to Android Emulator

Heck, it might not work when deployed to an actual device.  It *does* work when deployed from VisualStudio Mac to Android Emulator.  1 point for VisualStudio Mac!  If you know why it doesn't work on Windows, please share!


#### Why are the Xaml examples not in the Forms9PatchDemo.Source apps?

Because I haven't been able to take the time to figure out why Xaml files are not being compiled and embedded properly when using Xamarin.Forms source.  Again, if you know how to do this, please share!


#### UWP: Unable to load `System.Runtime. The located assembly's manifest definition does not match the assembly reference.`

Change the Windows target version to 14393.   

#### UWP: Getting exceptions at Xamarin.Forms.Init

When using `Compile with .Net Native tool chain` build setting, `System.IO.FileNotFoundException` execeptions for the following files (maybe more) can occur at Xamarin.Forms.Init when the Common Language Runtime Exceptions are all enabled:

 - clrcompression
 - libgl
 - libglex
 - SkiaSharp.Views

You need to use `Compile with .Net Native tool chain` for your app to be accepted by Windows Store.  What appears to work is to return the Common Language Runtime Exceptions to its defaults.  As of this writing (2/19/2018), the defaults are:
 
 - System.Reflection.MissingMetaDataException
 - System.Reflection.MissingRuntimeArtifactException 
 - System.Windows.Markup.XamlParseException


 #### UWP: Crashes when when trying to open an image using `Xamarin.Forms.ImageSource.FromResource` but *not* when using `Forms9Patch.ImageSource.FromMultiResource`

 Boy, did I loose a lot of time on this one.  Turns out the solution to [UWP: Getting exceptions at Xamarin.Forms.Init](UWP: Getting exceptions at Xamarin.Forms.Init) was the fix.  I have no idea why.
