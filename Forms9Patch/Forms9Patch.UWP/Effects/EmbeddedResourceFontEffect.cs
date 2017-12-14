// /*******************************************************************
//  *
//  * EmbeddedResourceFontEffect.cs copyright 2016 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using System.Linq;
using System.Reflection;
using Windows.UI.Xaml.Media;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportEffect(typeof(Forms9Patch.UWP.EmbeddedResourceFontEffect), "EmbeddedResourceFontEffect")]
namespace Forms9Patch.UWP
{
	/// <summary>
	/// Custom font effect.
	/// </summary>
	public class EmbeddedResourceFontEffect : PlatformEffect
	{
		static int _instances;
        int _instance;
        Forms9Patch.EmbeddedResourceFontEffect _embeddedResourceFontEffect;

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
                    Toast.Create("UNLICENSED COPY", "Only 4 instances of Forms9Patch.EmbeddedResourceFontEffect is available without a license.");
                    _elementFontFamilyProperty = null;
                    _controlFontFamilyProperty = null;
                }
                else
                {
                    _embeddedResourceFontEffect = (Forms9Patch.EmbeddedResourceFontEffect)Element.Effects.FirstOrDefault(e => e is Forms9Patch.EmbeddedResourceFontEffect);
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
            _embeddedResourceFontEffect = null;
		}

		/// <param name="args">To be added.</param>
		/// <summary>
		/// Called when a property is changed
		/// </summary>
		/// <remarks>To be added.</remarks>
		protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs args)
		{
			if (args.PropertyName == Xamarin.Forms.Label.FontFamilyProperty.PropertyName ||
			    args.PropertyName == Xamarin.Forms.Label.FontProperty.PropertyName)
				UpdateFont();
            else
                base.OnElementPropertyChanged(args);
        }

        void UpdateFont()
		{
            if (_elementFontFamilyProperty != null && _controlFontFamilyProperty!=null)
            {
                var fontFamily = _elementFontFamilyProperty.GetValue(Element) as string;
                var assembly = _embeddedResourceFontEffect?.Assembly;
                if (assembly != null && !Settings.AssembliesToInclude.Contains(assembly))
                    Settings.AssembliesToInclude.Add(assembly);
                var uwpFontFamily = FontService.ReconcileFontFamily(fontFamily);
                _controlFontFamilyProperty.SetValue(Control, new FontFamily(uwpFontFamily));
            }
		}
	}

}

