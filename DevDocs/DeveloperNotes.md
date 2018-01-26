# Developer Notes

## Project Configuration

#### Notes for those starting from scratch 

1. Switch to a Xamarin.Forms commit tagged 2.4.0-sr2
2. Xamarin.Forms projects used:
   1. Xamarin.Forms.Platform
   2. Xamarin.Forms.Core
   3. Xamarin.Forms.Xaml
   4. Xamarin.Forms.iOS
   5. Xamarin.Forms.Android.FormsViewGroup
   6. Xamarin.Forms.Android
   7. Xamarin.Forms.UAP
3. Xamarin.Forms.Android.FormsViewGroup needs to have Sytem.Xml frame work added (right click on References, add reference / Assemblies / framework)
4. Xamarin.Forms.Android:
   1. needs to be built with Android 7.1 (Project Properties / Application / Compile using Android version)
   2. needs to have ```ROOT_RENDERERS``` symbol defined (Project Properties / Compiler / Define Symbols)
   3. Properties/AssemblyInfo.cs: comment out ```[assembly: ExportRenderer (typeof (Toolbar), typeof (ToolbarRenderer))]```
5. Set up ```packages``` symbolic links (see below)
6. Set up symbolic link that links from ```Xamarin.Forms/.nuget``` to ```.nuget``` in the solution's root directory.
7. Be sure there is a copy of the ```Xamarin.Forms.2.4.0.280``` nuget package in the solution's ```packages``` directory.
    

Note that the above may need to be repeated if switching to a different Xamarin.Forms git commit.

#### Every submodule needs a ```packages``` folder

Because of the way VisualStudio, Nuget, and Git (don't) work together, relative paths are not valid for Nuget packages referenced in project files (that are from VisualStudio solutions) that have been added as git submodules.  To work around for this, go to the following directories (below) and symbolically link the ```packages``` subdirectory to the solution's working Nuget packages directory:

 - Forms9Patch/Forms9PatchDemo
 - Forms9Patch/P42
 - Forms9Patch/Xamarin.Forms

Below are example commands of how to symbolically link to your VisualStudio solution's (and git root module) Nuget packages folder.  For the case of building Forms9Patch source and demo code using just this git module, the below command lines are meant to be executed in the above directories.

 - **Windows:** ``` c:> mklink /D packages ..\packages ```
 - **OSX:** ``` $ ln -s ../packages packages ```

If you are using Forms9Patch source as a submodule in another git module, modify the above command lines to cause the symbolic links to point to the ```packages``` directory in the root directory of the VisualStudio solution associated with that git module.   


#### Address error:  ``` Project '..\Xamarin.Forms ... .csproj' targets 'netstandard2.0' ```

1. You're very likely referencing the latest and greatest Xamarin.Forms ```master``` branch commit.  Back up by switching to a ```2.4.0``` build.   
2. Manual clean out the ```bin``` and ```obj``` folders (SuperClean.exe) and Restore Packages.

#### Changes to Forms9PatchDemo, Forms9Patch, or FormsGestures source code isn't updating

This may likely not be a problem in the future but currently VisualStudio doesn't always show changes to ```.target``` files.  The extreme version of this can be seen in how ```.target``` file references are not seen at all in ```.PCL```, ```.iOS```, ```.Droid``` and ```.UWP``` projects.  Why am I using ```.target``` files then?  Because it saves a lot of work in keeping the source code for various permutations of builds in sync.



#### Forms9PatchDemo.PCL.Droid won't run when built and deployed from VisualStudio 2017 (Windows) to Android Emulator

Heck, it might not work when deployed to an actual device.  It *does* work when deployed from VisualStudio Mac to Android Emulator.  1 point for VisualStudio Mac!  If you know why it doesn't work on Windows, please share!


#### Why are the Xaml examples not in the Forms9PatchDemo.Source apps?

Because I haven't been able to take the time to figure out why Xaml files are not being compiled and embedded properly when using Xamarin.Forms source.  Again, if you know how to do this, please share!


#### UWP: Unable to load `System.Runtime. The located assembly's manifest definition does not match the assembly reference.`

Change the Windows target version to 14393.   
