using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esame1
{
    internal abstract class Pubblicazione
    {
        public string Nome { get; }
        public DateTime DataUscita { get; }
        public string Categoria { get; }
        public int NStore { get; set; }

        public Pubblicazione(string nome, DateTime dataUscita, string categoria, int nStore)
        {
            Nome = nome;
            DataUscita = dataUscita;
            Categoria = categoria;
            NStore = nStore;
        }

        public void incrementNStore(int incremento)
        {
            NStore = NStore + incremento;
        }

        public override string ToString()
        {
            return ($"[PUBBLICAZIONE] Nome: {Nome}, Data di uscita: {DataUscita}, Categoria: {Categoria}, Store in magazzino: {NStore}");
        }

        public string CSVString()
        {
            string s = DataUscita.ToString("dd/MM/yyyy");
            return ($"{Nome},{s},{Categoria},{NStore}");
        }


    }
}
