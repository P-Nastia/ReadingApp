using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{

    [Table("tbl_paragraphs")]
    public class ParagraphEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(10000)]
        public string Text { get; set; }
        [ForeignKey("Chapter")]
        public int ChapterId { get; set; }
        public ChapterEntity Chapter { get; set; }
        public ICollection<UserCommentEntity> UserComments { get; set; }
    }
}
