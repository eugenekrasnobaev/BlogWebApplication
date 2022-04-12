using BlogDAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BlogDAL.Repositories
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        protected IBlogContext Db { get; set; }

        public virtual void Add(TEntity item)
        {
            Db.Add(item);
        }

        /// <summary>
        /// Soft deleting item. After removal, the item remains in the database.
        /// Marking related entities as inactive.
        /// </summary>
        public virtual void Delete(TEntity item)
        {
            item.IsActive = false;
            Db.Update(item);
        }
        /// <summary>
        /// Hard deleting item. After removal, the item deleted in the database.
        /// </summary>
        /// <param name="item"></param>
        public virtual void DeleteFromDb(TEntity item)
        {
            Db.Remove(item);
        }

        public virtual TEntity GetById(int id)
        {
            return Db.Set<TEntity>()
                .Where(i => i.IsActive)
                .FirstOrDefault(i => i.Id == id);
        }

        public TEntity GetByIdWithInclude(int id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return Include(includeProperties).Where(i => i.IsActive).FirstOrDefault(p => p.Id == id);
        }

        public virtual void Update(TEntity item)
        {
            Db.Update(item);
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return Db.Set<TEntity>()
                .Where(i => i.IsActive)
                .ToList();
        }

        public virtual IEnumerable<TEntity> Get(Func<TEntity, bool> predicate)
        {
            return Db.Set<TEntity>()
                .Where(predicate)
                .Where(i => i.IsActive)
                .ToList();
        }

        public void Save()
        {
            Db.SaveChanges();
        }

        private IQueryable<TEntity> Include(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return includeProperties.Aggregate(Db.Set<TEntity>().AsQueryable(), (current, includeProperty) => current.Include(includeProperty));
        }
    }
}
