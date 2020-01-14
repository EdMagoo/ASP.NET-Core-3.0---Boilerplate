﻿using System;
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
     * 
     * As this is "repository" class is generic we need to define the DbSets in DbContext
     * because this class may recive multiples implementations of IEntity and DbContexts
     * and we dont have access to the DbSets from the DbContext, therefore, its necesary to initialize 
     * each one individualy.
     * 
     * For more information: https://medium.com/net-core/repository-pattern-implementation-in-asp-net-core-21e01c6664d7
     */
    public abstract class GenericRepository<T, U, K> : IRepository<T, U>
        where T : class, IEntity
        where U : struct
        where K : DbContext
    {

        readonly K dbContext;
        readonly DbSet<T> dbSet;

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

        // there are other form of get method, like a get method with IQueryable and filters
        // for more info: https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application#create-a-generic-repository

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

        public async Task<List<T>> QueryGet(QueryFiltersDto queryFiltersDto)
        {
            return await GenericQuery.BuildQuery<T>(dbSet, queryFiltersDto).ToListAsync();
        }
    }
}
