using System;
using System.Collections.Generic;
using Android.Runtime;
using Android.Views;
using Xamarin.Forms;

//[assembly: ExportRenderer(typeof(Forms9Patch.RootPage), typeof(Forms9Patch.Droid.PageRenderer))]
//[assembly: ExportRenderer(typeof(Forms9Patch.ContentPage), typeof(Forms9Patch.iOS.PageRenderer))]
namespace Forms9Patch.Droid
{
    /*
    public static class FirstResponderExtensions
    {
        public static UIView GetFirstResponder(this UIView parent)
        {
            if (parent.IsFirstResponder)
                return parent;
            foreach (var subview in parent.Subviews)
            {
                var responder = subview.GetFirstResponder();
                if (responder != null)
                    return responder;
            }
            return null;
        }

        public static object GetFirstResponder(this UIViewController parent)
        {
            if (parent.IsFirstResponder)
                return parent;
            var view = parent.View.GetFirstResponder();
            if (view != null)
                return view;
            foreach (var subview in parent.ChildViewControllers)
            {
                var responder = subview.GetFirstResponder();
                if (responder != null)
                    return responder;
            }
            return null;
        }
    }
    */
    /*
    public class PageRenderer : Xamarin.Forms.Platform.Android.PageRenderer, Android.Views.View.IOnKeyListener //, IUIKeyInput
    {
        public PageRenderer()
        {

        }


        protected override void OnAttachedToWindow()
        {
            System.Diagnostics.Debug.WriteLine("PageRenderer.OnAttachedToWindow");
            System.Diagnostics.Debug.WriteLine("\t Parent=[" + this.Parent + "]");
            var root = this.GetFurthestAncestor<Android.Views.View>();
            System.Diagnostics.Debug.WriteLine("\t Ancestor=[" + root + "]");

            base.OnAttachedToWindow();
        }

        public override bool OnKeyUp(Android.Views.Keycode keyCode, Android.Views.KeyEvent e)
        {
            System.Diagnostics.Debug.WriteLine("PageRenderer.OnKeyUp[" + keyCode + "] e.Action[" + e.Action + "] e.Characters[" + e.Characters + "] e.DisplayLabel[" + e.DisplayLabel + "] e.Flags[" + e.Flags + "] e.MetaStates[" + e.MetaState + "] e.Modifiers[" + e.Modifiers + "] e.Unicode[" + (char)e.UnicodeChar + "] ");
            return base.OnKeyUp(keyCode, e);
        }

        public bool OnKey(Android.Views.View v, [GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            System.Diagnostics.Debug.WriteLine("OnKey[" + keyCode + "] e.Action[" + e.Action + "] e.Characters[" + e.Characters + "] e.DisplayLabel[" + e.DisplayLabel + "] e.Flags[" + e.Flags + "] e.MetaStates[" + e.MetaState + "] e.Modifiers[" + e.Modifiers + "] e.Unicode[" + (char)e.UnicodeChar + "] ");
            return false;
        }
    }
    */
}