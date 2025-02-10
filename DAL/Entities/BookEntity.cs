using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    [Table("tbl_books")]
    public class BookEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        [Required]
        [StringLength(200)]
        public string Author { get; set; }
        public ICollection<ChapterEntity> Chapters { get; set; } 
        public ICollection<UserEntity> Users { get; set; }
        public string CoverURL { get; set; }
    }
}
