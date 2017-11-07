// /*******************************************************************
//  *
//  * CustomFontEffect.cs copyright 2016 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportEffect(typeof(Forms9Patch.UWP.CustomFontEffect), "CustomFontEffect")]
namespace Forms9Patch.UWP
{
	/// <summary>
	/// Custom font effect.
	/// </summary>
	public class CustomFontEffect : PlatformEffect
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
			    //args.PropertyName == Xamarin.Forms.Label.FontSizeProperty.PropertyName ||
			    //args.PropertyName == Xamarin.Forms.Label.FontAttributesProperty.PropertyName ||
			    args.PropertyName == Xamarin.Forms.Label.FontProperty.PropertyName)
				UpdateFont();
		}

		void UpdateFont()
		{
			if (instances < 4 || Settings.IsLicenseValid)
			{
				var elementType = Element.GetType();
				var elementFontFamilyProperty = elementType.GetProperty("FontFamily");
				if (elementFontFamilyProperty == null)
				{
					Forms9Patch.Toast.Create("Forms9Patch Custom Font Effect","Element of type ["+elementType+"] does not have FontFamily property");
					return;
				}
                var elementFontFamily = elementFontFamilyProperty.GetValue(Element) as string;
                var controlType = Control.GetType();
				var controlFontFamilyProperty = controlType.GetProperty("FontFamily");
                //var fontFamilyName = FontExtensions.EmbeddedFontFamilyName(elementFontFamily);
                //controlFontFamilyProperty.SetValue(Control, fontFamilyName, null);
			}
		}
	}
}

