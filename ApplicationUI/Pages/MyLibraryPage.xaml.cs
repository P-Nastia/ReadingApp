using ApplicationUI.ViewModels;
using ApplicationUI.Windows;
using System;
using System.Collections.Generic;
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

        private void btSwith_Click(object sender, RoutedEventArgs e)
        {
            ReadBookWindow readBookWindow = new ReadBookWindow(_myLibraryPageVM.SelectedBook,_myLibraryPageVM._bookService,_myLibraryPageVM._userService);
            readBookWindow.ShowDialog();
        }
    }
}
