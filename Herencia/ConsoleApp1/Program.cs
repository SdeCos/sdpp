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
            Console.WriteLine(persona.ToString());

            Empleado empleado = new Empleado("Jose Perez", 23);
            empleado.FechaNacimiento = DateTime.Now;
            empleado.DNI = "12345678A";
            Console.WriteLine($"Empleado {empleado.Nombre}, Id {empleado.IdEmpleado}");
            empleado.ComunicarIncidencia("Estoy enfermo");
            Console.WriteLine(empleado.ToString());
        }
    }
}