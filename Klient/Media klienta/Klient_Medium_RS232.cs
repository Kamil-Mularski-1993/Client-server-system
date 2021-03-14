using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Klient
{

    class Klient_Medium_RS232
    {
        static SerialPort _serialPort;

        public void Start(string uzytkownik)
        {
            _serialPort = new SerialPort();

            _serialPort.PortName = "COM2";// _serialPort.PortName; 
            _serialPort.BaudRate = 57600;
            _serialPort.Parity = Parity.None;
            _serialPort.DataBits = 8;
            _serialPort.StopBits = StopBits.One;
            _serialPort.Handshake = Handshake.None;
            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 500;
            _serialPort.Open();

            string wiadomosc = "";
            string tmp = "";
            string polecenie = "";
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
                            _serialPort.WriteLine(String.Format("{0}", wiadomosc));
                            // Console.WriteLine("TEST 2");
                            string odpowiedz = null;
                            odpowiedz = _serialPort.ReadLine();
                            // Console.WriteLine("TEST 3");

                            if ((!odpowiedz.Contains("EMPTY") && (odpowiedz != null) && (odpowiedz != ""))) Console.WriteLine("\nOdpowiedz z serwera: \n{0}", odpowiedz);
                            Thread.Sleep(500);
                            //Console.WriteLine("TEST 4");
                    }

                }
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
