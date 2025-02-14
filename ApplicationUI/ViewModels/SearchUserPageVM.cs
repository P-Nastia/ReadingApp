using ApplicationUI.Commands;
using ApplicationUI.Statics;
using ApplicationUI.TempModels;
using BLL.Interfaces;
using BLL.ModelsDTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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
        public BaseCommand SearchUserCommand => new BaseCommand(obj => SearchUser(), canExecute => true);
        public BaseCommand DownloadCommand => new BaseCommand(obj => Download(), canExecute => true);
        public string SearchString { get; set; }
        public SearchUserPageVM(IUserService<BookDTO, UserDTO, NotificationDTO> userService, IBookService<BookDTO, ParagraphDTO, UserCommentDTO> bookServic)
        {
            this._userService = userService;
            this._bookService = bookServic;
        }
        public void OnNotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        private UserDTO _user;
        public UserDTO User {
            get => _user;
            set
            {
                if(_user != value)
                {
                    _user = value;
                    OnNotifyPropertyChanged(nameof(User));
                }
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
        private byte[] _icon;
        public byte[] Icon
        {
            get => _icon;
            set
            {
                if (_icon != value)
                {
                    _icon = value;
                    OnNotifyPropertyChanged(nameof(Icon));
                }
            }
        }
        private BookDTO _selectedBook;
        public BookDTO SelectedBook
        {
            get => _selectedBook;
            set
            {
                if (_selectedBook != value) 
                {
                    _selectedBook = value;
                    OnNotifyPropertyChanged(nameof(SelectedBook));
                }
            }
        }
        private async void SearchUser()
        {
            await Statics.SoundPlayer.PlayButtonSoundAsync();
            if (!String.IsNullOrEmpty(SearchString) && !String.IsNullOrWhiteSpace(SearchString))
            {
                User = _userService.FindSimiliar(SearchString, "", "", false);
                Icon = await ServerService.DownloadImageBytesAsync(User.Icon);
                Visibility = Visibility.Visible;
            }
        }
        public async Task Download()
        {
            MessageBoxResult res = MessageBox.Show("Do you want to add this book?","info",MessageBoxButton.YesNo,MessageBoxImage.Information);
            if (res == MessageBoxResult.Yes)
            {
                await Statics.SoundPlayer.PlayButtonSoundAsync();
                if (SelectedBook != null)
                {
                    _userService.AddBook(StaticUser.User, _bookService.GetByNameAndAuthor(SelectedBook.Name, SelectedBook.Author));
                    MessageBox.Show("Book added to your library");
                }
            }
            else
            {

            }
        }
    }
}
