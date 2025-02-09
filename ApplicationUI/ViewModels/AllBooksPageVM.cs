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
                    Name = "Lessons In Faking",
                    Author = "Selina Mae",
                    FilePath = "D:\\Викачування\\_OceanofPDF.com_Lessons_in_Faking_-_Selina_Mae.epub",
                    Cover = File.ReadAllBytes("D:\\Викачування\\PDF-EPUB-Lessons-In-Faking-by-Selina-Mae-Download.jpg")
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
    }
}
