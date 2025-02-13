using ApplicationUI.Statics;
using ApplicationUI.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace ApplicationUI.Pages
{
    /// <summary>
    /// Interaction logic for MyProfilePage.xaml
    /// </summary>
    public partial class MyProfilePage : Page
    {
        private MyProfilePageVM _myProfilePageVM;
        public MyProfilePage(MyProfilePageVM myProfilePageVM)
        {
            InitializeComponent();
            _myProfilePageVM = myProfilePageVM;
            this.DataContext = myProfilePageVM;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _myProfilePageVM.Nickname = StaticUser.User.Nickname;
            _myProfilePageVM.Password = "..."; // Nothing to see here
            _myProfilePageVM.Email = StaticUser.User.Email;
            _myProfilePageVM.Phone = StaticUser.User.Phone;
            _myProfilePageVM.Icon = await ServerService.DownloadImageBytesAsync(StaticUser.User.Icon);
            _myProfilePageVM.OnNotifyPropertyChanged("Nickname");
            _myProfilePageVM.OnNotifyPropertyChanged("Password");
            _myProfilePageVM.OnNotifyPropertyChanged("Email");
            _myProfilePageVM.OnNotifyPropertyChanged("Phone");
            _myProfilePageVM.OnNotifyPropertyChanged("Icon");
        }
    }
}
