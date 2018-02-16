using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace App3
{
    public class MessageTemplateSelector : DataTemplateSelector
    {
        public DataTemplate MessageTemplate { get; set; }
        public DataTemplate BetweenMessageTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return ((MessageListItem)item).MessageListType == MessageListType.Message? MessageTemplate : BetweenMessageTemplate;
        }
    }

    public class MessageListItem
    {
        private string _title;
        public MessageListType MessageListType { get; set; }

        public string Title
        {
            get
            {
                switch (MessageListType)
                {
                    case MessageListType.InBeween:
                    {
                        if (Received.Date == DateTime.Now.Date)
                        {
                            return "Today";
                        }

                        if (Received.Date.AddDays(1) == DateTime.Now.Date)
                        {
                            return "Yesterday";
                        }
                        var title = ReceivedFormatted;
                        return title;
                    }
                    default:
                    {
                        return _title;
                    }
                }
            }
            set
            {
                _title = value;

            }
        }
        public DateTime Received { get; set; }

        public string ReceivedFormatted
        {
            get
            {
                switch (MessageListType)
                {
                    case MessageListType.InBeween:
                    {
                        return Received.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern,
                            CultureInfo.CurrentCulture);
                    }
                    default:
                    {
                        return Received.ToString("hh:mm");
                    }
                }
            }
        }
        public string Body { get; set; }
        public Color BackgroundColor { get; set; }
        public Color TextColor { get; set; }
    }

    public enum MessageListType
    {
        Message,
        InBeween
    }
}
