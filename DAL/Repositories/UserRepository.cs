using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class UserRepository : IUserRepository<BookEntity,UserEntity>
    {
        private AppDBContext _dbContext;
        public UserRepository(AppDBContext dBContext) { _dbContext = dBContext; }

        public async Task Add(UserEntity item)
        {
            await _dbContext.Users.AddAsync(item);
            await _dbContext.SaveChangesAsync();

        }

        public async Task AddBook(UserEntity userEntity, BookEntity entity)
        {
            AppDBContext tempDB = new AppDBContext();
            var user = await tempDB.Users.Include(u=>u.Books).FirstOrDefaultAsync(u => u.Id == userEntity.Id);
            var book = await tempDB.Books.Include(b=>b.Users).Include(p=>p.Paragraphs).ThenInclude(p=>p.UserComments).FirstOrDefaultAsync(b => b.Name == entity.Name && b.Author == entity.Author);
            if (user != null && book != null)
            {
                user.Books.Add(book);
                book.Users.Add(user);
                await tempDB.SaveChangesAsync();
            }
        }

        public IQueryable<UserEntity> GetAll()
        {
            lock (this)
            {
                AppDBContext tempDB = new AppDBContext();
                return tempDB.Users.Include(u => u.Books);
            }
        }

        public BookEntity GetBook(UserEntity entity, int id)
        {
            AppDBContext tempDB = new AppDBContext();
            var book = tempDB.Books.Include(b=>b.Paragraphs).ThenInclude(p=>p.UserComments).ThenInclude(uc=>uc.User).Include(b=>b.Users).FirstOrDefault(b => b.Id == id && b.Users.Contains(entity));
            return book;
        }

        public UserEntity GetById(int id)
        {
            lock (this)
            {
                AppDBContext tempDB = new AppDBContext();
                var user = tempDB.Users.Include(u=>u.Books).ThenInclude(b=>b.Paragraphs).ThenInclude(b=>b.UserComments).ThenInclude(uc=>uc.User).Include(u => u.Books).ThenInclude(b => b.Users).FirstOrDefault(u => u.Id == id);
                return user;
            }
        }

        public async Task Remove(UserEntity item)
        {
            AppDBContext tempDB = new AppDBContext();
            var user = await tempDB.Users.FirstOrDefaultAsync(i => i.Id == item.Id);
            if (user != null)
            {
                foreach (var book in user.Books)
                    user.Books.Remove(book);
                tempDB.Users.Remove(user);
                await tempDB.SaveChangesAsync();
            }
        }

        public async Task RemoveBook(UserEntity userEntity, BookEntity entity)
        {
            AppDBContext tempDB = new AppDBContext();
            var user = await tempDB.Users.Include(u=>u.Books).FirstOrDefaultAsync(i => i.Id == userEntity.Id);
            var book = await tempDB.Books.Include(b => b.Users).Include(b => b.Paragraphs).FirstOrDefaultAsync(i => i.Id == entity.Id);
            if (user != null && book != null)
            {
                user.Books.Remove(book);
                book.Users.Remove(user);
                await tempDB.SaveChangesAsync();
            }
        }

        public async Task UpdateUser(UserEntity user)
        {
            AppDBContext tempDB = new AppDBContext();
            var userFromDB = await tempDB.Users.Include(u => u.Books).ThenInclude(b => b.Paragraphs).ThenInclude(p => p.UserComments).FirstOrDefaultAsync(i => i.Id == user.Id);
            if (userFromDB != null)
            {
                userFromDB.Password = user.Password;
                userFromDB.Phone = user.Phone;
                userFromDB.Email = user.Email;
                userFromDB.Nickname = user.Nickname;
                userFromDB.Icon = user.Icon;
                await tempDB.SaveChangesAsync();
            }
        }
    }
}
