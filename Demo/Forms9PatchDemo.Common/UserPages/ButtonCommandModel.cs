using System.Runtime.CompilerServices;

namespace Forms9PatchDemo
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class ButtonCommandModel : Xamarin.Forms.BindableObject
    {
        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged();
                }
            }
        }

        public Xamarin.Forms.Command F9PButtonCommand => new Xamarin.Forms.Command(OnF9PButtonCommand);
        public Xamarin.Forms.Command RegularButtonCommand => new Xamarin.Forms.Command(OnRegularButtonCommand);

        public ButtonCommandModel()
        {
            Title = "Button?";
        }

        private void OnF9PButtonCommand()
        {
            Title = "F9P Button";
        }

        private void OnRegularButtonCommand()
        {
            Title = "Regular Button";
        }


        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
        }
    }
}
