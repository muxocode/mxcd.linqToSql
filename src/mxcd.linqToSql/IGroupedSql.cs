using System;
using System.Linq.Expressions;

namespace mxcd.linqToSql
{
    /// <summary>
    /// Grouped SQL
    /// </summary>
    public interface IGroupedSql
    {
        /// <summary>
        /// AVG query
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <typeparam name="TReturn">Property type</typeparam>
        /// <param name="property">Property</param>
        /// <returns>Query</returns>
        string Avg<T, TReturn>(Expression<Func<T, TReturn>> property) where T : class;
        /// <summary>
        /// COUNT query
        /// </summary>
        /// <returns>Query</returns>
        string Count();
        /// <summary>
        /// MAX query
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <typeparam name="TReturn">Property type</typeparam>
        /// <param name="property">Property</param>
        /// <returns>Query</returns>
        string Max<T, TReturn>(Expression<Func<T, TReturn>> property) where T : class;
        /// <summary>
        /// MIN query
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <typeparam name="TReturn">Property type</typeparam>
        /// <param name="property">Property</param>
        /// <returns>Query</returns>
        string Min<T, TReturn>(Expression<Func<T, TReturn>> property) where T : class;
        /// <summary>
        /// SUM query
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <typeparam name="TReturn">Property type</typeparam>
        /// <param name="property">Property</param>
        /// <returns>Query</returns>
        string Sum<T, TReturn>(Expression<Func<T, TReturn>> property) where T : class;
    }
}