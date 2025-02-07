using ApplicationUI.ViewModels;
using ApplicationUI.Windows;
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
    /// Interaction logic for SignupPage.xaml
    /// </summary>
    public partial class SignupPage : Page
    {
        SignupPageVM _signupPageVM;
        public SignupPage(SignupPageVM signupPageVM)
        {
            InitializeComponent();

            #region ImageConfig 
            // Setting images/Icons
            string CD = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\Images\\"; // Maybe there is  a better way to get the project directory...
            SignUpImage.Source = new BitmapImage(new Uri($"{CD}signUp.png", UriKind.Absolute));
            #endregion

            _signupPageVM = signupPageVM;
            this.DataContext = _signupPageVM;
        }
        private void signUpClick(object sender, MouseButtonEventArgs e)
        {
            _signupPageVM.Password = this.passwordPB.Password;
            if (_signupPageVM.IsInputCorrect())
            {
                VerificationWindow verificationWindow = new VerificationWindow(_signupPageVM.Email);
                verificationWindow.ShowDialog();
                if (verificationWindow.isVerificated)
                {
                    _signupPageVM.SignUp();
                }
            }
        }

        private void TextBox_TextInput(object sender, TextCompositionEventArgs e)
        {

        }
    }
}
