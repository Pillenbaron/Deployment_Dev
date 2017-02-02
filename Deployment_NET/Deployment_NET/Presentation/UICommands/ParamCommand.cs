using System;
using System.Windows.Input;

namespace awinta.Deployment_NET.Presentation.UICommands
{
    public class ParamCommand<T> : ICommand
    {
        private readonly Action<T> executeDelegate;

        public ParamCommand(Action<T> execute)
        {
            executeDelegate = execute;
        }

        #region Implementation of ICommand

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            executeDelegate?.Invoke((T) parameter);
        }

        public event EventHandler CanExecuteChanged;

        #endregion
    }
}