# Developer Notes

## Project Configuration

### Every solution needs a ```packages``` folder

Because of the way VisualStudio, Nuget, and Git (don't) work together, relative paths are not valid for Nuget packages referenced in project files (that are from VisualStudio solutions) that have been added as git submodules.  To work around for this, go to the solutions root folder and the root folder for each submodule (ex: Xamarin.Forms) and symbolically link ```packages``` directory to your working Nuget packages folder.

Windows: ``` c:> mklink /D packages \Users\ben\.nuget\packages ```

OSX: ``` $ ln -s /Users/ben/.nuget/packages packages ```

### Address error:  ``` Project '..\Xamarin.Forms ... .csproj' targets 'netstandard2.0' ```

1. You're very likely referencing the latest and greatest Xamarin.Forms ```master``` branch commit.  Back up to something like ```2.4.0```.  
2. Manual clean out the ```bin``` and ```obj``` folders

### Every root solution that builds Xamarin.Forms from source needs a ```.nuget``` folder

Windows: ``` c:> mklink /D .nuget Xamarin.Forms\.nuget```

OSX: ``` $ ln -s Xamarin.Forms/.nuget .nuget```



