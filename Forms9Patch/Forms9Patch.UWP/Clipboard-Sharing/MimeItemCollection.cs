using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using Windows.Storage;
using Windows.ApplicationModel.DataTransfer;

namespace Forms9Patch.UWP
{
    class MimeItemCollection : BaseMimeItemCollection
    {
        #region Static Implementation
        #endregion


        #region Properties
        public override string Description => base.Description;

        readonly string _plainText;
        public override string PlainText => _plainText;

        readonly string _htmlText;
        public override string HtmlText => _htmlText;

        readonly string _rtfText;

        public override List<IMimeItem> Items
        {
            get
            {
                try
                {

                    if (_items != null)
                        return _items;

                    _items = new List<IMimeItem>();

                    if (_plainText != null)
                        _items.Add(new ReturnKeyValueMimeItem("text/plain", _plainText));
                    if (_htmlText != null)
                        _items.Add(new ReturnKeyValueMimeItem("text/html", _htmlText));
                    if (_rtfText != null)
                        _items.Add(new ReturnKeyValueMimeItem("text/rtf", _rtfText));

                    var dataPackageView = Windows.ApplicationModel.DataTransfer.Clipboard.GetContent();

                    var availableFormats = dataPackageView?.AvailableFormats?.ToList() ?? new List<string>();
                    for (int i = availableFormats.Count - 1; i >= 0; i--)
                    {
                        foreach (var property in dataPackageView.Properties)
                        {
                            var key = property.Key;
                            var value = property.Value;
                            if (!MimeItem.ValidValue(value))
                                continue;
                            var item = new ReturnKeyValueMimeItem(key, value);
                            _items.Add(item);
                            if (!string.IsNullOrWhiteSpace(key) && availableFormats.Contains(key))
                                availableFormats.Remove(key);
                        }
                    }

                    if (dataPackageView?.Contains(Windows.ApplicationModel.DataTransfer.StandardDataFormats.StorageItems) ?? false)
                    {
                        if (AsyncHelper.RunSync(() => dataPackageView.GetStorageItemsAsync().AsTask()) is IReadOnlyList<IStorageItem> storageItems)
                        foreach (var storageItem in storageItems)
                        {
                            if (storageItem is StorageFile storageFile)
                            {
                                var item = new StorageFileReturnMimeItem(storageFile);
                                _items.Add(item);
                                var key = item.MimeType;
                                if (!string.IsNullOrWhiteSpace(key) && availableFormats.Contains(key))
                                    availableFormats.Remove(key);
                            }
                        }
                    }

                    return _items;
                }
                catch (Exception e)
                {
                    Forms9Patch.Debug.RequestUserHelp(e);
                }
                return new List<IMimeItem>();
            }
        }
        #endregion


        #region Constructor
        public MimeItemCollection()
        {
            if (Windows.ApplicationModel.DataTransfer.Clipboard.GetContent() is Windows.ApplicationModel.DataTransfer.DataPackageView dataPackageView)
            {
                if (dataPackageView.Contains(StandardDataFormats.Text))
                    _plainText = AsyncHelper.RunSync(() => dataPackageView.GetTextAsync().AsTask());
                if (dataPackageView.Contains(StandardDataFormats.Html))
                {
                    _htmlText = AsyncHelper.RunSync(() => dataPackageView.GetHtmlFormatAsync().AsTask());
                    if (!string.IsNullOrWhiteSpace(_htmlText) && _htmlText.StartsWith("Version:"))
                    {
                        //var header = _htmlText.Substring(0, 144);
                            // we're going to assume this is an html fragment
                            var startFragmentIndex = _htmlText.IndexOf("StartFragment:");  //14
                            var endFragmentIndex = _htmlText.IndexOf("EndFragment:");  // 12
                            if (startFragmentIndex > 0 && endFragmentIndex > 0)
                            {
                                var startIndexText = _htmlText.Substring(startFragmentIndex + 14, 10);
                                var endIndexText = _htmlText.Substring(endFragmentIndex + 12, 10);
                                if (Int32.TryParse(startIndexText, out int startIndex) && Int32.TryParse(endIndexText, out int endIndex))
                                    _htmlText = _htmlText.Substring(startIndex, endIndex - startIndex);
                            }
                            else
                            {
                                var startHtmlIndex = _htmlText.IndexOf("StartHTML:");  //10
                                var endHtmlIndex = _htmlText.IndexOf("EndHTML:");  // 8
                                if (startHtmlIndex > 0 && endHtmlIndex > 0)
                                {
                                    var startIndexText = _htmlText.Substring(startHtmlIndex + 14, 10);
                                    var endIndexText = _htmlText.Substring(endHtmlIndex + 12, 10);
                                    if (Int32.TryParse(startIndexText, out int startIndex) && Int32.TryParse(endIndexText, out int endIndex))
                                        _htmlText = _htmlText.Substring(startIndex, endIndex - startIndex);
                                }
                            }
                    }
                }
                if (dataPackageView.Contains(StandardDataFormats.Rtf))
                    _rtfText = AsyncHelper.RunSync(() => dataPackageView.GetRtfAsync().AsTask());
            }
        }
        #endregion
    }
}