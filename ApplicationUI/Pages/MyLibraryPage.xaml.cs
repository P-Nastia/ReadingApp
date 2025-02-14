using ApplicationUI.Statics;
using ApplicationUI.ViewModels;
using ApplicationUI.Windows;
using System.Windows;
using System.Windows.Controls;

namespace ApplicationUI.Pages
{
    /// <summary>
    /// Interaction logic for MyLibraryPage.xaml
    /// </summary>
    public partial class MyLibraryPage : Page
    {
        private MyLibraryPageVM _myLibraryPageVM;
        public MyLibraryPage(MyLibraryPageVM myLibraryPageVM)
        {
            InitializeComponent();
            _myLibraryPageVM = myLibraryPageVM;
            this.DataContext = _myLibraryPageVM;
        }
        private async void btSwith_Click(object sender, RoutedEventArgs e)
        {
            await SoundPlayer.PlayButtonSoundAsync();
            ReadBookWindow readBookWindow = new ReadBookWindow(_myLibraryPageVM.SelectedBook, _myLibraryPageVM._bookService, _myLibraryPageVM._userService);
            readBookWindow.ShowDialog();
        }
    }
}
