using System.Threading.Tasks;
using Forms9Patch.Elements.Popups.Core.Animations.Base;
using Xamarin.Forms;

namespace Forms9Patch.Elements.Popups.Core.Animations
{
    /// <summary>
    /// Popup fade animation
    /// </summary>
    public class FadeAnimation : BaseAnimation
    {
        private double _defaultOpacity;

        /// <summary>
        /// Huh?
        /// </summary>
        public bool HasBackgroundAnimation { get; set; } = true;

        /// <summary>
        /// Called before Popup appears 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="page"></param>
        public override void Preparing(View content, PopupPage page)
        {
            if (HasBackgroundAnimation)
            {
                _defaultOpacity = page.Opacity;
                page.Opacity = 0;
            }
            else if(content != null)
            {
                _defaultOpacity = content.Opacity;
                content.Opacity = 0;
            }
        }

        /// <summary>
        /// Called after the Popup disappears
        /// </summary>
        /// <param name="content"></param>
        /// <param name="page"></param>
        public override void Disposing(View content, PopupPage page)
        {
            if (HasBackgroundAnimation)
            {
                page.Opacity = _defaultOpacity;
            }
            else if (content != null)
            {
                page.Opacity = _defaultOpacity;
            }
        }

        /// <summary>
        /// Called when animating the popup's appearance
        /// </summary>
        /// <param name="content"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public override Task Appearing(View content, PopupPage page)
        {
            if (HasBackgroundAnimation)
            {
                return page.FadeTo(1, DurationIn, EasingIn);
            }
            if (content != null)
            {
                return content.FadeTo(1, DurationIn, EasingIn);
            }

            return Task.FromResult(0);
        }

        /// <summary>
        /// Called when animating the popup's disappearance
        /// </summary>
        /// <param name="content"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public override Task Disappearing(View content, PopupPage page)
        {
            _defaultOpacity = page.Opacity;

            if (HasBackgroundAnimation)
            {
                return page.FadeTo(0, DurationOut, EasingOut);
            }
            if (content != null)
            {
                return content.FadeTo(0, DurationOut, EasingOut);
            }

            return Task.FromResult(0);
        }
    }
}
