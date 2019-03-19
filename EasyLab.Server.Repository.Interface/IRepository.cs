/*************************************************************
* © 2015 EasyLab Industries Corporation All rights reserved
* Author:          Kruce.Zhang
* Date:            1/12/2015
* Revision:        0.1       Draft
*                  
************************************************************/
using System;
using System.Linq;
using System.Linq.Expressions;

namespace EasyLab.Server.Repository.Interface
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        /// <summary>
        /// Get Enetity by keys
        /// </summary>
        /// <param name="KeyValues">The keys values</param>
        /// <returns></returns>
        TEntity GetByKey(params object[] KeyValues);

        /// <summary>
        /// Get the Query
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> GetQuery();

        /// <summary>
        /// Get the Query
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Gets one entity based on matching criteria
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> criteria);

        /// <summary>
        /// Gets the first entity based on criteria
        /// </summary>
        /// <param name="?"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> criteria);

        /// <summary>
        /// Adds the specified entity
        /// </summary>
        /// <param name="entity"></param>
        void Add(TEntity entity);

        /// <summary>
        /// Attaches the specified entity
        /// </summary>
        /// <param name="entity"></param>
        void Attach(TEntity entity);

        /// <summary>
        /// Deletes the specified entity
        /// </summary>
        /// <param name="entity"></param>
        void Delete(TEntity entity);

        /// <summary>
        /// Delete one or many entities matching the specified criteria
        /// </summary>
        /// <param name="criteria"></param>
        void Delete(Expression<Func<TEntity, bool>> criteria);

        /// <summary>
        /// Updates changes of the existing entity
        /// </summary>
        /// <param name="entity"></param>
        void Update(TEntity entity);

        /// <summary>
        /// Count the specified entity
        /// </summary>
        /// <returns></returns>
        int Count();

        /// <summary>
        /// Counts entities with the specified entity
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        int Count(Expression<Func<TEntity, bool>> criteria);

        /// <summary>
        /// Gets the unit of work
        /// </summary>
        IUnitOfWork UnitOfWork { get; set; }
    }
}
