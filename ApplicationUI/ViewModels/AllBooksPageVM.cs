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
using System.Media;

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
        private List<LibraryBook> _availableBooks;
        public List<LibraryBook> AvailableBooks;
        public LibraryBook SelectedBook { get; set; }
        public BaseCommand SearchCommand => new BaseCommand(execute => Search(), canExecute => true);
        public BaseCommand DownloadCommand => new BaseCommand(execute => Download(SelectedBook), canExecute => true);
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
            //SoundPlayer.PlayButtonSound();
            if (!String.IsNullOrWhiteSpace(SearchString) && !String.IsNullOrWhiteSpace(SearchString))
            {
                AvailableBooks = null;
                OnNotifyPropertyChanged(nameof(AvailableBooks));
                Application.Current.Dispatcher.Invoke(() =>
                {
                    string downloadDirectory = Directory.GetCurrentDirectory() + $"\\Files";
                    Directory.CreateDirectory(downloadDirectory);

                    ChromeOptions options = new ChromeOptions();
                    var driverService = ChromeDriverService.CreateDefaultService();
                    driverService.HideCommandPromptWindow = true;
                    using (IWebDriver driver = new ChromeDriver(driverService,options))
                    {
                        driver.Manage().Window.Maximize();
                        driver.Navigate().GoToUrl("https://knigogo.top/");

                        IWebElement inputField = driver.FindElement(By.XPath("//*[@id=\"searchform\"]/input"));
                        inputField.Clear();
                        inputField.SendKeys($"{SearchString}");
                        IWebElement submitButton = driver.FindElement(By.XPath("//*[@id=\"searchform\"]/button"));
                        submitButton.Click();
                        Thread.Sleep(1000);
                        IWebElement resultResponse = driver.FindElement(By.XPath("/html/body/div[2]/div/div[1]/div/div[1]/h2"));

                        AvailableBooks = new List<LibraryBook>();
                        _availableBooks = new List<LibraryBook>();
                        if (resultResponse.GetAttribute("innerText") == "КНИГИ")
                        {
                            try
                            {
                                int count = 1;
                                while (true)
                                {
                                    LibraryBook book = new LibraryBook();
                                    string downloadResponse = driver.FindElement(By.XPath($"/html/body/div[2]/div/div[1]/div/div[1]/div[2]/div[1]/div[{count}]/div/div/div[1]/a/span")).GetAttribute("innerText");
                                    if (downloadResponse == "Скачати")
                                    {
                                        string bookWebSource = driver.FindElement(By.XPath($"/html/body/div[2]/div/div[1]/div/div[1]/div[2]/div[1]/div[{count}]/a")).GetAttribute("href");
                                        string title = driver.FindElement(By.XPath($"/html/body/div[2]/div/div[1]/div/div[1]/div[2]/div[1]/div[{count}]/div/a")).GetAttribute("innerText");
                                        string author = driver.FindElement(By.XPath($"/html/body/div[2]/div/div[1]/div/div[1]/div[2]/div[1]/div[{count}]/div/span/a")).GetAttribute("innerText");
                                        string url = driver.FindElement(By.XPath($"/html/body/div[2]/div/div[1]/div/div[1]/div[2]/div[1]/div[{count}]/a/span/img")).GetAttribute("src");
                                        book.Author = author;
                                        book.Name = title;
                                        book.BookPageLink = bookWebSource;
                                        book.CoverURL = url;
                                        book.Id = AvailableBooks.Count + 1;

                                        AvailableBooks.Add(book);
                                    }
                                    count++;
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        else
                        {
                            MessageBox.Show("No books");
                        }
                        OnNotifyPropertyChanged(nameof(AvailableBooks));
                        Thread.Sleep(1000);
                        driver.Quit();
                    }
                });
            }
        }


        EpubBook bookFile;
        public void Download(LibraryBook libraryBook)
        {
            var driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            ChromeOptions options = new ChromeOptions();
            string downloadDirectory = Directory.GetCurrentDirectory() + $"\\Files";
            Directory.CreateDirectory(downloadDirectory);

            options.AddUserProfilePreference("download.default_directory", downloadDirectory);
            options.AddUserProfilePreference("plugins.always_open_pdf_externally", true);
            options.AddUserProfilePreference("download.prompt_for_download", false);

            using(IWebDriver driver=new ChromeDriver(driverService, options))
            {
                driver.Navigate().GoToUrl(libraryBook.BookPageLink);
                Thread.Sleep(3000);
                driver.Quit();
            }
        }
        private async void ParseBook(LibraryBook libraryBook)
        {
            await Task.Run(() =>
            {
                bookFile = EpubReader.ReadBook(libraryBook.FilePath);
                BookDTO book = new BookDTO()
                {
                    Name = libraryBook.Name,
                    Author = libraryBook.Author,
                    Users = new List<UserDTO>(),
                    Chapters = new List<ChapterDTO>(),
                    CoverURL = libraryBook.CoverURL
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
