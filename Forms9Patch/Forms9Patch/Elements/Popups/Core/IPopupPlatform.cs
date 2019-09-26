using System;
using System.Threading.Tasks;

namespace Forms9Patch.Elements.Popups.Core
{
    internal interface IPopupPlatform
    {
        event EventHandler OnInitialized;

        bool IsInitialized { get; }

        bool IsSystemAnimationEnabled { get; }

        Task AddAsync(PopupPage page);

        Task RemoveAsync(PopupPage page);
    }
}
