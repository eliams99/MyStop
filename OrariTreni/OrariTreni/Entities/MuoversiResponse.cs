using System;
using System.Collections.Generic;
using System.Text;

namespace MyStop.Entities
{
    class MuoversiResponse : List<ElementoRichiesta>
    {
        public int Stato { get; set; }

        public string Lang { get; set; }

        public List<ElementoRichiesta> Richiesta => this;

        public List<Orario> Orari;
    }
}
