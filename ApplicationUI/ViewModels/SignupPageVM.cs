using ApplicationUI.Commands;
using ApplicationUI.Pages;
using ApplicationUI.Statics;
using BLL.Interfaces;
using BLL.ModelsDTO;
using EmailSender.Services;
using Microsoft.Win32;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using File = System.IO.File;

namespace ApplicationUI.ViewModels
{
    public class SignupPageVM : INotifyPropertyChanged
    {
        private IUserService<BookDTO, UserDTO , NotificationDTO> _userService;

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnNotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }



        public string Nickname { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Icon { get; set; }




        public BaseCommand PickIconCommand => new BaseCommand(execute => PickIcon(), canExecute => true);
        public SignupPageVM(IUserService<BookDTO, UserDTO, NotificationDTO> userService)
        {
            _userService = userService;
        }
        private void PickIcon()
        {
            SoundPlayer.PlayButtonSound();
            OpenFileDialog dialog = new OpenFileDialog() {Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png" };
            dialog.ShowDialog();
            if(!String.IsNullOrEmpty(dialog.FileName) && !String.IsNullOrWhiteSpace(dialog.FileName))
            {
                Icon = dialog.FileName;
            }
        }



        public async Task SignUp()
        {
            var user = _userService.FindSimiliar(Nickname, Password, Email);
            if (user == null)
            {
                UserDTO userDTO = new UserDTO()
                {
                    Password = BCrypt.Net.BCrypt.HashPassword(this.Password),
                    Nickname = this.Nickname,
                    Phone = this.Phone,
                    Email = this.Email,
                    Icon = File.ReadAllBytes(this.Icon),
                    Books = new List<BookDTO>()
                };
                await _userService.Add(userDTO);
                user = _userService.FindSimiliar(Nickname, Password, Email);
                if(user != null)
                {
                    StaticUser.IsLoggedIn = true;
                    StaticUser.User = user;
                    StaticUser.UserNeedsToSignUp = false;
                }
                if (StaticUser.IsLoggedIn == false)
                {
                    MessageBox.Show("User wasn`t registered", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    Nickname = string.Empty;
                    Password = string.Empty;
                    Email = string.Empty;
                    Phone = string.Empty;
                    OnNotifyPropertyChanged(nameof(Nickname));
                    OnNotifyPropertyChanged(nameof(Password));
                    OnNotifyPropertyChanged(nameof(Email));
                    OnNotifyPropertyChanged(nameof(Phone));
                }
            }
            else
            {
                MessageBox.Show("This user exists", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public bool IsInputCorrect()
        {
            return IsPasswordValid() && IsPhoneValid() && IsNicknameValid() && IsIconValid() && IsEmailValid();
        }


        public bool IsPasswordValid()
        {
            return !string.IsNullOrWhiteSpace(Password) &&
                   Password.Length >= 6 &&
                   Password.Any(char.IsDigit) &&
                   Password.Any(ch => char.IsLetterOrDigit(ch));
        }

        public bool IsPhoneValid()
        {
            return !string.IsNullOrWhiteSpace(Phone) &&
                   Phone.Length == 13 &&
                   Phone.StartsWith("+38");
        }

        public bool IsNicknameValid()
        {
            return !string.IsNullOrWhiteSpace(Nickname);
        }

        public bool IsIconValid()
        {
            return !string.IsNullOrWhiteSpace(Icon);
        }

        public bool IsEmailValid()
        {
            return EmailService.IsValidEmail(Email);
        }


        public void ReturnToSignIn()
        {
            StaticUser.UserNeedsToSignUp = false;   
        }


    }
}
