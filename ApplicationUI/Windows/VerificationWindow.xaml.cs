using ApplicationUI.Statics;
using EmailSender.Services;
using System.Windows;

namespace ApplicationUI.Windows
{
    /// <summary>
    /// Interaction logic for VerificationWindow.xaml
    /// </summary>
    public partial class VerificationWindow : Window
    {
        private string _verificationCode;
        public bool isVerificated;

        public VerificationWindow(string email)
        {
            InitializeComponent();
            isVerificated = false;
            var random = new Random();
            for (int i = 0; i < 6; i++)
                _verificationCode += random.Next(0, 10).ToString();


            EmailService.SendEmail(email, "Verification", $"Your verification code is: {_verificationCode}");
        }
        private async void Confirm_Click(object sender, RoutedEventArgs e)
        {
            await SoundPlayer.PlayButtonSoundAsync();
            if (codeTB.Text == _verificationCode)
            {
                MessageBox.Show("Success", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                isVerificated = true;
                this.Close();
            }
            else
                MessageBox.Show("Incorrect code", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
