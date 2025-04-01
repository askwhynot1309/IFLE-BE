using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.GenericRepositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "",
            int? pageIndex = null,
            int? pageSize = null);

        Task<TEntity> GetSingle(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        Task<TEntity> GetByID(int id);

        Task Insert(TEntity entity);

        Task InsertRange(List<TEntity> entities);

        Task Delete(TEntity entity);

        Task Update(TEntity entityToUpdate);

        Task<int> Count(Expression<Func<TEntity, bool>> filter = null);

        Task UpdateRange(List<TEntity> entities);

        Task DeleteRange(List<TEntity> entities);
    }
}
