using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace EjAsincronia
{
    class Program
    {
        private static bool done = false;

        static void Main(string[] args)
        {
            bool salir = false;
            while (salir == false)
            {
                done = false;
                Console.WriteLine();
                Console.WriteLine("********************************************************************");
                Console.WriteLine("Indique el modo de ejecución deseado:");
                Console.WriteLine("     - 1. Hacer Desayuno de forma Síncrona");
                Console.WriteLine("     - 2. Hacer Desayuno de forma Asíncrona");
                Console.WriteLine("     - 3. Hacer Desayuno de forma Asíncrona y Organizada");
                char LaOpcion = Console.ReadKey().KeyChar;
                Console.WriteLine();
                switch (LaOpcion)
                {
                    case '1':
                        HacerDesayunoSincrono();
                        break;
                    case '2':
                        HacerDesayunoAsync();
                        break;
                    case '3':
                        HacerDesayunoOrganizadoAsync();
                        break;
                    default:
                        salir = true;
                        return;
                }

                while (!done || salir)
                {
                    Console.CursorLeft = 0;
                    Console.WriteLine("----- El cocinero silba ({0}) -----", DateTime.Now.ToString("HH:mm:ss.fff"));
                    Task.Delay(1000).Wait();
                }
            }
        }

        private static void HacerDesayunoSincrono()
        {
            Stopwatch cronometro = new Stopwatch();
            cronometro.Start();

            TazaCafe LaTazaCafe = PrepararCafe();
            Console.WriteLine(" - El café está listo");

            HuevosFritos LosHuevos = FreirHuevos(2);
            Console.WriteLine(" - Los huevos están listos");

            Beicon ElBeicon = FreirBeicon(3);
            Console.WriteLine(" - El beicon está listo");

            Tostadas LasTostadas = HacerTostadas(2);
            UntarMantequilla(LasTostadas);
            UntarMermelada(LasTostadas);
            Console.WriteLine(" - Las tostadas están listas");

            ZumoNaranja ElZumoNaranja = PrepararZumo();
            Console.WriteLine(" - El zumo está listo");

            Console.WriteLine("El desayuno está listo!");
            cronometro.Stop();

            Console.WriteLine($"Se han necesitado {cronometro.ElapsedMilliseconds / 1000} segundos");
            done = true;
        }

        private async static void HacerDesayunoAsync()
        {
            Stopwatch cronometro = new Stopwatch();
            cronometro.Start();

            Task<TazaCafe> TareaTazaCafe = PrepararCafeAsync();
            TazaCafe LaTazaCafe = await TareaTazaCafe;
            Console.WriteLine(" - El café está listo");

            Task<HuevosFritos> TareaHuevosFritos = FreirHuevosAsync(2);
            HuevosFritos LosHuevos = await TareaHuevosFritos;
            Console.WriteLine(" - Los huevos están listos");

            Task<Beicon> TareaBeicon = FreirBeiconAsync(3);
            Beicon ElBeicon = await TareaBeicon;
            Console.WriteLine(" - El beicon está listo");

            Task<Tostadas> TareaTostadas = HacerTostadasAsync(2);
            Tostadas LasTostadas = await TareaTostadas;
            await UntarMantequillaAsync(LasTostadas);
            await UntarMermeladaAsync(LasTostadas);
            Console.WriteLine(" - Las tostadas están listas");

            Task<ZumoNaranja> TareaZumoNaranja = PrepararZumoAsync();
            ZumoNaranja ElZumoNaranja = await TareaZumoNaranja;
            Console.WriteLine(" - El zumo está listo");

            Console.WriteLine("El desayuno está listo!");
            cronometro.Stop();

            Console.WriteLine($"Se han necesitado {cronometro.ElapsedMilliseconds / 1000} segundos");
            done = true;
        }

        private async static void HacerDesayunoOrganizadoAsync()
        {
            Stopwatch cronometro = new Stopwatch();
            cronometro.Start();

            Task<TazaCafe> TareaTazaCafe = PrepararCafeAsync();

            Task<HuevosFritos> TareaHuevosFritos = FreirHuevosAsync(2);

            Task<Beicon> TareaBeicon = FreirBeiconAsync(3);

            Task<ZumoNaranja> TareaZumoNaranja = PrepararZumoAsync();

            Task<Tostadas> TareaTostadas = HacerTostadasAsync(2);

            Tostadas LasTostadas = await TareaTostadas;
            await UntarMantequillaAsync(LasTostadas);
            await UntarMermeladaAsync(LasTostadas);
            Console.WriteLine(" - Las tostadas están listas");

            TazaCafe LaTazaCafe = await TareaTazaCafe;
            Console.WriteLine(" - El café está listo");

            HuevosFritos LosHuevos = await TareaHuevosFritos;
            Console.WriteLine(" - Los huevos están listos");

            Beicon ElBeicon = await TareaBeicon;
            Console.WriteLine(" - El beicon está listo");
            
            ZumoNaranja ElZumoNaranja = await TareaZumoNaranja;
            Console.WriteLine(" - El zumo está listo");

            Console.WriteLine("El desayuno está listo!");
            cronometro.Stop();

            Console.WriteLine($"Se han necesitado {cronometro.ElapsedMilliseconds / 1000} segundos");
            done = true;
        }


        #region Preparar Café
        private static TazaCafe PrepararCafe()
        {
            Console.WriteLine("Haciendo café");
            Task.Delay(1000).Wait();
            Console.WriteLine("Sirviendo café");
            return new TazaCafe();
        }

        private static async Task<TazaCafe> PrepararCafeAsync()
        {
            Console.WriteLine("Haciendo café");
            await Task.Delay(1000);
            Console.WriteLine("Sirviendo café");
            return new TazaCafe();
        }
        #endregion

        #region Preparar Zumo
        private static ZumoNaranja PrepararZumo()
        {
            Console.WriteLine("Exprimiendo naranjas");
            Task.Delay(1000).Wait();
            Console.WriteLine("Sirviendo el zumo de naranja");
            return new ZumoNaranja();
        }

        private static async Task<ZumoNaranja> PrepararZumoAsync()
        {
            Console.WriteLine("Exprimiendo naranjas");
            await Task.Delay(1000);
            Console.WriteLine("Sirviendo el zumo de naranja");
            return new ZumoNaranja();
        }
        #endregion

        #region Preparar Tostadas
        private static Tostadas HacerTostadas(int NumTostadas)
        {
            for (int slice = 0; slice < NumTostadas; slice++)
            {
                Console.WriteLine("Colocando una tostada en la tostadora");
            }
            Console.WriteLine("Encender tostadora...");
            Task.Delay(2000).Wait();
            Console.WriteLine("Retirar las tostadas de la tostadora");

            return new Tostadas();
        }

        private static async Task<Tostadas> HacerTostadasAsync(int NumTostadas)
        {
            for (int slice = 0; slice < NumTostadas; slice++)
            {
                Console.WriteLine("Colocando una tostada en la tostadora");
            }
            Console.WriteLine("Encender tostadora...");
            await Task.Delay(2000);
            Console.WriteLine("Retirar las tostadas de la tostadora");

            return new Tostadas();
        }


        private static void UntarMantequilla(Tostadas LasTostadas)
        {
            Console.WriteLine("Untando las tostadas con mantequilla");
            Task.Delay(1000).Wait();
            Console.WriteLine("Tostadas untadas con mantequilla");
        }

        private static async Task UntarMantequillaAsync(Tostadas LasTostadas)
        {
            Console.WriteLine("Untando las tostadas con mantequilla");
            await Task.Delay(1000);
            Console.WriteLine("Tostadas untadas con mantequilla");
        }

        private static void UntarMermelada(Tostadas LasTostadas)
        {
            Console.WriteLine("Untando las tostadas con mermelada");
            Task.Delay(1000).Wait();
            Console.WriteLine("Tostadas untadas con mermelada");
        }

        private static async Task UntarMermeladaAsync(Tostadas LasTostadas)
        {
            Console.WriteLine("Untando las tostadas con mermelada");
            await Task.Delay(1000);
            Console.WriteLine("Tostadas untadas con mermelada");
        }
        #endregion

        #region Preparar Huevos

        private static void FreirHuevos()
        {
            FreirHuevos(1);
        }
        
        private static HuevosFritos FreirHuevos(int NumHuevos)
        {
            Console.WriteLine("Calentando la sartén para los huevos...");
            Task.Delay(2000).Wait();
            Console.WriteLine($"Cascando {NumHuevos} huevos");
            Console.WriteLine("Cocinando los huevos ...");
            Task.Delay(2000).Wait();
            Console.WriteLine("Sirviendo los huevos en un plato");

            return new HuevosFritos();
        }

        private static async Task<HuevosFritos> FreirHuevosAsync(int NumHuevos)
        {
            Console.WriteLine("Calentando la sartén para los huevos...");
            await Task.Delay(2000);
            Console.WriteLine($"Cascando {NumHuevos} huevos");
            Console.WriteLine("Cocinando los huevos ...");
            await Task.Delay(2000);
            Console.WriteLine("Sirviendo los huevos en un plato");

            return new HuevosFritos();
        }
        #endregion

        #region Preparar Beicon
        private static Beicon FreirBeicon(int NumLonchas)
        {
            Console.WriteLine($"Colocando {NumLonchas} lonchas de beicon en la sartén");
            Console.WriteLine("Haciendo el beicon por un lado...");
            Task.Delay(2000).Wait();
            for (int slice = 0; slice < NumLonchas; slice++)
            {
                Console.WriteLine("Volteando una loncha de beicon");
            }
            Console.WriteLine("Haciendo el beicon por el otro lado...");
            Task.Delay(2000).Wait();
            Console.WriteLine("Sirviendo el beicon en un plato");

            return new Beicon();
        }

        private static async Task<Beicon> FreirBeiconAsync(int NumLonchas)
        {
            Console.WriteLine($"Colocando {NumLonchas} lonchas de beicon en la sartén");
            Console.WriteLine("Haciendo el beicon por un lado...");
            await Task.Delay(2000);
            for (int slice = 0; slice < NumLonchas; slice++)
            {
                Console.WriteLine("Volteando una loncha de beicon");
            }
            Console.WriteLine("Haciendo el beicon por el otro lado...");
            await Task.Delay(2000);
            Console.WriteLine("Sirviendo el beicon en un plato");

            return new Beicon();
        }
        #endregion
    }

    internal class ZumoNaranja
    {
    }

    internal class Tostadas
    {
    }

    internal class Beicon
    {
    }

    internal class HuevosFritos
    {
    }

    internal class TazaCafe
    {
    }
}
