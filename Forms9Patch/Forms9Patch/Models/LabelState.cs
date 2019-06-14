using Xamarin.Forms;

namespace Forms9Patch.UWP
{
    class LabelState
    {
        public TextAlignment HorizontalTextAlignment;
        public TextAlignment VerticalTextAlignment;
        public string FontFamily;
        public double FontSize = -1;
        public FontAttributes FontAttributes;
        public string Text;
        public LineBreakMode LineBreakMode;

        public string HtmlText;
        public AutoFit AutoFit = AutoFit.None;
        public int Lines;
        public double SynchronizedFontSize = -1;
        public double FittedFontSize=-1;


        public LabelState() { }

        public LabelState(Label label)
        {
            HorizontalTextAlignment = label.HorizontalTextAlignment;
            VerticalTextAlignment = label.VerticalTextAlignment;
            FontFamily = label.FontFamily;
            FontSize = label.FontSize;
            FontAttributes = label.FontAttributes;
            Text = label.Text;
            HtmlText = label.HtmlText;
            AutoFit = label.AutoFit;
            Lines = label.Lines;

            FittedFontSize = label.FittedFontSize;
            SynchronizedFontSize = label.SynchronizedFontSize;
        }

        public bool HasChanged(Label label)
        {
            return FontFamily != label.FontFamily
                || FontSize != label.FontSize
                || FontAttributes != label.FontAttributes
                || Text != label.Text
                || HtmlText != label.HtmlText
                || AutoFit != label.AutoFit
                || Lines != label.Lines
                || SynchronizedFontSize != label.SynchronizedFontSize;
        }

    }


}
