using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace awinta.Deployment_NET.UICommands
{
    class AsyncCommand : ICommand
    {

        private readonly Func<Task> executeDelegate;

        public event EventHandler CanExecuteChanged;

        public AsyncCommand(Func<Task> execute)
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
