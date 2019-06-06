using System;
using UIKit;
using Foundation;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Forms9Patch.iOS
{
    public class TextControlState : IEquatable<TextControlState>
    {
        #region Properties

        public UIFont Font
        {
            get
            {
                //System.Diagnostics.Debug.WriteLine(GetType() + ".Font  FontDescriptor=[" + FontDescriptor?.DebugDescription + "] FontPointSize=[" + FontPointSize + "]");
                var result = UIFont.FromDescriptor(FontDescriptor, FontPointSize);
                //System.Diagnostics.Debug.WriteLine(GetType() + ".Font=[" + result + "] ");
                return result;
            }
        }

        public nfloat FontPointSize { get; set; }
        public UIFontDescriptor FontDescriptor { get; set; }
        public UITextAlignment HorizontalTextAlignment { get; set; }
        public Xamarin.Forms.TextAlignment VerticalTextAlignment { get; set; }

        public AutoFit AutoFit { get; set; }
        public int Lines { get; set; }
        public UILineBreakMode LineBreakMode { get; set; } = UILineBreakMode.CharacterWrap;

        public double AvailWidth { get; set; }
        public double AvailHeight { get; set; }

        NSAttributedString _attributedString;
        public NSAttributedString AttributedString
        {
            get => _attributedString;
            set
            {
                if (value != _attributedString)
                {
                    _text = null;
                    _attributedString = value;
                }
            }
        }

        NSString _text;
        public NSString Text
        {
            get => _text;
            set
            {
                if (value != _text)
                {
                    _attributedString = null;
                    _text = value;
                }
            }
        }

        public bool IsNullOrEmpty => (AttributedString == null || AttributedString.Length == 0) && string.IsNullOrEmpty(_text);

        #endregion

        public TextControlState()
        {
            FontPointSize = UIFont.LabelFontSize;
            FontDescriptor = UIFont.SystemFontOfSize(UIFont.LabelFontSize).FontDescriptor;
            HorizontalTextAlignment = UITextAlignment.Left;
            VerticalTextAlignment = TextAlignment.Start;
            AutoFit = AutoFit.None;
            Lines = 0;
            LineBreakMode = UILineBreakMode.WordWrap;
        }

        public TextControlState(TextControlState other)
        {
            //Font = other.Font;
            FontPointSize = other.FontPointSize;
            FontDescriptor = other.FontDescriptor;
            HorizontalTextAlignment = other.HorizontalTextAlignment;
            VerticalTextAlignment = other.VerticalTextAlignment;

            AutoFit = other.AutoFit;
            Lines = other.Lines;
            LineBreakMode = other.LineBreakMode;

            _attributedString = other.AttributedString;
            _text = other.Text;

        }

        public override bool Equals(object obj)
        {
            return Equals(obj as TextControlState);
        }

        public bool Equals(TextControlState other)
        {
            return other != null &&
                   FontPointSize.Equals(other.FontPointSize) &&
                   EqualityComparer<UIFontDescriptor>.Default.Equals(FontDescriptor, other.FontDescriptor) &&
                   HorizontalTextAlignment == other.HorizontalTextAlignment &&
                   VerticalTextAlignment == other.VerticalTextAlignment &&
                   AutoFit == other.AutoFit &&
                   HorizontalTextAlignment == other.HorizontalTextAlignment &&
                   LineBreakMode == other.LineBreakMode &&
                   Math.Abs(AvailWidth - other.AvailWidth) < 0.1 &&
                   Math.Abs(AvailHeight - other.AvailHeight) < 0.1 &&
                   EqualityComparer<NSAttributedString>.Default.Equals(AttributedString, other.AttributedString) &&
                   EqualityComparer<NSString>.Default.Equals(Text, other.Text)
                   ;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FontPointSize, FontDescriptor, VerticalTextAlignment, AutoFit, LineBreakMode, Text, AttributedString);
        }

        public static bool operator ==(TextControlState left, TextControlState right)
        {
            return EqualityComparer<TextControlState>.Default.Equals(left, right);
        }

        public static bool operator !=(TextControlState left, TextControlState right)
        {
            return !(left == right);
        }
    }
}
