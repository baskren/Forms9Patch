using Android.Text.Style;
using Android.Graphics;

namespace Forms9Patch.Droid
{
	class CustomTypefaceSpan : MetricAffectingSpan
	{
		readonly Typeface _typeface;

		public CustomTypefaceSpan (Typeface typeface) {
			_typeface = typeface;
		}

		public override void UpdateDrawState (Android.Text.TextPaint tp) {
			Apply (tp);
		}

		public override void UpdateMeasureState (Android.Text.TextPaint p) {
			Apply (p);
		}

		void Apply(Paint paint) {
			Typeface oldTypeface = paint.Typeface;
			int oldStyle = (int)(oldTypeface != null ? oldTypeface.Style : 0);
			int fakeStyle = oldStyle & ~((int)_typeface.Style);

			if ((fakeStyle & (int)TypefaceStyle.Bold) != 0) {
				//paint.setFakeBoldText(true);
				paint.FakeBoldText = true;
			}

			if ((fakeStyle & (int)TypefaceStyle.Italic) != 0) {
				//paint.setTextSkewX(-0.25f);
				paint.TextSkewX = -0.25f;
			}

			//paint.setTypeface(typeface);
			paint.SetTypeface (_typeface);
		}



	}
}

