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
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private LoginPageVM _loginPageVM;
        public LoginPage(LoginPageVM loginPageVM)
        {
            InitializeComponent();
            _loginPageVM = loginPageVM;
            this.DataContext = _loginPageVM;
        }

        private void logInClick(object sender, MouseButtonEventArgs e)
        {
            _loginPageVM.Password = this.passwordPB.Password;
            _loginPageVM.Login();
        }
        private void signUpClick(object sender, MouseButtonEventArgs e)
        {
            _loginPageVM.SignUp();
        }

        private void PasswordBox_TextInput(object sender, TextCompositionEventArgs e)
        {
           
        }
    }
}
