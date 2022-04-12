using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BlogDAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> Get(Func<T, bool> predicate);
        IEnumerable<T> GetAll();
        T GetById(int id);
        T GetByIdWithInclude(int id, params Expression<Func<T, object>>[] includeProperties);
        void Add(T item);
        void Update(T item);
        void Delete(T item);
        void DeleteFromDb(T item);
        void Save();
    }
}
