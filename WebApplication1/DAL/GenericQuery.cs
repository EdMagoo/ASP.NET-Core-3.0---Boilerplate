using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.DAL
{
    /** the principal methods to be used by other classes in this class
     * return an IQueryable<T>
     *
     * SQL like basic query structure:
     * 
     * SELECT [FUNCTION(entity1.property1)][,property2,...]
     * FROM entity1
     * [WHERE entity1.property1 =||<||>||<=||>=||!=||LIKE value1 [AND||OR] entity1.property2 =||<||>||<=||>=||!=||LIKE value2 ... ]
     * [GROUP BY property2[,...]]
     * [HAVING [FUNCTION(entity1.property1)] =||<||>||<=||>=||!=||LIKE value1 [AND||OR ...]]
     * [ORDER BY entity1.property1 ASC||DESC[, ...]]
     */
    public abstract class GenericQuery<T, U, K>  
        where T : class, IEntity
        where U : struct
        where K : DbContext
    {
        IQueryable<T> Query { get; set; }
    
        protected GenericQuery(K dbContext)
        {
            Query = dbContext.Set<T>().AsNoTracking();
        }

        protected IQueryable<T> BuildQuery()
        {
            IQueryable<T> query = Query;
                
            /*
             * Initial operations
             *
             * Select
             * Where
             * Order by
             */

            return query;
        }
    }
}