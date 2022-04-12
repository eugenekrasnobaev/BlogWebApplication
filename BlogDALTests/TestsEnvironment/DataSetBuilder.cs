using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BlogDAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace BlogDALTests.TestsEnvironment
{
    public sealed class DataSetBuilder<TEntity, TContext> 
        where TEntity : class, IEntity
        where TContext : DbContext
    {
        public List<TEntity> Data { get; set; } = new List<TEntity>();
        public Mock<TContext> DbContext { get; set; }
        public Mock<DbSet<TEntity>> MockDbSet { get; set; }

        public DataSetBuilder() : this(new Mock<TContext>())
        {
            //Do nothing.
        }

        public DataSetBuilder(Mock<TContext> dbContext)
        {
            DbContext = dbContext;
            SetData();
        }

        public void SetData(params TEntity[] data)
        {
            Data.AddRange(data);
        }

        public TEntity GetData(int id)
        {
            return Data.Single(item => item.Id == id);
        }

        public void SetOptions(Expression<Func<TContext, DbSet<TEntity>>> expression)
        {
            MockDbSet = new Mock<DbSet<TEntity>>();
            SetDataProvidersForDataSet();
            SetInteractionWithDataSet(expression);
        }

        public void SetInteractionWithDataSet(Expression<Func<TContext, DbSet<TEntity>>> expression)
        {
            DbContext.Setup(expression).Returns(MockDbSet.Object);
            DbContext.Setup(m => m.Add(It.IsAny<TEntity>())).Callback<TEntity>(Data.Add);
            DbContext.Setup(m => m.Set<TEntity>()).Returns(MockDbSet.Object);
            DbContext.Setup(m => m.Remove(It.Is<TEntity>(u => Data.Contains(u))))
                .Callback<TEntity>(u => Data.Remove(u));
            DbContext.Setup(m => m.Update(It.Is<TEntity>(u => Data.Contains(u))));
            DbContext.Setup(m => m.Find<TEntity>(It.IsAny<object[]>()))
                .Returns<object[]>(id => Data.Single(u => u.Id == (int) id[0]));
            ThrowExceptionOnHandlingNotExistingEntity();
        }

        public void SetDataProvidersForDataSet()
        {
            MockDbSet.As<IQueryable<TEntity>>().Setup(m => m.Provider).Returns(Data.AsQueryable().Provider);
            MockDbSet.As<IQueryable<TEntity>>().Setup(m => m.Expression).Returns(Data.AsQueryable().Expression);
            MockDbSet.As<IQueryable<TEntity>>().Setup(m => m.ElementType).Returns(Data.AsQueryable().ElementType);
            MockDbSet.As<IQueryable<TEntity>>().Setup(m => m.GetEnumerator()).Returns(Data.AsQueryable().GetEnumerator());
        }

        /// <summary>
        /// For simplicity, suppose that entity exists if id is less than count.
        /// </summary>
        private void ThrowExceptionOnHandlingNotExistingEntity()
        {
            DbContext.Setup(m => m.Update(It.Is<TEntity>(u => !Data.Contains(u)))).Throws(new Exception());
            DbContext.Setup(m => m.Remove(It.Is<TEntity>(u => !Data.Contains(u)))).Throws(new Exception());
        }
    }
}
