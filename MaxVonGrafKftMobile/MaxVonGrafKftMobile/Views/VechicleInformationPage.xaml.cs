using MaxVonGrafKftMobileModel;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace MaxVonGrafKftMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VechicleInformationPage : ContentPage
    {
        private ReservationView reservationView;
        private VehicleViewByTypeForMobile selectedVehicle;
        private List<int> locationIdList;
        decimal jpp;
        decimal jmp;
        decimal ins;


        //public VechicleInformationPage(ReservationView reservationView, VehicleViewByTypeForMobile selectedVehicle)
        //{
        //    InitializeComponent();
        //    this.reservationView = reservationView;
        //    this.selectedVehicle = selectedVehicle;
        //    if (selectedVehicle.VehicleImageUrl != null)
        //    {
        //        vehileTypeImage.Source = ImageSource.FromUri(new Uri(selectedVehicle.VehicleImageUrl));
        //    }
        //    vehicleSampleLabel.Text = selectedVehicle.vehicleName;
        //    vehicleTypeLabel.Text = selectedVehicle.VehicleType;
        //    //loactionLabel.Text = reservationView.StartLocationName + " - " + reservationView.EndLocationName;
        //    //timeLineLabel.Text = ((DateTime)reservationView.StartDate).ToString("ddd,MM/dd,hh:mm") + " - " + ((DateTime)reservationView.EndDate).ToString("ddd,MM/dd,hh:mm");
        //    seatCountLabel.Text = selectedVehicle.Seats.ToString() + " Seats";
        //    bagCountLabel.Text = selectedVehicle.NoOfLuggage.ToString() + " Bags";
        //    gearLabel.Text = selectedVehicle.Transmission.ToString();
        //    if (selectedVehicle.doors != null)
        //    {
        //        doorCountLabel.Text = selectedVehicle.doors.ToString() + " Doors";
        //    }
        //    if (selectedVehicle.HtmlContent != null)
        //    {
        //        VehicleDiscription.Text = selectedVehicle.HtmlContent.Replace("<p>", "").Replace("</p>", "").Replace("&nbsp;&nbsp;", "");
        //    }
        //    totalAmtLabel.Text = "$ " + selectedVehicle.DailyRate.ToString();
        //    //if (selectedVehicle.HtmlContent != null)
        //    //{
        //    //    desWebView = new WebView();
             
        //    //    var htmlSource = new HtmlWebViewSource();
        //    //    htmlSource.Html = @"<html><body>
        //    //                          <h1>Xamarin.Forms</h1>
        //    //                          <p>Welcome to WebView.</p>
        //    //                          </body></html>";
        //    //    desWebView.Source = htmlSource;
        //    //}
        //}

        public VechicleInformationPage(ReservationView reservationView, VehicleViewByTypeForMobile selectedVehicle, List<int> locationIdList) 
        {
            InitializeComponent();
            this.reservationView = reservationView;
            this.selectedVehicle = selectedVehicle;
            if (selectedVehicle.VehicleImageUrl != null)
            {
                vehileTypeImage.Source = ImageSource.FromUri(new Uri(selectedVehicle.VehicleImageUrl));
            }
            vehicleSampleLabel.Text = selectedVehicle.vehicleName;
            vehicleTypeLabel.Text = selectedVehicle.VehicleType;
            //loactionLabel.Text = reservationView.StartLocationName + " - " + reservationView.EndLocationName;
            //timeLineLabel.Text = ((DateTime)reservationView.StartDate).ToString("ddd,MM/dd,hh:mm") + " - " + ((DateTime)reservationView.EndDate).ToString("ddd,MM/dd,hh:mm");
            seatCountLabel.Text = selectedVehicle.Seats.ToString() + " Seats";
            bagCountLabel.Text = selectedVehicle.NoOfLuggage.ToString() + " Bags";
            gearLabel.Text = selectedVehicle.Transmission.ToString();
            if (selectedVehicle.doors != null)
            {
                doorCountLabel.Text = selectedVehicle.doors.ToString() + " Doors";
            }
            if (selectedVehicle.HtmlContent != null)
            {
                VehicleDiscription.Text = selectedVehicle.HtmlContent.Replace("<p>", "").Replace("</p>", "").Replace("&nbsp;&nbsp;", "");
            }
            totalAmtLabel.Text = "$ " + selectedVehicle.DailyRateWithInsurance.ToString();
            this.locationIdList = locationIdList;


            jpp = 0;
            jmp = 0;
            ins = 0;

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                PopupNavigation.Instance.PopAllAsync();
            }

            if(selectedVehicle != null)
            {
                if(selectedVehicle.mantatoryMiscChargeDetails != null)
                {
                    foreach( ReservationMiscDetails s in selectedVehicle.mantatoryMiscChargeDetails)
                    {
                        if(s.Name== "Jax Protection Plan")
                        {
                            jpp = Math.Round(Convert.ToDecimal(s.MiscChargePerDay), 2);
                        }

                        if (s.Name == "Jax Maintenance Plan")
                        {
                            jmp = Math.Round(Convert.ToDecimal(s.MiscChargePerDay), 2);
                        }
                        if (s.Name == "Insurance")
                        {
                            ins = Math.Round(Convert.ToDecimal(s.MiscChargePerDay), 2);
                        }

                    }
                }
            }

            dailyRateDetailLabel.Text = "( Per day )";
            DailyRateTotal.Text = "$ " + ((decimal)selectedVehicle.RateDetail.DailyRate).ToString("0.00");

            weeklyRateDetailLabeljpp.Text = "( Per day )";
            weeklyRateTotaljpp.Text = "$ " + (jpp).ToString("0.00");

            weekEndRateDetailLabeljmp.Text = "( Per day )";
            weekEndRateTotaljmp.Text = "$ " + (jmp).ToString("0.00");

            weekEndRateDetailLabeljIns.Text = "( Per day )";
            weekEndRateTotalIns.Text = "$ " + (ins).ToString("0.00");


        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            if (selectedVehicle.SharableLink != null)
            {
                _ = ShareUri(selectedVehicle.SharableLink);
            }
            else
            {
                _ = ShareUri("");
            }
        }

        private void btnPriceDetail_Tapped(object sender, EventArgs e)
        {
            Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new Popups.PriceDetailPopup(selectedVehicle));
        }

        private void btnContinue_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new BookNow(reservationView, selectedVehicle,locationIdList));
        }

        public async Task ShareUri(string uri)
        {

            var screenshot = await Screenshot.CaptureAsync();
            var stream = await screenshot.OpenReadAsync();
            string name = "vehDetails" + ".png";

            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), name);

            using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                stream.CopyTo(fileStream);
            }


            await Share.RequestAsync(new ShareFileRequest
            {
                Title = Title,
                File = new ShareFile(fileName)
            });


           
        }

        private void btnBack_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}