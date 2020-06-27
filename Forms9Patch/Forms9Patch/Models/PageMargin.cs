using System;
namespace Forms9Patch
{
    /// <summary>
    /// Class to specify ths margins (in points) of a page
    /// </summary>
    public class PageMargin 
    {
        public double Left { get; set; }
        public double Right { get; set; }
        public double Top { get; set; }
        public double Bottom { get; set; }

        public double HorizontalThickness => Left + Right;
        public double VerticalThickness => Top + Bottom;


        internal PageMargin()
        {
        }

        public PageMargin CreateInInches(double left, double right, double top, double bottom)
        {
            return new PageMargin
            {
                Left = left * 72,
                Right = right * 72,
                Top = top * 72,
                Bottom = bottom * 72
            };
        }

        public PageMargin CreateInInches(double horizontal, double vertical)
            => new PageMargin
            {
                Left = Right = horizontal * 72,
                Top = Bottom = vertical * 72
            };

        public PageMargin CreateInInches(double all)
            => new PageMargin
            {
                Left = Right = Top = Bottom = all * 72
            };

        public PageMargin CreateInMillimeters(double left, double right, double top, double bottom)
            => CreateInInches(left * 25.6, right * 25.4, top * 25.4, bottom * 25.4);

        public PageMargin CreateInMillimeters(double horizontal, double vertical)
            => CreateInInches(horizontal * 25.4, vertical * 25.4);

        public PageMargin CreateInMillimeters(double all)
            => CreateInInches(all * 25.4);
        }
}
