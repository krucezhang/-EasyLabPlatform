/*************************************************************
* © 2015 EasyLab Industries Corporation All rights reserved
* Author:          Kruce.Zhang
* Date:            1/28/2015
* Revision:        0.1       Draft
*                  
************************************************************/
using System.Linq;
using System.Collections.Generic;

namespace EasyLab.Server.Common.Extensions
{
    public static class LinqExtension
    {
        public static IEnumerable<TSource> Page<TSource>(this IEnumerable<TSource> query, uint pageIndex, uint pageSize)
        {
            int skipped = (int)(pageIndex * pageSize);
            if (skipped == 0)
            {
                return query.Take((int)pageSize);
            }
            return query.Skip(skipped).Take((int)pageSize);
        }

        public static IQueryable<TSource> Page<TSource>(this IQueryable<TSource> query, uint pageIndex, uint pageSize)
        {
            int skipped = (int)(pageIndex * pageSize);
            if (skipped == 0)
            {
                return query.Take((int)pageSize);
            }
            return query.Skip(skipped).Take((int)pageSize);
        }
    }
}
