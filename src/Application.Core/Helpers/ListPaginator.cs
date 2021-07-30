using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Core.Helpers
{
    public static class ListPaginator
    {

        public static IQueryable<T> Pagination<T>(this IQueryable<T> list,ref int pageNumber, int pageSize)
        {
            var totalOfEntities = list.Count();

            if (totalOfEntities == 0 
             || (pageNumber <= 0 
              && pageSize <=0))
            {
                return list;
            }
            
            var totalOfPages = totalOfEntities % pageNumber == 0 ? totalOfEntities / pageNumber : (totalOfEntities / pageNumber) + 1;
            if (pageNumber > totalOfPages)
            {
                pageNumber = totalOfPages;
            }

            return list.Skip(((pageNumber - 1) * pageSize)).Take(pageSize);
        }
    }
}
