using ApplicationUI.Commands;
using ApplicationUI.Windows;
using BLL.Interfaces;
using BLL.ModelsDTO;
using System.ComponentModel;
using System.IO;
using System.Linq;
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

        public string MessageIconSource { get; set; }

        private IBookService<BookDTO, ParagraphDTO, UserCommentDTO> _bookService;
        private IUserService<BookDTO, UserDTO, NotificationDTO> _userService;
        private Brush _background;
        public Brush ChapterBackground
        {
            get => _background;
            set
            {
                if(_background!= value)
                {
                    _background = value;
                    OnNotifyPropertyChanged(nameof(ChapterBackground));
                }
            }
        }
        public ChapterPage(ChapterDTO chapter, IBookService<BookDTO, ParagraphDTO, UserCommentDTO> bookService, IUserService<BookDTO, UserDTO, NotificationDTO> userService)
        {
            InitializeComponent();

            Chapter = chapter;
            _bookService = bookService;
            _userService = userService;
            this.DataContext = this;
            Chapter = _userService.LoadParagraphs(Chapter);
            OnNotifyPropertyChanged(nameof(Chapter));
            ChapterBackground = Brushes.WhiteSmoke;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnNotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public BaseCommand ChangeBackgroundCommand => new BaseCommand(obj => ChangeBackground(), canExecute => true);
        private async void textRB_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                var clickedItem = (ParagraphDTO)paragraphsLB.SelectedItem;
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

        private void Paragraph_MouseEnter(object sender, MouseEventArgs e)
        {
            Grid grid = sender as Grid;
            foreach (var GridItem in grid.Children)
            {
                if(GridItem is Border && (GridItem as Border).Name == "CommentPortal")
                {
                    (GridItem as Border).Visibility = Visibility.Visible;
                }
            }
        }

        private void Paragraph_MouseLeave(object sender, MouseEventArgs e)
        {
            Grid grid = sender as Grid;
            foreach (var GridItem in grid.Children)
            {
                if (GridItem is Border && (GridItem as Border).Name == "CommentPortal")
                {
                    (GridItem as Border).Visibility = Visibility.Hidden;
                }
            }
        }
        private void ChangeBackground()
        {
            if (ChapterBackground == Brushes.WhiteSmoke)
                ChapterBackground = Brushes.Moccasin;
            else
                ChapterBackground = Brushes.WhiteSmoke;
        }
    }
}
