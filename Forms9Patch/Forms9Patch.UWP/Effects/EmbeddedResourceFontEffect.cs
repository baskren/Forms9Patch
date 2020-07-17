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
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class EmbeddedResourceFontEffect : PlatformEffect
    {
        #region Debug support
        bool DebugCondition
        {
            get
            {
                //string labelTextStart = "Żyłę;^`g ";
                //string labelTextStart = "X";
                //return (Element.Text != null && Element.Text.StartsWith(labelTextStart)) || (Element.HtmlText != null && Element.HtmlText.StartsWith(labelTextStart));
                return true;
            }
        }

        void DebugMessage(string message, [System.Runtime.CompilerServices.CallerMemberName] string callerName = null)
        {
            if (DebugCondition)
                System.Diagnostics.Debug.WriteLine(GetType() + "." + callerName + ": " + message);
        }
        #endregion

        static int _instances;
        int _instance;
        Forms9Patch.EmbeddedResourceFontEffect _embeddedResourceFontEffect;

        PropertyInfo _elementFontFamilyProperty = null;
        PropertyInfo _controlFontFamilyProperty = null;
        PropertyInfo _controlTextProperty = null;

        

        /// <summary>
        /// Called when the effect is attached.
        /// </summary>
        protected override void OnAttached()
        {
            _elementFontFamilyProperty = Element?.GetType().GetProperty("FontFamily");
            _controlFontFamilyProperty = Control?.GetType().GetProperty("FontFamily");
            _controlTextProperty = Control.GetType().GetProperty("Text");
            if (_elementFontFamilyProperty != null)
            {
                _instance = _instances++;
                _embeddedResourceFontEffect = (Forms9Patch.EmbeddedResourceFontEffect)Element.Effects.FirstOrDefault(e => e is Forms9Patch.EmbeddedResourceFontEffect);
                UpdateFont();
            }
        }

        private void OnEffectPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Forms9Patch.EmbeddedResourceFontEffect.Assembly))// "Assembly")
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
            if (_elementFontFamilyProperty != null)
            {
                var fontFamilyName = _elementFontFamilyProperty.GetValue(Element) as string;
                var assembly = _embeddedResourceFontEffect?.Assembly;
                if (assembly == null && fontFamilyName != null)
                    assembly = AssemblyExtensions.AssemblyFromResourceId(fontFamilyName);
                if (assembly != null && !Settings.AssembliesToInclude.Contains(assembly))
                    Settings.AssembliesToInclude.Add(assembly);
                var uwpFontFamilyName = FontService.ReconcileFontFamily(fontFamilyName);
                if (uwpFontFamilyName == null)
                    Console.WriteLine("WARNING EmbeddedResourceFontEffect: Could not find ResourceId [" + fontFamilyName + "] in visible assemblies.");
                //DebugMessage("uwpFontFamily=[" + uwpFontFamilyName + "] Length=[" + uwpFontFamilyName.Length + "]");

                var fontFamily = new FontFamily(uwpFontFamilyName);
                if (_controlFontFamilyProperty != null)
                    _controlFontFamilyProperty.SetValue(Control, fontFamily);
                else if (Control is Windows.UI.Xaml.Controls.TextBlock textBlock)
                    textBlock.FontFamily = fontFamily;
                else if (Control is Windows.UI.Xaml.Controls.TextBox textBox)
                    textBox.FontFamily = fontFamily;
                else if (Control is Windows.UI.Xaml.Controls.Button button)
                    button.FontFamily = fontFamily;
                else if (Control is Windows.UI.Xaml.Controls.ComboBox comboBox)
                    comboBox.FontFamily = fontFamily;
                else if (Control is Xamarin.Forms.Platform.UWP.FormsComboBox formsComboBox)
                    formsComboBox.FontFamily = fontFamily;
                else if (Control is Windows.UI.Xaml.Controls.DatePicker datePicker)
                    datePicker.FontFamily = fontFamily;
                else if (Control is Xamarin.Forms.Platform.UWP.FormsTextBox formsTextBox)
                    formsTextBox.FontFamily = fontFamily;
                else
                    Console.WriteLine("WARNING EmbeddedResourceFontEffect: Could not find FontFamily property for native element of type [" + Control.GetType() + "]");
            }

        }
    }

}

