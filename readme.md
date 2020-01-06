# LinqToSql
Library that converts expressions to SQL string

# Examples

```csharp
var oSqlParser = new LinqToSqlFactory().Create(LinqToSqlType.SqlServerWithNoLock).From(table);

//LIST
var sQuery = oSqlParser.List();
var aListaNum = new List<long>() { 1, 2, 3, 4, 5 };
sQuery = oSqlParser.Where((Paciente x) => aListaNum.Contains(x.Id)).List();
sQuery = oSqlParser.Where((Paciente x) => aListaNum.Contains(x.Id)).List(1,3);
              
//MAX
sQuery = oSqlParser.Max((Paciente x) => x.Id);
sQuery = oSqlParser.Where((Paciente x) => x.Id < 10).Max((Paciente x) => x.Id);
sQuery = oSqlParser.GroupBy((Paciente x) => x.MayorEdad).Max((Paciente x) => x.Id);

//COUNT
sQuery = oSqlParser.Count();
sQuery = oSqlParser.Where((Paciente x) => x.Id < 10).Max((Paciente x) => x.Id);
sQuery = oSqlParser.GroupBy((Paciente x) => x.MayorEdad).Count();

//MIN
sQuery = oSqlParser.Min((Paciente x) => x.Id);
sQuery = oSqlParser.Where((Paciente x) => x.Id < 10).Min((Paciente x) => x.Id);
sQuery = oSqlParser.Where((Paciente x) => x.Id < 10).GroupBy((Paciente x) => 
                                                             
//SUM
sQuery = oSqlParser.Sum((Paciente x) => x.Id);
sQuery = oSqlParser.Where((Paciente x) => x.Id < 10).Sum((Paciente x) => x.Id);
sQuery = oSqlParser.Where((Paciente x) => x.Id < 10).GroupBy((Paciente x) => 
                                                             
//FIRST
sQuery = oSqlParser.Fisrt();
sQuery = oSqlParser.Where((Paciente x) => x.Id < 10).Fisrt();
sQuery = oSqlParser.Where((Paciente x) => x.Id > 10).OrderBy((Paciente x) => 
                                                             
//DELETE
sQuery = oSqlParser.Delete();
sQuery = oSqlParser.Where((Paciente x) => x.Id < 10).Delete();

//INSERT
sQuery = oSqlParser.InsertMasive(entities, includePrimaryKey: true);
sQuery = oSqlParser.InsertMasive(entities);
sQuery = oSqlParser.Insert(entity);
                                                             
//UPDATE
sQuery = oSqlParser.Update(oObj);
sQuery = oSqlParser.Update(new {Name="NewName"});
sQuery = oSqlParser.Where<Paciente>(x => x.Id == oObj.Id).Update(oObj);
```
<hr/>
Learn more in https://muxocode.com

<p align="center">
  <img src="https://muxocode.com/branding.png">
</p>