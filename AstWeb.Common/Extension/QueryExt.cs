using NPoco.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AstWeb.Common.Extension
{
    public static class QueryExt
    {
        public static IQueryProvider<TSource> HasWhere<TSource>(this IQueryProvider<TSource> query, object target,
            Expression<Func<TSource, bool>> whereExpression)
        {
            if (target != null)
            {
                query.Where(whereExpression);
            }
            return query;
        }

        public static IQueryable<TSource> HasWhere<TSource>(this IQueryable<TSource> query, object target,
            Expression<Func<TSource, bool>> whExpression)
        {
            if (target != null)
            {
                query = query.Where(whExpression);
            }
            return query;
        }
        public static IQueryable<TSource> HasWhere<TSource>(this IQueryable<TSource> query, object target,
            Expression<Func<TSource, int, bool>> whExpression)
        {
            if (target != null)
            {
                query = query.Where(whExpression);
            }
            return query;
        }
    }
}
