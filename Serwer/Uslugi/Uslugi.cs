using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Serwer
{
    class Uslugi
    {
        public string Uslugi_Menu(string zapytanie, Konfiguracja konfiguracja)
        {
            string odpowiedz = null;
            if (zapytanie.Contains("SET"))
                if (konfiguracja.Configure_service == "ON") odpowiedz = SET_CONFIGURATION(zapytanie); else odpowiedz = "Usluga konfiguracji serwera jest wylaczona \n";
            else if (zapytanie.Contains("PING"))
                if (konfiguracja.Ping_service == "ON") odpowiedz = PING(zapytanie); else odpowiedz = "Usluga PING jest wylaczona \n"; 
            else if (zapytanie.Contains("LOGIN")) odpowiedz = LOGIN(zapytanie);
            else if (zapytanie.Contains("TO "))
                if(konfiguracja.Chat_service == "ON") odpowiedz = CHAT(zapytanie); else odpowiedz = "Usluga CHAT jest wylaczona \n";
            else if (zapytanie.Contains("END")) odpowiedz = END(zapytanie);
            else if (zapytanie.Contains("CHECK")) odpowiedz = CHECK(zapytanie);
            else if (zapytanie.Contains("CONFIGURATION")) odpowiedz = CHECK_CONFIGURATION(zapytanie);
            

            return odpowiedz;
        }
        
        public static string PING(string zapytanie)
        {
            string[] dane = zapytanie.Split(' ');
            DateTime Stop_time = DateTime.Now;
            int roznica = Math.Abs(DateTime.Now.Millisecond - int.Parse(dane[3]));
            return "PONG " + roznica +" ms \n";
        }

        public static string LOGIN(string zapytanie)
        {
           // string[] dane = zapytanie.Split(' ');
            return "Zalogowano \n";
        }

        public static string CHAT(string zapytanie)
        {
            string[] dane = zapytanie.Split(' ');
            string wiadomosc = null;
            for (int i = 3; i < dane.Count(); i++)
                wiadomosc += dane[i] + " ";
            return "FROM " + dane[2] + ": " + wiadomosc + " \n";
        }

        public static string END(string zapytanie)
        {
            return "END \n";
        }

        public static string CHECK(string zapytanie)
        {
            return zapytanie;
            //return CHAT(zapytanie);
        }

        public static string CHECK_CONFIGURATION(string zapytanie)
        {
            Konfiguracja konfiguracja = new Konfiguracja();
            konfiguracja.Wczytywanie();

            string konf = null;
            konf += "Destepnosc medii: \n";
            konf += "TCP: " + konfiguracja.TCP_medium + " \n";
            konf += "UDP: " + konfiguracja.UDP_medium + " \n";
            konf += "RS232: " + konfiguracja.RS232_medium + " \n";
            konf += "FILE MANAGER: " + konfiguracja.File_medium + " \n";
            konf += "\nDestepnosc uslug: \n";
            konf += "Ping: " + konfiguracja.Ping_service + " \n";
            konf += "Chat: " + konfiguracja.Chat_service + " \n";
            konf += "Configure: " + konfiguracja.Configure_service + " \n";
            return konf;
        }

        public static string SET_CONFIGURATION(string zapytanie)
        {            
            string[] fragmenty = zapytanie.Split(' ');
            Konfiguracja konfiguracja = new Konfiguracja();
            Konfiguracja konfiguracja_2 = new Konfiguracja();

            if (fragmenty[3] == "TCP") konfiguracja.TCP_medium = fragmenty[4];
            if (fragmenty[3] == "UDP") konfiguracja.UDP_medium = fragmenty[4];
            if (fragmenty[3] == "RS232") konfiguracja.RS232_medium = fragmenty[4];
            if (fragmenty[3] == "FILE") konfiguracja.File_medium = fragmenty[4];

            if (fragmenty[3] == "PING") konfiguracja.Ping_service = fragmenty[4];
            if (fragmenty[3] == "CHAT") konfiguracja.Chat_service = fragmenty[4];
            if (fragmenty[3] == "CONFIGURE") konfiguracja.Configure_service = fragmenty[4];

            konfiguracja.Zapisywanie();

            return "Wprowadzono nowe ustawienia. Zostaną aktywowane przy ponownym uruchominiu serwera \n";

        }
    }
}
