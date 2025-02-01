﻿using ApplicationUI.Pages;
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
        //private UserService us;
        //private BooksService bs;
        public MainWindow(IUserService<BookDTO, UserDTO> userService, IBookService<BookDTO, ParagraphDTO, UserCommentDTO> bookService, LoginPageVM loginPageVM, SignupPageVM signupPageVM, MyLibraryPageVM myLibraryPageVM, AllBooksPageVM allBooksPageVM)
        {
            InitializeComponent();
            PageViewModel pageViewModel = new PageViewModel(this, userService, bookService, loginPageVM, signupPageVM, myLibraryPageVM, allBooksPageVM);
            this.DataContext = pageViewModel;
        }
    }
}