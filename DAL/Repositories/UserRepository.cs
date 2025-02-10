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
    public class UserRepository : IUserRepository<BookEntity,UserEntity,NotificationEntity>
    {
        public AppDBContext _dbContext;
        public UserRepository(AppDBContext dBContext) { _dbContext = dBContext;}

        public async Task Add(UserEntity item)
        {
            await _dbContext.Users.AddAsync(item);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddBook(UserEntity userEntity, BookEntity entity)
        {
            AppDBContext tempDB = new AppDBContext();
            var user = await tempDB.Users.Include(u=>u.Books).FirstOrDefaultAsync(u => u.Id == userEntity.Id);
            var book = await tempDB.Books.Include(b=>b.Users).FirstOrDefaultAsync(b => b.Id == entity.Id);
            if (user != null && book != null)
            {
                user.Books.Add(book);
                book.Users.Add(user);
                await tempDB.SaveChangesAsync();
            }
        }

        public async Task AddNotification(UserEntity userEntity, NotificationEntity entity)
        {
            //AppDBContext tempDB = new AppDBContext();
            var user = await _dbContext.Users.Include(u => u.Notifications).FirstOrDefaultAsync(u => u.Id == userEntity.Id);
            if (user != null)
            {
                await _dbContext.Notifications.AddAsync(entity);
                user.Notifications.Add(entity);
                UpdateUser(user);
                //await _dbContext.SaveChangesAsync();
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
            var book = await tempDB.Books.Include(b => b.Users).Include(b => b.Chapters).ThenInclude(b => b.Paragraphs).FirstOrDefaultAsync(i => i.Id == entity.Id);
            if (user != null && book != null)
            {
                user.Books.Remove(book);
                book.Users.Remove(user);
                await tempDB.SaveChangesAsync();
            }
        }

        public async Task RemoveNotification(UserEntity userEntity, NotificationEntity entity)
        {
            AppDBContext tempDB = new AppDBContext();
            var user = await tempDB.Users.Include(u => u.Notifications).FirstOrDefaultAsync(u => u.Id == userEntity.Id);
            if (user != null)
            {
                var NotificationToRemove = user.Notifications.FirstOrDefault(uc => uc.Id == entity.Id);

                if (NotificationToRemove != null)
                {
                    user.Notifications.Remove(NotificationToRemove);

                    tempDB.Notifications.Remove(NotificationToRemove);

                    await tempDB.SaveChangesAsync();
                }
            }
        }

        public async Task UpdateUser(UserEntity user)
        {
            AppDBContext tempDB = new AppDBContext();
            var userFromDB = await tempDB.Users.Include(n=>n.Notifications).Include(u => u.Books).ThenInclude(b => b.Chapters).ThenInclude(b => b.Paragraphs).ThenInclude(p => p.UserComments).FirstOrDefaultAsync(i => i.Id == user.Id);
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
