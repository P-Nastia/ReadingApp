﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        [ForeignKey("ParagraphId")]
        public int ParagraphId { get; set; }
        public ParagraphEntity Paragraph { get; set; }
        public DateTime Published { get; set; }
    }
}
