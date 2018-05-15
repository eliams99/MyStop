using OrariTreni.Entities;
using OrariTreni.Services;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace OrariTreni.Pages
{
    public partial class StationPage : ContentPage
    {
        private bool _isRefreshing = false;

        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    IsRefreshing = true;
                    try
                    {
                        await GetTrains("stazione", Title);
                    }
                    catch
                    {
                        await DisplayAlert("Errore", "Servizio non disponibile", "Ok");
                    }
                    IsRefreshing = false;
                });
            }
        }

        public StationPage(string stationId)
        {
            InitializeComponent();
            SetBindings();

            Title = stationId;

            GetTrains("stazione", stationId);
        }

        public StationPage(string stationId, string stationName)
        {
            InitializeComponent();
            SetBindings();

            Title = stationName;

            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;
            GetTrains("codiceStazione", stationId);
        }

        public StationPage(StopItem stop)
        {
            InitializeComponent();
            SetBindings();

            Title = stop.Stop;

            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;
            GetTrains("stazione", stop.Stop);
        }

        private void SetBindings()
        {
            TrainsListView.BindingContext = this;
            TrainsListView.BindingContext = this;
        }

        private async Task GetTrains(string stationAttribute, string station)
        {
            Parsing parsing = new Parsing();
            
            try
            {
                var trains = await parsing.GetTrainsAsync("http://mobile.my-link.it/mylink/mobile/stazione", stationAttribute, station);
                if (trains.Count != 0)
                    TrainsListView.ItemsSource = trains;
                else
                    await DisplayAlert("Avviso", "Nessun treno in partenza o in arrivo", "Ok");
            }
            catch
            {
                await DisplayAlert("Errore", "Servizio non disponibile", "Ok");
            }
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
        }

        private async void OnTrainTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;

            await Navigation.PushAsync(new TrainPage((TrainItem)e.Item));
        }

        private void FavoritesToolbarItemClicked(object sender, EventArgs e)
        {
            
        }
    }
}