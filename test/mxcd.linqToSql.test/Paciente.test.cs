using mxcd.linqToSql.test._base;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace mxcd.linqToSql.test
{
    public class PacienteLongTest : SqlQueryBaseTest<Paciente, long, long, long>
    {
        protected override Expression<Func<Paciente, bool>> filter => x => x.Nombre.StartsWith("M");

        protected override string schema => null;

        protected override string tableName => "Pacientes";

        protected override bool withNoLock => false;

        protected override Expression<Func<Paciente, long>> GetGroup()
        {
            return x => x.Id;
        }

        protected override Expression<Func<Paciente, long>> GetOrder()
        {
            return x => x.Id;
        }

        protected override Expression<Func<Paciente, long>> GetSelect()
        {
            return x => x.Id;
        }
    }

    public class PacienteStringTest : SqlQueryBaseTest<Paciente, string, string, string>
    {
        protected override Expression<Func<Paciente, bool>> filter => x => x.Nombre.StartsWith("M");

        protected override string schema => null;

        protected override string tableName => "Pacientes";

        protected override bool withNoLock => false;

        protected override Expression<Func<Paciente, string>> GetGroup()
        {
            return x => x.Nombre;
        }

        protected override Expression<Func<Paciente, string>> GetOrder()
        {
            return x => x.Nombre;
        }

        protected override Expression<Func<Paciente, string>> GetSelect()
        {
            return x => x.Nombre;
        }
    }

    public class PacienteBoolTest : SqlQueryBaseTest<Paciente, bool, bool, bool>
    {
        protected override Expression<Func<Paciente, bool>> filter => x => x.Nombre.StartsWith("M");

        protected override string schema => null;

        protected override string tableName => "Pacientes";

        protected override bool withNoLock => false;

        protected override Expression<Func<Paciente, bool>> GetGroup()
        {
            return x => x.MayorEdad;
        }

        protected override Expression<Func<Paciente, bool>> GetOrder()
        {
            return x => x.MayorEdad;
        }

        protected override Expression<Func<Paciente, bool>> GetSelect()
        {
            return x => x.MayorEdad;
        }
    }
}
