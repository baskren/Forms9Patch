using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace App3
{
    public class ViewModel
    {
        private readonly ObservableCollection<MessageListItem> _messageList = new ObservableCollection<MessageListItem>();

        public ObservableCollection<MessageListItem> Messages
        {
            get { return _messageList; }
        }

        public ViewModel()
        {
            DateTime.TryParse("3/14/2016", out DateTime testDate);


            Messages.Add(new MessageListItem()
            {
                MessageListType = MessageListType.InBeween,
                Received = testDate, //DateTime.Now.AddDays(-3),
                BackgroundColor = Color.FromHex("#3686A9"),
                TextColor = Color.White
            });
            Messages.Add(new MessageListItem()
            {
                MessageListType = MessageListType.Message,
                Received = DateTime.Now.AddDays(-3),
                Title = "You have a problem",
                Body = "<font color='white'>Lets try this<a href='http://tweakers.net'>Tweakers</a></font>",
                BackgroundColor = Color.Wheat,
                TextColor = Color.Black
            });
            Messages.Add(new MessageListItem()
            {
                MessageListType = MessageListType.Message,
                Received = DateTime.Now.AddDays(-3),
                Title = "You have a problem",
                Body = @"Xamarin.Forms Bla Bla  Bla Bla  Bla Bla ",
                BackgroundColor = Color.White,
                TextColor = Color.Black
            });
            Messages.Add(new MessageListItem()
            {
                MessageListType = MessageListType.Message,
                Received = DateTime.Now.AddDays(-3),
                Title = "You have a problem",
                Body = @"Xamarin.Forms Bla Bla ",
                BackgroundColor = Color.Green,
                TextColor = Color.Red
            });
            Messages.Add(new MessageListItem()
            {
                MessageListType = MessageListType.Message,
                Received = DateTime.Now.AddDays(-3),
                Title = "You have a problem",
                Body = @"Xamarin.Forms Bla Bla  Bla Bla  Bla Bla  Bla Bla  Bla Bla  Bla Bla  Bla Bla ",
                BackgroundColor = Color.Orange,
                TextColor = Color.DarkGreen
            });
            Messages.Add(new MessageListItem()
            {
                MessageListType = MessageListType.InBeween,
                Received = DateTime.Now.AddDays(-2),
                BackgroundColor = Color.FromHex("#3686A9"),
                TextColor = Color.White
            });
            Messages.Add(new MessageListItem()
            {
                MessageListType = MessageListType.Message,
                Received = DateTime.Now.AddDays(-2),
                Title = "You have a problem",
                Body = @"Xamarin.Forms Bla Bla "
                ,
                BackgroundColor = Color.Orange,
                TextColor = Color.DarkGreen
            });
            Messages.Add(new MessageListItem()
            {
                MessageListType = MessageListType.Message,
                Received = DateTime.Now.AddDays(-2),
                Title = "You have a problem",
                Body = @"Xamarin.Forms Bla Bla  Bla Bla "
                ,
                BackgroundColor = Color.Orange,
                TextColor = Color.DarkGreen
            });
            Messages.Add(new MessageListItem()
            {
                MessageListType = MessageListType.Message,
                Received = DateTime.Now.AddDays(-2),
                Title = "You have a problem",
                Body = @"Xamarin.Forms Bla Bla  Bla Bla  Bla Bla  Bla Bla "
                ,
                BackgroundColor = Color.Orange,
                TextColor = Color.DarkGreen
            });
            Messages.Add(new MessageListItem()
            {
                MessageListType = MessageListType.InBeween,
                Received = DateTime.Now.AddDays(-1),
                Title = "Yesterday",
                BackgroundColor = Color.FromHex("#3686A9"),
                TextColor = Color.White

            });
            Messages.Add(new MessageListItem()
            {
                MessageListType = MessageListType.Message,
                Received = DateTime.Now.AddDays(-1),
                Title = "You have a problem",
                Body = @"Xamarin.Forms Bla Bla "
                ,
                BackgroundColor = Color.Orange,
                TextColor = Color.DarkGreen
            });

            Messages.Add(new MessageListItem()
            {
                MessageListType = MessageListType.InBeween,
                Received = DateTime.Now.AddDays(-1),
                Title = "Today",
                BackgroundColor = Color.FromHex("#3686A9"),
                TextColor = Color.White
            });

            Messages.Add(new MessageListItem()
            {
                MessageListType = MessageListType.Message,
                Received = DateTime.Now,
                Title = "You have a problem",
                Body = @"Xamarin.Forms Bla Bla  Bla Bla  Bla Bla  Bla Bla  Bla Bla "
                ,
                BackgroundColor = Color.Orange,
                TextColor = Color.DarkGreen
            });
            Messages.Add(new MessageListItem()
            {
                MessageListType = MessageListType.Message,
                Received = DateTime.Now,
                Title = "You have a problem",
                Body = @"<font color='white'>Xamarin.Forms</font>"
                ,
                BackgroundColor = Color.Orange,
                TextColor = Color.DarkGreen
            });

        }
    }



}
