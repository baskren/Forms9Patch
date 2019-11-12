using System.Collections.Generic;
using System.Threading.Tasks;
using Forms9Patch.Elements.Popups.Core.Animations.Base;
using Xamarin.Forms;

namespace Forms9Patch.Elements.Popups.Core.Animations
{
    /// <summary>
    /// Used for sweep animation of popups
    /// </summary>
    public class MoveAnimation : FadeBackgroundAnimation
    {
        private double _defaultTranslationX;
        private double _defaultTranslationY;

        /// <summary>
        /// Start position for appearing
        /// </summary>
        public MoveAnimationOptions PositionIn { get; set; }
        /// <summary>
        /// End position for disappearing
        /// </summary>
        public MoveAnimationOptions PositionOut { get; set; }

        /// <summary>
        /// The move animation for popups
        /// </summary>
        public MoveAnimation(): this(MoveAnimationOptions.Bottom, MoveAnimationOptions.Bottom) {}

        /// <summary>
        /// The move animation for popups
        /// </summary>
        /// <param name="positionIn"></param>
        /// <param name="positionOut"></param>
        public MoveAnimation(MoveAnimationOptions positionIn, MoveAnimationOptions positionOut)
        {
            PositionIn = positionIn;
            PositionOut = positionOut;

            DurationIn = DurationOut = 300;
            EasingIn = Easing.SinOut;
            EasingOut = Easing.SinIn;
        }

        /// <summary>
        /// Called before Popup appears 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="page"></param>
        public override void Preparing(View content, PopupPage page)
        {
            base.Preparing(content, page);

            HidePage(page);

            if(content == null) return;

            UpdateDefaultTranslations(content);
        }

        /// <summary>
        /// Called after the Popup disappears
        /// </summary>
        /// <param name="content"></param>
        /// <param name="page"></param>
        public override void Disposing(View content, PopupPage page)
        {
            base.Disposing(content, page);

            ShowPage(page);

            if (content == null) return;

            content.TranslationX = _defaultTranslationX;
            content.TranslationY = _defaultTranslationY;
        }

        /// <summary>
        /// Called when animating the popup's appearance
        /// </summary>
        /// <param name="content"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async override Task Appearing(View content, PopupPage page)
        {
            var taskList = new List<Task>
            {
                base.Appearing(content, page)
            };

            if (content != null)
            {
                var topOffset = GetTopOffset(content, page);
                var leftOffset = GetLeftOffset(content, page);

                if (PositionIn == MoveAnimationOptions.Top)
                {
                    content.TranslationY = -topOffset;
                }
                else if (PositionIn == MoveAnimationOptions.Bottom)
                {
                    content.TranslationY = topOffset;
                }
                else if (PositionIn == MoveAnimationOptions.Left)
                {
                    content.TranslationX = -leftOffset;
                }
                else if (PositionIn == MoveAnimationOptions.Right)
                {
                    content.TranslationX = leftOffset;
                }

                taskList.Add(content.TranslateTo(_defaultTranslationX, _defaultTranslationY, DurationIn, EasingIn));
            }

            ShowPage(page);

            await Task.WhenAll(taskList);
        }

        /// <summary>
        /// Called when animating the popup's disappearance
        /// </summary>
        /// <param name="content"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async override Task Disappearing(View content, PopupPage page)
        {
            var taskList = new List<Task>
            {
                base.Disappearing(content, page)
            };

            if (content != null)
            {
                UpdateDefaultTranslations(content);

                var topOffset = GetTopOffset(content, page);
                var leftOffset = GetLeftOffset(content, page);

                if (PositionOut == MoveAnimationOptions.Top)
                {
                    taskList.Add(content.TranslateTo(_defaultTranslationX, -topOffset, DurationOut, EasingOut));
                }
                else if (PositionOut == MoveAnimationOptions.Bottom)
                {
                    taskList.Add(content.TranslateTo(_defaultTranslationX, topOffset, DurationOut, EasingOut));
                }
                else if (PositionOut == MoveAnimationOptions.Left)
                {
                    taskList.Add(content.TranslateTo(-leftOffset, _defaultTranslationY, DurationOut, EasingOut));
                }
                else if (PositionOut == MoveAnimationOptions.Right)
                {
                    taskList.Add(content.TranslateTo(leftOffset, _defaultTranslationY, DurationOut, EasingOut));
                }
            }

            await Task.WhenAll(taskList);
        }

        private void UpdateDefaultTranslations(View content)
        {
            _defaultTranslationX = content.TranslationX;
            _defaultTranslationY = content.TranslationY;
        }
    }
}
