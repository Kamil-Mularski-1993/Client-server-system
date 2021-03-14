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
    class Serwer_Medium_TCP
    {
        public void Start(List<Obiekt_klienta> lista_klientow, Mutex blokada, Konfiguracja konfiguracja)
        {
            TcpListener serwer = new TcpListener(IPAddress.Any, 12345);
            serwer.Start();
            TcpClient aktywny_klient = null;
            byte[] bytes = new byte[256];
            while (true)
            {
                TcpClient klient = serwer.AcceptTcpClient(); 
                if (aktywny_klient != klient)
                {
                    Console.WriteLine("\nSerwer zauwazyl klienta\n");
                    aktywny_klient = klient;
                    NetworkStream strumien = klient.GetStream();
                    new Thread(() =>
                    {
                      //  string uzytkownik = null;
                        while (true)
                        {                           
                            string dane = null;
                            string odpowiedz = "";
                            byte[] wiadomosc = null;
                            Uslugi uslugi = new Uslugi();
                           // string[] czesci = null;
                            while (true)
                            {
                                dane += Encoding.ASCII.GetString(bytes, 0, strumien.Read(bytes, 0, bytes.Length));
                                odpowiedz = uslugi.Uslugi_Menu(dane, konfiguracja);
                                if (odpowiedz != null) break;
                            }
                                                      
                           
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
                                strumien.Write(wiadomosc, 0, wiadomosc.Length);
                            }

                            if(odpowiedz.Contains("END")) break;
                        }
                        Console.WriteLine("Zakonczono");
                        klient.Close();
                        strumien.Close();

                    }).Start();
                }

            }
        }
    }
}
