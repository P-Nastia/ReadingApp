using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IUser<T1,T2> where T1 : class where T2 : class
    {
        Task RemoveBook(T2 userEntity, T1 entity);
        Task AddBook(T2 userEntity, T1 entity);
        Task<T1> GetBook(T2 entity,int id);
        Task Add(T2 item);
        Task Remove(T2 item);
        IQueryable<T2> GetAll();
        Task<T2> GetById(int id);
    }
}
