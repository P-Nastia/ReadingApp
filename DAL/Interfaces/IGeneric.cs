using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IGeneric<T> where T : class
    {
        Task Add(T item); 
        Task Remove(T item);
        IQueryable<T> GetAll();
        Task<T> GetById(int id);
    }
}
