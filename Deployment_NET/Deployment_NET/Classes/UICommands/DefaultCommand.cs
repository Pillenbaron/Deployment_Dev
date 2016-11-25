using System;
using System.Windows.Input;

namespace awinta.Deployment_NET.UICommands
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

            if (parameter is bool)
            {

                return !(bool)parameter;

            }

            return true;

        }

        public void Execute(object parameter)
        {

            executeDelegate();

        }

    }
}
