using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Forms9PatchDemo
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class ListViewWithF9PLabelInCells : ContentPage
    {

        public ListViewWithF9PLabelInCells()
        {
            var listView = new ListView();

            listView.ItemTapped += (sender, e) => Forms9Patch.Toast.Create("ListView item tapped", "Content is [" + ((TestClass)e.Item).Content + "]");
            listView.ItemTemplate = new DataTemplate(typeof(TestClassCell));
            listView.ItemsSource = new List<TestClass>
            {
                new TestClass { BackgroundColor = Color.Aqua, TextColor=Color.Blue, Content="1 There are <a href=\"Apples 1\">Apples 1</a> for all." },
                new TestClass { BackgroundColor = Color.Fuchsia, TextColor=Color.Black, Content="2 Cheese" },
                new TestClass { BackgroundColor = Color.Blue, TextColor=Color.White, Content="3 There were <a href=\"Apples 2\">Apples 2</a> for all." },
                new TestClass { BackgroundColor = Color.Gray, TextColor=Color.Black, Content="4 Mango" },

                new TestClass { BackgroundColor = Color.Green, TextColor=Color.White, Content="5 There be <a href=\"Apples 3\">Apples 3</a> for all." },
                new TestClass { BackgroundColor = Color.Lime, TextColor=Color.Black, Content="6 Socks" },
                new TestClass { BackgroundColor = Color.Maroon, TextColor=Color.White, Content="7 There not <a href=\"Apples 4\">Apples 4</a> for all." },
                new TestClass { BackgroundColor = Color.Navy, TextColor=Color.White, Content="8 Pennies" },
                new TestClass { BackgroundColor = Color.Olive, TextColor=Color.White, Content="9 There ham <a href=\"Apples 5\">Apples 5</a> for all." },
                new TestClass { BackgroundColor = Color.Orange, TextColor=Color.Black, Content="10 Cherries" },
                new TestClass { BackgroundColor = Color.Pink, TextColor=Color.Blue, Content="11 There small <a href=\"Apples 6\">Apples 6</a> for all." },
                new TestClass { BackgroundColor = Color.Purple, TextColor=Color.White, Content="12 Lemons" },
                new TestClass { BackgroundColor = Color.Red, TextColor=Color.Blue, Content="13 chicken <a href=\"Apples 7\">Apples 7</a> for all." },
                new TestClass { BackgroundColor = Color.Silver, TextColor=Color.Black, Content="14 Zebras" },
                new TestClass { BackgroundColor = Color.Teal, TextColor=Color.Black, Content="15 Zipper <a href=\"Apples 8\">Apples 8</a> for all." },
                new TestClass { BackgroundColor = Color.White, TextColor=Color.Black, Content="16 Chips" },
                new TestClass { BackgroundColor = Color.Yellow, TextColor=Color.Black, Content="17 Be <a href=\"Apples 9\">Apples 9</a> for all." },
                new TestClass { BackgroundColor = Color.Black, TextColor=Color.White, Content="18 Salsa" },
            };

            Content = listView;
        }

        [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
        public class TestClass
        {
            public Color BackgroundColor { get; set; }
            public Xamarin.Forms.ImageSource StatusIcon { get; set; }
            public string Content { get; set; }
            public Color TextColor { get; set; }

        }

        [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
        public class TestClassCell : ViewCell
        {

            readonly Xamarin.Forms.Image _statusIcon = new Xamarin.Forms.Image
            {
                VerticalOptions = LayoutOptions.Start,
                Margin = new Thickness(0, 3, 0, 0)
            };

            readonly Forms9Patch.Label _label = new Forms9Patch.Label
            {
                HorizontalOptions = LayoutOptions.Start
            };

            Xamarin.Forms.StackLayout _stackLayout = new Xamarin.Forms.StackLayout
            {
                Orientation = StackOrientation.Horizontal,
            };

            Forms9Patch.Frame _frame = new Forms9Patch.Frame
            {
                OutlineColor = Color.Gray,
                OutlineWidth = 1,
                OutlineRadius = 4,
                Padding = new Thickness(8, 12, 8, 0),
                Margin = new Thickness(5, 3, 5, 6),
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };

            public TestClassCell()
            {

                _label.ActionTagTapped += (sender, e) => Forms9Patch.Toast.Create(null, e.Href);
                _stackLayout.Children.Add(_statusIcon);
                _stackLayout.Children.Add(_label);
                _frame.Content = _stackLayout;
                View = _frame;

                _label.SetBinding(Forms9Patch.Label.HtmlTextProperty, "Content");
                _label.SetBinding(Forms9Patch.Label.TextColorProperty, "TextColor");
                _statusIcon.SetBinding(Xamarin.Forms.Image.SourceProperty, "StatusIcon");
                _frame.SetBinding(Forms9Patch.Frame.BackgroundColorProperty, "BackgroundColor");
            }
        }
    }
}

