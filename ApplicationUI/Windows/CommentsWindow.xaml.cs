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
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ApplicationUI.Windows
{
    /// <summary>
    /// Interaction logic for CommentsWindow.xaml
    /// </summary>
    public partial class CommentsWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnNotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                
            }
        }
        public ParagraphDTO Paragraph { get; set; }
        public ICollection<UserCommentDTO> UserCommentCollection { get; set; }
        private IBookService<BookDTO, ParagraphDTO, UserCommentDTO> _bookService;
        private IUserService<BookDTO, UserDTO> _userService;
        public CommentsWindow(ParagraphDTO paragraph, IBookService<BookDTO, ParagraphDTO, UserCommentDTO> bookService, IUserService<BookDTO, UserDTO> userService)
        {
            InitializeComponent();
            _bookService = bookService;
            _userService = userService;
            this.DataContext = this;
            Paragraph = paragraph;
            UserCommentCollection = paragraph.UserComments;
            OnNotifyPropertyChanged("UserCommentCollection");
        }
        private void SetImageSource(object sender, RoutedEventArgs e)
        {
            var item = (UserCommentDTO)((FrameworkElement)sender).DataContext;
            var imageControl = (Image)sender;

            if (item.User.Icon != null)
            {
                using (MemoryStream ms = new MemoryStream(item.User.Icon))
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = ms;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    imageControl.Source = bitmap; 
                }
            }
        }

        private void SendComment(object sender, MouseButtonEventArgs e)
        {
            
            if (!String.IsNullOrEmpty(this.commentTB.Text) && !String.IsNullOrWhiteSpace(this.commentTB.Text))
            {
                
                UserCommentDTO uc = new UserCommentDTO()
                {
                    ParagraphId = Paragraph.Id,
                    Comment = this.commentTB.Text,
                    Published = DateTime.Now.ToUniversalTime(),
                    UserId = StaticUser.User.Id
                };
                this._bookService.AddComment(uc);
                this.commentTB.Clear();
                Paragraph = this._bookService.GetBook(Paragraph.BookId).Paragraphs.Where(p => p.Id == Paragraph.Id).FirstOrDefault();
                UserCommentCollection = Paragraph.UserComments;
                OnNotifyPropertyChanged("UserCommentCollection");
                //Thread.Sleep(3000);
            }
        }

        private void DeleteComment(object sender, MouseButtonEventArgs e)
        {
            var listBox = (ListBox)sender;
            var clickedItem = (UserCommentDTO)listBox.SelectedItem;
            if (clickedItem.UserId == StaticUser.User.Id && clickedItem != null)
            {
                MessageBoxResult result = MessageBox.Show("Do you want to delete comment?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    _bookService.DeleteComment(clickedItem);
                    Paragraph = this._bookService.GetBook(Paragraph.BookId).Paragraphs.Where(p => p.Id == Paragraph.Id).FirstOrDefault();
                    UserCommentCollection = Paragraph.UserComments;
                    OnNotifyPropertyChanged("UserCommentCollection");
                    //Thread.Sleep(3000);
                }
            }
            else
            {
                MessageBox.Show("You can delete only your comments", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
