using ApplicationUI.Commands;
using ApplicationUI.Pages;
using ApplicationUI.Statics;
using ApplicationUI.TempModels;
using BLL.Interfaces;
using BLL.ModelsDTO;
using BLL.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace ApplicationUI.ViewModels
{
    public class PageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private AllBooksPage _allBooksPage;
        private LoginPage _loginPage;
        private SignupPage _signUpPage;
        private MyLibraryPage _myLibraryPage;
        private MyProfilePage _myProfilePage;
        private NotificationPage _notificationPage;
        private SearchUserPage _searchUserPage;

        private AllBooksPageVM _allBooksPageVM;
        private LoginPageVM _loginPageVM;
        private SignupPageVM _signUpPageVM;
        private MyLibraryPageVM _myLibraryPageVM;
        private MyProfilePageVM _myProfilePageVM;
        private NotificationPageVM _notificationPageVM;
        private SearchUserPageVM _searchUserPageVM;

        private Page _currentPage;
        public IUserService<BookDTO, UserDTO, NotificationDTO> userService;
        public IBookService<BookDTO, ParagraphDTO, UserCommentDTO> bookService;

        public bool IsLoggedIn
        {
            get { return StaticUser.IsLoggedIn; }
            set
            {
                if (StaticUser.IsLoggedIn != value)
                {
                    StaticUser.IsLoggedIn = value;
                    _currentPage = _myLibraryPage;
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

        public PageViewModel(MainWindow mainWindow, IUserService<BookDTO, UserDTO, NotificationDTO> userService, IBookService<BookDTO, ParagraphDTO, UserCommentDTO> bookService,LoginPageVM loginPageVM,SignupPageVM signupPageVM,MyLibraryPageVM myLibraryPageVM,AllBooksPageVM allBooksPageVM,MyProfilePageVM myProfilePageVM, NotificationPageVM notificationPageVM,SearchUserPageVM searchUserPageVM)
        {
            this.userService = userService;
            this.bookService = bookService;

            this._loginPageVM = loginPageVM;
            this._signUpPageVM = signupPageVM;
            this._myLibraryPageVM = myLibraryPageVM;
            this._allBooksPageVM = allBooksPageVM;
            this._myProfilePageVM = myProfilePageVM;
            this._notificationPageVM = notificationPageVM;
            this._searchUserPageVM = searchUserPageVM;

            this._loginPage = new LoginPage(_loginPageVM);
            this._signUpPage = new SignupPage(_signUpPageVM);
            this._myLibraryPage = new MyLibraryPage(_myLibraryPageVM);
            this._allBooksPage = new AllBooksPage(_allBooksPageVM);
            this._myProfilePage = new MyProfilePage(_myProfilePageVM);
            this._notificationPage = new NotificationPage(_notificationPageVM);
            this._searchUserPage = new SearchUserPage(_searchUserPageVM);
            this.CurrentPage = _loginPage;
            RunWhileLoggin();
        }
        public ICommand ShowLoginPage
        {
            get
            {
                return new BaseCommand(obj => CurrentPage = _loginPage);
            }
        }
        private async Task RunWhileLoggin()
        {
            await Task.Run(() =>
            {
                while (StaticUser.IsLoggedIn == false || StaticUser.UserNeedsToSignUp == true)
                {
                    if (StaticUser.IsLoggedIn == true)
                    {
                        break;
                    }
                    else if (StaticUser.UserNeedsToSignUp == true)
                    {
                        this.CurrentPage = _signUpPage;
                    }
                    else if (StaticUser.UserNeedsToSignUp == false)
                    {
                        this.CurrentPage = _loginPage;
                    }

                }
                if (StaticUser.IsLoggedIn == true)
                {
                    this.CurrentPage = _myProfilePage;
                    OnNotifyPropertyChanged(nameof(IsLoggedIn));
                }
            });
        }
        public ICommand ShowAllBooksPage
        {
            get
            {
                return new BaseCommand(obj =>
                {
                    SoundPlayer.PlayButtonSound();
                    List<LibraryBook> books = new List<LibraryBook>();
                    foreach(var book in bookService.GetAll())
                    {
                        var tempBook = new LibraryBook()
                        {
                            Name = book.Name,
                            Author = book.Author,
                            CoverURL = book.CoverURL
                        };
                        books.Add(tempBook);
                    }
                    _allBooksPageVM.AvailableBooks = books;
                    _allBooksPageVM.OnNotifyPropertyChanged(nameof(_allBooksPageVM.AvailableBooks));
                    CurrentPage = _allBooksPage;
                });
            }
        }

        public ICommand ShowMyLibraryPage
        {
            get
            {
                return new BaseCommand(obj =>
                {
                    SoundPlayer.PlayButtonSound();
                    _myLibraryPageVM.UserBooks = userService.GetById(StaticUser.User.Id).Books;
                    _myLibraryPageVM.OnNotifyPropertyChanged("UserBooks");
                    CurrentPage = _myLibraryPage;
                });
            }
        }

        public ICommand ShowMyProfilePage
        {
            get
            {
                return new BaseCommand(obj =>
                {
                    SoundPlayer.PlayButtonSound();
                    CurrentPage = _myProfilePage;
                });
            }
        }

        public ICommand ShowNotificationPage
        {
            get
            {
                return new BaseCommand(obj =>
                {
                    SoundPlayer.PlayButtonSound();
                    _notificationPageVM.Show();
                    CurrentPage = _notificationPage;
                });
            }
        }

        public ICommand ShowLogOutPage
        {
            get
            {
                return new BaseCommand(obj =>
                {
                    SoundPlayer.PlayButtonSound();
                    IsLoggedIn = false;
                    _myLibraryPageVM.UserBooks = null;
                    _myLibraryPageVM.OnNotifyPropertyChanged(nameof(_myLibraryPageVM.UserBooks));
                    CurrentPage = _loginPage;
                    //OnNotifyPropertyChanged(nameof(IsLoggedIn));
                    RunWhileLoggin();
                });
            }
        }

        public ICommand ShowSearchUserPage
        {
            get
            {
                return new BaseCommand(obj =>
                {
                    SoundPlayer.PlayButtonSound();
                    _searchUserPageVM.Visibility = System.Windows.Visibility.Hidden;
                    CurrentPage = _searchUserPage;
                });
            }
        }
    }
}
