using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Klient
{
    class Klient_Medium_TCP
    {
        public void Start(string uzytkownik)
        {

            TcpClient klient = new TcpClient("localhost", 12345);
            NetworkStream strumien = klient.GetStream();

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
                       // Console.WriteLine("TEST");
                        data = Encoding.ASCII.GetBytes(wiadomosc);
                        strumien.Write(data, 0, data.Length);
                       // Console.WriteLine("TEST 2");
                        byte[] dane = new byte[256];
                        string odpowiedz = string.Empty;
                        int bytes;
                        do
                        {
                            bytes = strumien.Read(dane, 0, dane.Length);
                            odpowiedz += Encoding.ASCII.GetString(dane, 0, bytes);
                        }
                        while (!odpowiedz.Contains('\n'));
                       // Console.WriteLine("TEST 3");
                        Thread.Sleep(500);
                        if ((!odpowiedz.Contains("EMPTY") && (odpowiedz != null) && (odpowiedz != ""))) Console.WriteLine("\nOdpowiedz z serwera: \n{0}", odpowiedz);
                        //Console.WriteLine("TEST 4");
                    }
                    
                }
                klient.Close();
                strumien.Close();
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
