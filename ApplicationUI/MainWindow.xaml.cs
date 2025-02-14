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
        private const double SidebarTriggerX = 250;
        public MainWindow(IUserService<BookDTO, UserDTO, NotificationDTO> userService, IBookService<BookDTO, ParagraphDTO, UserCommentDTO> bookService, LoginPageVM loginPageVM, SignupPageVM signupPageVM, MyLibraryPageVM myLibraryPageVM, AllBooksPageVM allBooksPageVM,MyProfilePageVM myProfilePageVM, NotificationPageVM notificationPageVM,SearchUserPageVM searchUserPageVM)
        {
            InitializeComponent();
            StaticUser.User = new UserDTO();
            PageViewModel pageViewModel = new PageViewModel(this, userService, bookService, loginPageVM, signupPageVM, myLibraryPageVM, allBooksPageVM, myProfilePageVM, notificationPageVM,searchUserPageVM);
            this.DataContext = pageViewModel;
        }


        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePosition = e.GetPosition(this); // Отримуємо координати миші

            if (mousePosition.X <= SidebarTriggerX && Sidebar.Width == 0)
            {
                // Запускаємо анімацію відкриття
                Storyboard expandStoryboard = (Storyboard)Sidebar.Resources["ExpandSidebar"];
                expandStoryboard.Begin();
            }
            else if (mousePosition.X >= SidebarTriggerX+220  && Sidebar.Width == 270)
            {
                // Запускаємо анімацію закриття
                Storyboard collapseStoryboard = (Storyboard)Sidebar.Resources["CollapseSidebar"];
                collapseStoryboard.Begin();
            }
        }


    }
}