using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Serwer
{
    interface IOdpowiedz
    {
        string Odpowiedz_serwera(string zapytanie, List<Obiekt_klienta> lista_klientow, Mutex blokada, Konfiguracja konfiguracja);
    }
}
