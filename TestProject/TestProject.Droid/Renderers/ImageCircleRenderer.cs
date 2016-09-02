using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using TestProject;
using TestProject.Droid;
using Android.Graphics;

[assembly: ExportRenderer(typeof(ImageCircle),typeof(ImageCircleRenderer))]
namespace TestProject.Droid
{
	public class ImageCircleRenderer : ImageRenderer
	{

		protected override void OnElementChanged (ElementChangedEventArgs<Image> e)
		{
			base.OnElementChanged (e);
			if (e.OldElement == null) {
				if ((int)Android.OS.Build.VERSION.SdkInt < 18)
					SetLayerType (Android.Views.LayerType.Software, null);
			}
		}

		protected override bool DrawChild(Canvas canvas, global::Android.Views.View child, long drawingTime) {
			try {
				var radius = Math.Min(Width, Height) / 2;
				var strokeWidth = 10;
				radius -= strokeWidth / 2;

				// Create path to clip
				var path = new Path();
				path.AddCircle(Width/2, Height/2, radius, Path.Direction.Ccw);
				canvas.Save();
				canvas.ClipPath(path);

				var result = base.DrawChild(canvas, child, drawingTime);

				canvas.Restore();

				// create path for circle border
				path = new Path();
				path.AddCircle(Width/2, Height/2, radius, Path.Direction.Ccw);

				var paint = new Paint();
				paint.AntiAlias = true;
				paint.StrokeWidth =  5;
				paint.SetStyle(Paint.Style.Stroke);
				paint.Color = global::Android.Graphics.Color.White;

				canvas.DrawPath(path, paint);

				// properly dispose
				paint.Dispose();
				path.Dispose();
				return result;
			
			} catch (Exception ex) {
				System.Diagnostics.Debug.WriteLine ("Unable to create circle image: " + ex);
			}
			return base.DrawChild (canvas, child, drawingTime);
		}

	}
}

