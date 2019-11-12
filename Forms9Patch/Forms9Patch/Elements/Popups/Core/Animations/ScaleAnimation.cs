using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Forms9Patch.Elements.Popups.Core.Animations
{
    /// <summary>
    /// Popup scale animation
    /// </summary>
    public class ScaleAnimation : FadeAnimation
    {
        private double _defaultScale;
        private double _defaultOpacity;
        private double _defaultTranslationX;
        private double _defaultTranslationY;

        /// <summary>
        /// Scale at beginning of appearing 
        /// </summary>
        public double ScaleIn { get; set; } = 0.8;
        /// <summary>
        /// Scale at end of disappearing 
        /// </summary>
        public double ScaleOut { get; set; } = 0.8;

        /// <summary>
        /// Position at beginning of appearing 
        /// </summary>
        public MoveAnimationOptions PositionIn { get; set; }
        /// <summary>
        /// Position at end of disappearing
        /// </summary>
        public MoveAnimationOptions PositionOut { get; set; }

        /// <summary>
        /// Popup scale animation
        /// </summary>
        public ScaleAnimation():this(MoveAnimationOptions.Center, MoveAnimationOptions.Center) {}

        /// <summary>
        /// Popup scale animation
        /// </summary>
        /// <param name="positionIn"></param>
        /// <param name="positionOut"></param>
        public ScaleAnimation(MoveAnimationOptions positionIn, MoveAnimationOptions positionOut)
        {
            PositionIn = positionIn;
            PositionOut = positionOut;
            EasingIn = Easing.SinOut;
            EasingOut = Easing.SinIn;

            if (PositionIn != MoveAnimationOptions.Center) DurationIn = 500;
            if (PositionOut != MoveAnimationOptions.Center) DurationOut = 500;
        }

        /// <summary>
        /// Called before Popup appears 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="page"></param>
        public override void Preparing(View content, PopupPage page)
        {
            if(HasBackgroundAnimation) base.Preparing(content, page);

            HidePage(page);

            if(content == null) return;

            UpdateDefaultProperties(content);

            if(!HasBackgroundAnimation) content.Opacity = 0;
        }

        /// <summary>
        /// Called after the Popup disappears
        /// </summary>
        /// <param name="content"></param>
        /// <param name="page"></param>
        public override void Disposing(View content, PopupPage page)
        {
            if (HasBackgroundAnimation) base.Disposing(content, page);

            ShowPage(page);

            if(content == null) return;

            content.Scale = _defaultScale;
            content.Opacity = _defaultOpacity;
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
                var topOffset = GetTopOffset(content, page) * ScaleIn;
                var leftOffset = GetLeftOffset(content, page) * ScaleIn;

                taskList.Add(Scale(content, EasingIn, ScaleIn, _defaultScale, true));

                if (PositionIn == MoveAnimationOptions.Top)
                {
                    content.TranslationY = -topOffset;
                    taskList.Add(content.TranslateTo(_defaultTranslationX, _defaultTranslationY, DurationIn, EasingIn));
                }
                else if (PositionIn == MoveAnimationOptions.Bottom)
                {
                    content.TranslationY = topOffset;
                    taskList.Add(content.TranslateTo(_defaultTranslationX, _defaultTranslationY, DurationIn, EasingIn));
                }
                else if (PositionIn == MoveAnimationOptions.Left)
                {
                    content.TranslationX = -leftOffset;
                    taskList.Add(content.TranslateTo(_defaultTranslationX, _defaultTranslationY, DurationIn, EasingIn));
                }
                else if (PositionIn == MoveAnimationOptions.Right)
                {
                    content.TranslationX = leftOffset;
                    taskList.Add(content.TranslateTo(_defaultTranslationX, _defaultTranslationY, DurationIn, EasingIn));
                }
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
                UpdateDefaultProperties(content);

                var topOffset = GetTopOffset(content, page) * ScaleOut;
                var leftOffset = GetLeftOffset(content, page) * ScaleOut;

                taskList.Add(Scale(content, EasingOut, _defaultScale, ScaleOut, false));

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

        private Task Scale(View content, Easing easing, double start, double end, bool isAppearing)
        {
            TaskCompletionSource<bool> task = new TaskCompletionSource<bool>();
            
            content.Animate("popIn", d =>
            {
                content.Scale = d;
            }, start, end,
            easing: easing,
            length: isAppearing ? DurationIn : DurationOut,
            finished: (d, b) =>
            {
                task.SetResult(true);
            });
            return task.Task;
        }

        private void UpdateDefaultProperties(View content)
        {
            _defaultScale = content.Scale;
            _defaultOpacity = content.Opacity;
            _defaultTranslationX = content.TranslationX;
            _defaultTranslationY = content.TranslationY;
        }
    }
}
