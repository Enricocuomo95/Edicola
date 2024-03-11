using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esame1
{
    internal class Abbonato
    {
        public string Nome {  get; set; }
        public string Cognome { get; set; }
        public string TipoAbbonamento {  get; set; }
        public string MetodoPagamento { get; set; }
        public Indirizzo Indirizzo { get; set; }



        public Abbonato(string Nome, string Cognome, string TipoAbbonamento, string MetodoPagamento, Indirizzo Indirizzo) 
        {
            this.Nome = Nome;
            this.TipoAbbonamento = TipoAbbonamento;
            this.Cognome = Cognome;
            this.MetodoPagamento = MetodoPagamento;
            this.Indirizzo = Indirizzo;
        }

        public Abbonato() { }   
        public string CSVString()
        {
            return ($"{Nome},{Cognome},{TipoAbbonamento},{MetodoPagamento},{Indirizzo}");
        }
    }
}
