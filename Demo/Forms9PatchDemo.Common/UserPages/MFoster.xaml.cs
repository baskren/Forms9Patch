using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ButtonBindingSample
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new ViewModel();
            //BackgroundColor = Color.Red;
        }
    }

    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class ViewModel
    {
        public ViewModel()
        {

            CommandWithDisabledState = new DelegateCommand(
                () => { }, 
                () => { return _toggle; });

            ToggleCommand = new DelegateCommand(
                () => {
                    _toggle = !_toggle;
                    CommandWithDisabledState.RaiseCanExecuteChanged();
                });
        }

        private bool _toggle = false;

        public DelegateCommand CommandWithDisabledState { get; }
        public DelegateCommand ToggleCommand { get; }
        public string Text => "Hello world";
    }


    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class DelegateCommand : ICommand
    {
        /// <summary>
        /// The _execute
        /// </summary>
        private readonly Action _execute;

        /// <summary>
        /// The _can execute
        /// </summary>
        private readonly Func<bool> _canExecute;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecute">The can execute.</param>
        /// <exception cref="System.ArgumentNullException">execute</exception>
        public DelegateCommand(Action execute, Func<bool> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException("execute");

            if (canExecute != null)
            {
                this._canExecute = canExecute;
            }
        }

        /// <summary>
        /// Initializes a new instance of the DelegateCommand class that
        /// can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <exception cref="ArgumentNullException">If the execute argument is null.</exception>
        public DelegateCommand(Action execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Occurs when changes occur that affect whether the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;


        /// <summary>
        /// Raises the can execute changed.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {

            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute.Invoke();
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public virtual void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                _execute.Invoke();
            }
        }
    }

}
