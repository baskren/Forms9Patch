using System;

namespace Forms9Patch
{
    /// <summary>
    /// Forms9Patch Thickness extension.
    /// </summary>
    public static class ThicknessExtension
    {
        /// <summary>
        /// Description the specified thickness.
        /// </summary>
        /// <param name="thickness">Thickness.</param>
        public static string Description(this Xamarin.Forms.Thickness thickness)
        {
            return "[Thickness:" + thickness.Left + "," + thickness.Top + "," + thickness.Right + "," + thickness.Bottom + "]";
        }

        /// <summary>
        /// Is the Thickness empty?
        /// </summary>
        /// <param name="thickness"></param>
        /// <returns></returns>
        public static bool IsEmpty(this Xamarin.Forms.Thickness thickness)
        {
            if (thickness.Left == 0.0 && thickness.Top == 0.0 && thickness.Right == 0.0)
                return thickness.Bottom == 0.0;
            return false;
        }
    }
}

