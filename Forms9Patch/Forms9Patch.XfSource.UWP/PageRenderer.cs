using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(Forms9Patch.RootPage), typeof(Forms9Patch.UWP.PageRenderer))]
//[assembly: ExportRenderer(typeof(Forms9Patch.ContentPage), typeof(Forms9Patch.UWP.PageRenderer))]
namespace Forms9Patch.UWP
{
    class PageRenderer  : Xamarin.Forms.Platform.UWP.PageRenderer
    {

        public PageRenderer() : base ()
        {
            KeyUp += OnKeyUp;
            KeyDown += OnKeyDown;

            Loaded += PageRenderer_Loaded;

            Window.Current.CoreWindow.CharacterReceived += CoreWindow_CharacterReceived;
            /*
            Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(4), () =>
             {
                 var element = Windows.UI.Xaml.Input.FocusManager.GetFocusedElement();
                 if (element is Windows.UI.Xaml.FrameworkElement frameworkElement)
                 {
                     //System.Diagnostics.Debug.WriteLine("\tName=[" + frameworkElement.Name + "]");
                     //System.Diagnostics.Debug.WriteLine("\tParent=[" + frameworkElement.Parent + "]");
                 }
                 //System.Diagnostics.Debug.WriteLine("Focus: ["+element+"]");
                 if (element is Windows.UI.Xaml.Controls.ScrollViewer scrollViewer && scrollViewer.Parent==null)
                 {
                     
                     System.Diagnostics.Debug.WriteLine("\tHorizontalScrollBarVisibility=["+ scrollViewer.HorizontalScrollBarVisibility + "]");
                     System.Diagnostics.Debug.WriteLine("\tVerticalScrollBarVisibility=[" + scrollViewer.VerticalScrollBarVisibility + "]");
                     System.Diagnostics.Debug.WriteLine("\tIsDeferredScrollingEnabled=[" + scrollViewer.IsDeferredScrollingEnabled + "]");
                     System.Diagnostics.Debug.WriteLine("\tHorizontalScrollMode=[" + scrollViewer.HorizontalScrollMode + "]");
                     System.Diagnostics.Debug.WriteLine("\tIsTabStop=[" + scrollViewer.IsTabStop + "]");
                     System.Diagnostics.Debug.WriteLine("\tIsVerticalRailEnabled=[" + scrollViewer.IsVerticalRailEnabled + "]");
                     System.Diagnostics.Debug.WriteLine("\tMargin=[" + scrollViewer.Margin + "]");
                     System.Diagnostics.Debug.WriteLine("\tVerticalScrollMode=[" + scrollViewer.VerticalScrollMode + "]");
                     System.Diagnostics.Debug.WriteLine("\tZoomMode=[" + scrollViewer.ZoomMode + "]");
                     //if (scrollViewer.HorizontalScrollBarVisibility== Windows.UI.Xaml.Controls.ScrollBarVisibility.Hidden  && scrollViewer.VerticalScrollBarVisibility == Windows.UI.Xaml.Controls.ScrollBarVisibility.Hidden && !scrollViewer.IsDeferredScrollingEnabled)
                     scrollViewer.IsTabStop = false;
                     Windows.UI.Xaml.Input.FocusManager.TryMoveFocus(Windows.UI.Xaml.Input.FocusNavigationDirection.Next);
                     
                     
                     scrollViewer.IsTabStop = false;
                     Windows.UI.Xaml.Input.FocusManager.TryMoveFocus(Windows.UI.Xaml.Input.FocusNavigationDirection.Next);
                     System.Diagnostics.Debug.WriteLine("NEUTRALIZED");
                     return false;
                     

                     System.Diagnostics.Debug.WriteLine("=================Hierarchy ============== \n"+ scrollViewer.GenerateHeirachry());
                     System.Diagnostics.Debug.WriteLine("=========================================");

                     return false;
                 }
                 return true;
             });
             */
        }

        private void CoreWindow_CharacterReceived(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.CharacterReceivedEventArgs args)
        {
            System.Diagnostics.Debug.WriteLine("CharRecv["+(char)args.KeyCode+"]["+args.KeyStatus+"]");
        }

        private void PageRenderer_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var root = this.GetFurthestAncestor<Windows.UI.Xaml.FrameworkElement>();
            if (root is Windows.UI.Xaml.Controls.ScrollViewer scrollViewer)
                scrollViewer.IsTabStop = false;
        }

        private void OnKeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("OnKeyDown[" + e.Key + "]["+e.OriginalKey+"]  Element=[" + Element + "] Parent=[" + Element.Parent + "]  \te.Handled=[" + e.Handled + "] \te.KeyStatus=[" + e.KeyStatus + "] ");

            // This is a french keyboard?  The quick brown fox jumped over the lazy dog? 
            
        }

        private void OnKeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("OnKeyUp[" + e.Key + "][" + e.OriginalKey + "]   Element=[" + Element + "] Parent=[" + Element.Parent + "]  \te.Handled=[" + e.Handled + "] \te.KeyStatus=[" + e.KeyStatus + "]");
        }

    }
}
