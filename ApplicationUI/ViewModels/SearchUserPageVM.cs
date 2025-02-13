using BLL.Interfaces;
using BLL.ModelsDTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ApplicationUI.ViewModels
{
    public class SearchUserPageVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private IUserService<BookDTO, UserDTO, NotificationDTO> _userService;
        private IBookService<BookDTO, ParagraphDTO, UserCommentDTO> _bookService;
        public void OnNotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        private Visibility _visibility;
        public Visibility Visibility
        {
            get => _visibility;
            set
            {
                if(_visibility != value)
                {
                    _visibility = value;
                    OnNotifyPropertyChanged(nameof(Visibility));
                }
            }
        }
        public SearchUserPageVM(IUserService<BookDTO, UserDTO, NotificationDTO> userService, IBookService<BookDTO, ParagraphDTO, UserCommentDTO> bookServic)
        {
            this._userService = userService;
            this._bookService = bookServic;
        }
    }
}
