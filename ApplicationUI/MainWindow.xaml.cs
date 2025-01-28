using BLL.ModelsDTO;
using BLL.Services;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
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
        public MainWindow(UserService userService, BooksService bookService)
        {
            InitializeComponent();
            UserService us = userService;
            BooksService bs = bookService;
            UserDTO user = new UserDTO()
            {
                Email = "jhkjsdhfg@gmail.com",
                Password = "1234",
                Icon = File.ReadAllBytes("D:\\Викачування\\photo_2025-01-11_20-20-25.jpg"),
                Nickname = "Bumblebee",
                Phone = "+380678451264",
                Books = new List<BookDTO>()
            };
            userService.Add(user);
        }
    }
}