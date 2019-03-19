/*************************************************************
* © 2015 EasyLab Industries Corporation All rights reserved
* Author:          Kruce.Zhang
* Date:            1/12/2015
* Revision:        0.1       Draft
*                  
************************************************************/
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using EasyLab.Server.Repository.Interface;

namespace EasyLab.Server.Data.Persistence
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private bool disposed = false;
        private IUnitOfWork<DbContext> unitOfWork;

        private DbContext DbContext
        {
            get
            {
                if (unitOfWork == null)
                {
                    return null;
                }
                return unitOfWork.Context;
            }
        }

        #region IRpository<TEntity>

        public TEntity GetByKey(params object[] keyValues)
        {
            return DbContext.Set<TEntity>().Find(keyValues);
        }

        public IQueryable<TEntity> GetQuery()
        {
            return DbContext.Set<TEntity>();
        }

        public IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>> predicate)
        {
            return GetQuery().Where(predicate);
        }

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> criteria)
        {
            return GetQuery().SingleOrDefault(criteria);
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> criteria)
        {
            return GetQuery().FirstOrDefault(criteria);
        }

        public virtual void Add(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            DbContext.Set<TEntity>().Add(entity);
        }

        public virtual void Attach(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            DbContext.Set<TEntity>().Attach(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            DbContext.Set<TEntity>().Remove(entity);
        }

        public virtual void Delete(Expression<Func<TEntity, bool>> criteria)
        {
            IEnumerable<TEntity> records = GetQuery(criteria);

            foreach (TEntity item in records)
            {
                Delete(item);
            }
        }

        public virtual void Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
        }

        public int Count()
        {
            return GetQuery().Count();
        }

        public int Count(Expression<Func<TEntity, bool>> criteria)
        {
            return GetQuery().Count(criteria);
        }

        public IUnitOfWork UnitOfWork
        {
            get
            {
                return unitOfWork;
            }
            set
            {
                if (!(value is IUnitOfWork<DbContext>))
                {
                    throw new ArgumentNullException("Expected IUnitOfWork<System.Data.Entity.DbContext>");
                }
                unitOfWork = value as IUnitOfWork<DbContext>;
            }
        }

        #endregion

        #region IDisposible

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (this.unitOfWork != null)
                    {
                        this.unitOfWork.Dispose();
                        this.unitOfWork = null;
                    }
                }
                disposed = true;
            }
        }

        ~GenericRepository()
        {
            Dispose(false);
        }

        #endregion
    }
}
