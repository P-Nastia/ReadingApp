using ApplicationUI.Statics;
using ApplicationUI.ViewModels;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;

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

        private async void logInClick(object sender, MouseButtonEventArgs e)
        {
            await SoundPlayer.PlayButtonSoundAsync();
            _loginPageVM.Password = passwordPB.Password;
            _loginPageVM.Nickname = NickName_TextInput.Text;
            ResetTextBoxBorders();
            NicknameLoginToolTip.IsOpen = false;
            PasswordLoginToolTip.IsOpen = false;
            if (_loginPageVM.IsInputCorrect())
            {   
                await _loginPageVM.Login();
            }
            else {
                if (!_loginPageVM.IsNicknameLoginValid())
                {
                    NickName_TextInput.BorderBrush = Brushes.Red;
                    NickName_TextInput.BorderThickness = new Thickness(2);
                    NicknameLoginToolTip.IsOpen = true;
                }
                else
                {
                    NicknameLoginToolTip.IsOpen= false;
                }

                if (!_loginPageVM.IsPasswordLoginValid())
                {
                    passwordPB.BorderBrush = Brushes.Red;
                    passwordPB.BorderThickness = new Thickness(2);
                    PasswordLoginToolTip.IsOpen = true;
                }
                else
                {
                    PasswordLoginToolTip.IsOpen= false;
                }
            }
        }
        private async void signUpClick(object sender, MouseButtonEventArgs e)
        {
            await SoundPlayer.PlayButtonSoundAsync();
            _loginPageVM.SignUp();
        }

        private void PasswordBox_TextInput(object sender, TextCompositionEventArgs e)
        {
           
        }


        public void ResetTextBoxBorders()
        {
            NickName_TextInput.BorderBrush = Brushes.Gray;
            passwordPB.BorderBrush = Brushes.Gray;

        }

    }
}
