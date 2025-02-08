using ApplicationUI.Commands;
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

namespace ApplicationUI.ViewModels
{
    public class MyLibraryPageVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public IUserService<BookDTO, UserDTO> _userService;
        public IBookService<BookDTO, ParagraphDTO, UserCommentDTO> _bookService;
        private UserDTO _user;
        public BaseCommand ShowBooksCommand => new BaseCommand(execute => Show(), canExecute => true);
        public BaseCommand ReadBookCommand => new BaseCommand(execute => ShowReadBookPage(), canExecute => true);
        public ICollection<BookDTO> UserBooks { get; set; }
        private BookDTO _selectedBook;
        public BookDTO SelectedBook
        {
            get
            {
                return _selectedBook;
            }
            set
            {
                if(value != null)
                {
                    _selectedBook = _userService.GetBook(_user, value.Id);
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
        public MyLibraryPageVM(IUserService<BookDTO, UserDTO> userService, IBookService<BookDTO, ParagraphDTO, UserCommentDTO> bookService)
        {
            _userService = userService;
            _bookService = bookService;
            UserBooks = new List<BookDTO>();
        }
        private async void Show()
        {
            SoundPlayer.PlayButtonSound();
            await Task.Run(() =>
            {
                _user = _userService.GetById(StaticUser.User.Id);
                UserBooks = _user.Books;
                OnNotifyPropertyChanged("UserBooks");
            });
        }
        private async void ShowReadBookPage()
        {
            await Task.Run(() =>
            {
                _user = _userService.GetById(StaticUser.User.Id);
                UserBooks = _user.Books;
                OnNotifyPropertyChanged("UserBooks");
            });
        }
    }
}
