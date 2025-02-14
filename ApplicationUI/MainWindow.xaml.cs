using ApplicationUI.Statics;
using ApplicationUI.ViewModels;
using BLL.Interfaces;
using BLL.ModelsDTO;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace ApplicationUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const double SidebarTriggerX = 100;
        public MainWindow(IUserService<BookDTO, UserDTO, NotificationDTO> userService, IBookService<BookDTO, ParagraphDTO, UserCommentDTO> bookService, LoginPageVM loginPageVM, SignupPageVM signupPageVM, MyLibraryPageVM myLibraryPageVM, AllBooksPageVM allBooksPageVM,MyProfilePageVM myProfilePageVM, NotificationPageVM notificationPageVM,SearchUserPageVM searchUserPageVM)
        public const int SideBarOpenWidth = 270;
        public const int SideBarClosedWidth = 100;
        public MainWindow(IUserService<BookDTO, UserDTO, NotificationDTO> userService, IBookService<BookDTO, ParagraphDTO, UserCommentDTO> bookService, LoginPageVM loginPageVM, SignupPageVM signupPageVM, MyLibraryPageVM myLibraryPageVM, AllBooksPageVM allBooksPageVM, MyProfilePageVM myProfilePageVM, NotificationPageVM notificationPageVM, SearchUserPageVM searchUserPageVM)
        {
            InitializeComponent();
            StaticUser.User = new UserDTO();
            PageViewModel pageViewModel = new PageViewModel(this, userService, bookService, loginPageVM, signupPageVM, myLibraryPageVM, allBooksPageVM, myProfilePageVM, notificationPageVM,searchUserPageVM);
            this.DataContext = pageViewModel;
        }


        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePosition = e.GetPosition(this); // Отримуємо координати миші

            if (mousePosition.X <= SideBarClosedWidth && Sidebar.Width == SideBarClosedWidth)
            {
                // Запускаємо анімацію відкриття
                Storyboard expandStoryboard = (Storyboard)Sidebar.Resources["ExpandSidebar"];
                expandStoryboard.Begin();
            }
            else if (mousePosition.X > SideBarOpenWidth && Sidebar.Width == SideBarOpenWidth)
            {
                // Запускаємо анімацію закриття
                Storyboard collapseStoryboard = (Storyboard)Sidebar.Resources["CollapseSidebar"];
                collapseStoryboard.Begin();
            }
        }

        private void CascadeLoginUI(object sender, DependencyPropertyChangedEventArgs e)
        {
            if((bool)e.NewValue == true)
            {
                Sidebar.Visibility = Visibility.Visible;
                MainFrame.Margin = new Thickness(100, 0, 0, 0);
            }
            else
            {
                Sidebar.Visibility = Visibility.Collapsed;
                MainFrame.Margin = new Thickness(0, 0, 0, 0);
            }
        }
    }
}