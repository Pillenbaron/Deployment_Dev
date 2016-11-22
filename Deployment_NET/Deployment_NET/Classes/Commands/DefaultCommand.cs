using System;
using System.Windows.Input;

namespace awinta.Deployment_NET.Commands
{
    public class DefaultCommand : ICommand
    {

        private Action executeDelegate = null;

        public event EventHandler CanExecuteChanged;

        public DefaultCommand(Action execute)
        {

            executeDelegate = execute;

        }

        public bool CanExecute(object parameter)
        {

            return true;

        }

        public void Execute(object parameter)
        {

            executeDelegate();

        }

    }
}
