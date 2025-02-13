using ApplicationUI.Commands;
using ApplicationUI.Statics;
using BLL.Interfaces;
using BLL.ModelsDTO;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ApplicationUI.ViewModels
{
    public class MyLibraryPageVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public IUserService<BookDTO, UserDTO, NotificationDTO> _userService;
        public IBookService<BookDTO, ParagraphDTO, UserCommentDTO> _bookService;
        private UserDTO _user;
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
        public MyLibraryPageVM(IUserService<BookDTO, UserDTO, NotificationDTO> userService, IBookService<BookDTO, ParagraphDTO, UserCommentDTO> bookService)
        {
            _userService = userService;
            _bookService = bookService;
            UserBooks = new List<BookDTO>();
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
