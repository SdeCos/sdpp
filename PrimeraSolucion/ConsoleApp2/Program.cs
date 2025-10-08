namespace ConsoleApp2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            //Recoger un valor de al linea de comando
            DateTime momentoActual = DateTime.Now;
            string line = Console.ReadLine();
            int elNumero = Convert.ToInt32(line);
            Persona laPersona = new Persona("Marta");
            Console.WriteLine(line.ToUpper());
        }
    }
}