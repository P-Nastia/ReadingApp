namespace BLL.ModelsDTO
{
    public class ParagraphDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int ChapterId { get; set; }
        public ChapterDTO Chapter { get; set; }
        public List<UserCommentDTO> UserComments { get; set; }
        public string DisplayText => Text;
    }
}