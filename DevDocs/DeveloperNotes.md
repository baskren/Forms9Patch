# Developer Notes

## Project Configuration

### Every solution needs a ```packages``` folder

Because of the way VisualStudio, Nuget, and Git (don't) work together, relative paths are not valid for Nuget packages referenced in project files (that are from VisualStudio solutions) that have been added as git submodules.  To work around for this, go to the solutions root folder and the root folder for each submodule (ex: Xamarin.Forms) and symbolically link ```packages``` directory to your working Nuget packages folder.

Windows: ``` c:> mklink /D packages \Users\ben\.nuget\packages ```

OSX: ``` $ ln -s /Users/ben/.nuget/packages packages ```

### Address error:  ``` Project '..\Xamarin.Forms ... .csproj' targets 'netstandard2.0' ```

1. You're very likely referencing the latest and greatest Xamarin.Forms ```master``` branch commit.  Back up by switching to branch ```2.4.0```.  You may have to wipe Xamarin.Forms submodule and reinstall.  If so, remember 
2. Manual clean out the ```bin``` and ```obj``` folders

### Every root solution that builds Xamarin.Forms from source needs a ```.nuget``` folder

Windows: ``` c:> mklink /D .nuget Xamarin.Forms\.nuget```

OSX: ``` $ ln -s Xamarin.Forms/.nuget .nuget```


### Address ```System.ArgumentException: element is not of type Xamarin.Forms.View``` in ```Xamarin.Forms.Platform.Android.VisualElementRenderer``` where ```element``` is ```Forms9Patch.RootPage``` when running Android Build



### Notes in case you have to start all over again

1. Xamarin.Forms projects to include:
   1. Xamarin.Forms.Platform
   2. Xamarin.Forms.Core
   3. Xamarin.Forms.Xaml
   4. Xamarin.Forms.iOS
   5. Xamarin.Forms.Android.FormsViewGroup
   6. Xamarin.Forms.Android
   7. Xamarin.Forms.UAP
2. Xamarin.Forms.Android 2.4.0 needs to be built with Android 7.0 (Project Properties / Application / Compile using Android version)
3. Xamarin.Forms.Android.FormsViewGroup needs to have Sytem.Xml frame work added (right click on References, add reference / Assemblies / framework)


XfNuget is for Nuget package builds 
XfSource is for internal debugging.


Making a change here to see if git picks up on it.
