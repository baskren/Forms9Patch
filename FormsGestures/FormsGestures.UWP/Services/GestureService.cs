using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormsGestures;

[assembly: Xamarin.Forms.Dependency(typeof(FormsGestures.UWP.GestureService))]
namespace FormsGestures.UWP
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class GestureService : IGestureService
    {
        #region IGestureService implementation

        public void For(Listener listener)
        {
            NativeGestureHandler.GetInstanceForListener(listener);
        }

        #endregion
    }
}
