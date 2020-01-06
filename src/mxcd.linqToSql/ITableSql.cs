using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace mxcd.linqToSql
{
    /// <summary>
    /// Table commands and queries
    /// </summary>
    public interface ITableSql:IGroupedSql, IFilteredSql, IOrderedSql
    {
        /// <summary>
        /// Insert a new entity
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="includeProps">Determines if props are incluyed, default:true</param>
        /// <param name="includeFields">Determines if fields are incluyed, default:false</param>
        /// <param name="primaryKeyName">Primary key name, default: "Id"</param>
        /// <param name="includePrimaryKey">Determines id primary key is incluyed, default: false</param>
        /// <returns></returns>
        string Insert<T>(T entity, bool includeProps = true, bool includeFields = false, string primaryKeyName = "Id", bool includePrimaryKey = false) where T : class;
        /// <summary>
        /// Insert a new list of entities
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="entities">Entities</param>
        /// <param name="includeProps">Determines if props are incluyed, default:true</param>
        /// <param name="includeFields">Determines if fields are incluyed, default:false</param>
        /// <param name="primaryKeyName">Primary key name, default: "Id"</param>
        /// <param name="includePrimaryKey">Determines id primary key is incluyed, default: false</param>
        string InsertMasive<T>(IEnumerable<T> entities, bool includeProps = true, bool includeFields = false, string primaryKeyName = "Id", bool includePrimaryKey = false) where T : class;
        /// <summary>
        /// Filtered query or command
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="expression">Filter</param>
        /// <returns></returns>
        IFilteredSql Where<T>(Expression<Func<T, bool>> expression) where T : class;
    }
}