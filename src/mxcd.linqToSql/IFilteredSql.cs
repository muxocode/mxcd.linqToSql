using System;
using System.Linq.Expressions;

namespace mxcd.linqToSql
{
    /// <summary>
    /// Filtered command or query
    /// </summary>
    public interface IFilteredSql:IOrderedSql, IGroupedSql
    {
        /// <summary>
        /// Deletes the given entities
        /// </summary>
        /// <returns>Command</returns>
        string Delete();
        /// <summary>
        /// Generates an order by
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <typeparam name="TReturn">Return type</typeparam>
        /// <param name="property">property</param>
        /// <returns></returns>
        IOrderedSql OrderBy<T, TReturn>(Expression<Func<T, TReturn>> property) where T : class;
        /// <summary>
        /// Updates the given collection
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="entity">entity values to update</param>
        /// <param name="includeProps">Include props values, default value=true</param>
        /// <param name="includeFields">Include fields values, default value=false</param>
        /// <returns>Command</returns>
        string Update<T>(T entity, bool includeProps = true, bool includeFields = false) where T : class;

    }
}