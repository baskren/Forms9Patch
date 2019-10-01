using System.Threading.Tasks;
using Forms9Patch.Elements.Popups.Core.Converters.TypeConverters;
using Xamarin.Forms;

namespace Forms9Patch.Elements.Popups.Core.Animations.Base
{
    /// <summary>
    /// Base class of Popup Animations
    /// </summary>
    public abstract class BaseAnimation : IPopupAnimation
    {
        private const uint DefaultDuration = 200;

        /// <summary>
        /// Duration of appearing animation
        /// </summary>
        [TypeConverter(typeof (UintTypeConverter))]
        public uint DurationIn { get; set; } = DefaultDuration;

        /// <summary>
        /// Duration of disappearing animation
        /// </summary>
        [TypeConverter(typeof (UintTypeConverter))]
        public uint DurationOut { get; set; } = DefaultDuration;

        /// <summary>
        /// Motion profile for appearing animation
        /// </summary>
        [TypeConverter(typeof(EasingTypeConverter))]
        public Easing EasingIn { get; set; } = Easing.Linear;

        /// <summary>
        /// Motion profile for disappearing animation
        /// </summary>
        [TypeConverter(typeof(EasingTypeConverter))]
        public Easing EasingOut { get; set; } = Easing.Linear;

        /// <summary>
        /// Called before Popup appears 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="page"></param>
        public abstract void Preparing(View content, PopupPage page);

        /// <summary>
        /// Called after the Popup disappears
        /// </summary>
        /// <param name="content"></param>
        /// <param name="page"></param>
        public abstract void Disposing(View content, PopupPage page);

        /// <summary>
        /// Called when animating the popup's appearance
        /// </summary>
        /// <param name="content"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public abstract Task Appearing(View content, PopupPage page);

        /// <summary>
        /// Called when animating the popup's disappearance
        /// </summary>
        /// <param name="content"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public abstract Task Disappearing(View content, PopupPage page);

        /// <summary>
        /// Called when calculating the top edge of the popup
        /// </summary>
        /// <param name="content"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        protected virtual int GetTopOffset(View content, Page page)
        {
            return (int)(content.Height + page.Height) / 2;
        }

        /// <summary>
        /// Called when calculating the left edge of the popup
        /// </summary>
        /// <param name="content"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        protected virtual int GetLeftOffset(View content, Page page)
        {
            return (int)(content.Width + page.Width) / 2;
        }

        /// <summary>
        /// Use this method for avoiding the problem with blinking animation on iOS
        /// See https://github.com/rotorgames/Rg.Plugins.Popup/issues/404
        /// </summary>
        /// <param name="page">Page.</param>
        protected virtual void HidePage(Page page)
        {
            page.IsVisible = false;
        }

        /// <summary>
        /// Use this method for avoiding the problem with blinking animation on iOS
        /// See https://github.com/rotorgames/Rg.Plugins.Popup/issues/404
        /// </summary>
        /// <param name="page">Page.</param>
        protected virtual void ShowPage(Page page)
        {
            //Fix: #404
            Device.BeginInvokeOnMainThread(() => page.IsVisible = true);
        }
    }
}
