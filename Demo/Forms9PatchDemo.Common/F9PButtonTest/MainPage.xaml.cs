using System.Collections.ObjectModel;
using F9PButtonTest.Model;
using F9PButtonTest.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace F9PButtonTest
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public static InfoMsg IMsg = new InfoMsg();

        public MainPage()
        {
            InitializeComponent();

            BindingContext = IMsg;

            var pages = new ObservableCollection<ContentView>
            {
                new Buttons1(),
                new Buttons2(),
            };
            Carousel.ItemsSource = pages;
        }
    }
}
