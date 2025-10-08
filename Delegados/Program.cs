namespace Delegados
{
    internal class Program
    {
        static void Main(string[] args)
        {
            OtraClase otraClase = new OtraClase();
            Deposito deposito = new Deposito();
            deposito.DelegadoEjecutar += Metodo1;
            deposito.DelegadoEjecutar += otraClase.OtroMetodo;
            deposito.EjecutarLosMetodosEnLosDelegados("Texto Prueba");

            deposito.DelegadoEjecutar("Otro Texto Prueba");

            deposito.EventoEjecutar += Metodo1;
            deposito.DispararElEvento("TextoEvento");
            Console.WriteLine("Hello, World!");
        }

        public static void Metodo1(string ElTexto)
        {
            Console.WriteLine($"Metodo 1: {ElTexto}");
        }
    }
}
