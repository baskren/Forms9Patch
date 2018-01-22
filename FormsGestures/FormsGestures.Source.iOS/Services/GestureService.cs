using System;
using Xamarin.Forms;
using FormsGestures;

[assembly: Dependency (typeof(FormsGestures.iOS.GestureService))]
namespace FormsGestures.iOS
{
	public class GestureService : IGestureService
	{
		#region IGestureService implementation

		public void For (Listener listener) {
			NativeGestureHandler.GetInstanceForListener (listener);
		}

		#endregion

	}
}

