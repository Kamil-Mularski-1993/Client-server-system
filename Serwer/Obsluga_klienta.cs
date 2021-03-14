using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Serwer
{
    class Obsluga_klienta
    {
        public List<Obiekt_klienta> lista = new List<Obiekt_klienta>();
        public string odpowiedz;

        public Obsluga_klienta Obsluga(List<Obiekt_klienta> lista_klientow, string dane, string odpowiedz)
        {
            try
            {
                Obsluga_klienta obiekt = new Obsluga_klienta();
                string[] czesci = dane.Split(' ');

                Console.WriteLine("Dane: {0}  |  Odp: {1}", dane, odpowiedz);

                if (dane.Contains("LOGIN"))
                {
                    // blokada.WaitOne();
                    // {
                    lista_klientow.Add(new Obiekt_klienta(czesci[2]));
                    // }
                    // blokada.ReleaseMutex();
                }

                if (!odpowiedz.Contains("CHECK"))
                {
                    // blokada.WaitOne();
                    //{
                    try
                    {
                        lista_klientow.Find(x => x.nazwa_klienta == czesci[1]).wiadomosc = odpowiedz;
                    }
                    catch
                    {
                        lista_klientow.Find(x => x.nazwa_klienta == czesci[2]).wiadomosc = "Brak zadanego odbiorcy \n";
                    }
                    //     }
                    //     blokada.ReleaseMutex();
                }

                // blokada.WaitOne();
                // {
                odpowiedz = lista_klientow.Find(x => x.nazwa_klienta == czesci[2]).wiadomosc;
                lista_klientow.Find(x => x.nazwa_klienta == czesci[2]).wiadomosc = "EMPTY \n";
                //}
                //blokada.ReleaseMutex();

                obiekt.lista = lista_klientow;
                obiekt.odpowiedz = odpowiedz;
                return obiekt;
            }
            catch
            {
                Console.WriteLine("Blad");
                Console.WriteLine("Dane: {0}  |  Odp: {1}", dane, odpowiedz);
                Console.ReadKey();

                return null;
            }
        }
    }
}
