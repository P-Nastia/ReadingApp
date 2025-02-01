using ApplicationUI.Commands;
using BLL.Interfaces;
using BLL.ModelsDTO;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using iText.Kernel.Pdf;
using System.Windows;
using ApplicationUI.Statics;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Canvas.Parser;

namespace ApplicationUI.ViewModels
{
    public class AllBooksPageVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private IUserService<BookDTO, UserDTO> _userService;
        private IBookService<BookDTO, ParagraphDTO, UserCommentDTO> _bookService;
        
        string href;
        private string downloadDirectory;
        public bool CanDownload { get; set; } = false;
        public string SearchString { get; set; }
        public string Response { get; set; }
        public BaseCommand SearchCommand => new BaseCommand(execute => Search(), canExecute => true);
        public BaseCommand DownloadCommand => new BaseCommand(execute => Download(), canExecute => true);
        public void OnNotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public AllBooksPageVM(IUserService<BookDTO, UserDTO> userService, IBookService<BookDTO, ParagraphDTO, UserCommentDTO> bookService)
        {
            _userService = userService;
            _bookService = bookService;
        }
        private void Search()
        {
            if (!String.IsNullOrWhiteSpace(SearchString) && !String.IsNullOrWhiteSpace(SearchString))
            {
                ChromeOptions options = new ChromeOptions();
                options.AddArgument("--headless");

                using (IWebDriver driver = new ChromeDriver(options))
                {
                    driver.Navigate().GoToUrl("https://www.ukrlib.com.ua/search.php?");

                    IWebElement inputField = driver.FindElement(By.XPath("//*[@id=\"cse-search-box\"]/input[1]"));

                    inputField.Clear();
                    Response = SearchString;
                    SearchString += " скачати повністю";
                    inputField.SendKeys(SearchString);

                    IWebElement submitButton = driver.FindElement(By.XPath("//*[@id=\"cse-search-box\"]/input[2]"));

                    submitButton.Click();
                    IReadOnlyCollection<IWebElement> links = driver.FindElements(By.XPath("//*[@id=\"___gcse_1\"]/div/div/div/div[5]/div[2]/div[1]/div/div[1]/div[1]/div/div[1]/div/a"));
                    if (links.Any())
                    {
                        CanDownload = true;
                        OnNotifyPropertyChanged("CanDownload");
                        Response += " found";
                        OnNotifyPropertyChanged("Response");
                        href = links.First().GetAttribute("href");
                    }
                    else
                    {
                        Response += " not found";
                        OnNotifyPropertyChanged("Response");
                    }
                    driver.Quit();
                }
            }
        }
        private void Download()
        {
            ChromeOptions options = new ChromeOptions();
            downloadDirectory = Directory.GetCurrentDirectory() + $"\\Files";
            Directory.CreateDirectory(downloadDirectory);

            options.AddArgument("--headless");
            options.AddUserProfilePreference("download.default_directory", downloadDirectory);
            options.AddUserProfilePreference("plugins.always_open_pdf_externally", true);
            options.AddUserProfilePreference("download.prompt_for_download", false);
            using (IWebDriver driver = new ChromeDriver(options))
            {
                
                driver.Navigate().GoToUrl(href);

                var pdfSources = driver.FindElements(By.XPath("//*[@id=\"mm-0\"]/div[2]/div/div[6]/div[3]/a[6]"));
                
                if (pdfSources.Any())
                {
                    var info = driver.FindElement(By.XPath("//*[@id=\"mm-0\"]/div[2]/div/div[4]/h2")).GetAttribute("innerText").Split(',');
                    var book = _bookService.GetByNameAndAuthor(info[1], info[0]);
                    
                    if (book == null)
                    {
                        pdfSources.First().Click();
                        Thread.Sleep(4000);
                        string pdfPath = $"{Directory.GetFiles(downloadDirectory)[0]}";
                        string text = "";

                        try
                        {
                            using (PdfReader reader = new PdfReader(pdfPath))
                            using (PdfDocument pdfDoc = new PdfDocument(reader))
                            {
                                for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
                                {
                                    var page = pdfDoc.GetPage(i);
                                    var strategy = new LocationTextExtractionStrategy();
                                    var textFromPage = PdfTextExtractor.GetTextFromPage(page, strategy);
                                    text += textFromPage;
                                }

                                List<string> paragraphs = text.Split(new[] { ".\n" }, StringSplitOptions.None).ToList();

                                BookDTO tempBook = new BookDTO()
                                {
                                    Author = info[0],
                                    Name = info[1],
                                    Users = new List<UserDTO>()
                                };
                                List<ParagraphDTO> p = new List<ParagraphDTO>();
                                foreach (var par in paragraphs)
                                {
                                    p.Add(new ParagraphDTO() { Text = par, Book = tempBook, UserComments = new List<UserCommentDTO>() });
                                }
                                tempBook.Paragraphs = p;
                                _bookService.AddBook(tempBook);
                                var bookFromDB = _bookService.GetByNameAndAuthor(tempBook.Name, tempBook.Author);
                                _userService.AddBook(StaticUser.User, bookFromDB);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error: {ex.Message}");
                        }
                    }
                    else
                    {
                        _userService.AddBook(StaticUser.User, book);
                    }
                }
                driver.Quit();
                MessageBox.Show("Book added to your library", "Download completed", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            var files = Directory.GetFiles(downloadDirectory);
            foreach(var file in files)
            {
                File.Delete(file);
            }
            Directory.Delete(downloadDirectory);
        }
    }
}
