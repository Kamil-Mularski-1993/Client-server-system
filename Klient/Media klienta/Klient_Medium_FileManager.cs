using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Klient
{
    [Serializable]
    class Klient_Medium_FileManager
    {
        public string sciezka = @"..\..\..\Pliki wymiany";
        public string uzytkownik = null;

        Mutex blokada_dostepu = new Mutex(false, "Klient-nadpisuje-c92ca5b1d057-4188-9e82-zapytania");
        Mutex blokada_dostepu_2 = new Mutex(false, "Odpowiedz-c92ca5b1d057-4188-9e82-odpowiedz");

        public void Start(string uzytkownik)
        {
            string plik = uzytkownik + @".out";
            this.uzytkownik = uzytkownik;

            if (!File.Exists(@"..\..\..\Pliki wymiany\" + plik))
            {
                FileStream fs = new FileStream(sciezka + @"\" + plik, FileMode.Create);
                fs.Close();
            }

            FileSystemWatcher obserwator_klienta = new FileSystemWatcher(sciezka);
            
            obserwator_klienta.NotifyFilter = NotifyFilters.LastAccess;
            obserwator_klienta.Changed += Zmiana_stanu;
            obserwator_klienta.Filter = plik;
            obserwator_klienta.EnableRaisingEvents = true;

            
            string wiadomosc = "";

            while (wiadomosc != "END")
            {
                wiadomosc = Zapytania.Zapytanie(uzytkownik);
                blokada_dostepu.WaitOne();
                    StreamWriter sw = new StreamWriter(sciezka + @"\zapytania.in");
                    sw.WriteLine(wiadomosc);
                    sw.Close();
                    File.SetLastAccessTime(sciezka + @"\zapytania.in", DateTime.Now);
                blokada_dostepu.ReleaseMutex();
            }
        }

        private void Zmiana_stanu(object sender, FileSystemEventArgs e)
        {           
            string plik = uzytkownik + @".out";
            blokada_dostepu_2.WaitOne();
                StreamReader sr = new StreamReader(sciezka + @"\" + plik);
                string odpowiedz = sr.ReadLine();
                sr.Close();
            blokada_dostepu_2.ReleaseMutex();
            Console.WriteLine("\nOdpowiedz z serwera: \n{0}",odpowiedz);           
        }
    }
}
