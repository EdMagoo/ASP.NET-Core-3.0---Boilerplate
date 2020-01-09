using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

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

        DbSet<T> DbSet { get; set; }
        
        protected GenericQuery(K dbContext)
        {
            DbSet = dbContext.Set<T>();
        }

        protected IQueryable<T> BuildQuery(QueryFiltersDto queryFilters)
        {
            /*
             * Initial operations
             *
             * Select
             * Where
             * Order by
             */

            var query = DbSet.AsNoTracking();

            if (queryFilters.QueryWhereFilter?.WhereOperation != null)
            {
                var whereExpression = BuildWhere(queryFilters.QueryWhereFilter);
            }

            return query;
        }

        Expression BuildWhere(QueryWhereFilter queryWhereFilter, Expression whereExpression = null)
        {
            while (true)
            {
                var pe = Expression.Parameter(typeof(T), "entity");

                switch (queryWhereFilter.WhereOperation)
                {
                    case WhereOperation.EqualTo:
                        // comparing property vs value
                        Expression left = Expression.Property(pe, queryWhereFilter.Property);
                        Expression right = Expression.Constant(queryWhereFilter.Value, left.Type);
                        whereExpression = Expression.Equal(left, right);
                        break;
                    case WhereOperation.LessThan:
                        break;
                    case WhereOperation.GreaterThan:
                        break;
                    case WhereOperation.LessThanOrEquals:
                        break;
                    case WhereOperation.GreaterThanOrEquals:
                        break;
                    case WhereOperation.NotEqualTo:
                        break;
                    case WhereOperation.Like:
                        break;
                    case WhereOperation.IsNull:
                        break;
                    case WhereOperation.NotLike:
                        break;
                    case WhereOperation.NotNull:
                        break;
                    case WhereOperation.Between:
                        break;
                    case WhereOperation.In:
                        break;
                    case WhereOperation.Some:
                        break;
                    case null:
                        break;
                    default:
                        throw new InvalidOperationException("WHERE operation not found!");
                }

                if (queryWhereFilter.NextFilter == null) return whereExpression;
                queryWhereFilter = queryWhereFilter.NextFilter;
            }
        }
    }
}