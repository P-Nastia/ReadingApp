using ApplicationUI.Pages;
using BLL.Interfaces;
using BLL.ModelsDTO;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace ApplicationUI.Windows
{
    /// <summary>
    /// Interaction logic for ReadBookWindow.xaml
    /// </summary>
    public partial class ReadBookWindow : Window, INotifyPropertyChanged
    {
        public BookDTO Book { get; set; }
        public IUserService<BookDTO, UserDTO, NotificationDTO> _userService;
        public IBookService<BookDTO, ParagraphDTO, UserCommentDTO> _bookService;

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnNotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public ReadBookWindow(BookDTO book, IBookService<BookDTO, ParagraphDTO, UserCommentDTO> bookService, IUserService<BookDTO, UserDTO, NotificationDTO> userService)
        {
            InitializeComponent();
            this._bookService = bookService;
            this._userService = userService;
           
            this.DataContext = this;
            this.Book = book;
            this.Book = _userService.LoadChapters(book);
            OnNotifyPropertyChanged("Book");
        }

        private void chaptersLB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChapterDTO selectedChapter = this.chaptersLB.SelectedItem as ChapterDTO;
            if (selectedChapter != null)
            {
                 Dispatcher.Invoke(()=> {
                    this.chapterFrame.Content = null;
                    this.chapterFrame.Content = new ChapterPage(selectedChapter, _bookService, _userService);
                });
            }
        }
    }
}
