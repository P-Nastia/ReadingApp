using ApplicationUI.TempModels;
using ApplicationUI.ViewModels;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
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
using System.Media;

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

            _allBooksPageVM = allBooksPageVM;
            this.DataContext = _allBooksPageVM;
        }

        private void booksLB_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _allBooksPageVM.Download(_allBooksPageVM.SelectedBook);
        }
    }
}
