using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace Serwer
{
    [Serializable]
    class Konfiguracja
    {
        public string TCP_medium { get; set; }
        public string UDP_medium { get; set; }
        public string RS232_medium { get; set; }
        public string File_medium { get; set; }

        public string Ping_service { get; set; }
        public string Chat_service { get; set; }
        public string Configure_service { get; set; }

        public Konfiguracja()
        {
                TCP_medium = "ON";
                UDP_medium = "ON";
                RS232_medium = "OFF";
                File_medium = "ON";

                Ping_service = "ON";
                Chat_service = "ON";
                Configure_service = "ON";
        }

        public Konfiguracja Wczytywanie()
        {
            Konfiguracja konfiguracja = new Konfiguracja();
            Mutex blokada_pliku_konfiguracji = new Mutex(false, "Plik-konf-7999-49d0-82fd-57bd4dd76613");

            blokada_pliku_konfiguracji.WaitOne();
            if (File.Exists(@"..\..\..\Konfiguracja.dat"))
            {              
                using (Stream input = File.OpenRead(@"..\..\..\Konfiguracja.dat"))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    konfiguracja = (Konfiguracja)bf.Deserialize(input);
                }
                Console.WriteLine("Wczytywanie powiodlo sie");
            }           
            blokada_pliku_konfiguracji.ReleaseMutex();

            return konfiguracja;
        }


        public void Zapisywanie()
        {
            Mutex blokada_pliku_konfiguracji = new Mutex(false, "Plik-konf-7999-49d0-82fd-57bd4dd76613");

            blokada_pliku_konfiguracji.WaitOne();
                using (Stream output = File.Create(@"..\..\..\Konfiguracja.dat"))
                {
                    BinaryFormatter bf2 = new BinaryFormatter();
                    bf2.Serialize(output, this);
                }
            blokada_pliku_konfiguracji.ReleaseMutex();
        }
    }
}
