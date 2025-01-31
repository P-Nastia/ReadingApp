using ApplicationUI.Commands;
using ApplicationUI.Pages;
using ApplicationUI.Statics;
using BLL.Interfaces;
using BLL.ModelsDTO;
using BLL.Services;
using Microsoft.Win32;
using OpenQA.Selenium.DevTools.V132.FileSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
            OpenFileDialog dialog = new OpenFileDialog() {Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png" };
            dialog.ShowDialog();
            if(!String.IsNullOrEmpty(dialog.FileName) && !String.IsNullOrWhiteSpace(dialog.FileName))
            {
                Icon = dialog.FileName;
            }
        }
        public void SignUp()
        {
            if(IsInputCorrect() == true)
            {
                var users = _userService.GetAll();
                foreach (var user in users)
                {
                    if (user.Password == Password && user.Nickname == Nickname)
                    {
                        MessageBox.Show("This user exists", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                    }
                }
                UserDTO userDTO = new UserDTO()
                {
                    Password = this.Password,
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
                    if (user.Password == Password && user.Nickname == Nickname)
                    {
                        StaticUser.User = user;
                        StaticUser.IsLoggedIn = true;
                        break;
                    }
                }
                if (StaticUser.IsLoggedIn == false)
                {
                    MessageBox.Show("User wasn`t registered", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Wrong input", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private bool IsInputCorrect()
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
            return true;
        }
    }
}
