using System;

namespace FormsGestures
{
	/// <summary>
	/// FormsGestures Rotate event arguments.
	/// </summary>
	public class RotateEventArgs : BaseGestureEventArgs {
		
        /// <summary>
        /// Current angle of gesture (degrees)
        /// </summary>
		public virtual double Angle { get; protected set; }

        /// <summary>
        /// Change in angle since last gesture event (in degrees)
        /// </summary>
		public virtual double DeltaAngle { get; protected set; }

        /// <summary>
        /// Total change in angle since start of gesture (in degrees)
        /// </summary>
		public virtual double TotalAngle { get; protected set; }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="source"></param>
        /// <param name="newListener"></param>
		public RotateEventArgs(RotateEventArgs source=null, Listener newListener=null) : base(source, newListener) {
			if (source != null) {
				Angle = source.Angle;
				DeltaAngle = source.DeltaAngle;
				TotalAngle = source.TotalAngle;
			}
		}

        /// <summary>
        /// helper function used to calculate rotation angles
        /// </summary>
        /// <param name="previous"></param>
		protected void CalculateAngles(RotateEventArgs previous) {
			if (Touches.Length > 1)
				Angle = GetAngle();
			else if (previous != null)
				Angle = previous.Angle;
			else
				Angle = 0.0;
			if (previous == null) {
				DeltaAngle = 0.0;
				TotalAngle = 0.0;
				return;
			}
			if (Touches.Length != previous.Touches.Length) {
				DeltaAngle = 0.0;
				TotalAngle = previous.TotalAngle;
				return;
			}
			DeltaAngle = Angle - previous.Angle;
			DeltaAngle += (DeltaAngle > 180) ? -360 : (DeltaAngle < -180) ? +360 : 0;
			TotalAngle = previous.TotalAngle + DeltaAngle;
		}

		double GetAngle() {
			double x = Touches[1].X - Touches[0].X;
			double y = Touches[1].Y - Touches[0].Y;
			double num = Math.Atan2(y, x);
			return num * 180.0 / 3.1415926535897931;
		}

		internal RotateEventArgs Diff(RotateEventArgs lastArgs) {
			return new RotateEventArgs {
				Cancelled = Cancelled,
				Handled = Handled,
				ViewPosition = ViewPosition,
				Touches = Touches,
				Listener = Listener,
				Angle = Angle,
				TotalAngle = TotalAngle,
				DeltaAngle = Angle - lastArgs.Angle
			};
		}

        /// <summary>
        /// Updates properties from the values from another instance
        /// </summary>
        /// <param name="source"></param>
        public void ValueFrom(RotateEventArgs source)
        {
            base.ValueFrom(source);
            Angle = source.Angle;
            DeltaAngle = source.DeltaAngle;
            TotalAngle = source.TotalAngle;
        }
	}
}
