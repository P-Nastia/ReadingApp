using ApplicationUI.Commands;
using ApplicationUI.Pages;
using ApplicationUI.Statics;
using BLL.Interfaces;
using BLL.ModelsDTO;
using BLL.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

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
                var user = _userService.FindSimiliar(Nickname, Password, "", true);
                if (user != null)
                {
                    StaticUser.User = user;
                    StaticUser.IsLoggedIn = true;
                    Nickname = string.Empty;
                    Password = string.Empty;
                    OnNotifyPropertyChanged("Nickname");
                    OnNotifyPropertyChanged("Password");
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
