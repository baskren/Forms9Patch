using System;

namespace Forms9Patch
{
    interface IClipboardService
    {
        IClipboardEntry Entry { get; set; }

        bool EntryCaching { get; set; }

        //bool EntryItemTypeCaching { get; set; }
    }
}