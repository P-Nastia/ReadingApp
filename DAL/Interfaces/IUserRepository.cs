using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IUserRepository<T1,T2, T3> where T1 : class where T2 : class where T3 : class
    {
        Task RemoveNotification(T2 userEntity, T3 entity);
        Task AddNotification(T2 userEntity, T3 entity);
        Task RemoveBook(T2 userEntity, T1 entity);
        Task AddBook(T2 userEntity, T1 entity);
        Task Add(T2 item);
        Task Remove(T2 item);
        Task UpdateUser(T2 user);
    }
}
