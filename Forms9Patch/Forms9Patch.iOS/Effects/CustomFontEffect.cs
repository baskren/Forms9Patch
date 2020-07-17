// /*******************************************************************
//  *
//  * EmbeddedResourceFontEffect.cs copyright 2016 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(Forms9Patch.iOS.EmbeddedResourceFontEffect), "EmbeddedResourceFontEffect")]
namespace Forms9Patch.iOS
{
    /// <summary>
    /// Custom font effect.
    /// </summary>
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class EmbeddedResourceFontEffect : PlatformEffect
    {
        static int instances;

        /// <summary>
        /// Called when the effect is attached.
        /// </summary>
        protected override void OnAttached()
        {
            instances++;
            if (Element != null)
                UpdateFont();
        }

        /// <summary>
        /// Called when the effect is detached
        /// </summary>
        protected override void OnDetached()
        {
        }

        /// <param name="args">To be added.</param>
        /// <summary>
        /// Called when a property is changed
        /// </summary>
        /// <remarks>To be added.</remarks>
        protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);
            if (args.PropertyName == Xamarin.Forms.Label.FontFamilyProperty.PropertyName ||
                args.PropertyName == Xamarin.Forms.Label.FontSizeProperty.PropertyName ||
                args.PropertyName == Xamarin.Forms.Label.FontAttributesProperty.PropertyName ||
                args.PropertyName == Xamarin.Forms.Label.FontProperty.PropertyName)
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
            var sizeProperty = elementType.GetProperty("FontSize");
            if (sizeProperty == null)
            {
                System.Console.WriteLine("Element of type [" + elementType + "] does not have FontSize property");
                return;
            }
            var attrProperty = elementType.GetProperty("FontAttributes");
            if (attrProperty == null)
            {
                System.Console.WriteLine("Element of type [" + elementType + "] does not have FontAttributes property");
                return;
            }
            var controlType = Control.GetType();
            var fontProperty = controlType.GetProperty("Font");
            if (fontProperty != null)
            {
                var fontFamily = (string)familyProperty.GetValue(Element);
                var fontSize = (double)sizeProperty.GetValue(Element);
                var fontAttributes = (FontAttributes)attrProperty.GetValue(Element);
                if (fontSize < 0)
                {
                    if (Control is UIButton)
                        fontSize = UIFont.ButtonFontSize * Math.Abs(fontSize);
                    else
                        fontSize = UIFont.LabelFontSize * Math.Abs(fontSize);
                }
                if (Math.Abs(fontSize) <= double.Epsilon * 10)
                    fontSize = UIFont.LabelFontSize;

                foreach (var effect in Element.Effects)
                {
                    if (effect is Forms9Patch.EmbeddedResourceFontEffect fontEffect)
                    {
                        var newFont = FontExtensions.BestFont(fontFamily, (float)fontSize, (fontAttributes & FontAttributes.Bold) != 0, (fontAttributes & FontAttributes.Italic) != 0, fontEffect.Assembly);
                        fontProperty.SetValue(Control, newFont, null);
                        return;
                    }
                }
            }
        }
    }
}

