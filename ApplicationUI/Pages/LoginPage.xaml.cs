using ApplicationUI.Statics;
using ApplicationUI.ViewModels;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

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

            #region ImageConfig 
            // Setting images/Icons
            string CD = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\Images\\"; // Maybe there is  a better way to get the project directory...
            LoginImage.Source = new BitmapImage(new Uri($"{CD}login.png", UriKind.Absolute));
            SignUpImage.Source = new BitmapImage(new Uri($"{CD}signUp.png", UriKind.Absolute));
            #endregion

            _loginPageVM = loginPageVM;
            this.DataContext = _loginPageVM;
        }

        private void logInClick(object sender, MouseButtonEventArgs e)
        {
            SoundPlayer.PlayButtonSound();
            _loginPageVM.Password = this.passwordPB.Password;
            _loginPageVM.Login();
            NickName_TextInput.Clear();
        }
        private void signUpClick(object sender, MouseButtonEventArgs e)
        {
            SoundPlayer.PlayButtonSound();
            _loginPageVM.SignUp();
        }

        private void PasswordBox_TextInput(object sender, TextCompositionEventArgs e)
        {
           
        }
    }
}
