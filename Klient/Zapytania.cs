using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klient
{
    class Zapytania
    {
        public static string Zapytanie(string uzytkownik)
        {
            string zadanie = Console.ReadLine();

            if (zadanie == "PING") return "PING " + uzytkownik + " " + uzytkownik + " " + DateTime.Now.Millisecond.ToString() + " \n";
            if (zadanie == "LOGIN") return "LOGIN " + uzytkownik + " " + uzytkownik + " \n";
            if (zadanie == "END") return "END " + uzytkownik + " " + uzytkownik + " \n";
            if (zadanie.Contains("TO")) {
                string[] dane = zadanie.Split(' ');
                string wiadomosc = null;
                for (int i = 2; i < dane.Count(); i++)
                    wiadomosc += dane[i] + " ";
                return "TO " + dane[1] + " " + uzytkownik + " " + wiadomosc + " \n";
            }
            if (zadanie.Contains("CONFIGURATION")) return "CONFIGURATION " + uzytkownik + " " + uzytkownik + " \n";
            if (zadanie.Contains("SET"))
            {
                string[] wiadomosc = zadanie.Split(' ');
                return "SET " + uzytkownik + " " + uzytkownik + " " + wiadomosc[1] + " " + wiadomosc[2] + " \n";
            }
            

            return "";
        }
    }
}
