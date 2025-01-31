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
    /// Interaction logic for SignupPage.xaml
    /// </summary>
    public partial class SignupPage : Page
    {
        SignupPageVM _signupPageVM;
        public SignupPage(SignupPageVM signupPageVM)
        {
            InitializeComponent();
            _signupPageVM = signupPageVM;
            this.DataContext = _signupPageVM;
        }
        private void signUpClick(object sender, MouseButtonEventArgs e)
        {
            _signupPageVM.SignUp();
        }
    }
}
