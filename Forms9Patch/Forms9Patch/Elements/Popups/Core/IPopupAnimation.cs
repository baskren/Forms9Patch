using System.Threading.Tasks;
using Forms9Patch.Elements.Popups.Core;
using Xamarin.Forms;

namespace Forms9Patch.Elements.Popups.Core
{
    public interface IPopupAnimation
    {
        void Preparing(View content, PopupPage page);
        void Disposing(View content, PopupPage page);
        Task Appearing(View content, PopupPage page);
        Task Disappearing(View content, PopupPage page);
    }
}
