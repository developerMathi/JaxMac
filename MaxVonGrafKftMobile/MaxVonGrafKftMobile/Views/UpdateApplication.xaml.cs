using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MaxVonGrafKftMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdateApplication : ContentPage
    {
        public UpdateApplication()
        {
            InitializeComponent();
        }

        private async void updateBtn_Clicked(object sender, EventArgs e)
        {
            if (Xamarin.Forms.Device.OS == TargetPlatform.iOS)
            {
                var location = RegionInfo.CurrentRegion.Name.ToLower();
                string url = "https://apps.apple.com/" + location + "/app/jax-rideshare-rentals/id1533816763";
                await Browser.OpenAsync(url, BrowserLaunchMode.External);

            }

            else if (Xamarin.Forms.Device.OS == TargetPlatform.Android)
            {
               string url = "https://play.google.com/store/apps/details?id=com.navotar.JAXRideshareRental";
               await Browser.OpenAsync(url, BrowserLaunchMode.External);
            }
        }
    }
}