using ApplicationUI.Statics;
using BLL.Interfaces;
using BLL.ModelsDTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ParagraphDTO Paragraph
        {
            get { return _paragraphDTO; }
            set
            {
                if (value != null) 
                {
                    _paragraphDTO = value;
                    UserCommentCollection = _paragraphDTO.UserComments;
                }
                
            }
        }
        private ParagraphDTO _paragraphDTO;
        private List<UserCommentDTO> _userCommentCollection;
        public List<UserCommentDTO> UserCommentCollection
        {
            get => _userCommentCollection;
            set
            {
                if(_userCommentCollection != value)
                {
                    _userCommentCollection = value;
                    OnNotifyPropertyChanged(nameof(UserCommentCollection));
                }
            }
        }


        private IBookService<BookDTO, ParagraphDTO, UserCommentDTO> _bookService;
        private IUserService<BookDTO, UserDTO, NotificationDTO> _userService;
        public CommentsWindow(ParagraphDTO paragraph, IBookService<BookDTO, ParagraphDTO, UserCommentDTO> bookService, IUserService<BookDTO, UserDTO, NotificationDTO> userService)
        {
            InitializeComponent();

            #region ImageConfig 
            // Setting images/Icons
            string CD = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\Images\\"; // Maybe there is a better way to get the project directory...
            AddCommentImage.Source = new BitmapImage(new Uri($"{CD}addComment.png", UriKind.Absolute));
            #endregion

            _bookService = bookService;
            _userService = userService;
            this.DataContext = this;
            Paragraph = this._bookService.GetParagraph(paragraph.Id);
            UserCommentCollection = Paragraph.UserComments;
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

        private async void SendComment(object sender, MouseButtonEventArgs e)
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
                await this._bookService.AddComment(uc);

                NotifyCommentReply(this.commentTB.Text);
                this.commentTB.Clear();
                Paragraph = this._bookService.GetParagraph(Paragraph.Id);
                UserCommentCollection = Paragraph.UserComments;
            }
        }

        private void NotifyCommentReply(string reply)
        {
            if (commentTB.Text.Contains("↵"))
            {
                //Verifying reply...
                int MesText = commentTB.Text.IndexOf(":");
                int ReplyEnd = commentTB.Text.IndexOf("↵");
                string UserNickname = commentTB.Text.Substring(0,MesText);
                string Message = commentTB.Text.Substring(MesText+2, (ReplyEnd - MesText) - 2);
                int ShortMesLength = Message.Length > 25? 25 : Message.Length-1;

                var Original = UserCommentCollection.Where(X=>{
                    string XComment = X.Comment;
                    if (XComment.Contains("↵")) // Remove X Reply message as it is not included in the current reply message
                    {
                        XComment = XComment.Substring(XComment.IndexOf("↵") + 1);
                    }

                    if (X.User.Nickname == UserNickname && XComment.Length>=ShortMesLength && XComment.Substring(0, ShortMesLength) == Message.Substring(0, ShortMesLength))
                    {
                        return true;
                    }
                    return false;
                }).FirstOrDefault();

                if (Original != null)// When Verified
                {
                    // Sending Notification
                    string LoadLink = $"Comment:{_paragraphDTO.Chapter.Book.Name}/{_paragraphDTO.Chapter.Book.Author}/{_paragraphDTO.Chapter.Name}/{_paragraphDTO.Id}";
                    _userService.AddNotification(Original.User, new NotificationDTO() { Subject = "Someone Replied to your comment!", Message = $"{StaticUser.User.Nickname} replied to your comment: {Message}", HotLoadLink=LoadLink });
                }
            }
        }

        private async void DeleteComment(object sender, RoutedEventArgs e)
        {
            var listBox = CommentList;
            var clickedItem = (UserCommentDTO)listBox.SelectedItem;
            if (clickedItem.UserId == StaticUser.User.Id && clickedItem != null)
            {
                MessageBoxResult result = MessageBox.Show("Do you want to delete comment?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    await _bookService.DeleteComment(clickedItem);
                    Paragraph = this._bookService.GetParagraph(Paragraph.Id);
                    UserCommentCollection = Paragraph.UserComments;
                }
            }
            else
            {
                MessageBox.Show("You can delete only your comments", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ReplyComment(object sender, RoutedEventArgs e)
        {
            var listBox = CommentList;
            var clickedItem = (UserCommentDTO)listBox.SelectedItem;
            if (clickedItem != null) {
                string CommentDescription = clickedItem.Comment;

                if (commentTB.Text.Contains("↵"))
                {
                    commentTB.Text = commentTB.Text.Substring(commentTB.Text.IndexOf("↵") + 1); // Clears any other reply text, limiting the user to only reply to one message.
                }
                if (CommentDescription.Contains("↵"))// If reply is to a reply
                {
                    CommentDescription = CommentDescription.Substring(CommentDescription.IndexOf("↵") + 1);
                }
                if(CommentDescription.Length > 25)// Shortening a long message
                {
                    CommentDescription = CommentDescription.Substring(0, 30) + "...";
                }

                string ReplyDescription = $"{clickedItem.User.Nickname}: {CommentDescription}↵";
                this.commentTB.Text = this.commentTB.Text.Insert(0, ReplyDescription);
            }
        }
    }
}
