using System.Collections.Generic;

namespace mxcd.linqToSql
{
    internal interface ISqlGenerator
    {
        string Avg(string key, string expression, string groupby = null);
        string Count(string expression, string groupby = null);
        string Delete(string expression = null);
        string Fisrt(string keys, string expression, string orderby = null);
        string Insert(string keys, IEnumerable<string> values);
        string Insert(string keys, string values);
        string Max(string key, string expression, string groupby = null);
        string Min(string key, string expression, string groupby = null);
        string Select(string keys, string expression, string order = null);
        string Select(string keys, string expression, int page, int registers, string order = null);
        string SelectGroup(string keys, string expression, string groupby = null);
        string Sum(string key, string expression, string groupby = null);
        string Update(IDictionary<string, string> FieldNameValues, string expression = null);
    }
}