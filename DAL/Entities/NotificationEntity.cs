﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    [Table("tbl_notifications")]
    public class NotificationEntity
    {
        [Key]
        public int Id { get; set; }

        [StringLength(60)]
        public string Subject { get; set; }

        [StringLength(100)]
        public string Message { get; set; }

        [StringLength(100)]
        public string HotLoadLink { get; set; } // Instructions for the program to respond to user interaction
    }
}
