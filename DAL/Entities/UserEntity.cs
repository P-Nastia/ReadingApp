using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{

    [Table("tbl_users")]
    public class UserEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Nickname { get; set; }
        [Required]
        [StringLength(20)]
        public string Phone { get; set; }
        [StringLength(100)]
        public string Email { get; set; }
        
        // The password encryption is long so I removed the restriction on length...
        //
        //[StringLength(255)] 
        //
        public string Password { get; set; }
        public ICollection<BookEntity> Books { get; set; }
        public byte[] Icon { get; set; }
    }
}
