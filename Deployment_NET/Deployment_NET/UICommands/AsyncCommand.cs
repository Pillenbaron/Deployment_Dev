using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace awinta.Deployment_NET.UICommands
{
    internal class AsyncCommand : ICommand
    {
        private readonly Func<Task> executeDelegate;

        public AsyncCommand(Func<Task> execute)
        {
            executeDelegate = execute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if (parameter is bool)
                return !(bool) parameter;

            return true;
        }

        public void Execute(object parameter)
        {
            executeDelegate();
        }
    }
}