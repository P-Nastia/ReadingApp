using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IUser<EntityT> : IGeneric<UserEntity> where EntityT : class 
    {
        Task RemoveBook(UserEntity userEntity, EntityT entity);
        Task AddBook(UserEntity userEntity, EntityT entity);
        Task<EntityT> GetBook(UserEntity entity,int id);
    }
}
