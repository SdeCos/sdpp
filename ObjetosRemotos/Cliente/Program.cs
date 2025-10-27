using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading.Tasks;
using BibliotecaClases;

namespace Cliente
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TcpChannel tcpChannel = new TcpChannel();
            ChannelServices.RegisterChannel(tcpChannel);
            Type requiredType = typeof(MovieTicketInterface);
            MovieTicketInterface remoteObject =
            (MovieTicketInterface)Activator.GetObject(requiredType,
            "tcp://localhost:9998/MovieTicketBooking");
            Console.WriteLine(remoteObject.GetTicketStatus("Ticket No: 3344"));
        }
    }
}
