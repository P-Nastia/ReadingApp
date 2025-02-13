using ApplicationUI.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ApplicationUI.Pages
{
    /// <summary>
    /// Interaction logic for SearchUserPage.xaml
    /// </summary>
    public partial class SearchUserPage : Page
    {
        private SearchUserPageVM _searchUserPageVM;
        public SearchUserPage(SearchUserPageVM searchUserPageVM)
        {
            InitializeComponent();
            string CD = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\Images\\";
            SearchImage.Source = new BitmapImage(new Uri($"{CD}searchButton.png", UriKind.Absolute));
            _searchUserPageVM = searchUserPageVM;
            this.DataContext = _searchUserPageVM;
        }
    }
}
