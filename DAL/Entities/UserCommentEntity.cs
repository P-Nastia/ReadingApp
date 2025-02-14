using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    [Table("tbl_usersComments")]
    public class UserCommentEntity
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public UserEntity User { get; set; }
        public string Comment { get; set; }
        [ForeignKey("Paragraph")]
        public int ParagraphId { get; set; }
        public ParagraphEntity Paragraph { get; set; }
        public DateTime Published { get; set; }
    }
}
