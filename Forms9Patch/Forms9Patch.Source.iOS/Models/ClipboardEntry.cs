using System;
using System.Collections.Generic;
using Forms9Patch;
using UIKit;
using Foundation;
using System.Linq;
using MobileCoreServices;

namespace Forms9Patch.iOS
{
    class ClipboardEntry : BaseClipboardEntry
    {
        public override string Description => UIPasteboard.General?.Name;

        public override List<IMimeItem> Items
        {
            get
            {
                if (_items != null)
                    return _items;
                _items = new List<IMimeItem>();
                var uiPasteboardItems = UIPasteboard.General?.Items;
                if (uiPasteboardItems == null || uiPasteboardItems.Length < 1)
                    return _items = new List<IMimeItem>();

                foreach (var item in uiPasteboardItems)
                {
                    foreach (var kvp in item)
                    {
                        var returnMimeItem = LazyMimeItem.Parse(kvp);
                        if (returnMimeItem != null)
                            _items.Add(returnMimeItem);
                    }
                }
                return _items;
            }
        }

    }

}