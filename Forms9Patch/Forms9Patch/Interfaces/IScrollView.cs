using System;
using Xamarin.Forms;

namespace Forms9Patch
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    interface IScrollView
    {
        bool ScrollBy(double delta, bool animated);
        bool ScrollTo(double offset, bool animated);
        double ScrollOffset { get; }
        double HeaderHeight { get; }
        bool IsScrollEnabled { get; set; }
    }
}