﻿using System;
using System.Windows.Input;

namespace awinta.Deployment_NET.Presentation.UICommands
{
    public class DefaultCommand : ICommand
    {
        private readonly Action executeDelegate;

        public DefaultCommand(Action execute)
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