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
        private IUserService<BookDTO, UserDTO> _userService;
        private IBookService<BookDTO, ParagraphDTO, UserCommentDTO> _bookService;
        public BaseCommand ShowBooks => new BaseCommand(execute => Show(), canExecute => true);
        public ICollection<BookDTO> UserBooks { get; set; }
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
            //var user = _userService.GetById(StaticUser.User.Id);
            //var book = _userService.GetBook(user, 1);
            //UserBooks = user.Books;
        }
        private void Show()
        {
            var user = _userService.GetById(StaticUser.User.Id);
            UserBooks = user.Books;
            OnNotifyPropertyChanged("UserBooks");
        }
    }
}
