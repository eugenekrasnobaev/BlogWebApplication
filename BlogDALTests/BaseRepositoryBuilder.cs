using BlogDAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BlogDALTests
{
    public abstract class BaseRepositoryBuilder<TEntity, TContext>
        where TEntity : class, IEntity
        where TContext : DbContext
    {
        public List<TEntity> Data { get; set; } = new List<TEntity>();
        public Mock<TContext> DbContext { get; set; } = new Mock<TContext>();

        public BaseRepositoryBuilder<TEntity, TContext> SetData(params TEntity[] data)
        {
            Data.AddRange(data);
            return this;
        }

        public TEntity GetEntity(int id)
        {
            return Data.Single(item => item.Id == id);
        }

        protected void SetOptions(Expression<Func<TContext, DbSet<TEntity>>> expression)
        {
            var mockDbSet = new Mock<DbSet<TEntity>>();
            SetDataProvidersForDataSet(mockDbSet);
            SetInteractionWithDataSet(mockDbSet, expression);
        }

        protected void SetInteractionWithDataSet(Mock<DbSet<TEntity>> mockDbSet, Expression<Func<TContext, DbSet<TEntity>>> expression)
        {
            DbContext.Setup(expression).Returns(mockDbSet.Object);
            DbContext.Setup(m => m.Add(It.IsAny<TEntity>())).Callback<TEntity>(Data.Add);
            DbContext.Setup(m => m.Set<TEntity>()).Returns(mockDbSet.Object);
            ThrowExceptionOnHandlingNotExistingEntity(Data);
            DbContext.Setup(m => m.Remove(It.Is<TEntity>(u => u.Id <= Data.Count))).Callback<TEntity>(u => Data.Remove(u));
            DbContext.Setup(m => m.Find<TEntity>(It.IsAny<object[]>()))
                .Returns<object[]>(id => Data.Single(u => u.Id == (int)id[0]));
        }

        protected void SetDataProvidersForDataSet(Mock<DbSet<TEntity>> mockUserDbSet)
        {
            mockUserDbSet.As<IQueryable<TEntity>>().Setup(m => m.Provider).Returns(Data.AsQueryable().Provider);
            mockUserDbSet.As<IQueryable<TEntity>>().Setup(m => m.Expression).Returns(Data.AsQueryable().Expression);
            mockUserDbSet.As<IQueryable<TEntity>>().Setup(m => m.ElementType).Returns(Data.AsQueryable().ElementType);
            mockUserDbSet.As<IQueryable<TEntity>>().Setup(m => m.GetEnumerator()).Returns(Data.AsQueryable().GetEnumerator());
        }

        /// <summary>
        /// For simplicity, suppose that entity exists if id is less than count.
        /// </summary>
        /// <param name="data"></param>
        private void ThrowExceptionOnHandlingNotExistingEntity(IList<TEntity> data)
        {
            DbContext.Setup(m => m.Update(It.Is<TEntity>(u => u.Id > Data.Count))).Throws(new Exception());
            DbContext.Setup(m => m.Remove(It.Is<TEntity>(u => u.Id > Data.Count))).Throws(new Exception());
        }

    }
}
