# Using Forms9Patch's Layouts

Just as `Xamarin.Forms.Frame` extends `Xamarin.Forms.ContentView` by adding the `OutlineColor` and `HasShadow` properties, `Forms9Patch.AbsoluteLayout`, `Forms9Patch.Frame`, `Forms9Patch.Grid`, `Forms9Patch.RelativeLayout` 
and `Forms9Patch.Stacklayout` extends their Xamarin.Forms counterparts with those two properties and adds the `BackgroundImage`, `ElementShape`, `OutlineWidth`, `OutlineRadius`, `OutlineColor` and `ShadowInverted` properties.  
In otherwords, everything you can do to decorate a `Forms9Patch.Image`, you can do to decorate Forms9Patch's layouts.

Additionally, the Forms9Patch layouts have the IgnoreChildren property as a way to improve app responsiveness.  When a child element is updated, it kicks off a measurement and layout cycle that can propogate up through the 
view hierarchy - potentially consuming a lot of CPU.  I'm looking at you, Xamarin.Forms.Android!  Sometimes this is necessary in order to make room for or reclaim room from the updated child element.  However, there a lot of 
instances where the updated child will never have an impact upon the rest of the view hierarchy - other than slowing everything down while Xamarin Forms goes through the measure-layout cycle.  For example, if you have a Grid 
with fixed (GridUnitType.Absolute) or proportional (GridUnitType.Star) sized rows and columns then any change to a child will not change the Grid's layout.  So, to keep an update of a child from propogating up the view hierarchy, 
set the IgnoreChildren property to true.  Please note that this is a "running with sissors" feature that should be used carefully.

## Code Example

```csharp
var frame = new Forms9Patch.Frame {
    Content = new Xamarin.Forms.Label {
        Text = "Frame OutlineRadius & Shadow",
        TextColor = Color.Black,
        FontSize = 12,
    },
    Padding = new Thickness(10),
    Background = Color.FromHex( 12),
    OutlineRadius = 2,
    HasShadow = true,
}
```

////  SHOW ABOVE EXAMPLE HERE FOR ALL THREE PLATFORMS

## XAML Exmaple

```xaml
<?xml version="1.0" encoding="UTF-8"?>
<ContentPage
    xmlns="http://xamarin.com/schemas/2014/forms"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:local="clr-namespace:MyXamlDemo;assembly=MyXamlDemo"
    xmlns:Forms9Patch="clr-namespace:Forms9Patch;assembly=Forms9Patch"
    x:Class="MyXamlDemo.MyPage"
    Padding="5, 20, 5, 5">
    <StackLayout>
        <f9p:Frame
            Padding="20"
            OutlineColor="Blue"
            OutlineWidth="3"
            OutlineRadius="10"
            BackgroundColor="Gray"
            />
            <Label Text="Forms9Patch.Frame w/ OutlineWidth+OutlineRadius"
                TextColor="White"
                HorizontalOptions="Center"
                VerticalOptions="Center"
                FontSize="14"
                />
        </f9p:Frame>
    </StackLayout>
</ContentPage>
```
////  SHOW ABOVE EXAMPLE HERE FOR ALL THREE PLATFORMS

