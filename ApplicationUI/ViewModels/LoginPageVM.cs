using ApplicationUI.Statics;
using BLL.Interfaces;
using BLL.ModelsDTO;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ApplicationUI.ViewModels
{
    public class LoginPageVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private IUserService<BookDTO, UserDTO, NotificationDTO> _userService;
        public string Nickname { get; set; }
        public string Password { get; set; }
        public void OnNotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public LoginPageVM(IUserService<BookDTO, UserDTO, NotificationDTO> userService)
        {
            _userService = userService;
        }
        public async Task Login()
        {
            if (IsInputCorrect())
            {
                var user = _userService.FindSimiliar(Nickname, Password,"");
                if (user != null)
                {
                    if (user.Nickname == Nickname && BCrypt.Net.BCrypt.Verify(Password, user.Password))
                    {
                        StaticUser.User = user;
                        StaticUser.IsLoggedIn = true;
                        Nickname = string.Empty;
                        Password = string.Empty;
                        OnNotifyPropertyChanged("Nickname");
                        OnNotifyPropertyChanged("Password");
                    }
                }
                if(StaticUser.IsLoggedIn == false)
                {
                    MessageBox.Show("User doesn`t exist", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    Nickname = string.Empty;
                    Password = string.Empty;
                    OnNotifyPropertyChanged("Nickname");
                    OnNotifyPropertyChanged("Password");
                }
            }
        }


        public bool IsInputCorrect()
        {
            return IsNicknameLoginValid() && IsPasswordLoginValid();
        }

        public bool IsNicknameLoginValid()
        {
            return !string.IsNullOrWhiteSpace(Nickname);
        }

        public bool IsPasswordLoginValid()
        {
            return !string.IsNullOrWhiteSpace(Password);
        }


        public void SignUp() {
            StaticUser.UserNeedsToSignUp = true;
        }
    }
}
