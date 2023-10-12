using DAL.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        // properties
        protected readonly MyContext databaseContext;
        protected readonly DbSet<TEntity> table;

        // constructor
        public GenericRepository(MyContext databaseContext)
        {
            this.databaseContext = databaseContext;
            table = this.databaseContext.Set<TEntity>();
        }


        /// <summary>
        /// GetByIdAsync
        /// </summary>
        /// <param name="id"></param>
        /// <returns>TEntity</returns>
        /// <exception cref="EntityNotFoundException"></exception>
        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            return await table.FindAsync(id);
        }


        /// <summary>
        /// GetAllAsync
        /// </summary>
        /// <returns>IEnumerable<TEntity></returns>
        /// <exception cref="Exception"></exception>
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await table.ToListAsync()
            ?? throw new Exception($"Couldn't retrieve entities {typeof(TEntity).Name} ");
        }


        /// <summary>
        /// AddAsync
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual async Task AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(TEntity)} entity must not be null");
            }
            await table.AddAsync(entity);
        }


        /// <summary>
        /// UpdateAsync
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task UpdateAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(TEntity)} entity must not be null");
            }
            await Task.Run(() => table.Update(entity));

        }

        /// <summary>
        /// DeleteByIdAsync
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task DeleteByIdAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            await Task.Run(() => table.Remove(entity));
        }


        /// <summary>
        /// DeleteAsync
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
            }
            await Task.Run(() => table.Remove(entity));
        }

        /// <summary>
        /// GetCompleteEntityAsync
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
    }
}
