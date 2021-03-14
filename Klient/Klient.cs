using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Klient
{
    [Serializable]
    class Klient
    {
        static void Main(string[] args)
        {
            Console.WriteLine("---------------Witamy w kliencie I projektu PSK------------------\n");
            Console.WriteLine("Podaj nazwe uzytkownika:");
            string uzytkownik = Console.ReadLine();

            Console.WriteLine("\nWybierz sposób komunikacji:");
            Console.WriteLine("1. TCP");
            Console.WriteLine("2. UDP");
            Console.WriteLine("3. RS 232");
            Console.WriteLine("4. Pliki dyskowe");
            string wybor = Console.ReadLine();
            Console.WriteLine("\n\n--------------------------------------------------");
            Console.WriteLine("Dostepne zapytania: ");
            Console.WriteLine("LOGIN - logowanie do uslugi; niezbedne przy TCP, UDP i RS");
            Console.WriteLine("PING - sprawdzanie szybkosci transmisji");
            Console.WriteLine("TO nazwa_odboircy tresc_wiadomosci - wyslanie wiadomosci, do konkretnej osoby");
            Console.WriteLine("CONFIGURATION - podejrzenie konfiguracji serwera");
            Console.WriteLine("SET OPCJA TRYB - ustawianie dostepnych uslug i miedow");
            Console.WriteLine("END - zakonczenie polaczenia");
            Console.WriteLine("\n--------------------------------------------------");
            switch (wybor)
            {
                case "1":
                    Klient_Medium_TCP tcp = new Klient_Medium_TCP();
                    tcp.Start(uzytkownik);
                    break;
                case "2":
                    Klient_Medium_UDP udp = new Klient_Medium_UDP();
                    udp.Start(uzytkownik);
                    break;
                case "3":
                    Klient_Medium_RS232 rs232 = new Klient_Medium_RS232();
                    rs232.Start(uzytkownik);
                    break;
                case "4":
                    Klient_Medium_FileManager filemanager = new Klient_Medium_FileManager();
                    filemanager.Start(uzytkownik);
                    break;
               // case "5":
                   // Klient_Medium_NetRemoting netremoting = new Klient_Medium_NetRemoting();
                  //  netremoting.Start(uzytkownik);
                  //  break;

            }
            Console.ReadKey();
        }


    }
}
