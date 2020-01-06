namespace mxcd.linqToSql
{
    /// <summary>
    /// Specifies the table name and schema to work
    /// </summary>
    public interface ILinqToSql
    {
        /// <summary>
        /// Specifies the table name and schema
        /// </summary>
        /// <param name="tableName">Table name</param>
        /// <param name="schema">schema</param>
        /// <returns></returns>
        ITableSql From(string tableName, string schema = null);
    }
}