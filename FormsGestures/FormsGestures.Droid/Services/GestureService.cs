using FormsGestures;

[assembly: Xamarin.Forms.Dependency(typeof(FormsGestures.Droid.GestureService))]
namespace FormsGestures.Droid
{
    /// <summary>
    /// FormsGesture.Droid Gesture service.
    /// </summary>
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class GestureService : IGestureService
    {
        public GestureService()
        {
            Settings.Init();
        }


        #region IGestureService implementation

        public void For(Listener listener)
        {
            NativeGestureHandler.ActivateInstanceForListener(listener);
        }

        /*
        public void Cancel()
        {
            NativeGestureListener.Cancel();
        }
        */
        #endregion

    }
}

