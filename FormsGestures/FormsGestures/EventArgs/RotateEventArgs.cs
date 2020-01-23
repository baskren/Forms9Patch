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

		int direction = 1;

		/// <summary>
		/// helper function used to calculate rotation angles
		/// </summary>
		/// <param name="previous"></param>
		protected void CalculateAngles(RotateEventArgs previous) {

			if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.Android)
		    	direction *= -1;

			if (WindowTouches.Length > 1)
				Angle = GetAngle(WindowTouches);
			else if (previous != null)
				Angle = previous.Angle;
			else
				Angle = 0.0;
			if (previous == null) {
				DeltaAngle = 0.0;
				TotalAngle = 0.0;
				return;
			}
			if (WindowTouches.Length != previous.WindowTouches.Length) {
				DeltaAngle = 0.0;
				TotalAngle = previous.TotalAngle;
				return;
			}
			DeltaAngle = Angle - previous.Angle;
			DeltaAngle += (DeltaAngle > 180) ? -360 : (DeltaAngle < -180) ? +360 : 0;
			TotalAngle = previous.TotalAngle + DeltaAngle;
            //System.Diagnostics.Debug.WriteLine("detalAngle=["+DeltaAngle.ToString("N2")+"]");

        }

		double GetAngle(Xamarin.Forms.Point[] touches) {
            var x = touches[1].X - touches[0].X;
            var y = touches[1].Y - touches[0].Y;
            var num = Math.Atan2(y, x);
			var result = num * 180.0 / Math.PI;
            /*
			System.Diagnostics.Debug.WriteLine("[RotateEventArgs."
                + P42.Utils.ReflectionExtensions.CallerMemberName() + ":"
                + P42.Utils.ReflectionExtensions.CallerLineNumber()
                + "] result = " + result.ToString("N3") + " T0=["+Touches[0].ToString("N3")+"] T1=["+Touches[1].ToString("N3")+"]"  );
                */

            //System.Diagnostics.Debug.WriteLine("angle=["+result.ToString("N2")+"]");

            return result;
		}

		internal RotateEventArgs Diff(RotateEventArgs lastArgs) {
			return new RotateEventArgs {
				Cancelled = Cancelled,
				Handled = Handled,
				ElementPosition = ElementPosition,
				ElementTouches = ElementTouches,
                WindowTouches = WindowTouches,
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
