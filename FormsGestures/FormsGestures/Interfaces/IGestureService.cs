using Xamarin.Forms;

namespace FormsGestures
{
    /// <summary>
    /// Interface for FormsGestures' service.
    /// </summary>
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    interface IGestureService
    {
        void For(Listener handler);
        //void Cancel();
    }
}

