using System;
using System.Collections.Generic;
using System.Text;

namespace MyStop.Entities
{
    class ElementoRichiesta
    {
        public DateTime Date { get; set; }

        public List<int> Options { get; set; }

        public DateTime When { get; set; }

        public int TipoData { get; set; }

        public string Stop { get; set; }
    }
}
