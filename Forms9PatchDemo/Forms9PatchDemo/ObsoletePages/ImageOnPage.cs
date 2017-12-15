using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Forms9PatchDemo.Pages
{
    public class ImageOnPage : ContentPage
    {
        public ImageOnPage()
        {
            Content = new StackLayout
            {
                Children = {
                    
                    new Image
                    {
                        Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.button"),
                      
                    }
                }
            };
        }
    }
}