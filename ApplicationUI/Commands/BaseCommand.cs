using System.Windows.Input;

namespace ApplicationUI.Commands
{
    public class BaseCommand : ICommand
    {
        private readonly Action<object> _action;
        private readonly Func<object, bool> _canExecute;
        public BaseCommand(Action<object> action, Func<object, bool> canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }
        public BaseCommand(Action<object> action)
        {
            _action = action;
            _canExecute = new Func<object, bool>(x => true);
        }
        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
        public bool CanExecute(object? parameter)
        {
            return _canExecute?.Invoke(parameter) ?? false;
        }

        public void Execute(object? parameter)
        {
            _action.Invoke(parameter);
        }
    }
}
