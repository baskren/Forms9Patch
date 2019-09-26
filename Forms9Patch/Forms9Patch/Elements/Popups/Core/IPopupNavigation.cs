using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forms9Patch.Elements.Popups.Core
{
    public interface IPopupNavigation
    {
        IReadOnlyList<PopupPage> PopupStack { get; }

        Task PushAsync(PopupPage page, bool animate = true);

        Task PopAsync(bool animate = true);

        Task PopAllAsync(bool animate = true);

        Task RemovePageAsync(PopupPage page, bool animate = true);
    }
}
