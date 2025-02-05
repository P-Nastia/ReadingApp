using ApplicationUI.Statics;
using ApplicationUI.ViewModels;
using BLL.ModelsDTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        public void SetImageSource(object sender, RoutedEventArgs e)
        {
            var imageControl = (Image)sender;

            if (StaticUser.User.Icon != null)
            {
                using (MemoryStream ms = new MemoryStream(StaticUser.User.Icon))
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = ms;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    imageControl.Source = bitmap;
                }
            }
            _myProfilePageVM.Nickname = StaticUser.User.Nickname;
            _myProfilePageVM.Password = StaticUser.User.Password;
            _myProfilePageVM.Email = StaticUser.User.Email;
            _myProfilePageVM.Phone = StaticUser.User.Phone;
            _myProfilePageVM.OnNotifyPropertyChanged("Nickname");
            _myProfilePageVM.OnNotifyPropertyChanged("Password");
            _myProfilePageVM.OnNotifyPropertyChanged("Email");
            _myProfilePageVM.OnNotifyPropertyChanged("Phone");
        }
    }
}
