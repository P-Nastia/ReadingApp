using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IUserService<T1, T2> where T1 : class where T2 : class
    {
        Task RemoveBook(T2 userEntity, T1 entity);
        Task AddBook(T2 userEntity, T1 entity);
        T1 GetBook(T2 entity, int id);
        Task Add(T2 item);
        Task Remove(T2 item);
        IEnumerable<T2> GetAll();
        T2 GetById(int id);
        Task UpdateUser(T2 item);
    }
}
