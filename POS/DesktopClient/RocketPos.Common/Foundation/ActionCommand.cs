using System;
using System.Windows.Input;

namespace RocketPos.Common.Foundation
{
    public class ActionCommand : ICommand
    {
        private readonly Action<Object> _action;
        private readonly Predicate<Object> _predicate;
        
        //For when you don't need a predicate, or a required condition
        public ActionCommand(Action<Object> action) : this(action, null)
        {
            _action = action;
        }

        public ActionCommand(Action<Object> action, Predicate<Object> predicate)
        {
            if (action == null)
            {
                throw new Exception("Common.Foundation - ActionCommand - You must specify an Action<T>");
            }

            _action = action;
            _predicate = predicate;
        }

        public bool CanExecute(object parameter)
        {
            if (_predicate == null)
                return true;

            return _predicate(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value;  }
        }

        public void Execute(object parameter)
        {
            _action(parameter);
        }

        public void Execute()
        {
            Execute(null);
        }
    }
}
