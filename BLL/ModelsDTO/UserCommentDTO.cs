using System.ComponentModel.DataAnnotations.Schema;

namespace BLL.ModelsDTO
{
    public class UserCommentDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public UserDTO User { get; set; }
        public string Comment { get; set; }
        public int ParagraphId { get; set; }
        public ParagraphDTO Paragraph { get; set; }
        public DateTime Published { get; set; }
    }
}