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

namespace Serwer
{
    class Serwer_Medium_NetRemoting
    {
        public void Start(Konfiguracja konfiguracja)
        {
            TcpChannel polaczenie = new TcpChannel(9999);
            ChannelServices.RegisterChannel(polaczenie, false);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(Serwer_Odpowiedz_NetRemoting), "IOdpowiedz", WellKnownObjectMode.SingleCall);
        }

    }

    class Serwer_Odpowiedz_NetRemoting : MarshalByRefObject, IOdpowiedz
    {
        public string Odpowiedz_serwera(string zapytanie, List<Obiekt_klienta> lista_klientow, Mutex blokada, Konfiguracja konfiguracja)
        {
            Uslugi uslugi = new Uslugi();
            string odpowiedz = uslugi.Uslugi_Menu(zapytanie, konfiguracja);

            blokada.WaitOne();
                Obsluga_klienta obiekt = new Obsluga_klienta();
                obiekt = obiekt.Obsluga(lista_klientow, zapytanie, odpowiedz);
                lista_klientow = obiekt.lista;
                odpowiedz = obiekt.odpowiedz;
            blokada.ReleaseMutex();

            return odpowiedz;
        }
    }
}
