﻿using ApplicationUI.Pages;
using ApplicationUI.Statics;
using ApplicationUI.ViewModels;
using BLL.Interfaces;
using BLL.ModelsDTO;
using BLL.Services;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ApplicationUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const double SidebarTriggerX = 250;
        public MainWindow(IUserService<BookDTO, UserDTO, NotificationDTO> userService, IBookService<BookDTO, ParagraphDTO, UserCommentDTO> bookService, LoginPageVM loginPageVM, SignupPageVM signupPageVM, MyLibraryPageVM myLibraryPageVM, AllBooksPageVM allBooksPageVM,MyProfilePageVM myProfilePageVM, NotificationPageVM notificationPageVM)
        {
            InitializeComponent();

            #region ImageConfig 
            // Setting images/Icons
            string CD = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\Images\\"; // Maybe there is a better way to get the project directory...
            MyProfileImage.Source = new BitmapImage(new Uri($"{CD}myProfile.png", UriKind.Absolute));
            MyBooksImage.Source = new BitmapImage(new Uri($"{CD}myLibrary.jpg", UriKind.Absolute));
            LibraryImage.Source = new BitmapImage(new Uri($"{CD}libraryPageImage.png", UriKind.Absolute));
            LogOutImage.Source = new BitmapImage(new Uri($"{CD}LogOut.jpg", UriKind.Absolute));
            NotificationImage.Source = new BitmapImage(new Uri($"{CD}NotificationBell.png", UriKind.Absolute));
            #endregion
            StaticUser.User = new UserDTO();
            PageViewModel pageViewModel = new PageViewModel(this, userService, bookService, loginPageVM, signupPageVM, myLibraryPageVM, allBooksPageVM, myProfilePageVM, notificationPageVM);
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
            else if (mousePosition.X > SidebarTriggerX && Sidebar.Width == 270)
            {
                // Запускаємо анімацію закриття
                Storyboard collapseStoryboard = (Storyboard)Sidebar.Resources["CollapseSidebar"];
                collapseStoryboard.Begin();
            }
        }


    }
}