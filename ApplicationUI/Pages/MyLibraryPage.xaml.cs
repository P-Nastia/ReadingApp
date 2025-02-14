using ApplicationUI.Statics;
using ApplicationUI.TempModels;
using ApplicationUI.ViewModels;
using ApplicationUI.Windows;
using BLL.ModelsDTO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

        private void StackPanelRead_Click(object sender, RoutedEventArgs e)
        {
            MenuItem MI = (MenuItem)sender;
            if ((MI.Parent as ContextMenu).Parent is StackPanel panel && panel.Tag is BookDTO selectedBook)
            {
                _myLibraryPageVM.SelectedBook = selectedBook;
            }
        }

        private void StackPanelRemove_Click(object sender, RoutedEventArgs e)
        {

            if (sender is StackPanel panel && panel.Tag is BookDTO selectedBook)
            MenuItem MI = (MenuItem)sender;
            if ((MI.Parent as ContextMenu).Parent is StackPanel panel && panel.Tag is BookDTO selectedBook)
            {
                _myLibraryPageVM.SelectedBook = selectedBook;
            }
        }
    }
}
