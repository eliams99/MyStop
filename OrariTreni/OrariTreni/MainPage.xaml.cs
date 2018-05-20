using MyStop.Services;
using OrariTreni.Entities;
using OrariTreni.Pages;
using OrariTreni.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OrariTreni
{
    public partial class MainPage : ContentPage
    {
        List<string> stationId;
        StationItem stationItem = new StationItem();

        public MainPage()
        {
            InitializeComponent();
            Muoversi m = new Muoversi();
            m.ProvaX509();
        }

        private async void OnSearchStationButtonClicked(object sender, EventArgs e)
        {
            await ManageStationPage();
        }

        private async Task ManageStationPage()
        {
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;
            try
            {
                await GetStationName();
            }
            catch
            {
                await DisplayAlert("Errore", "Servizio non disponibile", "Ok");
            }
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
        }

        private async Task GetStationName()
        {
            Parsing parsing = new Parsing();

            if (StationEntry.IsVisible == true)     // Se al nome è associata una sola stazione
            {
                stationItem.StationName = await parsing.GetStationNameAsync("http://mobile.my-link.it/mylink/mobile/stazione", "stazione", StationEntry.Text.Trim());
                if (stationItem.StationName != "Cerca Treno Per Stazione")
                    await Navigation.PushAsync(new StationPage(stationItem.StationName.Substring(13)));
                else
                    await GetMultipleStations();
            }
            else    // Se al nome sono associate più stazioni
            {
                stationItem.StationName = await parsing.GetStationNameAsync("http://mobile.my-link.it/mylink/mobile/stazione", "codiceStazione", stationId[StationPicker.SelectedIndex]);
                await Navigation.PushAsync(new StationPage(stationId[StationPicker.SelectedIndex], StationPicker.SelectedItem.ToString()));
            }
        }

        private async Task GetMultipleStations()
        {
            Parsing parsing = new Parsing();
            
            try
            {
                var stationValues = await parsing.ShowStationsAsync("http://mobile.my-link.it/mylink/mobile/stazione", StationEntry.Text.Trim());
                StationPicker.ItemsSource = stationValues.Item2;
                StationPicker.SelectedIndex = 0;
                stationId = stationValues.Item1;
                StationPicker.IsVisible = true;
                StationEntry.IsVisible = false;
            }
            catch
            {
                await DisplayAlert("Errore", "Stazione non esistente", "Ok");
            }
        }

        private void OnStationTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;

            //await Navigation
        }

        private async void ChartsToolbarItemClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ChartsPage());
        }

        private async void AboutToolbarItemClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AboutPage());
        }
    }
}