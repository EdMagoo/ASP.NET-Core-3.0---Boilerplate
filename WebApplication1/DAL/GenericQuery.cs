using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
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
    public static class GenericQuery
    {
        public static IQueryable<T> BuildQuery<T>(DbSet<T> dbSet, QueryFiltersDto queryFilters) where  T : class, IEntity
        {
            /*
             * Initial operations
             *
             * Select
             * Where
             * Order by
             */

            var query = dbSet.AsNoTracking();

            if (queryFilters.QueryWhereFilter?.WhereOperation == null) return query;
            
            var whereFunc = BuildWhere<T>(queryFilters.QueryWhereFilter);
            query = query.Where(whereFunc);

            return query;
        }
        
        static Expression<Func<T, bool>> BuildWhere<T>(QueryWhereFilter queryWhereFilter) where T : class, IEntity
        {
            var pe = Expression.Parameter(typeof(T), typeof(T).Name);
            Expression whereExpression = null;
            
            while (true)
            {

                switch (queryWhereFilter.WhereOperation)
                {
                    case WhereOperation.EqualTo:
                        // comparing property vs value
                        Expression left = Expression.Property(pe, queryWhereFilter.Property);
                        // probably this is not secure, but it's very useful XD
                        Expression right = Expression.Constant(TypeDescriptor.GetConverter(left.Type).ConvertFromString(queryWhereFilter.Value), left.Type);

                        if (whereExpression != null && queryWhereFilter.WhereOperator != null)
                        {
                            whereExpression = AddWhereOperator(whereExpression, Expression.Equal(left, right),
                                queryWhereFilter.WhereOperator);
                        }
                        else
                        {
                            whereExpression = Expression.Equal(left, right);
                        }
                        
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
                        throw new ArgumentOutOfRangeException(nameof(queryWhereFilter.WhereOperation), queryWhereFilter.WhereOperation, "WHERE operator not found!");
                }

                if (queryWhereFilter.NextFilter == null && whereExpression != null)
                {
                    var whereFunc = Expression.Lambda<Func<T, bool>>(whereExpression, pe);
                    return whereFunc;
                }
                queryWhereFilter = queryWhereFilter.NextFilter;
            }
        }

        static Expression AddWhereOperator(Expression whereExpressionLeft, Expression whereExpressionRight, WhereOperator? whereOperator)
        {
            switch (whereOperator)
            {
                case WhereOperator.And:
                    whereExpressionLeft = Expression.And(whereExpressionLeft, whereExpressionRight);
                    break;
                case WhereOperator.Or:
                    whereExpressionLeft = Expression.OrElse(whereExpressionLeft, whereExpressionRight);
                    break;
                case WhereOperator.Not: // this is not != or <>
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(whereOperator), whereOperator, "WHERE operator not found!");
            }
            
            return whereExpressionLeft;
        }
    }
}