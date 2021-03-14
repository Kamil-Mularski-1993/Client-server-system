using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Klient
{
    class Klient_Medium_UDP
    {
        public void Start(string uzytkownik)
        {
            UdpClient klient = new UdpClient(10001);
            IPEndPoint klient_end_point = new IPEndPoint(IPAddress.Any, 10001);
            klient.Connect("localhost", 12345);

            string wiadomosc = "";
            string tmp = "";
            string polecenie = "";
            byte[] data;
            bool work = false;
            object blok = new object();

            new Thread(() =>
            {
                while (!wiadomosc.Contains("END"))
                {
                    lock (blok)
                    {
                        if (tmp == "") tmp = "CHECK " + uzytkownik + " " + uzytkownik + " \n";
                        wiadomosc = tmp;
                        tmp = "";
                    }
                    if (wiadomosc.Contains("LOGIN")) work = true;
                    if (work == true)
                    {
                        data = Encoding.ASCII.GetBytes(wiadomosc);
                        klient.Send(data, data.Length);
                        byte[] dane = new byte[256];
                        string odpowiedz = string.Empty;
                        byte[] bytesRecive = klient.Receive(ref klient_end_point);
                        odpowiedz += Encoding.ASCII.GetString(bytesRecive, 0, bytesRecive.Length);
                        Thread.Sleep(500);
                        if ((!odpowiedz.Contains("EMPTY") && (odpowiedz != null) && (odpowiedz != ""))) Console.WriteLine("\nOdpowiedz z serwera: \n{0}", odpowiedz);
                    }

                }
                klient.Close();
            }).Start();

            while (!polecenie.Contains("END"))
            {
                polecenie = Zapytania.Zapytanie(uzytkownik);
                lock (blok)
                {
                    tmp = polecenie;
                }
                polecenie = "";
                Thread.Sleep(500);
            }
        }

    }
}
