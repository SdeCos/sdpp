using System;
using BibliotecaClases;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading.Tasks;

namespace Servidor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TicketServer();
        }
        static void TicketServer()
        {
            Console.WriteLine("Ticket Server started...");
            TcpChannel tcpChannel = new TcpChannel(9998);
            ChannelServices.RegisterChannel(tcpChannel);
            Type commonInterfaceType = typeof(MovieTicket);
            //Type commonInterfaceType = Type.GetType("MovieTicket");
            RemotingConfiguration.RegisterWellKnownServiceType(commonInterfaceType,
            "MovieTicketBooking", WellKnownObjectMode.SingleCall);
            System.Console.WriteLine("Press ENTER to quitnn");
            System.Console.ReadLine();
        }


    }
}
