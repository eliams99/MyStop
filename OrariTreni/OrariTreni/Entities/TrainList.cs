using System;
using System.Collections.Generic;
using System.Text;

namespace OrariTreni.Entities
{
    class TrainList : List<TrainItem>
    {
        public string Title { get; set; }

        public List<TrainItem> Trains => this;
    }
}
