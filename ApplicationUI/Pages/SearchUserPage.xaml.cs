using ApplicationUI.TempModels;
using ApplicationUI.ViewModels;
using BLL.ModelsDTO;
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
            _searchUserPageVM = searchUserPageVM;
            this.DataContext = _searchUserPageVM;
        }

        private async void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is StackPanel panel && panel.Tag is BookDTO selectedBook)
            {
                _searchUserPageVM.SelectedBook = selectedBook;
                await _searchUserPageVM.Download();

            }

        }

    

    }
}
