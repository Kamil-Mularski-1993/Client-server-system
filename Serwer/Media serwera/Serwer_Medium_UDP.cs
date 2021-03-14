using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Serwer
{
    class Serwer_Medium_UDP
    {
        public void Start(List<Obiekt_klienta> lista_klientow, Mutex blokada, Konfiguracja konfiguracja)
        {
            UdpClient client = new UdpClient(12345);
            while (true)
            {
                IPEndPoint client_end_point = new IPEndPoint(IPAddress.Any, 10001);

                byte[] bytes = new byte[256];

                
                string dane = null;
                string odpowiedz = "";
                byte[] wiadomosc = null;
                Uslugi uslugi = new Uslugi();
                bytes = client.Receive(ref client_end_point);

                dane += Encoding.ASCII.GetString(bytes, 0, bytes.Length);
                odpowiedz = uslugi.Uslugi_Menu(dane, konfiguracja);


                blokada.WaitOne();
                    Obsluga_klienta obiekt = new Obsluga_klienta();
                    obiekt = obiekt.Obsluga(lista_klientow, dane, odpowiedz);
                    lista_klientow = obiekt.lista;
                    odpowiedz = obiekt.odpowiedz;
                blokada.ReleaseMutex();

                Console.WriteLine("{0}", dane);
                Console.WriteLine("{0}", odpowiedz);
                if (odpowiedz != "")
                {
                    wiadomosc = Encoding.ASCII.GetBytes(odpowiedz);
                    client.Send(wiadomosc, wiadomosc.Length, client_end_point);
                }

                //Console.WriteLine("Zakonczono");
            }
           
        }
    }
}
