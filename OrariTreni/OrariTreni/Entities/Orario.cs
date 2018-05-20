using System;
using System.Collections.Generic;
using System.Text;

namespace MyStop.Entities
{
    class Orario
    {
        public DateTime Time { get; set; }

        public string Run { get; set; }

        public string Line { get; set; }

        public List<Fermata> Fermate { get; set; }
    }
}
