using System;
using Android.Text.Style;
using Android.Graphics;
using Android.Text;

namespace Forms9Patch.Droid
{
	class BaselineSpan : MetricAffectingSpan
	{
        readonly float _ratio;

		public BaselineSpan (float ratio)
		{
			_ratio = ratio;
		}

		public override void UpdateDrawState (TextPaint tp) {
			Apply (tp);
		}

		public override void UpdateMeasureState (TextPaint p) {
			Apply (p);
		}

		void Apply(TextPaint paint) {
			paint.BaselineShift = (int)(_ratio * paint.Ascent ());
		}

	}
}

