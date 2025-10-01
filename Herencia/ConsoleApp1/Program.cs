namespace Herencia
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Persona persona = new Persona("Juan Perez");
            persona.FechaNacimiento = DateTime.Now;
            persona.DNI = "12345678A";
            persona.ComunicarIncidencia("Estoy enfermo");

            Empleado empleado = new Empleado("Jose Perez");
            empleado.FechaNacimiento = DateTime.Now;
            empleado.DNI = "12345678A";
            empleado.ComunicarIncidencia("Estoy enfermo");
        }
    }
}