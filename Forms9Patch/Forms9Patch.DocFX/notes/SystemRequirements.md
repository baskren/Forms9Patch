# System Requirements

Forms9Patch is an enhancement to Xamarin.Forms.  As such, first be sure you've met the [Xamarin.Forms system requirements](https://developer.xamarin.com/guides/cross-platform/getting_started/requirements/).

Additionally:

- Forms9Patch requires Xamarin.Forms version 2.4.0.280 or newer.
- To use the `background-color` style attribute within `Forms9Patch.Label.HtmlText` markup text in UWP applications, your UWP applications will need to be built with a minimum Windows version of 10.0.16299.0 (Windows 10 Fall Creators Update).
- To build your Android apps, you will need to add the following to your Android application project's `Resources/values/strings.xml` file:

   ```xml
   <string name="forms9patch_copy_paste_authority">your_Android_app_package_name_here.f9pcopypaste</string>
   ```
