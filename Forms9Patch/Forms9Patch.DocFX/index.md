# Forms9Patch 

Simplify image management and text formatting in your PCL and Shared Library Xamarin.Forms iOS, Android and UWP applications.

## Overview

Xamarin Forms is great for developing cross platform applications (certainly very sane) but it is missing some important features:

 - Patch scalable Images
 - Single point of use, cross platform, multi-screen / multi-resolution image management
 - Simple, markup formatted text for labels and buttons
 - Single point of use, cross platform, custom font management
 - Segmented button control
 - Pop-ups as a first class layout element

Android developers can use NinePatch bitmaps, the drawable directory naming convention, Html.FromHtml, and a bunch of complex file manipulations to address the image issue.  Likewise, iOS developers can use ResizeableImageWithCapInsets, the @2x, @3x, @4x file naming convention, and some 3rd party libraries for this purpose.  Forms9Patch enhances Xamarin Forms to make multi-resolution / multi-screen image management, custom fonts, and HTML text formatting easy for PCL and Shared Library applications for iOS, Android and UWP.

## So, what exactly is Forms9Patch?

Simply stated, Forms9Patch is three core elements, five layouts, three buttons, six pop-ups, and some extensions and services.   The three core elements are:

 - `Forms9Patch.ImageSource`: a free to use derivative of `Xamarin.Forms.ImageSource` to support multi-device / multi-density image management from Embedded Resources
 - `Forms9Patch.Image`: an enhanced implementation of `Xamarin.Forms.Image` with border (outline), shadow, patch scaling, tiling, and tinting support
 - `Forms9Patch.Label`: an enhanced implementation of `Xamarin.Forms.Label` for easy access to single-point-of-use custom fonts, auto-fitting, and text formatting via markup tags

Next, it is derivative of the five `Xamarin.Forms.Layout` subclasses that applies the enhancements from `Forms9Patch.Image` (patch-scaling, tiling, tinting, borders, and shadows) to the layouts' background.

 - `Forms9Patch.Frame`
 - `Forms9Patch.AbsoluteLayout`
 - `Forms9Patch.Grid`
 - `Forms9Patch.RelativeLayout`
 - `Forms9Patch.StackLayout`

Additionally, the three button elements are:

 - `Forms9Patch.Button`: an enhanced implementation of `Xamarin.Forms.Button` that applies the enhancements from `Forms9Patch.Image' to the button's background and icon image and the enhancements from `Forms9Patch.Label` to the buttons' icon (as text) and label, and adds long-press events
 - `Forms9Patch.StateButton`: a further enhanced implementation of `Forms9Patch.Button` that allow most enhanced properties to be configured by the button's state (Default, Pressing, Selected, Disabled, DisabledAndSelected) 
 - `Forms9Patch.SegmentControl`: a segmented button control that allows for border, separator, and shadow control

Likewise, it is a series of pop-up elements that have page overlay backgrounds and the ability to be canceled by tapping the background:

 - `Forms9Patch.ModalPopup`: takes the enhancements (borders; patch-scalable, tintable, tile-able background images; shadows) from `Forms9Patch.Frame` and puts it into a pop-up layout
 - `Forms9Patch.BubblePopup`: goes a step further than `Forms9Patch.ModalPopup` by pointing to a target `Xamarin.Forms.VisualElement`
 - `Forms9Patch.ActivityIndicatorPopup`: a convenience element that presents a `Xamarin.Forms.ActivityIndicator` over a page overlay
 - `Forms9Patch.Toast`: a convenience element that presents a title, a message and an optional confirmation button
 - `Forms9Patch.PermissionPopup`: a convenience element that presents a title, a message, and an accept and a decline button
 - `Forms9Patch.TargetedToast`: similar to `Forms9Patch.Toast` but enhanced to point to a `Xamarin.Forms.VisualElement`

Then there are Forms9Patch's extensions.  A few noteworthy ones are:

 - Color Extensions
 - WebView Extensions 
 - String Extensions
 - HtmlString Extensions

And lastly is Forms9Patch's services:

 - Keyboard Service: Want to put away the system keyboard?  Want to be notified when the system keyboard appears or disappears? 
 - Key Clicks Service: Do you want to make a system keyboard sound (and vibration, if available)?
 - Application Info Service: Do you want to quickly know your application's Name, Bundle/Package ID, Build number and Version string?
 - OS Info Service: What is the version of the device's operating system?
 



 


