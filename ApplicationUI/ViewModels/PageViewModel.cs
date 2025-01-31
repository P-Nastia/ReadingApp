﻿using ApplicationUI.Commands;
using ApplicationUI.Pages;
using ApplicationUI.Statics;
using BLL.Interfaces;
using BLL.ModelsDTO;
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
        public IUserService<BookDTO, UserDTO> userService;
        public IBookService<BookDTO, ParagraphDTO, UserCommentDTO> bookService;

        private readonly bool _isLoggedIn;
        public bool IsLoggedIn
        {
            get { return StaticUser.IsLoggedIn; }
            set
            {
                if(StaticUser.IsLoggedIn != value)
                {
                    StaticUser.IsLoggedIn = value;
                    _currentPage = _myLibraryPage;
                    OnNotifyPropertyChanged(nameof(StaticUser.IsLoggedIn));
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

        public PageViewModel(MainWindow mainWindow, IUserService<BookDTO, UserDTO> userService, IBookService<BookDTO, ParagraphDTO, UserCommentDTO> bookService,LoginPageVM loginPageVM,SignupPageVM signupPageVM,MyLibraryPageVM myLibraryPageVM,AllBooksPageVM allBooksPageVM)
        {
            this.userService = userService;
            this.bookService = bookService;
            this._loginPage = new LoginPage(loginPageVM);
            this._signUpPage = new SignupPage(signupPageVM);
            this._myLibraryPage = new MyLibraryPage(myLibraryPageVM);
            this._allBooksPage = new AllBooksPage(allBooksPageVM);
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
                    else if(StaticUser.UserNeedsToSignUp == true)
                    {
                        this.CurrentPage = _signUpPage;
                        OnNotifyPropertyChanged("CurrentPage");
                    }
                }
                if (StaticUser.IsLoggedIn == true)
                {
                    this.CurrentPage = _myLibraryPage;
                    OnNotifyPropertyChanged(nameof(StaticUser.IsLoggedIn));
                    OnNotifyPropertyChanged("CurrentPage");
                }
            });
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
