# OBSOLETE!!!

Forms9Patch is now obsolete.  I highly recommend you take a look at the [Uno Platform](https://platform.uno) framework, as it is everything Xamarin.Forms and MAUI promised to be while at the same time being faster, more stable, and more feature complete.  If you find Uno Platform helpful, then you might want to take a look at the following projects:

- [P42.Utils.Uno](https://github.com/baskren/P42.Utils)
- [P42.Uno.Controls](https://github.com/baskren/P42.Uno.Controls)
- [P42.Uno.HtmlExtensions](https://github.com/baskren/P42.Uno.HtmlExtensions)
- [P42.Uno.HardwareKeys](https://github.com/baskren/P42.Uno.HardwareKeys)
- [P42.Uno.Xamarin.Essentials](https://github.com/baskren/P42.Uno.Xamarin.Essentials)
- [P42.Uno.AbstractScanner](https://github.com/baskren/P42.Uno.AbstractScanner) Barcode scanning for iOS Camera, iOS + Honeywell Captova, Android Camera, Zebra Android Devices, and Windows
- [P42.VirtualKeyboard](https://github.com/baskren/P42.VirtualKeyboard) Dealing with on screen keysboards



# Forms9Patch ![Alt text](./docs/logo.svg) 

A suite of elements built to simplify image management, text formatting, PNG generation, PDF generation, and printing for your NetStandard, PCL, and Shared Library Xamarin.Forms iOS, Android and UWP applications.

You can learn more at https://baskren.github.io/Forms9Patch/index.html

## Overview
Xamarin Forms is great for developing cross platform applications (certainly very sane) but it is missing some important features:

 - Patch scalable images
 - SVG images
 - Single point of use, cross platform, multi-screen / multi-resolution image management
 - Simple, markup formatted text for labels and buttons
 - Single point of use, cross platform, custom font management
 - Segmented button control
 - Pop-ups as a first class layout element
 - Multi-object Clipboard and Inter-app Data Sharing
 - PNG and PDF generation
 - Printing
 
Android developers can use NinePatch bitmaps, the drawable directory naming convention, Html.FromHtml, and a bunch of complex file manipulations to address the image issue. Likewise, iOS developers can use ResizeableImageWithCapInsets, the @2x, @3x, @4x file naming convention, and some 3rd party libraries for this purpose. Custom fonts are a bit more complicated and label layouts take things up another notch. Forms9Patch enhances Xamarin Forms to make multi-resolution / multi-screen image management, custom fonts, and HTML text formatting easy for NetStandard, PCL and Shared Library applications for iOS, Android and UWP.

## Documentation

There are a number of guides to get you started:

- [Getting Started with VisualStudio Mac](https://baskren.github.io/Forms9Patch/guides/GettingStartedMac.html)
- [Getting Started with VisualStudio Windows](https://baskren.github.io/Forms9Patch/guides/GettingStartedWindows.html)
- [Working with Images](https://baskren.github.io/Forms9Patch/guides/Image.html)
- [Working with platform independent and yet (if you want it) device **dependent** image sources](https://baskren.github.io/Forms9Patch/guides/ImageSource.html)
- [Layouts with flair](https://baskren.github.io/Forms9Patch/guides/Layouts.html)
- [Autofit and HTML markup Labels](https://baskren.github.io/Forms9Patch/guides/Label.html)
- [Button magic](https://baskren.github.io/Forms9Patch/guides/Buttons.html)
- [A suite of popups!](https://baskren.github.io/Forms9Patch/guides/Popups.html)
- [platform independent Custom fonts made easy](https://baskren.github.io/Forms9Patch/guides/CustomFonts.html)
- [PNGs from HTML or Xamarin.Forms.WebView](https://baskren.github.io/Forms9Patch/guides/ToPngService.html)
- [PDFs from HTML or Xamarin.Forms.WebView](https://baskren.github.io/Forms9Patch/guides/ToPdfService.html)
- [Printing made easy](https://baskren.github.io/Forms9Patch/guides/PrintService.html)

If you want to learn more (or findout about Forms9Patch features not in the above guides):

- [Documentation Web site](https://baskren.github.io/Forms9Patch/)
- [**ANDROID** Special notes](https://baskren.github.io/Forms9Patch/notes/Android.html)

## Demos

Demo apps are in the `Demo` folder of this repository.

 - [Demo app using the Forms9Patch **NuGet package**](https://github.com/baskren/Forms9Patch/tree/master/Demo/UsingForms9PatchNuGet)
 - [Demo app using the Forms9Patch **source code**](https://github.com/baskren/Forms9Patch/tree/master/Demo/UsingForms9PatchSource)
