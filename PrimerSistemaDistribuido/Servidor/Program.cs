using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
namespace Servidor
{
    class MyTcpListener
    {
        public static void Main()
        {
            Console.WriteLine("Programa Servidor...");
            TcpListener server = null;
            try
            {
                // Set the TcpListener on port 13000.
                Int32 port = 13000;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();



                // Enter the listening loop.
                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also use server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();

                    ProcesadorCliente procesador = new ProcesadorCliente(client);
                    ThreadStart ts = new ThreadStart(procesador.Procesar);
                    Thread hilo = new Thread(ts);
                    hilo.Start();

                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                server.Stop();
            }

            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }
    }
}

