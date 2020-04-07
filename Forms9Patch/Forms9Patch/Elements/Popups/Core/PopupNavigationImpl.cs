using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Forms9Patch.Elements.Popups.Core
{
    internal class PopupNavigationImpl : IPopupNavigation
    {
        readonly object _locker = new object();

        private readonly List<PopupPage> _popupStack = new List<PopupPage>();

        IPopupPlatform _popupPlatform;
        private IPopupPlatform PopupPlatform => _popupPlatform = _popupPlatform ?? DependencyService.Get<IPopupPlatform>();

        public IReadOnlyList<PopupPage> PopupStack => _popupStack;

        public event EventHandler<NavigationEventArgs> Pushed;

        public event EventHandler<NavigationEventArgs> Popped;

        public event EventHandler<AllPagesPoppedEventArgs> PoppedAll;

        public PopupNavigationImpl()
            => PopupPlatform.OnInitialized += OnInitialized;


        private async void OnInitialized(object sender, EventArgs e)
        {
            if (PopupStack.Any())
                await PopAllAsync(false);
        }

        public Task PushAsync(PopupPage page, bool animate = true)
        {
            lock (_locker)
            {
                if (_popupStack.Contains(page))
                    return Task.CompletedTask;

                _popupStack.Add(page);

                var task = InvokeThreadSafe(async () =>
                {
                    animate = CanBeAnimated(animate);

                    if (animate)
                    {
                        page.PreparingAnimation();
                        await AddAsync(page);
                        await page.AppearingAnimation();
                    }
                    else
                        await AddAsync(page);

                    page.AppearingTransactionTask = null;
                });
                page.AppearingTransactionTask = task;
                Pushed?.Invoke(this, new NavigationEventArgs(page));
                return task;
            }
        }

        public Task PopAsync(bool animate = true)
        {
            lock (_locker)
            {
                animate = CanBeAnimated(animate);
                if (!PopupStack.Any())
                    return Task.CompletedTask;

                return RemovePageAsync(PopupStack.Last(), animate);
            }
        }

        public Task PopAllAsync(bool animate = true)
        {
            lock (_locker)
            {
                animate = CanBeAnimated(animate);
                if (!PopupStack.Any())
                    return Task.CompletedTask;
                var popupPages = PopupStack.ToList();
                var popupTasks = popupPages.Select(page => RemovePageAsync(page, animate));
                return Task.WhenAll(popupTasks).ContinueWith((Task continuationAction) => PoppedAll?.Invoke(this, new AllPagesPoppedEventArgs(popupPages)));
            }
        }

        public Task RemovePageAsync(PopupPage page, bool animate = true)
        {
            lock (_locker)
            {
                if (page == null)
                    return Task.CompletedTask;

                if (!_popupStack.Contains(page))
                    return Task.CompletedTask;

                if (page.DisappearingTransactionTask != null)
                    return page.DisappearingTransactionTask;

                var task = InvokeThreadSafe(async () =>
                {
                    if (page.AppearingTransactionTask != null)
                        await page.AppearingTransactionTask;

                    lock (_locker)
                    {
                        if (!_popupStack.Contains(page))
                            return;
                    }

                    animate = CanBeAnimated(animate);

                    if (animate)
                        await page.DisappearingAnimation();

                    await RemoveAsync(page);

                    if (animate)
                        page.DisposingAnimation();

                    lock (_locker)
                    {
                        _popupStack.Remove(page);
                        page.DisappearingTransactionTask = null;
                    }
                });

                page.DisappearingTransactionTask = task;
                Popped?.Invoke(this, new NavigationEventArgs(page));
                return task;
            }
        }

        // Private

        private async Task AddAsync(PopupPage page)
            => await PopupPlatform.AddAsync(page);


        private async Task RemoveAsync(PopupPage page)
            => await PopupPlatform.RemoveAsync(page);


        // Internal 

        internal void RemovePopupFromStack(PopupPage page)
        {
            if (_popupStack.Contains(page))
                _popupStack.Remove(page);
        }

        #region Animation

        private bool CanBeAnimated(bool animate)
            => animate && PopupPlatform.IsSystemAnimationEnabled;


        #endregion

        #region Helpers

        Task InvokeThreadSafe(Func<Task> action)
        {
            var tcs = new TaskCompletionSource<bool>();

            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    await action.Invoke();
                    tcs.SetResult(true);
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            });

            return tcs.Task;
        }

        #endregion
    }
}
