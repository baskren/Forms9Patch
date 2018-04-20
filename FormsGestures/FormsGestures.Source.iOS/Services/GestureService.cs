using System;
using Xamarin.Forms;
using FormsGestures;

[assembly: Dependency(typeof(FormsGestures.iOS.GestureService))]
namespace FormsGestures.iOS
{
    public class GestureService : IGestureService
    {
        public GestureService()
        {
            Settings.Init();
        }

        public void Cancel()
        {
            NativeGestureHandler.Cancel();
        }


        #region IGestureService implementation

        public void For(Listener listener)
        {
            NativeGestureHandler.GetInstanceForListener(listener);
        }

        #endregion

    }
}

