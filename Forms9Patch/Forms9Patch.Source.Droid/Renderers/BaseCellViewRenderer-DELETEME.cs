using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using P42.Utils;

[assembly: ExportRenderer(typeof(Forms9Patch.BaseCellView), typeof(Forms9Patch.Droid.BaseCellViewRenderer))]
namespace Forms9Patch.Droid
{
    internal class BaseCellViewRenderer : VisualElementRenderer<Forms9Patch.BaseCellView>
    {
        List<EventHandler<VisualElementChangedEventArgs>> _elementChangedHandlers
        {
            get
            {
                var result = (List<EventHandler<VisualElementChangedEventArgs>>)P42.Utils.ReflectionExtensions.GetFieldValue(this, "_elementChangedHandlers");
                return result;
            }
        }


        protected override void OnElementChanged(ElementChangedEventArgs<BaseCellView> e)
        {
            var args = new VisualElementChangedEventArgs(e.OldElement, e.NewElement);

            //foreach (EventHandler<VisualElementChangedEventArgs> handler in _elementChangedHandlers)
            //    handler(this, args);
            for (int i = 0; i < _elementChangedHandlers.Count; i++)
                _elementChangedHandlers[i]?.Invoke(this, args);

        }
    }
}
