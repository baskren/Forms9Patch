// /*******************************************************************
//  *
//  * EmbeddedResourceFontEffect.cs copyright 2016 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using System.Reflection;
using Android.Graphics;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportEffect(typeof(Forms9Patch.Droid.EmbeddedResourceFontEffect), "EmbeddedResourceFontEffect")]
namespace Forms9Patch.Droid
{
    /// <summary>
    /// Custom font effect.
    /// </summary>
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class EmbeddedResourceFontEffect : PlatformEffect
    {
        static int instances;

        /// <summary>
        /// Called when the effect is attached
        /// </summary>
        protected override void OnAttached()
        {
            instances++;
            if (Element != null)
            {
                if (Element is Xamarin.Forms.Label)
                    Device.StartTimer(TimeSpan.FromMilliseconds(50), () =>
                    {
                        UpdateFont();
                        Control.Invalidate();
                        //Control.RequestLayout();
                        //((Layout)((VisualElement)Element).Parent)?.ForceLayout();
                        return false;
                    });
                else
                    UpdateFont();
            }
        }

        /// <summary>
        /// Called when the effect is detattached
        /// </summary>
        protected override void OnDetached()
        {
        }

        /// <param name="args">To be added.</param>
        /// <summary>
        /// Called when a property is changed
        /// </summary>
        protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);
            if (args.PropertyName == Xamarin.Forms.Label.FontFamilyProperty.PropertyName ||
                //args.PropertyName == Xamarin.Forms.Label.FontSizeProperty.PropertyName ||
                args.PropertyName == Xamarin.Forms.Label.FontAttributesProperty.PropertyName ||
                args.PropertyName == Xamarin.Forms.Label.FontProperty.PropertyName)
                    UpdateFont();
            else if (args.PropertyName == Xamarin.Forms.Label.TextProperty.PropertyName)
                    UpdateFont();
        }

        void UpdateFont()
        {
            var elementType = Element.GetType();
            var familyProperty = elementType.GetProperty("FontFamily");
            if (familyProperty == null)
            {
                System.Console.WriteLine("Element of type [" + elementType + "] does not have FontFamily property");
                return;
            }
            var controlType = Control.GetType();
            var typefaceProperty = controlType.GetProperty("Typeface");
            if (typefaceProperty != null)
            {
                var fontFamily = (string)familyProperty.GetValue(Element);
                //var fontSize = (double)sizeProperty.GetValue(Element);
                //var fontAttributes = (FontAttributes)attrProperty.GetValue(Element);

                // from Forms9Patch.LabelRenderer
                Assembly assembly = null;
                foreach (var effect in Element.Effects)
                {
                    if (effect is Forms9Patch.EmbeddedResourceFontEffect fontEffect)
                    {
                        assembly = fontEffect.Assembly;
                        break;
                    }
                }
                        Typeface newTypeface = FontManagment.TypefaceForFontFamily(fontFamily, assembly);
                if (newTypeface == null)
                {
                    var elementFontProperty = elementType.GetProperty(Xamarin.Forms.Label.FontProperty.PropertyName);
                    if (elementFontProperty == null)
                    {
                        System.Console.WriteLine("Element of type [" + elementType + "] does not have Font property");
                        return;
                    }
                    var f = (Font)elementFontProperty.GetValue(Element);
                    newTypeface = f.ToTypeface();
                }
                typefaceProperty.SetValue(Control, newTypeface);
            }
        }
    }
}

