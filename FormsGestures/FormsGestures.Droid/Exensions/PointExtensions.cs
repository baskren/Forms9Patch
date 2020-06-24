// /*******************************************************************
//  *
//  * AndroidPointExtensions.cs copyright 2016 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using Xamarin.Forms;

namespace FormsGestures.Droid
{
    public static class PointExtensions
    {
        public static readonly float Density = Droid.Settings.Context.Resources.DisplayMetrics.Density;

        public static Android.Graphics.Point ToNativePoint(this Point p)
            => new Android.Graphics.Point((int)p.X, (int)p.Y);
        
    }
}
