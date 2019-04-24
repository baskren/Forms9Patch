using System;
namespace Forms9Patch
{
    public class ContentPage : Xamarin.Forms.ContentPage
    {
    	// Some thoughts:
    	// It appears that, in iOS
    	// AddKeyCode and RemoveKeyCode could work except that we have yet to figure out:
    	// - what has Keyboard focus ... Xamarin.Forms.VisualElement.Focus doesn't map to that.
    	// - has no way of knowing when an teriary element gets focus (becuase we don't know if an element has focus).
    }
}
