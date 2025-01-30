using ApplicationUI.ViewModels;
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
        public MainWindow(PageViewModel pageViewModel)
        {
            InitializeComponent();
            this.DataContext = pageViewModel;
             //us = userService;
             //bs = bookService;
            //UserDTO user = new UserDTO()
            //{
            //    Email = "helloworld@gmail.com",
            //    Password = "world",
            //    Icon = File.ReadAllBytes("D:\\Викачування\\helena-lopes-S3TPJCOIRoo-unsplash-scaled.jpg"),
            //    Nickname = "HelloWorld",
            //    Phone = "+380967546832",
            //    Books = new List<BookDTO>()
            //};
            //AddUser(user);
            //var u = userService.GetById(2);

            //string pdfPath = "D:\\nechuy-levytskyy-ivan-semenovych-kaydasheva-simia907 (1).pdf";  // Вкажіть шлях до вашого PDF файлу
            //string text = "";

            //try
            //{
            //    using (PdfReader reader = new PdfReader(pdfPath))
            //    using (PdfDocument pdfDoc = new PdfDocument(reader))
            //    {
            //        for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
            //        {
            //            var page = pdfDoc.GetPage(i);
            //            var strategy = new LocationTextExtractionStrategy();
            //            var textFromPage = PdfTextExtractor.GetTextFromPage(page, strategy);
            //            text += textFromPage;
            //        }

            //        List<string> paragraphs = text.Split(new[] { ".\n" }, StringSplitOptions.None).ToList();

            //        //foreach (var p in paragraphs)
            //        //{
            //        //    AddRunToRichTextBox(p);
            //        //}
            //        //text = string.Join(".\n\n", paragraphs);

            //        // Вивести зчитаний текст
            //        BookDTO book = new BookDTO()
            //        {
            //            Author = "Іван Нечуй-Левицький",
            //            Name = "Кайдашева сім'я",
            //            Users = new List<UserDTO>()
            //        };
            //        List<ParagraphDTO> p = new List<ParagraphDTO>();
            //        foreach (var par in paragraphs)
            //        {
            //            p.Add(new ParagraphDTO() { Text = par, Book = book, UserComments = new List<UserCommentDTO>() });
            //        }
            //        book.Paragraphs = p;

            //        //AddBook(book);
            //        //AddUserBook(u, book);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"Error: {ex.Message}");
            //}
            //var user = us.GetById(2);
            //var book = bs.GetBook(1);
            //us.RemoveBook(user, book);
            //AddUserBook(user, book);
            //UserCommentDTO uc = new UserCommentDTO()
            //{
            //    ParagraphId = book.Paragraphs.ElementAt(30).Id,
            //    Comment = "Ого",
            //    Published = DateTime.Now.ToUniversalTime(),
            //    UserId = user.Id
            //};
            //AddComment(uc);

            //// видалення коментаря
            //var paragraph = book.Paragraphs.ElementAt(30);
            //var commentToDelete = paragraph.UserComments.ElementAt(0);
            //bs.DeleteComment(commentToDelete);


            // зчитування іконки користувача
            //using (var stream = new MemoryStream(user.Icon))
            //{
            //    // Create a BitmapImage from the MemoryStream
            //    BitmapImage bitmap = new BitmapImage();
            //    bitmap.BeginInit();
            //    bitmap.StreamSource = stream;
            //    bitmap.CacheOption = BitmapCacheOption.OnLoad; // Make sure the image is fully loaded
            //    bitmap.EndInit();

            //    // Set the BitmapImage as the source for the Image control
            //    image.Source = bitmap; // 'MyImageControl' is the Image control in XAML
            //}
        }
        private async Task AddBook(BookDTO book)
        {
            //string pdfPath = "D:\\_OceanofPDF.com_Claiming_His_Princess_A_Beauty_n_the_Beast_Romance_-_Parker_Grey.pdf";  // Вкажіть шлях до вашого PDF файлу
            //await bs.AddBook(book);
            
        }
        private async Task AddUserBook(UserDTO user,BookDTO book)
        {
            //string pdfPath = "D:\\_OceanofPDF.com_Claiming_His_Princess_A_Beauty_n_the_Beast_Romance_-_Parker_Grey.pdf";  // Вкажіть шлях до вашого PDF файлу
            user.Books.Add(book);
            book.Users.Add(user);
            //await us.AddBook(user, book);

        }
        private async Task AddUser(UserDTO user)
        {
            
            //string pdfPath = "D:\\_OceanofPDF.com_Claiming_His_Princess_A_Beauty_n_the_Beast_Romance_-_Parker_Grey.pdf";  // Вкажіть шлях до вашого PDF файлу
            //await us.Add(user);

        }
        private async Task AddComment(UserCommentDTO uc)
        {
            //string pdfPath = "D:\\_OceanofPDF.com_Claiming_His_Princess_A_Beauty_n_the_Beast_Romance_-_Parker_Grey.pdf";  // Вкажіть шлях до вашого PDF файлу
            //await bs.AddComment(uc);

        }
    }
}