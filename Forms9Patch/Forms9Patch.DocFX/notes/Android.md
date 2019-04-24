# Special notes for Android

Any cross platform developer knows that Android is a very special beast.  Unfortunately, Forms9Patch doesn't mitigate this.  In fact, until I get the time to remove some warts, some would argue `Forms9Patch.Clipboard` isn't doing most developers any favors.  That being said, here are some notes for your Android platform projects:

- AS OF VERSION 1.4, THE FOLLOWING IS NO LONGER NECESSARY.  Because of Forms9Patch's more comprehensive `Forms9Patch.Clipboard` functionality, you will have to add the following code to your Android platform project's `Resources/Values/string.xml` file, before you can build it:

   ```xml
   <string name="forms9patch_copy_paste_authority">your_Android_app_package_name_here.f9pcopypaste</string>
   ```

   I know - it's a pain.  At some point of time, I'll have to see if I can use some of the trickery used by the Xamarin.Facebook NuGet Package to get rid of this requirement.

- I've made too many mistakes in building the Forms9Patch NuGet package - and Android seems to bear the brunt of those mistakes.  If, after upgrading Forms9Patch, you see a bunch of errors like the following ...

    ```text
    /Users/ben/Projects/PizzaTruck/PizzaTruck.Mobile/Droid/Renderers/BottomBarPageRenderer.cs(42,42): Error CS0012: The type 'FormsViewGroup' is defined in an assembly that is not referenced. You must add a reference to assembly 'FormsViewGroup, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null'. (CS0012)

    /Users/ben/Projects/PizzaTruck/PizzaTruck.Mobile/Droid/MainActivity.cs(33,33): Error CS0012: The type 'FormsViewGroup' is defined in an assembly that is not referenced. You must add a reference to assembly 'FormsViewGroup, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null'. (CS0012)
    ```

    ... then I'm the one you need to blame.  To fix this you'll need to do the following in any Android platform projects that use Forms9Patch:

    - Remove the Forms9Patch NuGet package
    - Remove the Xamarin.Forms NuGet package
    - If present, remove any SkiaSharp NuGet packages
    - If present in the References folder, remove any references to `FormsViewGroup`
    - Reinstall the Xamarin.Forms NuGet package
    - Reinstall the Forms9Patch NuGet package

    If the above doesn't work, PLEASE LET ME KNOW by emailing me at: forms9patch(AT)buildcalc.com

- This isn't a Forms9Patch issue but just good advice: Do yourself a favor and never use any Android ARM emulators.  This means you'd be better off just deleting the `Android_ARM7a_Nouget (API 25)` emulator that comes pre-installed with VisualStudio 2017.
