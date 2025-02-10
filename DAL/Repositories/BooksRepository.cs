using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class BooksRepository : IBookRepository<BookEntity, ParagraphEntity,UserCommentEntity>
    {
        public AppDBContext dbContext;
        public BooksRepository(AppDBContext dBContext) { dbContext = dBContext; }

        public void AddBook(BookEntity item)
        {
            AppDBContext tempDB = new AppDBContext();
            tempDB.Books.Add(item);
            tempDB.SaveChanges();
        }

        public async Task AddComment(UserCommentEntity itemToAdd)
        {
            AppDBContext tempDB = new AppDBContext();
            var paragraph = await tempDB.Paragraphs.Include(p => p.UserComments).FirstOrDefaultAsync(p => p.Id == itemToAdd.ParagraphId);

            if (paragraph != null)
            {
                await tempDB.UsersComments.AddAsync(itemToAdd);
                paragraph.UserComments.Add(itemToAdd);
                await tempDB.SaveChangesAsync();

            }
        }

        public async Task DeleteComment(UserCommentEntity item)
        {
            AppDBContext tempDB = new AppDBContext();
            var paragraph = await tempDB.Paragraphs.Include(p => p.UserComments).FirstOrDefaultAsync(p => p.UserComments.Any(uc => uc.Id == item.Id));
            if (paragraph != null)
            {
                var commentToRemove = paragraph.UserComments.FirstOrDefault(uc => uc.Id == item.Id);

                if (commentToRemove != null)
                {
                    paragraph.UserComments.Remove(commentToRemove);

                    tempDB.UsersComments.Remove(commentToRemove);

                    await tempDB.SaveChangesAsync();
                }
            }
        }

        public IQueryable<BookEntity> GetAll()
        {
            AppDBContext tempDB = new AppDBContext();
            return tempDB.Books.Include(b => b.Users).ThenInclude(u => u.Books).Include(b => b.Chapters).ThenInclude(c=>c.Paragraphs).ThenInclude(p => p.UserComments).ThenInclude(uc => uc.User)
                .Include(b => b.Users).ThenInclude(u => u.Books).Include(b => b.Chapters).ThenInclude(b => b.Paragraphs).ThenInclude(p => p.UserComments).ThenInclude(uc => uc.Paragraph);
        }
    }
}
