using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herencia
{
    internal class Empleado: Persona
    {
        public int IdEmpleado { get; set; }
        public Empleado(string ElNombre, int ElIdEmpleado):base(ElNombre)
        {
            this.IdEmpleado = ElIdEmpleado;
        }
        public override void ComunicarIncidencia(string ExplicacionIncidencia)
        {
            base.ComunicarIncidencia(ExplicacionIncidencia);
            Console.WriteLine("Avisar RRHH");
        }
    }
}
