using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IBookRepository<T1,T2,T3> where T1 : class where T2 : class where T3 : class
    {
        Task AddBook(T1 item);
        Task AddComment(T2 itemTo, T3 itemToAdd);
        Task DeleteComment(T3 item);
        T1 GetBook(int id);
    }
}
