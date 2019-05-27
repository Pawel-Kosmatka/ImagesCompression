using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImagesCompression.Core
{
    class CommandHandler : ICommand
    {
        private Func<Task> action;
        private Func<bool> canExecute;

        public CommandHandler(Func<Task> action, Func<bool>canExecute)
        {
            this.action = action;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        public bool CanExecute(object parameter)
        {
            return canExecute();
        }

        public void Execute(object parameter)
        {
            action();
            CanExecute(parameter);
        }
    }
}
