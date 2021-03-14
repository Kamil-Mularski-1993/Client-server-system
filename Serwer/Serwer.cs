using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

namespace Serwer
{
    [Serializable]
    class Obiekt_klienta
    {
        public string nazwa_klienta;
        public string wiadomosc;
        public int licznik;

        public Obiekt_klienta(string nazwa_klienta)
        {
            this.nazwa_klienta = nazwa_klienta;
            this.wiadomosc = null;
            this.licznik = 0;
        }
    }

    class Serwer
    {
        static void Main(string[] args)
        {
            Konfiguracja konfiguracja = new Konfiguracja();
          //  konfiguracja.Zapisywanie();
            konfiguracja = konfiguracja.Wczytywanie();
            Console.WriteLine("----------------------SERWER I projektu PSK-----------------------\n");
            Console.WriteLine("Wstepna konfiguracja serwera:");
            Console.WriteLine("TCP: {0}",konfiguracja.TCP_medium);
            Console.WriteLine("UDP: {0}", konfiguracja.UDP_medium);
            Console.WriteLine("RS232: {0}", konfiguracja.RS232_medium);
            Console.WriteLine("File Manager: {0}", konfiguracja.File_medium);

            Console.WriteLine("Ping: {0}", konfiguracja.Ping_service);
            Console.WriteLine("Chat: {0}", konfiguracja.Chat_service);
            Console.WriteLine("Configure: {0}", konfiguracja.Configure_service);

            List<Obiekt_klienta> lista_klientow = new List<Obiekt_klienta>();
            Mutex blokada = new Mutex(false, "e1ffff8f-c91d-4188-9e82-c92ca5b1d057");

            if(konfiguracja.TCP_medium == "ON")
            new Thread(() =>
            {
                Serwer_Medium_TCP tcp = new Serwer_Medium_TCP();
                tcp.Start(lista_klientow, blokada, konfiguracja);
            }).Start();

            if (konfiguracja.UDP_medium == "ON")
                new Thread(() =>
            {
                Serwer_Medium_UDP udp = new Serwer_Medium_UDP();
                udp.Start(lista_klientow, blokada, konfiguracja);
            }).Start();
            
             // if(konfiguracja.RS232_medium == "ON")
                new Thread(() =>
                {
                    Serwer_Medium_RS232 rs232 = new Serwer_Medium_RS232();
                    rs232.Start(lista_klientow, blokada, konfiguracja);
                }).Start();
             

            if(konfiguracja.File_medium == "ON")
            new Thread(() =>
           {
               Serwer_Medium_FileManager filemanager = new Serwer_Medium_FileManager();
               filemanager.Start(konfiguracja);
           }).Start();
            /*
            new Thread(() =>
            {
                Serwer_Medium_NetRemoting netremoting = new Serwer_Medium_NetRemoting();
                netremoting.Start(konfiguracja);
            }).Start();
            */
            Console.ReadKey();
        }

    }
}
