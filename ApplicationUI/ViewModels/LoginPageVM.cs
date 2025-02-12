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
            if (!IsInputCorrect())
            {
                MessageBox.Show("Please enter both nickname and password", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var user = _userService.GetAll().FirstOrDefault(u => u.Nickname == Nickname);

            if (user == null || !BCrypt.Net.BCrypt.Verify(Password, user.Password))
            {
                MessageBox.Show("Invalid nickname or password", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            StaticUser.User = user;
            StaticUser.IsLoggedIn = true;

            // Очищення введених даних після входу
            Nickname = string.Empty;
            Password = string.Empty;
            OnNotifyPropertyChanged("Nickname");
            OnNotifyPropertyChanged("Password");
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
