using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.DAL
{
    /**
     * T - Entity
     * U - Type of ID
     * K - extention of DbContext
     */
    public abstract class GenericRepository<T, U, K> : IRepository<T, U>
        where T : class, IEntity
        where U : struct
        where K : DbContext
    {

        private readonly K dbContext;
        private readonly DbSet<T> dbSet;

        public GenericRepository(K dbContext)
        {
            this.dbContext = dbContext;
            dbSet = this.dbContext.Set<T>();
        }

        public async Task<List<T>> GetAll()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<T> GetById(U id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task<T> Put(T entity)
        {
            dbContext.Entry(entity).State = EntityState.Modified;

            await dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<T> Post(T entity)
        {
            dbContext.Add(entity);

            await dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<T> Delete(U id)
        {
            var entity = await dbSet.FindAsync(id);
            if (entity == null)
            {
                throw new NullReferenceException($"Entity with id: {id} not found!");
            }

            dbSet.Remove(entity);
            await dbContext.SaveChangesAsync();
            
            return entity;
        }

        public bool Exist(T entity)
        {
            return dbSet.Any(e => e.Id == entity.Id);
        }
    }
}
