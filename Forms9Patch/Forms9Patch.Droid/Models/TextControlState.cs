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

        public bool IsNullOrEmpty => (_textFormatted == null || _textFormatted.Length() == 0) && string.IsNullOrEmpty(_text);
        #endregion


        #region Fields
        public Android.Graphics.Typeface Typeface;

        public Xamarin.Forms.Color TextColor = Xamarin.Forms.Color.Default;

        public int AvailWidth;
        public int AvailHeight;

        public float TextSize;
        public int Lines = (int)Label.LinesProperty.DefaultValue;
        public AutoFit AutoFit = (AutoFit)Label.AutoFitProperty.DefaultValue;
        public LineBreakMode LineBreakMode = (LineBreakMode)Label.LineBreakModeProperty.DefaultValue;
        public float SyncFontSize;
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
        }
        #endregion


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
            if (a.AvailWidth != b.AvailWidth)
                return false;
            if (a.AvailHeight != b.AvailHeight)
                return false;
            if (System.Math.Abs(a.TextSize - b.TextSize) > 0.1)
                return false;
            if (a.Lines != b.Lines)
                return false;
            if (a.AutoFit != b.AutoFit)
                return false;
            if (a.LineBreakMode != b.LineBreakMode)
                return false;
            if (a._javaText != b._javaText)
                return false;
            if (a.Typeface != b.Typeface)
                return false;
            if (System.Math.Abs(a.SyncFontSize - b.SyncFontSize) > 0.1)
                return false;

            return true;
        }

        public static bool operator !=(TextControlState a, TextControlState b)
            => !(a == b);

        #endregion
    }
}
