using ApplicationUI.Commands;
using ApplicationUI.Statics;
using BLL.Interfaces;
using BLL.ModelsDTO;
using Microsoft.Win32;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ApplicationUI.ViewModels
{
    public class MyProfilePageVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private IUserService<BookDTO, UserDTO, NotificationDTO> _userService;
        public BaseCommand ChangePasswordCommand => new BaseCommand(obj => ChangePassword(), canExecute => true);
        public BaseCommand ChangeNicknameCommand => new BaseCommand(obj => ChangeNickname(), canExecute => true);
        public BaseCommand ChangePhoneCommand => new BaseCommand(obj => ChangePhone(), canExecute => true);
        public BaseCommand ChangeEmailCommand => new BaseCommand(obj => ChangeEmail(), canExecute => true);
        public BaseCommand ChangeIconCommand => new BaseCommand(obj => ChangeIcon(), canExecute => true);

        private bool _canChangeNickname;
        private byte[] _icon;
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
            get
            {
                if (_icon == null)
                {
                    LoadIconAsync();
                }
                return _icon;
            }
            set
            {
                if (_icon != value && StaticUser.User != null)
                {
                    _icon = value;
                    OnNotifyPropertyChanged(nameof(Icon));
                }
            }
        }
        private async void LoadIconAsync()
        {
            if (StaticUser.User.Icon != null && StaticUser.User != null)
            {
                Icon = await ServerService.DownloadImageBytesAsync(StaticUser.User.Icon);
            }
            else
            {
                var path = Directory.GetCurrentDirectory().Split('\\');
                string newPath = string.Join("\\", path.Take(path.Length - 3)) + "\\Images\\myProfile.png";
                Icon = File.ReadAllBytes(newPath);
            }
        }
        private async void ChangePassword()
        {
            await SoundPlayer.PlayButtonSoundAsync();
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
            await SoundPlayer.PlayButtonSoundAsync();
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
            await SoundPlayer.PlayButtonSoundAsync();
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
            await SoundPlayer.PlayButtonSoundAsync();
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
            await SoundPlayer.PlayButtonSoundAsync();
            OpenFileDialog dialog = new OpenFileDialog() { Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png" };
            dialog.ShowDialog();
            if (!String.IsNullOrEmpty(dialog.FileName) && !String.IsNullOrWhiteSpace(dialog.FileName))
            {
                var imageUrl = (await ServerService.UploadImageAsync(dialog.FileName)).ImageUrl;
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    StaticUser.User.Icon = imageUrl;
                    await _userService.UpdateUser(StaticUser.User);
                    Icon = await ServerService.DownloadImageBytesAsync(imageUrl);
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
        public MyProfilePageVM(IUserService<BookDTO, UserDTO, NotificationDTO> userService)
        {
            _userService = userService;
            CanChangeNickname = false;
            CanChangeEmail = false;
            CanChangePassword = false;
            CanChangePhone = false;
        }
        
    }
}
