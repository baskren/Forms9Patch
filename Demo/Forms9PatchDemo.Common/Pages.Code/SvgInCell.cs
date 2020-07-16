using System;
using Xamarin.Forms;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Forms9PatchDemo
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class SvgInCell : ContentPage
    {
        #region VisualElements
        readonly ListView _listView = new ListView
        {
            ItemTemplate = new DataTemplate(typeof(SvgInCellViewCell))
        };
        readonly Forms9Patch.SegmentedControl _segmentControl = new Forms9Patch.SegmentedControl
        {
            Margin = 5,
            Segments =
            {
                new Forms9Patch.Segment("PNG"),
                new Forms9Patch.Segment("SVG"),
                new Forms9Patch.Segment("JPG")
            },
            GroupToggleBehavior = Forms9Patch.GroupToggleBehavior.Radio,
        };
        readonly Grid _grid = new Grid
        {
            RowSpacing = 0,
            RowDefinitions =
            {
                new RowDefinition{ Height = 40 },
                new RowDefinition{ Height = GridLength.Star }
            }
        };
        #endregion

        #region Constructor
        public SvgInCell()
        {
            _grid.Children.Add(_segmentControl);
            _grid.Children.Add(_listView, 0, 1);
            _segmentControl.SegmentSelected += OnSegmentSelected;
            _segmentControl.SelectIndex(1);
            Content = _grid;
            _listView.ItemSelected += (sender, e) =>
            {
                if (e.SelectedItem != null)
                {
                    System.Diagnostics.Debug.WriteLine("e.SelectedItem: " + e.SelectedItem);
                    Navigation.PushAsync(new EmbeddedResourceImagePage { BindingContext = e.SelectedItem });
                    _listView.SelectedItem = null;
                }
            };
        }
        #endregion

        #region VisualElement Event Handlers
        private void OnSegmentSelected(object sender, Forms9Patch.SegmentedControlEventArgs e)
            => UpdateItemsSource();

        #endregion

        #region Layout
        void UpdateItemsSource()
        {
            if (_segmentControl.SelectedSegments.FirstOrDefault() is Forms9Patch.Segment segment)
            {
                var suffix = segment.HtmlText ?? segment.Text;
                var assembly = GetType().Assembly;
                var embeddedResources = assembly.GetManifestResourceNames();
                var itemsSource = embeddedResources.Where(er => er.EndsWith(suffix, StringComparison.OrdinalIgnoreCase));
                _listView.ItemsSource = itemsSource;
            }
        }
        #endregion
    }

    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class ImageWrapper : ContentView
    {
        #region Source property
        public static readonly BindableProperty SourceProperty = BindableProperty.Create("Source", typeof(Xamarin.Forms.ImageSource), typeof(ImageWrapper), default(Xamarin.Forms.ImageSource));
        public Xamarin.Forms.ImageSource Source
        {
            get => (Xamarin.Forms.ImageSource)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }
        #endregion Source property

        readonly Forms9Patch.Image _image = new Forms9Patch.Image
        {
            Margin = 5,
            Fill = Forms9Patch.Fill.AspectFit
        };

        public ImageWrapper()
        {
            Content = _image;
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == SourceProperty.PropertyName)
            {
                _image.Source = Source;
            }
        }
    }

    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class SvgInCellViewCell : ViewCell
    {
        #region VisualElements
        /*
        readonly Forms9Patch.Image _image = new Forms9Patch.Image
        {
            Margin = 5,
            Fill = Forms9Patch.Fill.AspectFit
        };
        */
        readonly ImageWrapper _image = new ImageWrapper();
        readonly Forms9Patch.Label _label = new Forms9Patch.Label
        {
            Margin = 5,
            Lines = 1,
            AutoFit = Forms9Patch.AutoFit.Width,
            VerticalTextAlignment = TextAlignment.Center
        };
        readonly Grid _grid = new Grid
        {
            ColumnSpacing = 0,
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = 50 },
                new ColumnDefinition { Width = GridLength.Star },
            }
        };
        #endregion


        #region Constructor
        public SvgInCellViewCell()
        {
            _grid.Children.Add(_image);
            _grid.Children.Add(_label, 1, 0);
            View = _grid;
        }
        #endregion


        #region Manual Binding
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (BindingContext is string embeddedResourceId)
            {
                _image.Source = Forms9Patch.ImageSource.FromResource(embeddedResourceId);
                _label.Text = embeddedResourceId;
            }
        }
        #endregion
    }

    public class EmbeddedResourceImagePage : ContentPage
    {
        #region VisualElements
        readonly Forms9Patch.Image _image = new Forms9Patch.Image
        {
            Margin = 5,
            Fill = Forms9Patch.Fill.AspectFit
        };
        readonly Forms9Patch.Label _label = new Forms9Patch.Label
        {
            Margin = 5,
            Lines = 1,
            AutoFit = Forms9Patch.AutoFit.Width,
            VerticalTextAlignment = TextAlignment.Center
        };
        readonly Grid _grid = new Grid
        {
            RowSpacing = 0,
            RowDefinitions =
            {
                new RowDefinition { Height = 50 },
                new RowDefinition { Height = GridLength.Star },
            }
        };
        #endregion


        #region Constructor
        public EmbeddedResourceImagePage()
        {
            _grid.Children.Add(_label);
            _grid.Children.Add(_image, 0, 1);
            Content = _grid;
        }
        #endregion


        #region Manual Binding
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (BindingContext is string embeddedResourceId)
            {
                _image.Source = Forms9Patch.ImageSource.FromResource(embeddedResourceId);
                _label.Text = embeddedResourceId;
            }
        }
        #endregion
    }
}