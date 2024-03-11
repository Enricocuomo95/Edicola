using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esame1
{
    internal class Vendita
    {
        public DateTime DataConsegna { get; set; }
        public Pubblicazione Pubblicazione { get; set; }

        //devo tener conto anche delle vendite ai clienti non abbonati 
        //in tal caso questo valore è null
        public Abbonato? Abbonato { get; set; } 
        
        public Vendita() 
        {
            //null di dafault infatti
            Abbonato = null;
        }

        public string CSVString()
        {
            string s = DataConsegna.ToString("MM/dd/yyyy");

            if (Abbonato != null)
                return ($"Abbiamo venduto {Pubblicazione} al cliente {Abbonato.CSVString()} in data {s} ");
            return ($"Abbiamo venduto {Pubblicazione.CSVString()} in data {s} ");
        }
    }
}
