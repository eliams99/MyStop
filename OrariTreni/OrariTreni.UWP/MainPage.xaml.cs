using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Profile;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace OrariTreni.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            LoadApplication(new OrariTreni.App());

            if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Desktop")
            {
                ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
                titleBar.BackgroundColor = Windows.UI.Colors.DarkCyan;
                titleBar.ForegroundColor = Windows.UI.Colors.White;
                titleBar.ButtonBackgroundColor = Windows.UI.Colors.DarkCyan;
                titleBar.ButtonForegroundColor = Windows.UI.Colors.White;
            }
        }
    }
}
