﻿using DAL.Entities;
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
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userEntity.Id);
            var book = await _dbContext.Books.FirstOrDefaultAsync(b => b.Name == entity.Name && b.Author == entity.Author);
            if (user != null && book != null)
            {
                user.Books.Add(book);
                await _dbContext.SaveChangesAsync();
            }
        }

        public IQueryable<UserEntity> GetAll()
        {
            return _dbContext.Users.Include(u => u.Books);
        }

        public Task<BookEntity> GetBook(UserEntity entity, int id)
        {
            var book =  _dbContext.Books.FirstOrDefaultAsync(b => b.Id == id && b.Users.Contains(entity));
            return book;
        }

        public async Task<UserEntity> GetById(int id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task Remove(UserEntity item)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(i => i.Id == item.Id);
            if (user != null)
            {
                foreach (var book in user.Books)
                    user.Books.Remove(book);
                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task RemoveBook(UserEntity userEntity, BookEntity entity)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(i => i.Id == userEntity.Id);
            if (user != null)
            {
                user.Books.Remove(entity);
            }
        }
    }
}
