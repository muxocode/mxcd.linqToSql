using mxcd.core.entities;
using mxcd.linqToSql.common;
using mxcd.util.sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Xunit;

namespace mxcd.linqToSql.test._base
{
    public abstract class SqlQueryBaseTest<T, TOrder, TGroup, TSelect> where T : class, IEntity, new()
    {
        protected abstract Expression<Func<T, bool>> filter { get; }
        protected abstract Expression<Func<T, TOrder>> GetOrder();
        protected abstract Expression<Func<T, TGroup>> GetGroup();
        protected abstract Expression<Func<T, TSelect>> GetSelect();

        protected abstract string schema { get; }
        protected abstract string tableName { get; }
        protected abstract bool withNoLock { get; }

        private string table()
        {
            return $"[{schema??"dbo"}].[{tableName}]";
        }

        private string predicate(bool isWithNoLock)
        {
            return isWithNoLock ? " WITH(NOLOCK)" : "";
        }

        private string stringExpression
        {
            get
            {
                return this.filter != null ? $" WHERE {this.filter.ToSql()}" : "";
            }
        }

        private string stringOrder
        {
            get
            {
                return this.GetOrder() != null ? $" ORDER BY " + $"{this.GetOrder().ToSql()}" : "";
            }
        }

        private string stringGroup
        {
            get
            {
                return this.GetOrder() != null ? $" GROUP BY " +
                    $"{this.GetGroup().ToSql()}" : "";
            }
        }

        private string finalQuery()
        {
            return $"{table()}{predicate(withNoLock)}";
        }

        private ILinqToSql Create()
        {
            return new LinqToSqlFactory().Create(withNoLock ? LinqToSqlType.SqlServerWithNoLock : LinqToSqlType.SqlServer);
        }

        [Fact]
        public void Fisrt()
        {
            var oQuery = Create();

            var text1 = oQuery.From(tableName, schema).Fisrt();

            Assert.True(text1 == $"SELECT TOP(1) * FROM {table()}");

            var text2 = oQuery.From(tableName,schema).Where(filter).Fisrt();

            Assert.True(text2 == $"SELECT TOP(1) * FROM {finalQuery()}{stringExpression}");

            var text3 = oQuery.From(tableName, schema).Where(filter).OrderBy(GetOrder()).Fisrt();

            Assert.True(text3 == $"SELECT TOP(1) * FROM {finalQuery()}{stringExpression}{stringOrder}");

            var text4 = oQuery.From(tableName, schema).OrderBy(GetOrder()).Fisrt();

            Assert.True(text4 == $"SELECT TOP(1) * FROM {finalQuery()}{stringOrder}");

            var text5 = oQuery.From(tableName, schema).Fisrt(this.GetSelect());

            Assert.True(text5 == $"SELECT TOP(1) {this.GetSelect().ToSql()} FROM {table()}");

            var text6 = oQuery.From(tableName, schema).Where(filter).OrderBy(GetOrder()).Fisrt(this.GetSelect());

            Assert.True(text6 == $"SELECT TOP(1) {this.GetSelect().ToSql()} FROM {finalQuery()}{stringExpression}{stringOrder}");
        }
        
        [Fact]
        public void Count()
        {

            var oQuery = Create();

            var text1 = oQuery.From(tableName, schema).Count();

            Assert.True(text1 == $"SELECT COUNT(*) FROM {table()}");

            var text2 = oQuery.From(tableName, schema).Where(filter).Count();

            Assert.True(text2 == $"SELECT COUNT(*) FROM {finalQuery()}{stringExpression}");

            var text5 = oQuery.From(tableName, schema).GroupBy(GetGroup()).Count();

            Assert.True(text5 == $"SELECT COUNT(*) FROM {table()}{stringGroup}");
        }


        [Fact]
        public void Max()
        {
            var oQuery = Create();

            var text1 = oQuery.From(tableName, schema).Max(this.GetSelect());

            Assert.True(text1 == $"SELECT MAX({this.GetSelect().ToSql()}) FROM {table()}");

            var text2 = oQuery.From(tableName, schema).Where(filter).Max(this.GetSelect());

            Assert.True(text2 == $"SELECT MAX({this.GetSelect().ToSql()}) FROM {finalQuery()}{stringExpression}");

            var text3 = oQuery.From(tableName, schema).Where(filter).GroupBy(this.GetGroup()).Max(this.GetSelect());

            Assert.True(text3 == $"SELECT MAX({this.GetSelect().ToSql()}) FROM {finalQuery()}{stringExpression}{stringGroup}");
        }

        [Fact]
        public void Min()
        {

            var oQuery = Create();

            var text1 = oQuery.From(tableName, schema).Min(this.GetSelect());

            Assert.True(text1 == $"SELECT MIN({this.GetSelect().ToSql()}) FROM {table()}");

            var text2 = oQuery.From(tableName, schema).Where(filter).Min(this.GetSelect());

            Assert.True(text2 == $"SELECT MIN({this.GetSelect().ToSql()}) FROM {finalQuery()}{stringExpression}");

            var text3 = oQuery.From(tableName, schema).Where(filter).GroupBy(this.GetGroup()).Min(this.GetSelect());

            Assert.True(text3 == $"SELECT MIN({this.GetSelect().ToSql()}) FROM {finalQuery()}{stringExpression}{stringGroup}");
        }

        [Fact]
        public void Sum()
        {
            var oQuery = Create();

            var text1 = oQuery.From(tableName, schema).Sum(this.GetSelect());

            Assert.True(text1 == $"SELECT SUM({this.GetSelect().ToSql()}) FROM {table()}");

            var text2 = oQuery.From(tableName, schema).Where(filter).Sum(this.GetSelect());

            Assert.True(text2 == $"SELECT SUM({this.GetSelect().ToSql()}) FROM {finalQuery()}{stringExpression}");

            var text3 = oQuery.From(tableName, schema).Where(filter).GroupBy(this.GetGroup()).Sum(this.GetSelect());

            Assert.True(text3 == $"SELECT SUM({this.GetSelect().ToSql()}) FROM {finalQuery()}{stringExpression}{stringGroup}");
        }

        [Fact]
        public void Avg()
        {
            var oQuery = Create();

            var text1 = oQuery.From(tableName, schema).Avg(this.GetSelect());

            Assert.True(text1 == $"SELECT AVG({this.GetSelect().ToSql()}) FROM {table()}");

            var text2 = oQuery.From(tableName, schema).Where(filter).Avg(this.GetSelect());

            Assert.True(text2 == $"SELECT AVG({this.GetSelect().ToSql()}) FROM {finalQuery()}{stringExpression}");

            var text3 = oQuery.From(tableName, schema).Where(filter).GroupBy(this.GetGroup()).Avg(this.GetSelect());

            Assert.True(text3 == $"SELECT AVG({this.GetSelect().ToSql()}) FROM {finalQuery()}{stringExpression}{stringGroup}");
        }


        [Fact]
        public void Select()
        {
            var oQuery = Create();

            var text1 = oQuery.From(tableName, schema).List();

            Assert.True(text1 == $"SELECT * FROM {table()}");

            var text2 = oQuery.From(tableName, schema).Where(filter).List();

            Assert.True(text2 == $"SELECT * FROM {finalQuery()}{stringExpression}");

            var text3 = oQuery.From(tableName, schema).Where(filter).OrderBy(GetOrder()).List();

            Assert.True(text3 == $"SELECT * FROM {finalQuery()}{stringExpression}{stringOrder}");

            var text4 = oQuery.From(tableName, schema).OrderBy(GetOrder()).List();

            Assert.True(text4 == $"SELECT * FROM {finalQuery()}{stringOrder}");

            var text5 = oQuery.From(tableName, schema).List(this.GetSelect());

            Assert.True(text5 == $"SELECT {this.GetSelect().ToSql()} FROM {table()}");

            var text6 = oQuery.From(tableName, schema).Where(filter).OrderBy(GetOrder()).List(this.GetSelect());

            Assert.True(text6 == $"SELECT {this.GetSelect().ToSql()} FROM {finalQuery()}{stringExpression}{stringOrder}");

        }

        [Fact]
        public void Delete()
        {
            var oQuery = Create();

            var text1 = oQuery.From(tableName, schema).Delete();

            Assert.True(text1 == $"DELETE FROM {table()}");

            var text2 = oQuery.From(tableName, schema).Where(this.filter).Delete();

            Assert.True(text2 == $"DELETE FROM {table()}{stringExpression}");

        }

        [Fact]
        public void Update()
        {
            var oQuery = Create();
            var sString = $"UPDATE {table()} SET [Id] = 3{stringExpression}";
            var sResult = oQuery.From(tableName, schema).Where(this.filter).Update(new { Id = 3 });

            var sString2 = $"UPDATE {table()} SET [Id] = 3";
            var sResult2 = oQuery.From(tableName, schema).Update(new { Id = 3 });

            var sString3 = $"UPDATE {table()} SET [Id] = 0, [MayorEdad] = 0, [Nombre] = null";
            var sResult3 = oQuery.From(tableName, schema).Update(new Paciente());

            var sString4 = $"UPDATE {table()} SET [Id] = 0, [MayorEdad] = 0, [Nombre] = null";
            var sResult4 = oQuery.From(tableName, schema).Where<Paciente>(null).Update(new Paciente());

            var sString5 = $"UPDATE {table()} SET [Id] = 3";
            var sResult5 = oQuery.From(tableName, schema).Where<Paciente>(null).Update(new { Id = 3 });

            Assert.True(sResult == sString);
            Assert.True(sResult2 == sString2);
            Assert.True(sResult3 == sString3);
            Assert.True(sResult4 == sString4);
            Assert.True(sResult5 == sString5);

        }

        [Fact]
        public void Insert()
        {
            var oQuery = Create();
            var sString = $"INSERT INTO [dbo].[Pacientes] ([MayorEdad], [Nombre]) OUTPUT inserted.[Id] VALUES (0, null)";
            var sResult = oQuery.From(tableName, schema).Insert(new Paciente());

            var sString2 = $"INSERT INTO [dbo].[Pacientes] ([Id], [MayorEdad], [Nombre]) OUTPUT inserted.[Id] VALUES (0, 0, null)";
            var sResult2 = oQuery.From(tableName, schema).Insert(new Paciente(), includePrimaryKey: true);

            var sString3 = $"INSERT INTO [dbo].[Pacientes] ([MayorEdad], [Nombre]) OUTPUT Inserted.[Id] SELECT 0, null UNION ALL SELECT 0, null  ";
            var sResult3 = oQuery.From(tableName, schema).InsertMasive(new Paciente[] { new Paciente(), new Paciente() });
            
            var sString4 = $"INSERT INTO [dbo].[Pacientes] ([Id], [MayorEdad], [Nombre]) OUTPUT Inserted.[Id] SELECT 0, 0, null UNION ALL SELECT 0, 0, null  ";
            var sResult4 = oQuery.From(tableName, schema).InsertMasive(new Paciente[] { new Paciente(), new Paciente() }, includePrimaryKey: true);

            var sString5 = $"INSERT INTO [dbo].[Pacientes] ([Id], [MayorEdad]) OUTPUT Inserted.[Nombre] SELECT 0, 0 UNION ALL SELECT 0, 0  ";
            var sResult5 = oQuery.From(tableName, schema).InsertMasive(new Paciente[] { new Paciente(), new Paciente() }, primaryKeyName:"Nombre");

            Assert.True(sResult == sString);
            Assert.True(sResult2 == sString2);
            Assert.True(sResult3 == sString3);
            Assert.True(sResult4 == sString4);
            Assert.True(sResult5 == sString5);

        }
    }
}
