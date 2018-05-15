using System;

namespace OrariTreni.Entities
{
    public class StopItem
    {
        public string Stop { get; set; }

        public DateTime PlannedArrival { get; set; }

        public DateTime ActualArrival { get; set; }

        public string ExpectedPlatform { get; set; }

        public string RealPlatform { get; set; }
    }
}
