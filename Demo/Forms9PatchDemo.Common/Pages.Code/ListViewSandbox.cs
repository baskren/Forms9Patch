using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Forms9PatchDemo
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class ListViewSandbox : Xamarin.Forms.ContentPage
    {
        readonly Button _button = new Button
        {
            Text = "Clear"
        };

        public ListViewSandbox()
        {
            var entry = Forms9Patch.Clipboard.Entry;
            var plain = entry.PlainText;
            /*
            var kids = new People
            {
                new Person("Jones","Abe", 10, "Forms9PatchDemo.Resources.kid1.jpeg"),
                new Person("Smith", "Bob", 11, "Forms9PatchDemo.Resources.kid2.jpeg"),
                new Person("Pista", "Chuck", 12, "Forms9PatchDemo.Resources.kid3.jpeg")
            };
            kids.Title = "Kids";

            var parents = new People
            {
                new Person("Jones","Elvis", 80, "Forms9PatchDemo.Resources.adult1.jpeg"),
                new Person("Smith", "Norris", 72, "Forms9PatchDemo.Resources.adult2.jpeg"),
                new Person("Pista", "Robert", 55, "Forms9PatchDemo.Resources.adult3.jpeg")
            };
            parents.Title = "Parents";

            var listsOfPeople = new ObservableCollection<object>
            {
                kids, parents
            };

            var listView = new Forms9Patch.ListView
            {
                ItemsSource = listsOfPeople,
                GroupHeaderTemplate = new Forms9Patch.GroupHeaderTemplate(typeof(PeopleGroupCell)),
                IsGroupingEnabled = true,
                GroupToggleBehavior = Forms9Patch.GroupToggleBehavior.None,
                VisibilityTest = (object obj) =>
                {
                    if (obj == null)
                        return false;
                    if (obj is Person person)
                        return person.IsVisible;
                    return true;
                },
                SeparatorColor = Color.Brown,

            };
            listView.ItemTemplates.Add(typeof(Person), typeof(PersonViewCell));
            */

            var list = new ObservableCollection<string>
            {
                "apples","peaches", "pears", "pizza"
            };

            var listView = new Forms9Patch.ListView
            {
                ItemsSource = list,
                IsGroupingEnabled = false,
            };

            Title = plain;
            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children =
                {
                    _button,
                    listView
                }
            };

            listView.ItemSelected += (s, e) =>
            {
                Debug.WriteLine("Item [" + e.SelectedItem + "] was selected");
                listView.SelectedItem = null;
                //kids.Remove(e.SelectedItem as Person);

            };

            _button.Clicked += async (sender, e) =>
            {
                list.Clear();
                await Task.Delay(100);
            };
        }


    }


    #region Person
    public class Person : BindableObject
    {
        public static readonly BindableProperty IsVisibleProperty = BindableProperty.Create("IsVisible", typeof(bool), typeof(Person), true);
        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }


        public string LastName
        {
            get;
            set;
        }

        public static BindableProperty FirstNameProperty = BindableProperty.Create("FirstName", typeof(string), typeof(Person), null, BindingMode.TwoWay);
        public string FirstName
        {
            get { return (string)GetValue(FirstNameProperty); }
            set { SetValue(FirstNameProperty, value); }
        }

        public int Age { get; set; }

        public Person(string lastName, string firstName, int age, string resourceId = null)
        {
            LastName = lastName;
            FirstName = firstName;
            Age = age;
            ResourceId = resourceId;
        }

        public string ResourceId
        {
            get; set;
        }

        public static BindableProperty IsActiveProperty = BindableProperty.Create("IsActive", typeof(bool), typeof(Person), default(bool), BindingMode.OneWay);
        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        public override string ToString()
        {
            return FirstName + " " + LastName;
        }


        public string DetailString
        {
            get
            {
                return FirstName + " is " + Age + " years old.";
            }
        }

        public Xamarin.Forms.ImageSource ImageSource
        {
            get => Forms9Patch.ImageSource.FromResource(ResourceId);
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == FirstNameProperty.PropertyName)
            {
                Debug.WriteLine("FirstName is now: " + FirstName);
                //LastName = FirstName;
            }
            else if (propertyName == IsActiveProperty.PropertyName)
                Debug.WriteLine(FirstName + " " + LastName + " IsActive: " + IsActive);

        }
    }
    #endregion

    #region People
    public class People : ObservableCollection<Person>, System.ComponentModel.INotifyPropertyChanged
    {
        bool _isVisible;
        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                foreach (var person in this)
                    if (person != null)
                        person.IsVisible = value;
            }
        }

        public string Title { get; set; }

        public People()
        {
            IsVisible = true;
        }

        public override string ToString()
        {
            return Title;
        }
    }
    #endregion

    #region PeopleViewModel
    /*
    public class PeopleViewModel : People
    {
        bool _isVisible;
        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                if (value != _isVisible)
                {
                    _isVisible = value;
                    OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("IsVisible"));
                }
            }
        }

        public People SourceModel { get; set; }

        public PeopleViewModel(People source)
        {
            SourceModel = source;
            Title = SourceModel.Title;
            Add(null);
            //foreach (var person in Source)
            //    Add(person);

        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.PropertyName == "IsVisible")
            {
                if (IsVisible)
                {
                    Clear();
                    foreach (var person in SourceModel)
                        Add(person);
                }
                else
                {
                    Clear();
                    Add(null);
                }
            }
        }
    }
    */
    #endregion

    #region PersonViewCell
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class PersonViewCell : Xamarin.Forms.Grid, Forms9Patch.ICellContentView
    {
        public double CellHeight
        {
            get;
            set;
        }

        #region Fields
        Label _firstNameLabel = new Label
        {
            VerticalOptions = LayoutOptions.Center,
            TextColor = Color.Gray
        };
        Label _lastNameLabel = new Label
        {
            VerticalOptions = LayoutOptions.Center,
            TextColor = Color.Black
        };
        Xamarin.Forms.Switch _isActiveSwitch = new Xamarin.Forms.Switch
        {
            VerticalOptions = LayoutOptions.Center
        };
        Image _mugShotImage = new Image();
        #endregion

        public PersonViewCell()
        {

            //var grid = new Grid
            //{
            RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition{ Height = new GridLength(1,GridUnitType.Star)},
                    new RowDefinition{ Height = new GridLength(1,GridUnitType.Star)}
            };
            ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition{ Width = new GridLength(50,GridUnitType.Absolute)},
                    new ColumnDefinition{Width = new GridLength(1,GridUnitType.Star)},
                    new ColumnDefinition{ Width = new GridLength(50, GridUnitType.Absolute)}
            };
            //};

            Children.Add(_isActiveSwitch, 0, 1, 0, 2);
            Children.Add(_lastNameLabel, 1, 0);
            Children.Add(_firstNameLabel, 1, 1);
            Children.Add(_mugShotImage, 2, 0);


            Margin = new Thickness(10, 0);
            RowSpacing = 0;
            ColumnSpacing = 10;

            Xamarin.Forms.Grid.SetRowSpan(_mugShotImage, 2);

            //View = grid;

            CellHeight = 10;

            _isActiveSwitch.IsVisible = false;

            _firstNameLabel.SetBinding(Xamarin.Forms.Label.TextProperty, Person.FirstNameProperty.PropertyName);
            _lastNameLabel.SetBinding(Xamarin.Forms.Label.TextProperty, "LastName");
            _mugShotImage.SetBinding(Xamarin.Forms.Image.SourceProperty, "ImageSource");
            _isActiveSwitch.SetBinding(Xamarin.Forms.Switch.IsToggledProperty, "IsActive");
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            _firstNameLabel.BindingContext = BindingContext;
            _lastNameLabel.BindingContext = BindingContext;
            _mugShotImage.BindingContext = BindingContext;
            _isActiveSwitch.BindingContext = BindingContext;

            CellHeight = BindingContext == null ? 10 : 60;
            _isActiveSwitch.IsVisible = BindingContext != null;
        }

        public void OnAppearing()
        {
            
        }

        public void OnDisappearing()
        {
            
        }
    }
    #endregion

    #region PeopleGroupCell
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class PeopleGroupCell : StackLayout, Forms9Patch.ICellContentView
    {
        #region Fields
        Label _titleLabel = new Label
        {
            HorizontalOptions = LayoutOptions.StartAndExpand,
            VerticalOptions = LayoutOptions.CenterAndExpand
        };
        Forms9Patch.Button _showHideButton = new Forms9Patch.Button
        {
            HorizontalOptions = LayoutOptions.End,
            VerticalOptions = LayoutOptions.CenterAndExpand,
            OutlineRadius = 4,
            BackgroundColor = Color.DarkGray,
            TextColor = Color.White,
        };
        #endregion

        public PeopleGroupCell()
        {
            Padding = new Thickness(10, 0);
            BackgroundColor = Color.BurlyWood;
            VerticalOptions = LayoutOptions.CenterAndExpand;
            Orientation = StackOrientation.Horizontal;
            Children.Add(_titleLabel);
            Children.Add(_showHideButton);
            _showHideButton.Clicked += (s, e) =>
            {
                if (BindingContext is People viewModel)
                {
                    viewModel.IsVisible = !viewModel.IsVisible;
                    _showHideButton.Text = viewModel.IsVisible ? "Hide" : "Show";
                }
            };
            HeightRequest = 40;
        }

        public double CellHeight => 40;

        public void OnAppearing()
        {
            
        }

        public void OnDisappearing()
        {
            
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext is People viewModel)
            {
                _titleLabel.Text = viewModel.Title;
                _showHideButton.Text = viewModel.IsVisible ? "Hide" : "Show";
            }
        }
    }
    #endregion
}