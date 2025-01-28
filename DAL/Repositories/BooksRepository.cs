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
        private AppDBContext _dbContext;
        public BooksRepository(AppDBContext dBContext) { _dbContext = dBContext; }

        public async Task AddBook(BookEntity item)
        {
            await _dbContext.Books.AddAsync(item);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddComment(ParagraphEntity itemTo, UserCommentEntity itemToAdd)
        {
            var paragraph = await _dbContext.Paragraphs.Include(p => p.UserComments).FirstOrDefaultAsync(p => p.Id == itemTo.Id); 

            var book = await _dbContext.Books.FirstOrDefaultAsync(b => b.Id == itemTo.BookId);

            if (paragraph != null && book != null)
            {
                await _dbContext.UsersComments.AddAsync(itemToAdd);
                var comment = await _dbContext.UsersComments.Where(uc => uc.Comment == itemToAdd.Comment && uc.UserId == itemToAdd.UserId).FirstOrDefaultAsync();
                if (comment != null)
                {
                    paragraph.UserComments.Add(comment);
                    await _dbContext.SaveChangesAsync();
                }
            }
        }

        public async Task DeleteComment(UserCommentEntity item)
        {
            var paragraph = await _dbContext.Paragraphs.Include(p => p.UserComments).FirstOrDefaultAsync(p => p.UserComments.Any(uc=>uc.Id==item.Id));
            if(paragraph != null)
            {
                var commentToRemove = paragraph.UserComments.FirstOrDefault(uc => uc.Id == item.Id);

                if (commentToRemove != null)
                {
                    paragraph.UserComments.Remove(commentToRemove);

                    _dbContext.UsersComments.Remove(commentToRemove);

                    await _dbContext.SaveChangesAsync();
                }
            }
        }

        public BookEntity GetBook(int id)
        {
            var book = _dbContext.Books
            .Where(b => b.Id == id)
            .Include(b => b.Paragraphs)
                .ThenInclude(p => p.UserComments)
                    .ThenInclude(uc => uc.User)  
            .FirstOrDefault();

            return book;
        }

    }
}
