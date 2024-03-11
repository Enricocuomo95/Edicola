using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esame1
{
    internal class Indirizzo
    {
        public string Via {  get; set; }
        public string Civico { get; set; }
        public string Cap {  get; set; }

        public override string ToString()
        {
            return ($" Via: {Via} {Civico} {Cap}");
        }

        public Indirizzo() { }
    }
}
