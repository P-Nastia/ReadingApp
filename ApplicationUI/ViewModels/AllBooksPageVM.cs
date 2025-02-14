using ApplicationUI.Commands;
using BLL.Interfaces;
using BLL.ModelsDTO;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.IO;
using System.Windows;
using ApplicationUI.Statics;
using VersOne.Epub;
using HtmlAgilityPack;
using ApplicationUI.TempModels;
using Microsoft.Win32;
using SoundPlayer = ApplicationUI.Statics.SoundPlayer;

namespace ApplicationUI.ViewModels
{
    public class AllBooksPageVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private IUserService<BookDTO, UserDTO, NotificationDTO> _userService;
        private IBookService<BookDTO, ParagraphDTO, UserCommentDTO> _bookService;
        
        private string downloadDirectory;
        public bool CanDownload { get; set; } = true;
        public string SearchString { get; set; }
        public string Response { get; set; }
        private List<LibraryBook> _availableBooks;
        public List<LibraryBook> AvailableBooks
        {
            get => _availableBooks;
            set
            {
                if (_availableBooks != value)
                {
                    _availableBooks = value;
                    OnNotifyPropertyChanged(nameof(AvailableBooks));
                }
            }
        }
        public LibraryBook SelectedBook { get; set; }
        public BaseCommand SearchCommand => new BaseCommand(execute => Search(), canExecute => true);
        public BaseCommand DownloadCommand => new BaseCommand(execute => DownloadFile(), canExecute => true);
        public void OnNotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public AllBooksPageVM(IUserService<BookDTO, UserDTO, NotificationDTO> userService, IBookService<BookDTO, ParagraphDTO, UserCommentDTO> bookService)
        {
            _userService = userService;
            _bookService = bookService;
        }
        private async void DownloadFile()
        {
            await SoundPlayer.PlayButtonSoundAsync();
            OpenFileDialog dialog = new OpenFileDialog() { Filter = "EPUB Files (*.epub)|*.epub" };
            dialog.ShowDialog();
            if (!String.IsNullOrEmpty(dialog.FileName) && !String.IsNullOrWhiteSpace(dialog.FileName))
            {
                LibraryBook book = new LibraryBook();
                book.FilePath = dialog.FileName;
                ParseBook(book);
            }
        }
        private async void Search()
        {
            await SoundPlayer.PlayButtonSoundAsync();
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
                    options.AddArgument("--headless");
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
                        driver.Quit();
                        SearchString = string.Empty;
                        OnNotifyPropertyChanged(nameof(SearchString));
                    }
                });
            }
        }

        EpubBook bookFile;
        public async Task Download(LibraryBook libraryBook)
        {
            MessageBoxResult res = MessageBox.Show("Do you want to add this book?", "info", MessageBoxButton.YesNo,MessageBoxImage.Information);
            if (res == MessageBoxResult.Yes)
            {
                await SoundPlayer.PlayButtonSoundAsync();
                if (libraryBook != null)
                {
                    var bookFromDB = _bookService.GetByNameAndAuthor(libraryBook.Name, libraryBook.Author);
                    if (bookFromDB == null)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            var driverService = ChromeDriverService.CreateDefaultService();
                            driverService.HideCommandPromptWindow = true;
                            ChromeOptions options = new ChromeOptions();
                            string downloadDirectory = Directory.GetCurrentDirectory() + $"\\Files";
                            Directory.CreateDirectory(downloadDirectory);
                            foreach (var f in Directory.GetFiles(downloadDirectory))
                            {
                                File.Delete(f);
                            }
                            options.AddArgument("--headless");
                            options.AddUserProfilePreference("download.default_directory", downloadDirectory);
                            options.AddUserProfilePreference("plugins.always_open_pdf_externally", true);
                            options.AddUserProfilePreference("download.prompt_for_download", false);

                            using (IWebDriver driver = new ChromeDriver(driverService, options))
                            {
                                try
                                {
                                    driver.Navigate().GoToUrl(libraryBook.BookPageLink);
                                    Thread.Sleep(5000);
                                    var elements = driver.FindElements(By.CssSelector(".lib_book_download_container a"));
                                    elements.Where(e => e.GetAttribute("innerText") == "epub").FirstOrDefault().Click();
                                    Thread.Sleep(3000);
                                    libraryBook.FilePath = $"{Directory.GetFiles(downloadDirectory)[0]}";
                                    Thread.Sleep(2000);

                                    ParseBook(libraryBook);
                                }
                                catch (Exception ex) { MessageBox.Show("Unable to download file"); }
                                driver.Quit();
                            }
                            AvailableBooks = new List<LibraryBook>();
                            OnNotifyPropertyChanged(nameof(AvailableBooks));

                        });
                    }
                    else
                    {
                        _userService.AddBook(StaticUser.User, bookFromDB);
                        MessageBox.Show("Book added to library");
                    }
                }           
            }
            else
            {

            }
        }
        private void ParseBook(LibraryBook libraryBook)
        {
            if (libraryBook.FilePath != null)
            {
                bookFile = EpubReader.ReadBook(libraryBook.FilePath);
                if (libraryBook.Name == null)
                {
                    libraryBook.Name = bookFile.Title;
                    libraryBook.Author = bookFile.Author;
                    libraryBook.CoverURL = "https://clipart-library.com/img/1144207.png";// static book cover url for books, that are downloaded from device and not from selenium
                }
                BookDTO book = new BookDTO()
                {
                    Name = libraryBook.Name,
                    Author = libraryBook.Author,
                    Users = new List<UserDTO>(),
                    Chapters = new List<ChapterDTO>(),
                    CoverURL = libraryBook.CoverURL
                };
                int partCount = 1;
                foreach (EpubTextContentFile textContentFile in bookFile.ReadingOrder)
                {
                    ChapterDTO chapter = new ChapterDTO()
                    {
                        Name = $"Part {partCount++}",
                        Book = book,
                        Paragraphs = new List<ParagraphDTO>()
                    };
                    HtmlDocument htmlDocument = new();
                    htmlDocument.LoadHtml(textContentFile.Content);
                    string s = "";
                    foreach (HtmlNode node in htmlDocument.DocumentNode.SelectNodes("//text()"))
                    {
                        if (node.InnerText.Trim() != "\n")
                        {
                            s += node.InnerText.Trim() + '\n';
                        }
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
                
                MessageBox.Show("Book downloaded");
            }
        }
    }
}
