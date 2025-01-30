using ApplicationUI.Commands;
using ApplicationUI.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ApplicationUI.ViewModels
{
    public class LoginPageVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private PageViewModel _pageViewModel;
        public ICommand LoginCommand { get; }
        public void OnNotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public LoginPageVM(PageViewModel pageViewModel)
        {
            _pageViewModel = pageViewModel;
        }
    }
}
