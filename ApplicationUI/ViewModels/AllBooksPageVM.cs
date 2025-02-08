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
using System.Collections.ObjectModel;
using VersOne.Epub;
using HtmlAgilityPack;
using ApplicationUI.TempModels;
using System.Security.Policy;

namespace ApplicationUI.ViewModels
{
    public class AllBooksPageVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private IUserService<BookDTO, UserDTO> _userService;
        private IBookService<BookDTO, ParagraphDTO, UserCommentDTO> _bookService;
        
        string href;
        private string downloadDirectory;
        public bool CanDownload { get; set; } = true;
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
        private async void Search()
        {
            
            if (!String.IsNullOrWhiteSpace(SearchString) && !String.IsNullOrWhiteSpace(SearchString))
            {
                await Task.Run(() =>
                {
                    var driverService = ChromeDriverService.CreateDefaultService();
                    driverService.HideCommandPromptWindow = true;
                    ChromeOptions options = new ChromeOptions();
                    options.AddArgument("--headless");

                    using (IWebDriver driver = new ChromeDriver(driverService, options))
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
                });
            }
        }
        EpubBook bookFile;
        private async void Download()//private async void Download(LibraryBook libraryBook)
        {
            await Task.Run(() =>
            {
                LibraryBook libraryBook = new LibraryBook()
                {
                    Name = "Girl Online Going Solo",
                    Author = "Zoe Sugg",
                    FilePath = "D:\\Викачування\\_OceanofPDF.com_Going_Solo_-_Zoe_Sugg.epub",
                    Cover = File.ReadAllBytes("D:\\Викачування\\PDF-EPUB-Girl-Online-Going-Solo-Download.jpg")
                };
                bookFile = EpubReader.ReadBook(libraryBook.FilePath);
                BookDTO book = new BookDTO()
                {
                    Name = libraryBook.Name,
                    Author = libraryBook.Author,
                    Users = new List<UserDTO>(),
                    Chapters = new List<ChapterDTO>()
                    
                };
                foreach (EpubTextContentFile textContentFile in bookFile.ReadingOrder)
                {
                    ChapterDTO chapter = new ChapterDTO()
                    {
                        Name = textContentFile.FileName,
                        Book = book,
                        Paragraphs = new List<ParagraphDTO>()
                    };
                    HtmlDocument htmlDocument = new();
                    htmlDocument.LoadHtml(textContentFile.Content);
                    string s = "";
                    foreach (HtmlNode node in htmlDocument.DocumentNode.SelectNodes("//text()"))
                    {
                        s += node.InnerText.Trim() + '\n';
                    }
                    List<string> paragraphs = s.Split(new[] { ".\n" }, StringSplitOptions.None).ToList();
                    List<ParagraphDTO> paragraphDTOs = new List<ParagraphDTO>();
                    foreach (var par in paragraphs)
                    {
                        paragraphDTOs.Add(new ParagraphDTO() { Text = par, Chapter = chapter, UserComments = new List<UserCommentDTO>() });
                    }
                    chapter.Paragraphs = paragraphDTOs;
                    book.Chapters.Add(chapter);
                }
                _bookService.AddBook(book);
                var bookFromDB = _bookService.GetByNameAndAuthor(book.Name, book.Author);
                _userService.AddBook(StaticUser.User, bookFromDB);
            });
        }
        //private async void Download()
        //{
        //    if (CanDownload)
        //    {
        //        await Task.Run(() =>
        //        {
        //            Response = String.Empty;
        //            OnNotifyPropertyChanged("Response");
        //            ChromeOptions options = new ChromeOptions();
        //            downloadDirectory = Directory.GetCurrentDirectory() + $"\\Files";
        //            Directory.CreateDirectory(downloadDirectory);

        //            options.AddArgument("--headless");
        //            options.AddUserProfilePreference("download.default_directory", downloadDirectory);
        //            options.AddUserProfilePreference("plugins.always_open_pdf_externally", true);
        //            options.AddUserProfilePreference("download.prompt_for_download", false);

        //            var driverService = ChromeDriverService.CreateDefaultService();
        //            driverService.HideCommandPromptWindow = true;

        //            bool isDownloaded = false;
        //            using (IWebDriver driver = new ChromeDriver(driverService, options))
        //            {

        //                driver.Navigate().GoToUrl(href);

        //                var pdfSources = driver.FindElements(By.XPath("//*[@id=\"mm-0\"]/div[2]/div/div[6]/div[3]/a[6]"));

        //                if (pdfSources.Any())
        //                {
        //                    var info = driver.FindElement(By.XPath("//*[@id=\"mm-0\"]/div[2]/div/div[4]/h2")).GetAttribute("innerText").Split(',');
        //                    var book = _bookService.GetByNameAndAuthor(info[1], info[0]);

        //                    if (book == null)
        //                    {
        //                        pdfSources.First().Click();
        //                        Thread.Sleep(4000);
        //                        Actions actions = new Actions(driver);
        //                        actions.MoveByOffset(500, 500).Click().Perform();
        //                        var filesFromDirectory = Directory.GetFiles(downloadDirectory);

        //                        string pdfPath = filesFromDirectory.Where(f => f.Contains(".pdf")).FirstOrDefault();
        //                        string text = "";

        //                        //try
        //                        //{
        //                        //    using (PdfReader reader = new PdfReader(pdfPath))
        //                        //    using (PdfDocument pdfDoc = new PdfDocument(reader))
        //                        //    {
        //                        //        for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
        //                        //        {
        //                        //            var page = pdfDoc.GetPage(i);
        //                        //            var strategy = new LocationTextExtractionStrategy();
        //                        //            var textFromPage = PdfTextExtractor.GetTextFromPage(page, strategy);
        //                        //            text += textFromPage;
        //                        //        }

        //                        //        List<string> paragraphs = text.Split(new[] { ".\n" }, StringSplitOptions.None).ToList();

        //                        //        BookDTO tempBook = new BookDTO()
        //                        //        {
        //                        //            Author = info[0],
        //                        //            Name = info[1],
        //                        //            Users = new List<UserDTO>()
        //                        //        };
        //                        //        List<ParagraphDTO> p = new List<ParagraphDTO>();
        //                        //        foreach (var par in paragraphs)
        //                        //        {
        //                        //            p.Add(new ParagraphDTO() { Text = par, Book = tempBook, UserComments = new List<UserCommentDTO>() });
        //                        //        }
        //                        //        tempBook.Paragraphs = p;
        //                        //        _bookService.AddBook(tempBook);
        //                        //        var bookFromDB = _bookService.GetByNameAndAuthor(tempBook.Name, tempBook.Author);
        //                        //        _userService.AddBook(StaticUser.User, bookFromDB);

        //                        //        SearchString = String.Empty;
        //                        //        OnNotifyPropertyChanged("SearchString");
        //                        //        MessageBox.Show("Book added to your library", "Download completed", MessageBoxButton.OK, MessageBoxImage.Information);
        //                        //        isDownloaded = true;
        //                        //    }
        //                        //}
        //                        //catch (Exception ex)
        //                        //{
        //                        //    MessageBox.Show($"Error: {ex.Message}");
        //                        //}
        //                    }
        //                    else
        //                    {
        //                        _userService.AddBook(StaticUser.User, book);
        //                        isDownloaded = true;
        //                        MessageBox.Show("Book added to your library", "Download completed", MessageBoxButton.OK, MessageBoxImage.Information);
        //                    }
        //                }
        //                driver.Quit();

        //            }
        //            var files = Directory.GetFiles(downloadDirectory);
        //            foreach (var file in files)
        //            {
        //                File.Delete(file);
        //            }
        //            Directory.Delete(downloadDirectory);
        //            if (isDownloaded == false)
        //            {
        //                MessageBox.Show("Impossible to download book to your library", "Download error", MessageBoxButton.OK, MessageBoxImage.Information);
        //            }
        //            CanDownload = false;
        //            OnNotifyPropertyChanged("CanDownload");
        //        });
        //    }
            
        //}
    }
}
