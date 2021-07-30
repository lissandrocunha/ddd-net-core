using Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Domain.Core.Interfaces
{
    public interface IRepository<TEntity> where TEntity : Entity<TEntity>
    {
        TEntity Insert(TEntity entity);
        IEnumerable<TEntity> Insert(IEnumerable<TEntity> entities);
        TEntity InsertAsync(TEntity entity);
        IEnumerable<TEntity> InsertAsync(IEnumerable<TEntity> entities);

        void Update(TEntity entity);
        void Update(IEnumerable<TEntity> entities);

        void Delete(Func<TEntity, bool> predicate);
        void Delete(TEntity entity);
        void Delete(IEnumerable<TEntity> entities);

        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
                                 int? take = null,
                                 int? skip = null,
                                 Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                 params Expression<Func<TEntity, object>>[] includes);        

    }
}
