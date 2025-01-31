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
        private IUserService<BookDTO, UserDTO> _userService;
        public string Nickname { get; set; }
        public string Password { get; set; }
        //public BaseCommand CommandLogin => new BaseCommand(execute => Login(), canExecute => Nickname != null && Password != null);
        public void OnNotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public LoginPageVM(IUserService<BookDTO, UserDTO> userService)
        {
            _userService = userService;
        }
        public void Login()
        {
            if (Nickname != null && Password != null)
            {
                var users = _userService.GetAll();
                foreach (var user in users)
                {
                    if (user.Password == Password && user.Nickname == Nickname)
                    {
                        StaticUser.User = user;
                        StaticUser.IsLoggedIn = true;
                        MessageBox.Show("Logged in");
                        break;
                    }
                }
            }
        }
        public void SignUp() {
            StaticUser.UserNeedsToSignUp = true;
        }
    }
}
