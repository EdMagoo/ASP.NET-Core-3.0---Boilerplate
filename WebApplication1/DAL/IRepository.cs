using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.DAL
{
    public interface IRepository<T, U> where T : class, IEntity where U : struct
    {
        Task<List<T>> GetAll();
        Task<T> GetById(U id);
        Task<T> Put(T entity);
        Task<T> Post(T entity);
        Task<T> Delete(U id);
        bool Exist(T entity);
    }
}
