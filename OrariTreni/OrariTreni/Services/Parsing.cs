using HtmlAgilityPack;
using OrariTreni.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OrariTreni.Services
{
    class Parsing       // Aggiungere metodo che non considera le stazioni non esistenti
    {
        private async Task<HtmlDocument> DownloadStringAsync(string url, string stationAttributeName, string station)
        {
            WebClient client = new WebClient();
            client.QueryString.Add(stationAttributeName, station);
            client.QueryString.Add("lang", "IT");
            var data = await client.UploadValuesTaskAsync(url, "POST", client.QueryString);
            String downloadedString = UnicodeEncoding.UTF8.GetString(data);

            downloadedString = WebUtility.HtmlDecode(downloadedString);
            HtmlDocument result = new HtmlDocument();
            result.LoadHtml(downloadedString);

            return result;
        }

        private async Task<HtmlDocument> DownloadStringAsync(string url)     //Override
        {
            WebClient client = new WebClient();
            Uri uri = new Uri(url);

            string downloadedString = await client.DownloadStringTaskAsync(uri);

            HtmlDocument result = new HtmlDocument();
            result.LoadHtml(downloadedString);

            return result;
        }

        public async Task<List<TrainList>> GetTrainsAsync(string url, string stationAttributeName, string station)       //Ottiene l'elenco dei treni per una determinata stazione
        {
            HtmlDocument result = await DownloadStringAsync(url, stationAttributeName, station);

            List<HtmlNode> bloccoRisultato = result.DocumentNode.Descendants().Where
                (x => (x.Name == "div" && x.Attributes["class"] != null &&
                    x.Attributes["class"].Value.Contains("bloccorisultato"))).ToList();

            TrainList departuresList = new TrainList();
            TrainList arrivalsList = new TrainList();

            foreach (var item in bloccoRisultato)
            {
                var trainNumber = item.Descendants("h2").ToList();
                var bloccoTreno = item.Descendants().Where(x => (x.Name == "div" && x.Attributes["class"] != null &&
                    x.Attributes["class"].Value.Contains("bloccotreno"))).ToList()[0].Descendants("strong").ToList();
                var link = item.SelectSingleNode(".//a[@href]");

                if (!link.Attributes["href"].Value.Contains("lang=IT"))     // Se non contiene lang=IT significa che è una partenza
                    departuresList.Add(
                        new TrainItem
                        {
                            TrainNumber = trainNumber[0].InnerText,
                            Time = DateTime.Parse(bloccoTreno[1].InnerText),
                            Destination = bloccoTreno[0].InnerText,
                            DetailLink = link.Attributes["href"].Value
                        });
                else
                    arrivalsList.Add(
                        new TrainItem
                        {
                            TrainNumber = trainNumber[0].InnerText,
                            Time = DateTime.Parse(bloccoTreno[1].InnerText),
                            Destination = bloccoTreno[0].InnerText,
                            DetailLink = link.Attributes["href"].Value
                        });
            }
            departuresList.Title = "Partenze";
            arrivalsList.Title = "Arrivi";

            List<TrainList> trainList = new List<TrainList>()
            {
                departuresList,
                arrivalsList
            };
            //List<TrainItem> filteredTrainItems = trainItems.Where(x => x.Time > DateTime.Now).ToList();

            return trainList;
        }

        public async Task<string> GetStationNameAsync(string url, string stationAttributeName, string station)       //Ottiene il nome della stazione dalla schermata lista treni per stazione
        {
            HtmlDocument result = await DownloadStringAsync(url, stationAttributeName, station);

            return result.DocumentNode.Descendants("h1").ToList()[0].InnerText;
        }

        public async Task<(List<string>, List<string>)> ShowStationsAsync(string url, string station)
        {
            HtmlDocument result = await DownloadStringAsync(url, "stazione", station);

            var select = result.DocumentNode.SelectNodes("//select[@name='codiceStazione']");
            List<HtmlNode> options = select[0].SelectNodes("//option").ToList();
            List<string> stationsId = new List<string>();
            List<string> stationsName = new List<string>();

            foreach (var item in options)
            {
                stationsId.Add(item.Attributes["value"].Value);
                stationsName.Add(item.InnerText);
            }
            return (stationsId, stationsName);
        }

        //public async bool StationExists(string url, string station)
        //{
        //    HtmlDocument result = await DownloadStringAsync(url, "stazione", station);

        //    result.
        //}

        public async Task<(string, string, string)> GetTrainDetailsAsync(string detailLink)      // Ottiene i dettagli del treno (stato, ultimo rilevamento)
        {
            HtmlDocument result = await DownloadStringAsync("http://mobile.my-link.it/mylink/mobile/" + detailLink);

            List<HtmlNode> evidenziato = result.DocumentNode.Descendants().Where
                (x => (x.Name == "div" && x.Attributes["class"] != null &&
                    x.Attributes["class"].Value.Contains("evidenziato"))).ToList();

            List<HtmlNode> corpoCentrale = result.DocumentNode.Descendants().Where
                (x => (x.Name == "div" && x.Attributes["class"] != null &&
                    x.Attributes["class"].Value.Contains("corpocentrale"))).ToList();
            HtmlNode destinationNode = corpoCentrale[corpoCentrale.ToList().Count - 1];

            TrainItem trainItem = new TrainItem();

            trainItem.StopsLink = evidenziato.ToList()[evidenziato.ToList().Count - 1].SelectSingleNode("//a").Attributes["href"].Value;

            return (EditText(HttpUtility.HtmlDecode(evidenziato.ToList()[evidenziato.ToList().Count - 1].InnerText)),
                    trainItem.StopsLink,
                    destinationNode.Descendants("h2").ToList()[0].InnerText);
        }

        private string EditText(string inputString)
        {
            inputString = inputString.Replace("\r", String.Empty);
            inputString = inputString.Replace("\t", String.Empty);
            inputString = inputString.Replace("\n\n\n\n\n", " ");
            inputString = inputString.Replace("\n\n", " ");
            inputString = inputString.Replace("\n\n\n\n\n\n", "\n");
            inputString = inputString.Replace("                \n                 \n                \n                    ", String.Empty);
            inputString = inputString.Replace("\n                \n", String.Empty);
            inputString = inputString.Replace("\n                                 \n                ", String.Empty);

            return inputString.Substring(1);
        }

        public async Task<List<StopItem>> GetTrainStopsAsync(string stopsLink)       //Ottiene le fermate
        {
            HtmlDocument result = await DownloadStringAsync("http://mobile.my-link.it" + stopsLink);

            List<HtmlNode> alreadyMade = result.DocumentNode.Descendants().Where
                (x => (x.Name == "div" && x.Attributes["class"] != null &&
                    x.Attributes["class"].Value.Contains("giaeffettuate"))).ToList();

            List<HtmlNode> stillNotMade = result.DocumentNode.Descendants().Where
                (x => (x.Name == "div" && x.Attributes["class"] != null &&
                    x.Attributes["class"].Value.Contains("corpocentrale"))).ToList();


            alreadyMade.AddRange(stillNotMade.ToList());
            List<StopItem> stopItems = new List<StopItem>();

            foreach (var item in alreadyMade)
            {
                var stop = item.Descendants("h2").ToList();
                var bloccoTreno = item.Descendants("p").ToList();
                stopItems.Add(new StopItem
                {
                    Stop = stop[0].InnerText,
                    PlannedArrival = DateTime.Parse(bloccoTreno[0].Descendants("strong").ToList()[0].InnerText),
                    //ActualArrival = DateTime.Parse(bloccoTreno[1].Descendants("strong").ToList()[0].InnerText)    //Nelle fermate non ancora effettuate manca il tag di chiusura </p>
                });
            }

            return stopItems;
        }

        //public void Trenord(string departureStation, string arrivalStation)
        //{
        //    WebClient client = new WebClient();
        //    client.QueryString.Add(stationAttributeName, departureStation);
        //    client.QueryString.Add("lang", "IT");
        //    var data = client.UploadValues(url, "POST", client.QueryString);
        //    String downloadedString = UnicodeEncoding.UTF8.GetString(data);

        //    downloadedString = WebUtility.HtmlDecode(downloadedString);
        //    HtmlDocument result = new HtmlDocument();
        //    result.LoadHtml(downloadedString);
        //}
    }
}