using mxcd.core.factory;
using mxcd.linqToSql.common;

namespace mxcd.linqToSql
{
    /// <summary>
    /// Factory of ILinqToSql
    /// </summary>
    public class LinqToSqlFactory : IFactory<LinqToSqlType, ILinqToSql>
    {
        /// <summary>
        /// Creates a new ILinqToSql
        /// </summary>
        /// <param name="obj">LinqToSqlType</param>
        /// <returns></returns>
        public ILinqToSql Create(LinqToSqlType obj)
        {
            return new LinqToSql(obj);
        }
    }
}
