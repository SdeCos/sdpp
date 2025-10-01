using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herencia
{
    internal class Persona
    {
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string DNI { get; set; }

        public Persona (string Elnombre)
        {
            this.Nombre = Elnombre;
        }

        public virtual void ComunicarIncidencia(string ExplicacionIncidencia)
        {
            Console.WriteLine($"Comunicar incidencia desde Persona: {this.Nombre}. Explicacion {ExplicacionIncidencia}");
        }

        override public string ToString()
        {
            return this.Nombre;
        }
    }
}
