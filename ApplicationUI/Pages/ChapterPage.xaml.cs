using ApplicationUI.Windows;
using BLL.Interfaces;
using BLL.ModelsDTO;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ApplicationUI.Pages
{
    /// <summary>
    /// Interaction logic for ChapterPage.xaml
    /// </summary>
    public partial class ChapterPage : Page, INotifyPropertyChanged
    {
        public ChapterDTO Chapter { get; set; }
        private IBookService<BookDTO, ParagraphDTO, UserCommentDTO> _bookService;
        private IUserService<BookDTO, UserDTO, NotificationDTO> _userService;
        public ChapterPage(ChapterDTO chapter, IBookService<BookDTO, ParagraphDTO, UserCommentDTO> bookService, IUserService<BookDTO, UserDTO, NotificationDTO> userService)
        {
            InitializeComponent();
            Chapter = chapter;
            _bookService = bookService;
            _userService = userService;
            this.DataContext = this;
            Chapter = _userService.LoadParagraphs(Chapter);
            OnNotifyPropertyChanged(nameof(Chapter));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnNotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        private async void textRB_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                var listBox = (ListBox)sender;
                var clickedItem = (ParagraphDTO)listBox.SelectedItem;
                CommentsWindow commentsWindow = new CommentsWindow(clickedItem, _bookService, _userService);
                commentsWindow.ShowDialog();
            });
        }

        // Загрузка нових абзаців, якщо scroll досяг низу
        private async void Page_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                var scrollViewer = FindVisualChild<ScrollViewer>(paragraphsLB);

                if (scrollViewer != null)
                {
                    bool isAtBottom = scrollViewer.VerticalOffset == scrollViewer.ScrollableHeight;

                    if (isAtBottom && e.Delta < 0)
                    {
                        double verticalOffset = scrollViewer.VerticalOffset;

                        Chapter = _userService.LoadParagraphs(Chapter);

                        paragraphsLB.ItemsSource = null;
                        paragraphsLB.ItemsSource = Chapter.Paragraphs;
                        OnNotifyPropertyChanged("Book");

                        scrollViewer.ScrollToVerticalOffset(scrollViewer.ScrollableHeight);
                    }
                }
            });

        }

        // залишає scroll на місці, де закінчилився абзац
        private static T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child is T)
                {
                    return (T)child;
                }

                T result = FindVisualChild<T>(child);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }

    }
}
