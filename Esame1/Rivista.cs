using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esame1
{
    internal class Rivista : Pubblicazione
    {
        public Rivista(string nome, DateTime dataUscita, string categoria, int nStore) : base(nome, dataUscita, categoria, nStore)
        {
        }
    }
}
