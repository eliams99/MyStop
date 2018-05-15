using System.Collections.Generic;
using Xamarin.Forms;
using SkiaSharp;
using Entry = Microcharts.Entry;
using Microcharts;

namespace OrariTreni.Pages
{
    public partial class ChartsPage : ContentPage
    {
        List<Entry> entries = new List<Entry>
        {
            new Entry(200)
            {
                Color=SKColor.Parse("#FF1943"),
                Label ="Gennaio",
                ValueLabel = "200"
            },
            new Entry(400)
            {
                Color = SKColor.Parse("00BFFF"),
                Label = "Marzo",
                ValueLabel = "400"
            },
            new Entry(-100)
            {
                Color =  SKColor.Parse("#00CED1"),
                Label = "Ottobre",
                ValueLabel = "-100"
            }
            };

        public ChartsPage()
        {
            InitializeComponent();
            var chart = new LineChart() { Entries = entries, BackgroundColor = SKColor.Empty };
            this.ChartView.Chart = chart;

        }
    }
}