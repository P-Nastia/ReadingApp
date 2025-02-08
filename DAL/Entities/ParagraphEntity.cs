using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

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
