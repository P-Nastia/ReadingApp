using ApplicationUI.Statics;
using BLL.Interfaces;
using BLL.ModelsDTO;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using ApplicationUI.Windows;

namespace ApplicationUI.ViewModels
{
    public class NotificationPageVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private IUserService<BookDTO, UserDTO, NotificationDTO> _userService;
        public IBookService<BookDTO, ParagraphDTO, UserCommentDTO> _bookService;

        public NotificationDTO Selected {  get; set; }

        private UserDTO _user = null;
        private List<NotificationDTO> _userNotifications { get; set; }
        public List<NotificationDTO> UserNotifications
        {
            get => _userNotifications;
            set
            {
                if (_userNotifications != value)
                {
                    _userNotifications = value;
                    OnNotifyPropertyChanged(nameof(UserNotifications));
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
        public NotificationPageVM(IUserService<BookDTO, UserDTO, NotificationDTO> userService, IBookService<BookDTO, ParagraphDTO, UserCommentDTO> bookService)
        {
            _bookService = bookService;
            _userService = userService;
            UserNotifications = new List<NotificationDTO>();
        }
        public async void Show()
        {
            await Task.Run(() =>
            {
                _user = _userService.GetById(StaticUser.User.Id);
                UserNotifications = new List<NotificationDTO>();
                UserNotifications = _user.Notifications.ToList();
                OnNotifyPropertyChanged("UserNotifications");
            });
        }

        public async void DeleteNotification()
        {
            if (Selected != null)
            {
                MessageBoxResult result = MessageBox.Show("Do you want to delete this notification?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    await _userService.RemoveNotification(StaticUser.User,Selected);
                    Show();
                }
            }
        }

        public async void HotLoadLink()
        {
            string Link = Selected.HotLoadLink;
            try
            {
                if(Link.Length > 8 && Link.Substring(0, 8) == "Comment:")
                {
                    // Extracting path
                    Link = Link.Replace("Comment:","");
                    string Book = Link.Substring(0, Link.IndexOf("/"));
                    Link = Link.Replace(Book+"/", "");
                    string Author = Link.Substring(0, Link.IndexOf("/"));
                    Link = Link.Replace(Author+"/", "");
                    string Chapter = Link.Substring(0, Link.IndexOf("/"));
                    Link = Link.Replace(Chapter+"/", "");
                    string Paragraph = Link;

                    // Opening path
                    var SelectedBook = _bookService.GetByNameAndAuthor(Book,Author);
                    ReadBookWindow readBookWindow = new ReadBookWindow(SelectedBook, _bookService, _userService);
                    readBookWindow.Show();

                    var SelectedChapter = readBookWindow.Book.Chapters.Find(X => X.Name == Chapter);
                    readBookWindow.chaptersLB.SelectedItem = SelectedChapter;

                    var SelectedParagraph = SelectedChapter.Paragraphs.Find(X=>X.Id == Int32.Parse(Paragraph)); 
                    CommentsWindow commentsWindow = new CommentsWindow(SelectedParagraph, _bookService, _userService);
                    commentsWindow.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to open notification!\nIt may be outdated or simply cannot be opened.", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
