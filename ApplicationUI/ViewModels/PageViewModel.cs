using ApplicationUI.Commands;
using ApplicationUI.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace ApplicationUI.ViewModels
{
    public class PageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private AllBooksPage _allBooksPage;
        private ReadBookPage _readBookPage;
        private LoginPage _loginPage;
        private SignupPage _signUpPage;
        private MyLibraryPage _myLibraryPage;

        private Page _currentPage;

        private bool _isLoggedIn;
        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set
            {
                if(_isLoggedIn != value)
                {
                    _isLoggedIn = value;
                    OnNotifyPropertyChanged(nameof(IsLoggedIn));
                }
            }
        }
        public void OnNotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public Page CurrentPage
        {
            get { return _currentPage; }
            set
            {
                _currentPage = value;
                OnNotifyPropertyChanged("CurrentPage");
            }
        }

        public PageViewModel(MainWindow mainWindow)
        {
            this.CurrentPage = _loginPage;
        }
        public ICommand ShowLoginPage
        {
            get
            {
                return new BaseCommand(obj => CurrentPage = _loginPage);
            }
        }
        public ICommand ShowSignupPage
        {
            get
            {
                return new BaseCommand(obj => CurrentPage = _signUpPage);
            }
        }
        public ICommand ShowAllBooksPage
        {
            get
            {
                return new BaseCommand(obj => CurrentPage = _allBooksPage);
            }
        }
        public ICommand ShowMyLibraryPage
        {
            get
            {
                return new BaseCommand(obj => CurrentPage = _myLibraryPage);
            }
        }
        public ICommand ShowReadBookPage
        {
            get
            {
                return new BaseCommand(obj => CurrentPage = _readBookPage);
            }
        }
    }
}
