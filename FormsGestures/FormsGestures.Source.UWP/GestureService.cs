using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormsGestures;

[assembly: Xamarin.Forms.Dependency(typeof(FormsGestures.UWP.GestureService))]
namespace FormsGestures.UWP
{
    public class GestureService : IGestureService
    {
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
