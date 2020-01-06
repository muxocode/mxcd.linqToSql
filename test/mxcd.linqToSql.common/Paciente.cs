using mxcd.core.entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace mxcd.linqToSql.test
{
    public class Paciente : IEntity
    {
        public string Nombre { get; set; }
        public bool MayorEdad { get; set; }
        public long Id { get; set; }

        object IEntity.Id => this.Id;
    }
}
