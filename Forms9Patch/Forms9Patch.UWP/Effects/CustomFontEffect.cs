// /*******************************************************************
//  *
//  * CustomFontEffect.cs copyright 2016 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using System.Linq;
using System.Reflection;
using Windows.UI.Xaml.Media;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using Forms9Patch.Extensions;
using System.Collections.Generic;
using Windows.Storage;

[assembly: ExportEffect(typeof(Forms9Patch.UWP.CustomFontEffect), "CustomFontEffect")]
namespace Forms9Patch.UWP
{
	/// <summary>
	/// Custom font effect.
	/// </summary>
	public class CustomFontEffect : PlatformEffect
	{
		static int _instances;
        int _instance;
        Forms9Patch.CustomFontEffect _customFontEffect;

        PropertyInfo _elementFontFamilyProperty = null;
        PropertyInfo _controlFontFamilyProperty = null;

		/// <summary>
		/// Called when the effect is attached.
		/// </summary>
		protected override void OnAttached()
		{
            _elementFontFamilyProperty = Element.GetType().GetProperty("FontFamily");
            _controlFontFamilyProperty = Control.GetType().GetProperty("FontFamily");
            if (_elementFontFamilyProperty != null && _controlFontFamilyProperty!=null)
            {
                _instance = _instances++;
                if (_instance > 4 && !Settings.IsLicenseValid)
                {
                    Toast.Create("UNLICENSED COPY", "Only 4 instances of Forms9Patch.CustomFontEffect is available without a license.");
                    _elementFontFamilyProperty = null;
                    _controlFontFamilyProperty = null;
                }
                else
                {
                    _customFontEffect = (Forms9Patch.CustomFontEffect)Element.Effects.FirstOrDefault(e => e is Forms9Patch.CustomFontEffect);
                    if (_customFontEffect != null)
                        _customFontEffect.PropertyChanged += OnEffectPropertyChanged;
                    UpdateFont();
                }
            }
        }

        private void OnEffectPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Assembly")
                UpdateFont();
        }

        /// <summary>
        /// Called when the effect is detached
        /// </summary>
        protected override void OnDetached()
		{
            if (_customFontEffect!=null)
                _customFontEffect.PropertyChanged -= OnEffectPropertyChanged;
            _customFontEffect = null;
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
			    args.PropertyName == Xamarin.Forms.Label.FontProperty.PropertyName)
				UpdateFont();
		}

		async void UpdateFont()
		{
            if (_elementFontFamilyProperty != null && _controlFontFamilyProperty!=null)
            {
                var fontFamily = _elementFontFamilyProperty.GetValue(Element) as string;

                if (fontFamily != null)
                {
                    var fontFamilyParts = fontFamily.Split('#');

                    var fontFileResourceId = fontFamilyParts[0];
                    var localStorageFileName = await PCL.Utils.EmbeddedResourceCache.LocalStorageSubPathForEmbeddedResourceAsync(fontFileResourceId, _customFontEffect.Assembly);

                    if (localStorageFileName != null)
                    {
                        string fontName = null;
                        if (fontFamilyParts.Count() > 1)
                            fontName = fontFamilyParts.Last();
                        else
                        {
                            var cachedFile = PCL.Utils.EmbeddedResourceCache.FileForEmbeddedResource(fontFileResourceId, _customFontEffect.Assembly);
                            fontName = PCL.Utils.TTFAnalyzer.FontFamily(cachedFile);
                        }
                        var fontFamilyString = "ms-appdata:///local/" + localStorageFileName + "#" + fontName;
                        var uwpFontFamily = new FontFamily(fontFamilyString);
                        _controlFontFamilyProperty.SetValue(Control, uwpFontFamily);
                    }
                }
            }
		}
	}

}

