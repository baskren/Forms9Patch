using System;
using Xamarin.Forms;

namespace Forms9Patch
{
    interface IScrollView
    {
        bool ScrollBy(double delta, bool animated);
        bool ScrollTo(double offset, bool animated);
        double ScrollOffset { get; }
        double HeaderHeight { get; }
        bool IsScrollEnabled { get; set; }
    }
}