using System;
namespace Forms9Patch
{
    /// <summary>
    /// Class to specify ths margins (in points) of a PDF page
    /// </summary>
    public class PageMargin 
    {
        /// <summary>
        /// Left margin (points)
        /// </summary>
        public double Left { get; set; }

        /// <summary>
        /// Right margin (points)
        /// </summary>
        public double Right { get; set; }

        /// <summary>
        /// Top margin (points)
        /// </summary>
        public double Top { get; set; }

        /// <summary>
        /// Bottom margin (points)
        /// </summary>
        public double Bottom { get; set; }

        /// <summary>
        /// Combined left and right margins (points)
        /// </summary>
        public double HorizontalThickness => Left + Right;

        /// <summary>
        /// Combined top and bottom margins (points)
        /// </summary>
        public double VerticalThickness => Top + Bottom;


        internal PageMargin()
        {
        }

        /// <summary>
        /// Creates a new PageMargin from left, right, top, and bottom dimensions (in inches)
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="top"></param>
        /// <param name="bottom"></param>
        /// <returns></returns>
        public static PageMargin CreateInInches(double left, double right, double top, double bottom)
        {
            return new PageMargin
            {
                Left = left * 72,
                Right = right * 72,
                Top = top * 72,
                Bottom = bottom * 72
            };
        }

        /// <summary>
        /// Creates a new PageMargin from horizontal and vertical margin dimensions (in inches)
        /// </summary>
        /// <param name="horizontal"></param>
        /// <param name="vertical"></param>
        /// <returns></returns>
        public static PageMargin CreateInInches(double horizontal, double vertical)
            => new PageMargin
            {
                Left = horizontal * 72,
                Right = horizontal * 72,
                Top = vertical * 72,
                Bottom = vertical * 72
            };

        /// <summary>
        /// Creates a new PageMargin from uniform margin dimensions (in inches)
        /// </summary>
        /// <param name="all"></param>
        /// <returns></returns>
        public static PageMargin CreateInInches(double all)
            => new PageMargin
            {
                Left = all * 72,
                Right = all * 72,
                Top = all * 72,
                Bottom = all * 72
            };

        /// <summary>
        /// Creates a new PageMargin from left, right, top, and bottom dimensions (in millimeters)
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="top"></param>
        /// <param name="bottom"></param>
        /// <returns></returns>
        public static PageMargin CreateInMillimeters(double left, double right, double top, double bottom)
            => CreateInInches(left * 25.6, right * 25.4, top * 25.4, bottom * 25.4);

        /// <summary>
        /// Creates a new PageMargin from horizontal and vertical margin dimensions (in millimeters)
        /// </summary>
        /// <param name="horizontal"></param>
        /// <param name="vertical"></param>
        /// <returns></returns>
        public static PageMargin CreateInMillimeters(double horizontal, double vertical)
            => CreateInInches(horizontal * 25.4, vertical * 25.4);

        /// <summary>
        ///  Creates a new PageMargin from uniform margin dimensions (in millimeters)
        /// </summary>
        /// <param name="all"></param>
        /// <returns></returns>
        public static PageMargin CreateInMillimeters(double all)
            => CreateInInches(all * 25.4);
        }
}
