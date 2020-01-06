using System;
using System.Collections.Generic;
using System.Text;

namespace mxcd.linqToSql.test
{
    public class Persona
    {
        public long Id { get; set; }
        public decimal? Saldo { get; set; }
        public DateTime? Fecha { get; set; }
        public string Nombre { get; set; }
        public int edad;
        public string Apellido1 { get; set; }
        public string apellido2;

    }
}
