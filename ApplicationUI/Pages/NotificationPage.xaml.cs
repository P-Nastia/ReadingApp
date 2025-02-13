using ApplicationUI.Statics;
using ApplicationUI.ViewModels;
using BLL.ModelsDTO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ApplicationUI.Pages
{
    /// <summary>
    /// Interaction logic for NotificationPage.xaml
    /// </summary>
    public partial class NotificationPage : Page
    {
        private NotificationPageVM _notificationPageVM;
        public NotificationPage(NotificationPageVM notificationPageVM)
        {
            InitializeComponent();
            _notificationPageVM = notificationPageVM;
            this.DataContext = _notificationPageVM;
        }

        private void DeleteNotification(object sender, RoutedEventArgs e)
        {
            //_notificationPageVM.Selected = (NotificationDTO)NotificationList.SelectedItem;
            _notificationPageVM.DeleteNotification();
        }

        private void ClearAll_Click(object sender, RoutedEventArgs e)
        {
            foreach(var item in NotificationList.Items)
            {
                _notificationPageVM.Selected = (NotificationDTO)item;
                _notificationPageVM.DeleteNotification();
            }
        }

        private async void LoadHotLink(object sender, MouseButtonEventArgs e)
        {
            await SoundPlayer.PlayButtonSoundAsync();
            _notificationPageVM.HotLoadLink();
        }
    }
}
