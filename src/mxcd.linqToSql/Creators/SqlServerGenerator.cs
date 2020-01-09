using mxcd.util.sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mxcd.linqToSql
{
    internal class SqlServerGenerator : ISqlGenerator
    {
        private string TableName { get; }
        private string Schema { get; }
        private bool WithNoLock { get; }
        private string PrimaryKeyTable { get; }

        public SqlServerGenerator(string tableName, string primaryKeyTable = "Id", string schema = null, bool withNoLock = false)
        {
            TableName = tableName;
            Schema = schema;
            WithNoLock = withNoLock;
            PrimaryKeyTable = primaryKeyTable;
        }
        protected virtual string TableQuery
        {
            get
            {
                var result = $"{TableName}";

                result = $"{Schema ?? "dbo"}.{result}";

                if (WithNoLock)
                {
                    result = $"{result} WITH(NOLOCK)";
                }

                return result;
            }
        }
        protected virtual string TableCommand
        {
            get
            {
                var result = $"{this.Schema ?? "dbo"}.{TableName}";

                return result;
            }
        }


        public string Select(string keys, string expression, string order = null)
        {
            string sResult = String.Format($"SELECT {keys??"*"} FROM {TableQuery}");

            if (expression != null)
                sResult = $"{sResult} WHERE {expression}";
            if (order != null)
                sResult = $"{sResult} ORDER BY {order}";

            return sResult;
        }

        public string SelectGroup(string keys, string expression, string groupby = null)
        {
            string sResult = Select(keys, expression);
            if (groupby != null)
                sResult = $"{sResult} GROUP BY {groupby}";

            return sResult;
        }

        public string Fisrt(string keys, string expression, string orderby = null)
        {
            return Select($"TOP(1) {keys??"*"}", expression, orderby);
        }

        public string Count(string expression, string groupby = null)
        {
            return SelectGroup("COUNT(*)", expression, groupby);
        }

        public string Max(string key, string expression, string groupby = null)
        {
            return SelectGroup($"MAX({key})", expression, groupby);
        }

        public string Min(string key, string expression, string groupby = null)
        {
            return SelectGroup($"MIN({key})", expression, groupby);
        }

        public string Avg(string key, string expression, string groupby = null)
        {
            return SelectGroup($"AVG({key})", expression, groupby);
        }

        public string Sum(string key, string expression, string groupby = null)
        {
            return SelectGroup($"SUM({key})", expression, groupby);
        }

        public string Select(string keys, string expression, int page, int registers, string order = null)
        {
            return String.Format(@"WITH C AS
            ( 
              SELECT ROW_NUMBER() OVER(ORDER BY {0}) AS rownum,
                {1}
              FROM {2}
              {5}
            )
            SELECT {1}
            FROM C
            WHERE rownum BETWEEN ({3}-1) * {4} + 1 AND {3} * {4} ORDER BY {6}",
                this.PrimaryKeyTable,
                keys??"*",
                TableQuery,
                page,
                registers,
                expression == null ? String.Empty : $"WHERE {expression}",
                order ?? this.PrimaryKeyTable
                );
        }

        public string Insert(string keys, string values)
        {
            return string.Format("INSERT INTO {0} ({1}) OUTPUT inserted.{3} VALUES ({2})",
                                     TableCommand,
                                     keys,
                                     values,
                                     this.PrimaryKeyTable);
        }

        public string Insert(string keys, IEnumerable<string> values)
        {
            string sResult = String.Empty;
            string sAux = String.Empty;


            foreach (var objectValues in values.Reverse())
            {
                sResult = $"{string.Format("SELECT {0} {1}", objectValues, sAux)} {sResult}";
                sAux = "UNION ALL";
            }


            sResult = string.Format("INSERT INTO {0} ({1}) OUTPUT Inserted.{3} {2}", TableCommand, keys, sResult, this.PrimaryKeyTable);

            return sResult;
        }

        public string Delete(string expression = null)
        {
            return string.Format("DELETE FROM {0}{1}", TableCommand, expression != null ? $" WHERE {expression}" : String.Empty);
        }

        public string Update(IDictionary<string, string> FieldNameValues, string expression = null)
        {
            string sResult;
            var Elementos = (from oObj in FieldNameValues
                             select String.Format("{0} = {1}", oObj.Key, oObj.Value)
                            ).ToArray();

            sResult = string.Format("UPDATE {0} SET {1}", TableCommand, string.Join(", ", Elementos));

            if (expression != null)
            {
                sResult = $"{sResult} WHERE {expression}";
            }


            return sResult;
        }
    }
}
