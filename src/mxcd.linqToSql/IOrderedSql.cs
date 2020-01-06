using System;
using System.Linq.Expressions;

namespace mxcd.linqToSql
{
    /// <summary>
    /// Order query
    /// </summary>
    public interface IOrderedSql
    {
        /// <summary>
        /// First query
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <typeparam name="TReturn">Property type</typeparam>
        /// <param name="property">Property</param>
        /// <returns>Query</returns>
        string Fisrt<T, TReturn>(Expression<Func<T, TReturn>> property) where T : class;
        /// <summary>
        /// First query
        /// </summary>
        /// <returns>Query</returns>
        string Fisrt();
        /// <summary>
        /// Generates a grouped interface
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <typeparam name="TReturn">Property type</typeparam>
        /// <param name="property">Property</param>
        /// <returns>Query</returns>
        IGroupedSql GroupBy<T, TReturn>(Expression<Func<T, TReturn>> property) where T : class;
        /// <summary>
        /// Generates a LIST
        /// </summary>
        /// <returns>Query</returns>
        string List();
        /// <summary>
        /// List query
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <typeparam name="TReturn">Property type</typeparam>
        /// <param name="property">Property</param>
        /// <returns>Query</returns>
        string List<T, TReturn>(Expression<Func<T, TReturn>> property = null) where T : class;
        /// <summary>
        /// Paged query
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="registryNumber">Numer of registers</param>
        /// <returns>Query</returns>
        string List(int page, int registryNumber);
        /// <summary>
        /// Paged query
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <typeparam name="TReturn">Property type</typeparam>
        /// <param name="page">Page</param>
        /// <param name="registryNumber">Numer of registers</param>
        /// <param name="property">Property</param>
        /// <returns>Query</returns>
        string List<T, TReturn>(int page, int registryNumber, Expression<Func<T, TReturn>> property = null) where T : class;
    }
}