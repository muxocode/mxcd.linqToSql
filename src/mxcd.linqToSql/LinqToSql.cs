using Microsoft.Extensions.Logging;
using mxcd.linqToSql.common;
using mxcd.util.sql;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using mxcd.util.entity;
using System.Linq;
using System.Collections;

namespace mxcd.linqToSql
{
    /// <summary>
    /// Type of sql generator
    /// </summary>
    public enum LinqToSqlType
    {
        /// <summary>
        /// Sql server
        /// </summary>
        SqlServer,
        /// <summary>
        /// Sql server with no lock
        /// </summary>
        SqlServerWithNoLock
    }
    internal class LinqToSql : ILinqToSql, ITableSql
    {
        private LinqToSqlType Type { get; }
        private ISqlGenerator SqlGenerator(LinqToSqlType type, string tableName, string primaryKeyTable = "Id", string schema = null)
        {
            ISqlGenerator result = null;
            switch (type)
            {
                case LinqToSqlType.SqlServer:
                    result = new SqlServerGenerator(tableName, primaryKeyTable, schema);
                    break;
                case LinqToSqlType.SqlServerWithNoLock:
                    result = new SqlServerGenerator(tableName, primaryKeyTable, schema, true);
                    break;
            }

            return result;
        }
        private ILogger<ILinqToSql> Logger { get; }
        private void Log(string text) => this.Logger?.LogTrace(text);
        private string Expression { get; set; }
        private string Grouped { get; set; }
        private string Order { get; set; }
        public string TableName { get; set; }
        public string Schema { get; set; }
        private Exception Error(string message, Exception oEx) => new LinqToSqlException($"ERROR ON {message}", oEx);
        public LinqToSql(LinqToSqlType type, ILogger<ILinqToSql> logger = null, string expression = null, string grouped = null, string order = null)
        {
            Type = type;
            Logger = logger;
            Grouped = grouped;
            Expression = expression;
            Order = order;
        }
        public ITableSql From(string tableName, string schema = null)
        {
            this.TableName = tableName;
            this.Schema = schema;

            return Create();
        }
        public string Max<T, TReturn>(Expression<Func<T, TReturn>> property) where T : class
        {
            try
            {
                var SqlConsultant = SqlGenerator(Type, tableName: $"[{TableName}]", schema: $"[{Schema ?? "dbo"}]");
                var result = SqlConsultant.Max(property.ToSql(), this.Expression, this.Grouped);
                Log(result);
                return result;
            }
            catch (Exception oEx)
            {
                throw Error("MAX", oEx);
            }
        }
        public string Min<T, TReturn>(Expression<Func<T, TReturn>> property) where T : class
        {
            try
            {
                var SqlConsultant = SqlGenerator(Type, tableName: $"[{TableName}]", schema: $"[{Schema ?? "dbo"}]");
                var result = SqlConsultant.Min(property.ToSql(), this.Expression, this.Grouped);
                Log(result);
                return result;
            }
            catch (Exception oEx)
            {
                throw Error("MIN", oEx);
            }
        }
        public string Avg<T, TReturn>(Expression<Func<T, TReturn>> property) where T : class
        {
            try
            {
                var SqlConsultant = SqlGenerator(Type, tableName: $"[{TableName}]", schema: $"[{Schema ?? "dbo"}]");
                var result = SqlConsultant.Avg(property.ToSql(), this.Expression, this.Grouped);
                Log(result);
                return result;
            }
            catch (Exception oEx)
            {
                throw Error("AVG", oEx);
            }
        }
        public string Sum<T, TReturn>(Expression<Func<T, TReturn>> property) where T : class
        {
            try
            {
                var SqlConsultant = SqlGenerator(Type, tableName: $"[{TableName}]", schema: $"[{Schema ?? "dbo"}]");
                var result = SqlConsultant.Sum(property.ToSql(), this.Expression, this.Grouped);
                Log(result);
                return result;
            }
            catch (Exception oEx)
            {
                throw Error("SUM", oEx);
            }
        }
        public string List<T, TReturn>(Expression<Func<T, TReturn>> property = null) where T : class
        {
            try
            {
                var SqlConsultant = SqlGenerator(Type, tableName: $"[{TableName}]", schema: $"[{Schema ?? "dbo"}]");
                var result = SqlConsultant.Select(property?.ToSql(), this.Expression, this.Order);
                Log(result);
                return result;
            }
            catch (Exception oEx)
            {
                throw Error("SELECT", oEx);
            }
        }
        public string List<T, TReturn>(int page, int registryNumber, Expression<Func<T, TReturn>> property) where T : class
        {
            try
            {
                var SqlConsultant = SqlGenerator(Type, tableName: $"[{TableName}]", schema: $"[{Schema ?? "dbo"}]");
                var result = SqlConsultant.Select(property?.ToSql(), this.Expression, page, registryNumber, this.Order);
                Log(result);
                return result;
            }
            catch (Exception oEx)
            {
                throw Error("SELECT", oEx);
            }
        }
        public string Count()
        {
            try
            {
                var SqlConsultant = SqlGenerator(Type, tableName: $"[{TableName}]", schema: $"[{Schema ?? "dbo"}]");
                var result = SqlConsultant.Count(this.Expression, this.Grouped);
                Log(result);
                return result;
            }
            catch (Exception oEx)
            {
                throw Error("COUNT", oEx);
            }
        }
        public string Fisrt<T, TReturn>(Expression<Func<T, TReturn>> property) where T : class
        {
            try
            {
                var SqlConsultant = SqlGenerator(Type, tableName: $"[{TableName}]", schema: $"[{Schema ?? "dbo"}]");
                var result = SqlConsultant.Fisrt(property?.ToSql(), this.Expression, this.Order);
                Log(result);
                return result;
            }
            catch (Exception oEx)
            {
                throw Error("FIRST", oEx);
            }
        }
        public string Insert<T>(T entity, bool includeProps = true, bool includeFields = false, string primaryKeyName = "Id", bool includePrimaryKey = false) where T : class
        {
            try
            {
                var SqlConsultant = SqlGenerator(Type, primaryKeyTable: $"[{primaryKeyName}]", tableName: $"[{TableName}]", schema: $"[{Schema ?? "dbo"}]");
                var value_keys = entity
                    .GetKeysValues(includeProps, includeFields, new List<string>() { includePrimaryKey ? "" : primaryKeyName })
                    .Reverse();

                var result = SqlConsultant.Insert(
                    $"[{string.Join("], [", value_keys.Select(x => x.Name))}]",
                    string.Join(", ", value_keys.Select(x => x.Value.ToSql(typeof(object)))));

                Log(result);
                return result;
            }
            catch (Exception oEx)
            {
                throw Error("INSERT", oEx);
            }
        }
        public string InsertMasive<T>(IEnumerable<T> entities, bool includeProps = true, bool includeFields = false, string primaryKeyName = "Id", bool includePrimaryKey = false) where T : class
        {
            try
            {
                var SqlConsultant = SqlGenerator(Type, primaryKeyTable: $"[{primaryKeyName}]", tableName: $"[{TableName}]", schema: $"[{Schema ?? "dbo"}]");
                var value_keys = entities
                    .Select(x =>
                        x.GetKeysValues(
                            includeProps,
                            includeFields,
                            new List<string>() { includePrimaryKey ? String.Empty : primaryKeyName })
                        .Reverse()
                    );

                var result = SqlConsultant.Insert(
                    $"[{string.Join("], [", value_keys.First().Select(x => x.Name))}]",
                    value_keys.Select(x => string.Join(", ", x.Select(y => y.Value.ToSql(typeof(object))))));

                Log(result);
                return result;
            }
            catch (Exception oEx)
            {
                throw Error("INSERT", oEx);
            }
        }
        public string Delete()
        {
            try
            {
                var SqlConsultant = SqlGenerator(Type, tableName: $"[{TableName}]", schema: $"[{Schema ?? "dbo"}]");
                var result = SqlConsultant.Delete(this.Expression);
                Log("DELETE");
                return result;
            }
            catch (Exception oEx)
            {
                throw Error("DELETE", oEx);
            }
        }
        public string Update<T>(T entity, bool includeProps = true, bool includeFields = false) where T : class
        {
            try
            {
                var SqlConsultant = SqlGenerator(Type, tableName: $"[{TableName}]", schema: $"[{Schema ?? "dbo"}]");
                var value_keys = entity.GetKeysValues(includeProps, includeFields);

                var result = SqlConsultant.Update(
                    value_keys.Reverse().ToDictionary(x => $"[{x.Name}]", x => x.Value.ToSql(typeof(object))),
                    this.Expression);

                Log(result);
                return result;
            }
            catch (Exception oEx)
            {
                throw Error("UPDATE", oEx);
            }

        }
        private LinqToSql Create(string expression=null, string grouped=null, string order=null)
        {
            return new LinqToSql(this.Type, this.Logger, expression??this.Expression, grouped??this.Grouped, order??this.Order)
            {
                TableName = TableName,
                Schema = Schema
            };
        }

        public IOrderedSql OrderBy<T, TReturn>(Expression<Func<T, TReturn>> property) where T : class
        {
            return Create(order: property.ToSql());
        }

        public IGroupedSql GroupBy<T, TReturn>(Expression<Func<T, TReturn>> property) where T : class
        {
            return Create(grouped: property.ToSql());
        }

        public IFilteredSql Where<T>(Expression<Func<T, bool>> expression) where T : class
        {
            return Create(expression: expression.ToSql());
        }

        public string Fisrt()
        {
            try
            {
                var SqlConsultant = SqlGenerator(Type, tableName: $"[{TableName}]", schema: $"[{Schema ?? "dbo"}]");
                var result = SqlConsultant.Fisrt(null, this.Expression, this.Order);
                Log(result);
                return result;
            }
            catch (Exception oEx)
            {
                throw Error("FIRST", oEx);
            }
        }

        public string List()
        {
            try
            {
                var SqlConsultant = SqlGenerator(Type, tableName: $"[{TableName}]", schema: $"[{Schema ?? "dbo"}]");
                var result = SqlConsultant.Select(null, this.Expression, this.Order);
                Log(result);
                return result;
            }
            catch (Exception oEx)
            {
                throw Error("SELECT", oEx);
            }
        }

        public string List(int page, int registryNumber)
        {
            try
            {
                var SqlConsultant = SqlGenerator(Type, tableName: $"[{TableName}]", schema: $"[{Schema ?? "dbo"}]");
                var result = SqlConsultant.Select(null, this.Expression, page, registryNumber, this.Order);
                Log(result);
                return result;
            }
            catch (Exception oEx)
            {
                throw Error("SELECT", oEx);
            }
        }
    }
}
