using ApplicationUI.Statics;
using BLL.Interfaces;
using BLL.ModelsDTO;
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
using System.Windows.Media.Imaging;

namespace ApplicationUI.ViewModels
{
    public class MyProfilePageVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private IUserService<BookDTO, UserDTO> _userService;

        private bool _canChangeNickname;
        public bool CanChangeNickname
        {
            get => _canChangeNickname;
            set
            {
                if(CanChangeNickname != value)
                {
                    _canChangeNickname = value;
                    OnNotifyPropertyChanged(nameof(CanChangeNickname));
                }
            }
        }
        public string Nickname
        {
            get => StaticUser.User?.Nickname;
            set
            {
                if(StaticUser.User != null && StaticUser.User.Nickname != value)
                {
                    StaticUser.User.Nickname = value;
                    OnNotifyPropertyChanged(nameof(Nickname));
                }
            }
        }

        private bool _canChangePassword;
        public bool CanChangePassword
        {
            get => _canChangePassword;
            set
            {
                if (CanChangePassword != value)
                {
                    _canChangePassword = value;
                    OnNotifyPropertyChanged(nameof(CanChangePassword));
                }
            }
        }
        public string Password
        {
            get => StaticUser.User?.Password;
            set
            {
                if (StaticUser.User != null && StaticUser.User.Password != value)
                {
                    StaticUser.User.Password = value;
                    OnNotifyPropertyChanged(nameof(Password));
                }
            }
        }

        private bool _canChangeEmail;
        public bool CanChangeEmail
        {
            get => _canChangeEmail;
            set
            {
                if (CanChangeEmail != value)
                {
                    _canChangeEmail = value;
                    OnNotifyPropertyChanged(nameof(CanChangeEmail));
                }
            }
        }
        public string Email
        {
            get => StaticUser.User?.Email;
            set
            {
                if (StaticUser.User != null && StaticUser.User.Email != value)
                {
                    StaticUser.User.Email = value;
                    OnNotifyPropertyChanged(nameof(Email));
                }
            }
        }

        private bool _canChangePhone;
        public bool CanChangePhone
        {
            get => _canChangePhone;
            set
            {
                if (CanChangePhone != value)
                {
                    _canChangePhone = value;
                    OnNotifyPropertyChanged(nameof(CanChangePhone));
                }
            }
        }
        public string Phone
        {
            get => StaticUser.User?.Phone;
            set
            {
                if (StaticUser.User != null && StaticUser.User.Phone != value)
                {
                    StaticUser.User.Phone = value;
                    OnNotifyPropertyChanged(nameof(Phone));
                }
            }
        }
        public void OnNotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public MyProfilePageVM(IUserService<BookDTO, UserDTO> userService)
        {
            _userService = userService;
            CanChangeNickname = false;
            CanChangeEmail = false;
            CanChangePassword = false;
            CanChangePhone = false;
        }
        
    }
}
