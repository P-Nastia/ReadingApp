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
            //optionsBuilder.UseNpgsql("Host=3.68.89.123;Port=5022;Username=krotPostgres;Database=krotPostgres;Password=p%rSDj4Imds07djc**dmUntdOidd3dZ_#WTi4B9Zo;Pooling=true;");
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=postgres;User Id=postgres;Password=Nastia_^2008;");
        }
    }
}
