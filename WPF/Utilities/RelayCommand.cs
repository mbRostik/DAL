using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPF.Utilities
{
    public class RelayCommand:ICommand
    {
        private readonly Action<object> _execute;

        private readonly Func<object, bool> _canexecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canexecute = null)
        {
            _execute = execute;
            _canexecute = canexecute;
        }

        public bool CanExecute(object parameter) => _canexecute ==null || _canexecute (parameter);

        public void Execute (object parameter) => _execute (parameter); 
    }
}
