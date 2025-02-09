﻿using ApplicationUI.Statics;
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
        private IUserService<BookDTO, UserDTO> _userService;
        public CommentsWindow(ParagraphDTO paragraph, IBookService<BookDTO, ParagraphDTO, UserCommentDTO> bookService, IUserService<BookDTO, UserDTO> userService)
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
                this.commentTB.Clear();
                Paragraph = this._bookService.GetParagraph(Paragraph.Id);
                UserCommentCollection = Paragraph.UserComments;
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
                
                if (CommentDescription.Contains("↵"))// If reply is to a reply
                {
                    CommentDescription = CommentDescription.Substring(CommentDescription.IndexOf("↵") + 2);
                }
                if(CommentDescription.Length > 25)// Shortening a long message
                {
                    CommentDescription = CommentDescription.Substring(0, 30) + "...";
                }

                string ReplyDescription = $"{clickedItem.User.Nickname}: {CommentDescription}↵\n";
                this.commentTB.Text = this.commentTB.Text.Insert(0, ReplyDescription);
            }
        }
    }
}
