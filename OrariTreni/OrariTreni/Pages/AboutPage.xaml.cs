using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OrariTreni.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AboutPage : ContentPage
	{
        public AboutPage()
        {
            InitializeComponent();

            E015Image.Source = ImageSource.FromResource("OrariTreni.Assets.E015-Logo_white.png", Assembly.GetExecutingAssembly());
            var embeddedImage = new Image
            {
                Source = ImageSource.FromResource("OrariTreni.Assets.E015-Logo_white.png")
            };
        }
	}
}