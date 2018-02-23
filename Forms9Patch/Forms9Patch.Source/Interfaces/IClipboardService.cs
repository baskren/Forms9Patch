using System;

namespace Forms9Patch
{
    interface IClipboardService
    {
        ClipboardEntry Entry { get; set; }
    }
}