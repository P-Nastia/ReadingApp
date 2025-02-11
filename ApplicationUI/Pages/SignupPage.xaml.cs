using ApplicationUI.Statics;
using ApplicationUI.ViewModels;
using ApplicationUI.Windows;
using System.IO;
using System.ServiceModel.Channels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

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
            ReturnImage.Source = new BitmapImage(new Uri($"{CD}Return.jpg", UriKind.Absolute));
            #endregion

            _signupPageVM = signupPageVM;
            this.DataContext = _signupPageVM;
        }
        private void signUpClick(object sender, MouseButtonEventArgs e)
        {
            SoundPlayer.PlayButtonSound();
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
            else if(!_signupPageVM.IsNicknameValid())
            {
                MessageBox.Show("Wrong nickname", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (!_signupPageVM.IsPasswordValid())
            {
                MessageBox.Show("Wrong password", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (!_signupPageVM.IsEmailValid())
            {
                MessageBox.Show("Wrong email", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (!_signupPageVM.IsPhoneValid())
            {
                MessageBox.Show("Wrong phone", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (!_signupPageVM.IsIconValid())
            {
                MessageBox.Show("Wrong icon", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TextBox_TextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void ReturnImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SoundPlayer.PlayButtonSound();
            _signupPageVM.ReturnToSignIn();
        }
    }
}
