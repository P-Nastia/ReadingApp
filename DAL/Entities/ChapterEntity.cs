using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    [Table("tbl_chapters")]
    public class ChapterEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(150)]
        public string Name { get; set; }
        public int BookId { get; set; }
        public BookEntity Book { get; set; }
        public ICollection<ParagraphEntity> Paragraphs { get; set; }
    }
}
