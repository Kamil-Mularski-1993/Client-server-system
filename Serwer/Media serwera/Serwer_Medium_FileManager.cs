using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Serwer
{
    class Serwer_Medium_FileManager
    {
        string sciezka = @"..\..\..\Pliki wymiany";
        Mutex blokada_dostepu = new Mutex(false, "Klient-nadpisuje-c92ca5b1d057-4188-9e82-zapytania");
        Mutex blokada_dostepu_2 = new Mutex(false, "Odpowiedz-c92ca5b1d057-4188-9e82-odpowiedz");

        public void Start(Konfiguracja konfiguracja)
        {
            
            if (!File.Exists(sciezka + @"\zapytania.in"))
            {
                FileStream fs = new FileStream(sciezka + @"\zapytania.in", FileMode.Create);
                fs.Close();
            }

            FileSystemWatcher obserwato_serwera = new FileSystemWatcher(sciezka);
            obserwato_serwera.NotifyFilter = NotifyFilters.LastAccess;
            obserwato_serwera.Filter = "zapytania.in";
            obserwato_serwera.Changed += Zmiana_stanu;
            obserwato_serwera.EnableRaisingEvents = true;

            while (true)
            {
                
            }
        }

        private void Zmiana_stanu(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine("\nZauwazono zapytanie");            
            string dane = null;
            blokada_dostepu.WaitOne();
                StreamReader sr = new StreamReader(sciezka + @"\zapytania.in");
                dane = sr.ReadLine();
                sr.Close();
            blokada_dostepu.ReleaseMutex();
            Console.WriteLine("Odebrano zapytanie");

            Uslugi uslugi = new Uslugi();
            Konfiguracja konfiguracja = new Konfiguracja();
            string odpowiedz = uslugi.Uslugi_Menu(dane, konfiguracja);
            string[] czesci = dane.Split(' ');
            string odbiorca = czesci[1];
            string nadawca = czesci[2];

            blokada_dostepu_2.WaitOne();
                StreamWriter sw = new StreamWriter(sciezka + @"\" + odbiorca + @".out");
                sw.WriteLine(odpowiedz);
                sw.Close();
                File.SetLastAccessTime(sciezka + @"\" + odbiorca + @".out", DateTime.Now);
            blokada_dostepu_2.ReleaseMutex();
            Console.WriteLine("Wyslano odpowiedz");
            
        }

    }
}
