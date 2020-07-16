using System.ComponentModel;
using Xamarin.Forms;


namespace Forms9PatchDemo
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public partial class ButtonCommand : ContentPage
    {
        public ButtonCommand()
        {
            InitializeComponent();
            BindingContext = new ButtonCommandModel();
        }
    }
}