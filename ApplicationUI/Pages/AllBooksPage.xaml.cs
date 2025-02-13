using ApplicationUI.ViewModels;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ApplicationUI.Pages
{
    /// <summary>
    /// Interaction logic for AllBooksPage.xaml
    /// </summary>
    public partial class AllBooksPage : Page
    {
        private AllBooksPageVM _allBooksPageVM;
        public AllBooksPage(AllBooksPageVM allBooksPageVM)
        {
            InitializeComponent();

            #region ImageConfig 
            // Setting images/Icons
            string CD = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\Images\\"; // Maybe there is  a better way to get the project directory...
            SearchImage.Source = new BitmapImage(new Uri($"{CD}searchButton.png", UriKind.Absolute));
            downloadFromDeviceImage.Source = new BitmapImage(new Uri($"{CD}downloadImage.png", UriKind.Absolute));
            #endregion

            _allBooksPageVM = allBooksPageVM;
            this.DataContext = _allBooksPageVM;
        }

        private void booksLB_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _allBooksPageVM.Download(_allBooksPageVM.SelectedBook);
        }
    }
}
