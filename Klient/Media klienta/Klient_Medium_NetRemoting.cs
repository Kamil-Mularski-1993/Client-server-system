using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;using System.Net;

namespace Klient
{
    class Klient_Medium_NetRemoting
    {
         public void Start(string uzytkownik) {

            string odpowiedz;

                TcpChannel kanalTCP = new TcpChannel();
                ChannelServices.RegisterChannel(kanalTCP,false);
                odpowiedz = (string)Activator.GetObject(typeof(string),"tcp://localhost:9999/odpowiedz");
            Console.WriteLine("{0}", odpowiedz);

        }
    }
}
