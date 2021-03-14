using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Serwer
{
    class Serwer_Medium_RS232
    {
        static SerialPort _serialPort;
        public void Start(List<Obiekt_klienta> lista_klientow, Mutex blokada, Konfiguracja konfiguracja)
        {
            _serialPort = new SerialPort();
            _serialPort.PortName = "COM1";
            _serialPort.BaudRate = 57600;
            _serialPort.Parity = Parity.None;
            _serialPort.DataBits = 8;
            _serialPort.StopBits = StopBits.One;
            _serialPort.Handshake = Handshake.None;
            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 500;
            _serialPort.Open();

            Thread readThread = new Thread(() => {

                while (true)
                {
                    try
                    {
                        string dane = null;
                        string odpowiedz = "";
                        //byte[] wiadomosc = null;
                        Uslugi uslugi = new Uslugi();

                        while (true)
                        {
                            dane = _serialPort.ReadLine();
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

                        _serialPort.WriteLine(String.Format("{0}", odpowiedz));

                        if (odpowiedz.Contains("END")) break;
                    }
                    catch (TimeoutException) { }
                }
            });
            readThread.Start();

            readThread.Join();
            _serialPort.Close();
        }
    }
}
