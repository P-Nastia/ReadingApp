using ApplicationUI.Commands;
using ApplicationUI.Statics;
using BLL.Interfaces;
using BLL.ModelsDTO;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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
        public BaseCommand ChangePasswordCommand => new BaseCommand(obj => ChangePassword(), canExecute => true);
        public BaseCommand ChangeNicknameCommand => new BaseCommand(obj => ChangeNickname(), canExecute => true);
        public BaseCommand ChangePhoneCommand => new BaseCommand(obj => ChangePhone(), canExecute => true);
        public BaseCommand ChangeEmailCommand => new BaseCommand(obj => ChangeEmail(), canExecute => true);
        public BaseCommand ChangeIconCommand => new BaseCommand(obj => ChangeIcon(), canExecute => true);

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

        public bool NewPasswordMode = false;
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
            get => StaticUser.User.Phone;
            set
            {
                if (StaticUser.User != null && StaticUser.User.Phone != value)
                {
                    StaticUser.User.Phone = value;
                    OnNotifyPropertyChanged(nameof(Phone));
                }
            }
        }
        public byte[] Icon
        {
            get {
                if (StaticUser.User.Icon != null && StaticUser.User != null)
                    return StaticUser.User.Icon;
                else
                {
                    var path = Directory.GetCurrentDirectory().Split('\\');
                    string newPath="";
                    for(int i=0;i<path.Count() - 3; i++)
                    {
                        newPath += path[i]+"\\";
                    }
                    return File.ReadAllBytes($"{newPath}Images\\myProfile.png");
                }
                
            }
            set
            {
                if(value != StaticUser.User.Icon && StaticUser.User != null)
                {
                    StaticUser.User.Icon = value;
                    OnNotifyPropertyChanged(nameof(Icon));
                }
            }
        }
        private async void ChangePassword()
        {
            SoundPlayer.PlayButtonSound();
            if (CanChangePassword == false)
            {
                CanChangePassword = true;
                Password = "Enter Old Password";
                OnNotifyPropertyChanged(nameof(CanChangePassword));
            }
            else
            {
                if (!NewPasswordMode)
                {
                    var user = _userService.GetAll().Where(X=>X.Nickname==Nickname).FirstOrDefault();
                    if (user != null && BCrypt.Net.BCrypt.Verify(Password, user.Password))
                    {
                        Password = "Enter New Password";
                        NewPasswordMode = true;
                    }
                    else
                    {
                        MessageBox.Show("Old Password not verified!");
                        Password = "";
                    }
                }
                else
                {
                    NewPasswordMode = false;
                    CanChangePassword = false;
                    OnNotifyPropertyChanged(nameof(CanChangePassword));
                    StaticUser.User.Password = BCrypt.Net.BCrypt.HashPassword(Password);
                    await _userService.UpdateUser(StaticUser.User);
                    Password = "...";
                }
            }
        }
        private async void ChangeNickname()
        {
            SoundPlayer.PlayButtonSound();
            if (CanChangeNickname == false)
            {
                CanChangeNickname = true;
                OnNotifyPropertyChanged(nameof(CanChangeNickname));
            }
            else
            {
                CanChangeNickname = false;
                OnNotifyPropertyChanged(nameof(CanChangeNickname));
                StaticUser.User.Nickname = Nickname;
                await _userService.UpdateUser(StaticUser.User);
            }
        }
        private async void ChangePhone()
        {
            SoundPlayer.PlayButtonSound();
            if (CanChangePhone == false)
            {
                CanChangePhone = true;
                OnNotifyPropertyChanged(nameof(CanChangePhone));
            }
            else
            {
                CanChangePhone = false;
                OnNotifyPropertyChanged(nameof(CanChangePhone));
                StaticUser.User.Phone = Phone;
                await _userService.UpdateUser(StaticUser.User);
            }
        }
        private async void ChangeEmail()
        {
            SoundPlayer.PlayButtonSound();
            if (CanChangeEmail == false)
            {
                CanChangeEmail = true;
                OnNotifyPropertyChanged(nameof(CanChangeEmail));
            }
            else
            {
                CanChangeEmail = false;
                OnNotifyPropertyChanged(nameof(CanChangeEmail));
                StaticUser.User.Email = Email;
                await _userService.UpdateUser(StaticUser.User);
            }
        }
        private async void ChangeIcon()
        {
            SoundPlayer.PlayButtonSound();
            OpenFileDialog dialog = new OpenFileDialog() { Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png" };
            dialog.ShowDialog();
            if (!String.IsNullOrEmpty(dialog.FileName) && !String.IsNullOrWhiteSpace(dialog.FileName))
            {
                StaticUser.User.Icon = await File.ReadAllBytesAsync(dialog.FileName);
                await _userService.UpdateUser(StaticUser.User);
                OnNotifyPropertyChanged(nameof(Icon));
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
