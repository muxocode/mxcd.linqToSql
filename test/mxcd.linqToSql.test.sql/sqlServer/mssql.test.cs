using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace mxcd.linqToSql.test.sql
{
    [TestClass]
    public class Mssql
    {
        string createTable(string name) => $@"CREATE TABLE {name}(
                                 Id decimal PRIMARY KEY,
                                 Nombre VARCHAR (50) NOT NULL,
                                 MayorEdad bit NOT NULL
                                );";

        string removeAll() => $@"DECLARE @sql NVARCHAR(max)=''

                                SELECT @sql += ' Drop table ' + QUOTENAME(TABLE_SCHEMA) + '.'+ QUOTENAME(TABLE_NAME) + '; '
                                FROM INFORMATION_SCHEMA.TABLES
                                WHERE TABLE_TYPE = 'BASE TABLE' and TABLE_NAME like '%{Token.Value}'

                                Exec Sp_executesql @sql";

        readonly string connection = "Server=localhost;Database=Master;User Id = SA;Password=deVops.Docker!";

        Lazy<string> Token = new Lazy<string>(() =>
                {
                    return DateTime.UtcNow.ToBinary().ToString();
 
                });
            
        

        Paciente[] getEntities()
        {
            var aLista = new List<Paciente>();

            for (int i = 1; i < 100; i++)
            {
                aLista.Add(new Paciente
                {
                    Id = i,
                    MayorEdad = i % 2 == 0,
                    Nombre = $"Paciente{i}"
                });
            }

            return aLista.ToArray();
        }

        void Create(IDbConnection dbConnection, string table)
        {
                dbConnection.Execute(createTable(table));
        }

        internal IDbConnection Connection
        {
            get
            {
                return new SqlConnection(connection);
            }
        }

        [TestInitialize]
        public void TestInitialize()
        {
        }

        [TestCleanup]
        public void TestCleanup()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                //Borramos la tabla
                dbConnection.Execute(removeAll());
            }
        }

        [TestMethod]
        public void Flujo_Command()
        {

            string table = $"Flujo_Command{Token.Value}";
            var oSqlParser = new LinqToSqlFactory().Create(LinqToSqlType.SqlServerWithNoLock).From(table);

            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                //Creamos la tabla
                Create(dbConnection, table);
                createTable(table);

                //Comprobamos la creación
                var sQuery = oSqlParser.List();
                IEnumerable<Paciente> aLista = dbConnection.Query<Paciente>(sQuery);
                Assert.IsTrue(aLista.Count() == 0);


                //INSERCION
                var aEntities = getEntities();
                sQuery = oSqlParser.Insert(getEntities()[0], includePrimaryKey: true);
                dbConnection.Execute(sQuery);

                sQuery = oSqlParser.InsertMasive(getEntities().Skip(1), includePrimaryKey: true);
                dbConnection.Execute(sQuery);

                //Listado
                sQuery = oSqlParser.List();
                aLista = dbConnection.Query<Paciente>(sQuery);
                Assert.IsTrue(aEntities.Count() == aLista.Count());

                //Update
                var oObj = aLista.First();
                oObj.Nombre = "Aitex";

                sQuery = oSqlParser.Where<Paciente>(x => x.Id == oObj.Id).Update(oObj);
                dbConnection.Execute(sQuery);

                //Ontenemos elemento
                sQuery = oSqlParser.Where<Paciente>(x => x.Id == oObj.Id).List();
                var newObj = dbConnection.Query<Paciente>(sQuery).Single();
                Assert.IsTrue(newObj.Id == oObj.Id && newObj.Nombre == "Aitex");

                //Nuevo
                var oNewElement = new Paciente()
                {
                    Id = 5000,
                    Nombre = "New Person",
                    MayorEdad = true
                };
                sQuery = oSqlParser.Insert(oNewElement, includePrimaryKey:true);
                dbConnection.Execute(sQuery);
                sQuery = oSqlParser.Where<Paciente>(x => x.Id == 5000 && x.Id > 4999).List();
          
                var oSearch = dbConnection.Query<Paciente>(sQuery).Single();
                Assert.IsTrue(oSearch.Id == 5000);

                //Borrado
                sQuery = oSqlParser.Where((Paciente x) => x.Id == 5000).Delete();
                dbConnection.Execute(sQuery);

                sQuery = oSqlParser.Where<Paciente>(x => x.Id == 5000).List();

                oSearch = dbConnection.Query<Paciente>(sQuery).SingleOrDefault();
                Assert.IsTrue(oSearch == null);
            }
        }

        [TestMethod]
        public void Flujo_Query()
        {

            string table = $"Flujo_Query_{Token.Value}";
            var oSqlParser = new LinqToSqlFactory().Create(LinqToSqlType.SqlServerWithNoLock).From(table);

            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                //Creamos la tabla
                Create(dbConnection, table);
                createTable(table);

                //Comprobamos la creación
                var sQuery = oSqlParser.List();
                IEnumerable<Paciente> aLista = dbConnection.Query<Paciente>(sQuery);
                Assert.IsTrue(aLista.Count() == 0);

                //INSERCION
                var aEntities = getEntities();
                sQuery = oSqlParser.InsertMasive(getEntities(), includePrimaryKey: true);
                dbConnection.Execute(sQuery);

                //MAX
                sQuery = oSqlParser.Max((Paciente x) => x.Id);
                var oMax = dbConnection.ExecuteScalar<long>(sQuery);
                Assert.IsTrue(oMax == 99);

                sQuery = oSqlParser.Where((Paciente x) => x.Id < 10).Max((Paciente x) => x.Id);
                oMax = dbConnection.ExecuteScalar<long>(sQuery);
                Assert.IsTrue(oMax == 9);

                sQuery = oSqlParser.GroupBy((Paciente x) => x.MayorEdad).Max((Paciente x) => x.Id);
                oMax = dbConnection.ExecuteScalar<long>(sQuery);
                Assert.IsTrue(oMax == 99);

                //COUNT
                sQuery = oSqlParser.Count();
                var oCount = dbConnection.ExecuteScalar<long>(sQuery);
                Assert.IsTrue(oCount == 99);

                sQuery = oSqlParser.Where((Paciente x) => x.Id < 10).Max((Paciente x) => x.Id);
                oCount = dbConnection.ExecuteScalar<long>(sQuery);
                Assert.IsTrue(oCount == 9);

                sQuery = oSqlParser.GroupBy((Paciente x) => x.MayorEdad).Count();
                oCount = dbConnection.ExecuteScalar<long>(sQuery);
                Assert.IsTrue(oCount == 50);

                //MIN
                sQuery = oSqlParser.Min((Paciente x) => x.Id);
                var oMin = dbConnection.ExecuteScalar<long>(sQuery);
                Assert.IsTrue(oMin == 1);

                sQuery = oSqlParser.Where((Paciente x) => x.Id < 10).Min((Paciente x) => x.Id);
                oMin = dbConnection.ExecuteScalar<long>(sQuery);
                Assert.IsTrue(oMin == 1);

                sQuery = oSqlParser.Where((Paciente x) => x.Id < 10).GroupBy((Paciente x) => x.MayorEdad).Min((Paciente x) => x.Id);
                oMin = dbConnection.ExecuteScalar<long>(sQuery);
                Assert.IsTrue(oMin == 1);

                //SUM
                sQuery = oSqlParser.Sum((Paciente x) => x.Id);
                var oSum = dbConnection.ExecuteScalar<long>(sQuery);
                Assert.IsTrue(oSum == 4950);

                sQuery = oSqlParser.Where((Paciente x) => x.Id < 10).Sum((Paciente x) => x.Id);
                oSum = dbConnection.ExecuteScalar<long>(sQuery);
                Assert.IsTrue(oSum == 45);


                sQuery = oSqlParser.Where((Paciente x) => x.Id < 10).GroupBy((Paciente x) => x.MayorEdad).Sum((Paciente x) => x.Id);
                oSum = dbConnection.ExecuteScalar<long>(sQuery);
                Assert.IsTrue(oSum == 25);


                //FIRST
                sQuery = oSqlParser.Fisrt();
                var oPaciente = dbConnection.Query<Paciente>(sQuery).SingleOrDefault();
                Assert.IsTrue(oPaciente.Id == 1);

                sQuery = oSqlParser.Where((Paciente x) => x.Id < 10).Fisrt();
                oPaciente = dbConnection.Query<Paciente>(sQuery).SingleOrDefault();
                Assert.IsTrue(oPaciente.Id == 1);

                sQuery = oSqlParser.Where((Paciente x) => x.Id > 10).OrderBy((Paciente x) => x.Nombre).Fisrt(); 
                oPaciente = dbConnection.Query<Paciente>(sQuery).SingleOrDefault();
                Assert.IsTrue(oPaciente.Id == 11);

                //LIST
                var aListaNum = new List<long>() { 1, 2, 3, 4, 5 };
                sQuery = oSqlParser.Where((Paciente x) => aListaNum.Contains(x.Id)).List();
                var aListaPac = dbConnection.Query<Paciente>(sQuery);
                Assert.IsTrue(aListaPac.Count() == 5);

                sQuery = oSqlParser.Where((Paciente x) => aListaNum.Contains(x.Id)).List(1,3);
                var aListaPac2 = dbConnection.Query<Paciente>(sQuery);
                Assert.IsTrue(aListaPac2.Count() == 3);


                //BORRAMOS LA TABLA
                sQuery = oSqlParser.Delete();
                dbConnection.Execute(sQuery);

                //MAX
                sQuery = oSqlParser.Max((Paciente x) => x.Id);
                oMax = dbConnection.ExecuteScalar<long>(sQuery);
                Assert.IsTrue(oMax == 0);

                sQuery = oSqlParser.Where((Paciente x) => x.Id < 10).Max((Paciente x) => x.Id);
                oMax = dbConnection.ExecuteScalar<long>(sQuery);
                Assert.IsTrue(oMax == 0);

                sQuery = oSqlParser.GroupBy((Paciente x) => x.MayorEdad).Max((Paciente x) => x.Id);
                oMax = dbConnection.ExecuteScalar<long>(sQuery);
                Assert.IsTrue(oMax == 0);

                //COUNT
                sQuery = oSqlParser.Count();
                oCount = dbConnection.ExecuteScalar<long>(sQuery);
                Assert.IsTrue(oCount == 0);

                sQuery = oSqlParser.Where((Paciente x) => x.Id < 10).Max((Paciente x) => x.Id);
                oCount = dbConnection.ExecuteScalar<long>(sQuery);
                Assert.IsTrue(oCount == 0);

                sQuery = oSqlParser.GroupBy((Paciente x) => x.MayorEdad).Count();
                oCount = dbConnection.ExecuteScalar<long>(sQuery);
                Assert.IsTrue(oCount == 0);

                //MIN
                sQuery = oSqlParser.Min((Paciente x) => x.Id);
                oMin = dbConnection.ExecuteScalar<long>(sQuery);
                Assert.IsTrue(oMin == 0);

                sQuery = oSqlParser.Where((Paciente x) => x.Id < 10).Min((Paciente x) => x.Id);
                oMin = dbConnection.ExecuteScalar<long>(sQuery);
                Assert.IsTrue(oMin == 0);

                sQuery = oSqlParser.Where((Paciente x) => x.Id < 10).GroupBy((Paciente x) => x.MayorEdad).Min((Paciente x) => x.Id);
                oMin = dbConnection.ExecuteScalar<long>(sQuery);
                Assert.IsTrue(oMin == 0);

                //SUM
                sQuery = oSqlParser.Sum((Paciente x) => x.Id);
                oSum = dbConnection.ExecuteScalar<long>(sQuery);
                Assert.IsTrue(oSum == 0);

                sQuery = oSqlParser.Where((Paciente x) => x.Id < 10).Sum((Paciente x) => x.Id);
                oSum = dbConnection.ExecuteScalar<long>(sQuery);
                Assert.IsTrue(oSum == 0);


                sQuery = oSqlParser.Where((Paciente x) => x.Id < 10).GroupBy((Paciente x) => x.MayorEdad).Sum((Paciente x) => x.Id);
                oSum = dbConnection.ExecuteScalar<long>(sQuery);
                Assert.IsTrue(oSum == 0);


                //FIRST
                sQuery = oSqlParser.Fisrt();
                oPaciente = dbConnection.Query<Paciente>(sQuery).SingleOrDefault();
                Assert.IsTrue(oPaciente == null);

                sQuery = oSqlParser.Where((Paciente x) => x.Id < 10).Fisrt();
                oPaciente = dbConnection.Query<Paciente>(sQuery).SingleOrDefault();
                Assert.IsTrue(oPaciente == null);

                sQuery = oSqlParser.Where((Paciente x) => x.Id > 10).OrderBy((Paciente x) => x.Nombre).Fisrt();
                oPaciente = dbConnection.Query<Paciente>(sQuery).SingleOrDefault();
                Assert.IsTrue(oPaciente == null);

                //LIST
                sQuery = oSqlParser.Where((Paciente x) => aListaNum.Contains(x.Id)).List();
                aListaPac = dbConnection.Query<Paciente>(sQuery);
                Assert.IsTrue(aListaPac.Count() == 0);

            }
        }
    }
}