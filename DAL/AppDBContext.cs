using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class AppDBContext : DbContext
    {
        public DbSet<ParagraphEntity> Paragraphs { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<BookEntity> Books { get; set; }
        public DbSet<UserCommentEntity> UsersComments { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=ep-aged-flower-a2ncptd2-pooler.eu-central-1.aws.neon.tech;Database=ReadAppDB;Username=neondb_owner;Password=npg_s4djBoW3QPgw");
        }
    }
}
