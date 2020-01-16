using System.ComponentModel;

namespace F9PButtonTest.Model
{
    public class InfoMsg : INotifyPropertyChanged
    {
        private string _msgText = "Press button, or slide...";

        public string MsgText
        {
            get => _msgText;
            set
            {
                _msgText = value;
                OnPropertyChanged(nameof(MsgText));
            }
        }

        // --------------------------------------------------------------------

        public event PropertyChangedEventHandler PropertyChanged;
        protected internal void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
