using System;
using Xamarin.Forms;
using System.Text;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms.Internals;

namespace Forms9PatchDemo
{
    #region Helper Classes

    #region Model Classes
    [Preserve(AllMembers = true)]
    public class Quote
    {
        public string QuoteText { get; set; }
    }

    [Preserve(AllMembers = true)]
    public class ExternalQuote : Quote
    {
    }

    [Preserve(AllMembers = true)]
    public class InternalQuote : Quote
    {
    }
    #endregion

    #region Cell Content Classes
    [Preserve(AllMembers = true)]
    class QuoteView : Grid
    {
        public readonly Forms9Patch.Image HeadShot = new Forms9Patch.Image
        {
            Fill = Forms9Patch.Fill.AspectFit,
            HeightRequest = 40,
            VerticalOptions = LayoutOptions.End,
            OutlineColor = Color.White,
            OutlineWidth = 3,
            ElementShape = Forms9Patch.ElementShape.Circle
        };

        public readonly Forms9Patch.Frame Bubble = new Forms9Patch.Frame();

        protected Forms9Patch.Label _label = new Forms9Patch.Label();

        public QuoteView()
        {

            Padding = new Thickness(5);

            Bubble.BackgroundImage = new Forms9Patch.Image();
            Bubble.Padding = 10;
            Bubble.Content = _label;

            RowDefinitions = new RowDefinitionCollection {
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = new GridLength(10) },
            };
            _label.TextColor = Color.White;
            _label.SetBinding(Label.TextProperty, "QuoteText");
        }
    }


    [Preserve(AllMembers = true)]
    class InternalQuoteView : QuoteView
    {
        public InternalQuoteView()
        {

            Bubble.BackgroundImage.Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.BubbleInternal");
            Bubble.Padding = new Thickness(10, 10, 17, 10);
            Bubble.HorizontalOptions = LayoutOptions.End;

            HeadShot.Source = Forms9Patch.ImageSource.FromResource("Forms9PatchDemo.Resources.236-lincoln.png");

            ColumnDefinitions = new ColumnDefinitionCollection {
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)  },
                new ColumnDefinition { Width = 40 },
            };

            Children.Add(Bubble, 0, 0);
            Children.Add(HeadShot, 1, 0);
            Grid.SetRowSpan(HeadShot, 2);

        }
    }

    [Preserve(AllMembers = true)]
    class ExternalQuoteView : QuoteView
    {
        public ExternalQuoteView()
        {

            Bubble.BackgroundImage.Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.BubbleExternal");
            Bubble.Padding = new Thickness(17, 10, 10, 10);
            Bubble.HorizontalOptions = LayoutOptions.Start;

            HeadShot.Source = Forms9Patch.ImageSource.FromResource("Forms9PatchDemo.Resources.236-baby.png");

            ColumnDefinitions = new ColumnDefinitionCollection {
                new ColumnDefinition { Width = 40 },
                new ColumnDefinition { Width = GridLength.Auto },
            };

            Children.Add(Bubble, 1, 0);
            Children.Add(HeadShot, 0, 0);
            Grid.SetRowSpan(HeadShot, 2);
        }
    }
    #endregion

    #region Cell Classes
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    class InternalQuoteViewCell : ViewCell
    {

        public InternalQuoteViewCell()
        {
            View = new InternalQuoteView();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            View.BindingContext = BindingContext;
        }
    }

    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    class ExternalQuoteViewCell : ViewCell
    {
        public ExternalQuoteViewCell()
        {
            View = new ExternalQuoteView();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            View.BindingContext = BindingContext;
        }
    }

    [Preserve(AllMembers = true)]
    class QuoteCellDataTemplateSelector : DataTemplateSelector
    {
        readonly DataTemplate internalTemplate;
        readonly DataTemplate externalTemplate;

        public QuoteCellDataTemplateSelector()
        {
            internalTemplate = new DataTemplate(typeof(InternalQuoteViewCell));
            externalTemplate = new DataTemplate(typeof(ExternalQuoteViewCell));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is InternalQuote)
                return internalTemplate;
            else if (item is ExternalQuote)
                return externalTemplate;
            else
                throw new InvalidCastException("Invalid applied to QuoteCellDataTemplateSelector");
        }
    }
    #endregion

    #endregion

    #region Content
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class ChatListPage : ContentPage
    {
        #region Model
        readonly ObservableCollection<Quote> thread = new ObservableCollection<Quote> {
            new InternalQuote { QuoteText = "I walk slowly, but I never walk backward." },
            new ExternalQuote { QuoteText = "I have never seen a thin person drinking Diet Coke." },
            new InternalQuote { QuoteText = "Whatever you are, be a good one." },
            new InternalQuote { QuoteText = "Always bear in mind that your own resolution to succeed is more important than any other." },
            new ExternalQuote { QuoteText = "President Obama - close down the flights from Ebola infected areas right now, before it is too late! What the hell is wrong with you?" },
            new InternalQuote { QuoteText = "Nearly all men can stand adversity, but if you want to test a man's character, give him power." },
            new InternalQuote { QuoteText = "I destroy my enemies when I make them my friends." },
            new ExternalQuote { QuoteText = "Wind turbines are ripping your country apart and killing tourism.  Electric bills in Scotland are skyrocketing-stop the madness" },
            new InternalQuote { QuoteText = "... for the people." },
            new InternalQuote { QuoteText = "America will never be destroyed from the outside. If we falter and lose our freedoms, it will be because we destroyed ourselves." },
            new InternalQuote { QuoteText = "A house divided against itself cannot stand." },
            new InternalQuote { QuoteText = "Those who deny freedom to others deserve it not for themselves." },
            new ExternalQuote { QuoteText = "Amazing how the haters & losers keep tweeting the name “F**kface Von Clownstick” like they are so original & like no one else is doing it..." },
            new ExternalQuote { QuoteText = "BenCarson is now leading in the polls in Iowa.  Too much Monsanto in the corn creates issues in the brain?" },
            new ExternalQuote { QuoteText = "I would like to extend my best wishes to all, even the haters and losers, on this special date, September 11th." },
            new InternalQuote { QuoteText = "Things may come to those who wait, but only the things left by those who hustle." },
            new InternalQuote { QuoteText = "Give me six hours to chop down a tree and I will spend the first four sharpening the axe." },
            new ExternalQuote { QuoteText = "Loser!" },
            new ExternalQuote { QuoteText = "My garndparents didn't come to America all the way from Germany just to see it get taken over by immigrants." },
            new InternalQuote { QuoteText = "Character is like a tree and reputation like a shadow.  The shadow is what we think of it; the tree is the real thing." },
            new ExternalQuote { QuoteText = "T-mobile service is terrible!  Why can't you do something to improve it for your customers.  I don't want it in my buildings." },
            new InternalQuote { QuoteText = "Be sure you put your feet in the right place, then stand firm." },
            new ExternalQuote { QuoteText = "Sorry losers and haters, but my I.Q. is one of the highest -and you all know it!  Please don't feel so stupid or insecure, it's not your fault." },
            new InternalQuote { QuoteText = "In the end, it's not the years in your life that count. It's the life in your years." },
        };

        public void SimulateLoadPrevious(int count)
        {
            var previousTopItem = thread[0];

            for (var z = 0; z < count; z++)
            {
                var random = new Random((int)DateTime.UtcNow.Ticks);

                var length = random.Next(10, 250);
                const string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
                var result = new StringBuilder(length);
                result.Append("[" + z + "] ");

                for (int i = 0; i < length; i++)
                    result.Append(characters[random.Next(characters.Length)]);

                Quote quote;
                var rand01 = random.Next(2);
                //System.Diagnostics.Debug.WriteLine ("rand01={0}", rand01);
                if (rand01 == 0)
                    quote = new ExternalQuote { QuoteText = result.ToString() };
                else
                    quote = new InternalQuote { QuoteText = result.ToString() };
                thread.Insert(0, quote);
                // counter act listview auto scrolling, ugly!
                _listView.ScrollTo(previousTopItem, ScrollToPosition.Start, false);
            }
        }
        #endregion


        readonly ListView _listView = new ListView
        {
            BackgroundColor = Color.Black,
            SeparatorColor = Color.Transparent,
            SeparatorVisibility = SeparatorVisibility.None,
            HasUnevenRows = true,

        };

        public ChatListPage()
        {
            _listView.ItemsSource = thread;
            _listView.ItemTemplate = new QuoteCellDataTemplateSelector();
            _listView.ItemAppearing += listView_ItemAppearing;

            BackgroundColor = Color.Black;
            Padding = new Thickness(0, 25, 0, 0);
            Content = new StackLayout
            {
                Children = {
                    new Label {
                        Text = "Forms9Patch.ContentView in dynamic view cell for ListView" ,
                        TextColor = Color.White
                    },
                    _listView,
                }
            };
        }

        bool _init = true;
        void listView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if (e.Item == thread[0] && _init)
            {
                _init = false;
                return;
            }
            if (e.Item == thread[0])
            {
                _init = true;
                SimulateLoadPrevious(20);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _listView.ScrollTo(thread.Last(), ScrollToPosition.End, false);
        }
    }
    #endregion
}




