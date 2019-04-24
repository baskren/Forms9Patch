using SkiaSharp;

namespace Forms9Patch
{
    /// <summary>
    /// Making using Skia just a little easier
    /// </summary>
    public static class SKRectExtensions
    {
        /// <summary>
        /// Returns X origin (left) or SKRect
        /// </summary>
        /// <param name="skRect"></param>
        /// <returns></returns>
        public static double X(this SKRect skRect) { return skRect.Left; }

        /// <summary>
        /// Returns Y origin (top) of SKRect
        /// </summary>
        /// <param name="skRect"></param>
        /// <returns></returns>
        public static double Y(this SKRect skRect) { return skRect.Top; }

        /// <summary>
        /// Enables use of doubles with Skia
        /// </summary>
        /// <param name="path"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void MoveTo(this SKPath path, double x, double y) => path.MoveTo((float)x, (float)y);

        /// <summary>
        /// Enables use of doubles with Skia
        /// </summary>
        /// <param name="path"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void LineTo(this SKPath path, double x, double y) => path.LineTo((float)x, (float)y);
    }
}
