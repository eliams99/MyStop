using System;

namespace OrariTreni.Entities
{
    public class TrainItem
    {
        public string TrainNumber { get; set; }

        public DateTime Time { get; set; }
        
        public string Destination { get; set; }

        public string Status { get; set; }

        public string DetailLink { get; set; }

        public string StopsLink { get; set; }
        

        //public List<StopItem> Stops { get; set; }
        //javascript:WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions("ctl00$ctl00$ctl00$ContentPlaceHolderDefault$cp_content$ctl01$ucMotoreRisultati_6$ButtonAvviaRicerca", "", true, "MotoreOrario", "", false, false))
        //javascript:WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions("&quot;ctl00$ctl00$ctl00$ContentPlaceHolderDefault$cp_content$ctl01$ucMotoreRisultati_6$ButtonAvviaRicerca&quot;, &quot;&quot;, true, &quot;MotoreOrario&quot;, &quot;&quot;, false, false"))
    }
}
