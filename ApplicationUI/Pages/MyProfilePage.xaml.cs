using ApplicationUI.ViewModels;
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
    /// Interaction logic for MyProfilePage.xaml
    /// </summary>
    public partial class MyProfilePage : Page
    {
        private MyProfilePageVM _myProfilePageVM;
        public MyProfilePage(MyProfilePageVM myProfilePageVM)
        {
            InitializeComponent();
            _myProfilePageVM = myProfilePageVM;
            this.DataContext = myProfilePageVM;
        }
    }
}
