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
    //public class BooksRepository : IGeneric<BookEntity>
    //{
    //    //private AppDBContext _dbContext;
    //    //public BooksRepository(AppDBContext dBContext) { _dbContext = dBContext; }
    //    //public async Task Add(BookEntity item)
    //    //{
    //    //    await _dbContext.Books.AddAsync(item);
    //    //    await _dbContext.SaveChangesAsync();
    //    //}

    //    //public IQueryable<BookEntity> GetAll()
    //    //{
    //    //    return _dbContext.Set<BookEntity>().AsNoTracking();
    //    //}

    //    //public async Task Remove(BookEntity item)
    //    //{
    //    //    await _dbContext.Books.r
    //    //}
    //}
}
