using System;
using System.Collections.Generic;
using Java.Lang;
using Xamarin.Forms;

namespace Forms9Patch.Droid
{

    class TextControlState : IEquatable<TextControlState>
    {
        #region Properties
        ICharSequence _textFormatted;
        public ICharSequence TextFormatted
        {
            get => _textFormatted;
            set
            {
                if (value != _textFormatted || value == null)
                {
                    _textFormatted = value;
                    _text = null;
                    _javaText = _textFormatted;
                }
            }
        }

        string _text;
        public string Text
        {
            get => _text;
            set
            {
                if (value != _text || value == null)
                {
                    _text = value;
                    _textFormatted = null;
                    if (value != null)
                        _javaText = new Java.Lang.String(_text);
                    else
                        _javaText = null;
                }
            }
        }

        ICharSequence _javaText;
        public ICharSequence JavaText => _javaText;

        public bool IsEmpty => (_textFormatted == null || _textFormatted.Length() == 0) && string.IsNullOrEmpty(_text);
        #endregion


        #region Fields
        public Android.Graphics.Typeface Typeface;

        public Xamarin.Forms.Color TextColor = Xamarin.Forms.Color.Default;

        public int AvailWidth;
        public int AvailHeight;

        public float TextSize = -1;
        public int Lines = (int)Label.LinesProperty.DefaultValue;
        public AutoFit AutoFit = (AutoFit)Label.AutoFitProperty.DefaultValue;
        public LineBreakMode LineBreakMode = (LineBreakMode)Label.LineBreakModeProperty.DefaultValue;
        public float SyncFontSize = -1;
        public float RenderedFontSize = -1;
        public string ElementHtmlText;
        public string ElementText;
        public float LineHeight = -1;
        #endregion


        #region Constructors
        public TextControlState() { }

        public TextControlState(TextControlState source)
        {
            _textFormatted = source._textFormatted;
            _text = source._text;
            _javaText = source._javaText;
            Typeface = source.Typeface;
            TextColor = source.TextColor;
            AvailWidth = source.AvailWidth;
            AvailHeight = source.AvailHeight;
            TextSize = source.TextSize;
            Lines = source.Lines;
            AutoFit = source.AutoFit;
            LineBreakMode = source.LineBreakMode;
            SyncFontSize = source.SyncFontSize;
            RenderedFontSize = source.RenderedFontSize;
            ElementHtmlText = source.ElementHtmlText;
            ElementText = source.ElementText;
            LineHeight = source.LineHeight;
        }
        #endregion


        public override string ToString()
        {
            var result = "{ ";
            result += ElementHtmlText ?? ElementText;
            result += ", (" + AvailWidth + "," + AvailHeight + "), ";
            result += ", tsz:" + TextSize + ", ";
            result += ", fit:" + AutoFit + ", ";
            result += ", brk:" + LineBreakMode;
            return result;
        }


        #region Equality Methods
        public override bool Equals(object obj)
        {
            var other = obj as TextControlState;
            if (other == null)
                return false;
            return this == other;
        }

        public bool Equals(TextControlState other)
        {
            return other != null &&
                   EqualityComparer<ICharSequence>.Default.Equals(_textFormatted, other._textFormatted);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_textFormatted);
        }

        public static bool operator ==(TextControlState a, TextControlState b)
        {

            if (a is null && b is null)
                return true;
            if (a is null || b is null)
                return false;

            return (System.Math.Abs(a.TextSize - b.TextSize) < 0.1)
                && a.ElementText == b.ElementText
                && a.ElementHtmlText == b.ElementHtmlText
                && a.Lines == b.Lines
                && a.AutoFit == b.AutoFit
                && a.LineBreakMode == b.LineBreakMode
                && a._javaText == b._javaText
                && a.Typeface == b.Typeface
                && System.Math.Abs(a.LineHeight - b.LineHeight) < 0.01;
            /*
            if (System.Math.Abs(a.TextSize - b.TextSize) > 0.1)
            {
                if (a._javaText.ToString().StartsWith("Lateral"))
                    System.Diagnostics.Debug.WriteLine("TextControlState" + ".== TextSize a.[" + a.TextSize + "] b.[" + b.TextSize + "]");
                return false;
            }
            if (a.Lines != b.Lines)
            {
                if (a._javaText.ToString().StartsWith("Lateral"))
                    System.Diagnostics.Debug.WriteLine("TextControlState" + ".== Lines a.[" + a.Lines + "] b.[" + b.Lines + "]");
                return false;
            }
            if (a.AutoFit != b.AutoFit)
            {
                if (a._javaText.ToString().StartsWith("Lateral"))
                    System.Diagnostics.Debug.WriteLine("TextControlState" + ".== AutoFit a.[" + a.AutoFit + "] b.[" + b.AutoFit + "]");
                return false;
            }
            if (a.LineBreakMode != b.LineBreakMode)
            {
                if (a._javaText.ToString().StartsWith("Lateral"))
                    System.Diagnostics.Debug.WriteLine("TextControlState" + ".== LineBreakMode a.[" + a.LineBreakMode + "] b.[" + b.LineBreakMode + "]");
                return false;
            }
            if (a._javaText != b._javaText)
            {
                if (a._javaText.ToString().StartsWith("Lateral"))
                    System.Diagnostics.Debug.WriteLine("TextControlState" + ".== _javaText a.[" + a._javaText + "] b.[" + b._javaText + "]");
                return false;
            }
            if (a.Typeface != b.Typeface)
            {
                if (a._javaText.ToString().StartsWith("Lateral"))
                    System.Diagnostics.Debug.WriteLine("TextControlState" + ".== Typeface a.[" + a.Typeface + "] b.[" + b.Typeface + "]");
                return false;
            }
            return true;
            */
        }

        public static bool operator !=(TextControlState a, TextControlState b)
            => !(a == b);

        #endregion
    }
}
