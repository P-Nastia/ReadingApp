﻿using ApplicationUI.Statics;
using ApplicationUI.ViewModels;
using ApplicationUI.Windows;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;

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
        private async void signUpClick(object sender, MouseButtonEventArgs e)
        {
            await SoundPlayer.PlayButtonSoundAsync();
            //оновлюю дані з text box
            _signupPageVM.Password = this.passwordPB.Password;
            _signupPageVM.Nickname = NicknameTB.Text;
            _signupPageVM.Email = EmailTB.Text;
            _signupPageVM.Phone = PhoneTB.Text;
            ResetTextBoxBorders();
            if (_signupPageVM.IsInputCorrect())
            {
                NicknameToolTip.IsOpen = false;
                EmailToolTip.IsOpen = false;
                PasswordToolTip.IsOpen = false;
                PhoneToolTip.IsOpen = false;
                IconToolTip.IsOpen = false;
                VerificationWindow verificationWindow = new VerificationWindow(_signupPageVM.Email);
                verificationWindow.ShowDialog();
                if (verificationWindow.isVerificated)
                {
                    await _signupPageVM.SignUp();
                }
            }
            else
            {

                if (!_signupPageVM.IsNicknameValid())
                {                
                    NicknameTB.BorderBrush = Brushes.Red;
                    NicknameTB.BorderThickness = new Thickness(2);
                    NicknameToolTip.IsOpen = true;
                }
                else
                {
                    NicknameToolTip.IsOpen = false;  // Закриваю ToolTip, якщо все вірно
                }
                if (!_signupPageVM.IsPasswordValid())
                {
                    passwordPB.BorderBrush = Brushes.Red;
                    passwordPB.BorderThickness = new Thickness(2);
                    PasswordToolTip.IsOpen = true;
                }
                else
                {
                    PasswordToolTip.IsOpen = false;
                }
                if (!_signupPageVM.IsEmailValid())
                {
                    EmailTB.BorderBrush = Brushes.Red;
                    EmailTB.BorderThickness = new Thickness(2);
                    EmailToolTip.IsOpen = true;
                }
                else
                {
                    EmailToolTip.IsOpen = false;
                }
                if (!_signupPageVM.IsPhoneValid())
                {
                    PhoneTB.BorderBrush = Brushes.Red;
                    PhoneTB.BorderThickness = new Thickness(2);
                    PhoneToolTip.IsOpen = true;
                }
                else
                {
                    PhoneToolTip.IsOpen = false;
                }
                if (!_signupPageVM.IsIconValid())
                {
                    IconBT.BorderBrush = Brushes.Red;
                    IconBT.BorderThickness = new Thickness(2);
                    IconToolTip.IsOpen = true;
                }
                else
                {
                    IconToolTip.IsOpen = false; 
                }
            }
        }

        private void TextBox_TextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private async void ReturnImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            await SoundPlayer.PlayButtonSoundAsync();
            _signupPageVM.ReturnToSignIn();
        }


        private void ResetTextBoxBorders()
        {
            NicknameTB.BorderBrush = Brushes.Gray;
            passwordPB.BorderBrush = Brushes.Gray;
            EmailTB.BorderBrush = Brushes.Gray;
            PhoneTB.BorderBrush = Brushes.Gray;
            IconBT.BorderBrush= Brushes.Gray;
        }

    }
}
