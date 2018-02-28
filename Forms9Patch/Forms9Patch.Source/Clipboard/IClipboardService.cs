using System;

namespace Forms9Patch
{
    interface IClipboardService
    {
        ClipboardEntry Entry { get; set; }

        bool EntryCaching { get; set; }

        //bool EntryItemTypeCaching { get; set; }
    }
}