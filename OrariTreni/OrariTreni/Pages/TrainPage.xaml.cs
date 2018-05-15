using OrariTreni.Entities;
using OrariTreni.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OrariTreni.Pages
{
    public partial class TrainPage : TabbedPage
    {
        //private bool _isRefreshing = false;

        //public bool IsRefreshing
        //{
        //    get { return _isRefreshing; }
        //    set
        //    {
        //        _isRefreshing = value;
        //        OnPropertyChanged(nameof(IsRefreshing));
        //    }
        //}

        //public ICommand RefreshCommand
        //{
        //    get
        //    {
        //        return new Command(async () =>
        //        {
        //            IsRefreshing = true;
        //            try
        //            {
        //                //await GetTrains("stazione", Title);
        //            }
        //            catch
        //            {
        //                await DisplayAlert("Errore", "Servizio non disponibile", "Ok");
        //            }
        //            IsRefreshing = false;
        //        });
        //    }
        //}


        public TrainPage(TrainItem trainInfo)
        {
            InitializeComponent();

            GetTrainInfo(trainInfo);
        }

        private async Task GetTrainInfo(TrainItem trainInfo)
        {
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;
            try
            {
                await SetContextAsync(trainInfo);
            }
            catch
            {
                await DisplayAlert("Errore", "Servizio non disponibile", "Ok");
            }
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
        }

        private async Task SetContextAsync(TrainItem trainInfo)
        {
            TrainItem trainItem = new TrainItem();
            trainItem.TrainNumber = trainInfo.TrainNumber;
            trainItem.Time = trainInfo.Time;

            Parsing parsing = new Parsing();
            var trainDetails = await parsing.GetTrainDetailsAsync(trainInfo.DetailLink);
            trainItem.Status = trainDetails.Item1;
            trainItem.Destination = trainDetails.Item3;

            List<StopItem> stops = await parsing.GetTrainStopsAsync(trainDetails.Item2);
            StopsListView.ItemsSource = stops;
            
            BindingContext = trainItem;
        }

        private async void OnStopTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;

            await Navigation.PushAsync(new StationPage((StopItem)e.Item));      // Perfezionare per stazioni "Generiche" / con codice
        }
    }
}
