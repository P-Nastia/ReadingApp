using ApplicationUI.Commands;
using ApplicationUI.Statics;
using BLL.Interfaces;
using BLL.ModelsDTO;
using EmailSender.Services;
using Microsoft.Win32;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using File = System.IO.File;

namespace ApplicationUI.ViewModels
{
    public class SignupPageVM : INotifyPropertyChanged
    {
        private IUserService<BookDTO, UserDTO> _userService;

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
        public SignupPageVM(IUserService<BookDTO, UserDTO> userService)
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
        public async void SignUp()
        {
            if(IsInputCorrect() == true)
            {
                var users = _userService.GetAll();

                bool isUnique = true;
                foreach (var user in users)
                {
                    if (BCrypt.Net.BCrypt.Verify(Password,user.Password) && user.Nickname == Nickname)
                    {
                        MessageBox.Show("This user exists", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                        isUnique = false;
                        break;
                    }
                }
                if (isUnique == true)
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
                    _userService.Add(userDTO);
                    users = _userService.GetAll();
                    foreach (var user in users)
                    {
                        if (BCrypt.Net.BCrypt.Verify(Password, user.Password) && user.Nickname == Nickname)
                        {
                            StaticUser.User = user;
                            StaticUser.IsLoggedIn = true;
                            StaticUser.UserNeedsToSignUp = false;
                            break;
                        }
                    }
                    if (StaticUser.IsLoggedIn == false)
                    {
                        MessageBox.Show("User wasn`t registered", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Wrong input", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public bool IsInputCorrect()
        {
            if (String.IsNullOrEmpty(Password) || String.IsNullOrWhiteSpace(Password))
                return false;
            if (String.IsNullOrEmpty(Phone) || String.IsNullOrWhiteSpace(Phone) || Phone[0] != '+' || Phone[1] != '3' || Phone[2] != '8' || Phone.Length != 13)
                return false;
            if (String.IsNullOrEmpty(Nickname) || String.IsNullOrWhiteSpace(Nickname))
                return false;
            if (String.IsNullOrEmpty(Icon) || String.IsNullOrWhiteSpace(Icon))
                return false;
            if (String.IsNullOrEmpty(Nickname) || String.IsNullOrWhiteSpace(Nickname))
                return false;
            if (!EmailService.IsValidEmail(Email))
                return false;
            return true;
        }
    }
}
