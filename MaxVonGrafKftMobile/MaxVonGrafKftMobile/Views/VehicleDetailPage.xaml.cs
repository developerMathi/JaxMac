using MaxVonGrafKftMobile.Popups;
using MaxVonGrafKftMobileController;
using MaxVonGrafKftMobileModel;
using MaxVonGrafKftMobileModel.AccessModels;
using MaxVonGrafKftMobileModel.Constants;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MaxVonGrafKftMobile.Views
{
    public partial class VehicleDetailPage : ContentPage
    {
        string token;
        int customerId;
        public static VehicleFilterSearch VehicleFilter;




        List<VehicleTypeResult> vehicleResults;
        //      GetVehicleDetailsMobileListResponse vehicleResponse;
        private ReservationView reservationView;
        ReservationConfigurationVehicleSearch search;

        GetReservationConfigurationMobileRequest vehicleMobileRequest;
        GetReservationConfigurationResponse vehicleMobileResponse;
        List<VehicleViewByTypeForMobile> forlistViewItemSource;
        List<VehicleViewByTypeForMobile> forlistViewItemSourceWithFiter;
        List<string> vehicletypeList;



        public VehicleDetailPage()
        {

            InitializeComponent();
            this.reservationView = new ReservationView();
            search = new ReservationConfigurationVehicleSearch();
            vehicleMobileRequest = new GetReservationConfigurationMobileRequest();
            vehicleMobileResponse = null;
            search.ClientId = Constants.ClientId;
            //search.LocationId = (int)reservationView.StartLocationId;
            //search.CheckInLocationId = (int)reservationView.EndLocationId;
            search.IsOnline = true;
            //search.StartDate = (DateTime)reservationView.StartDate;
            //search.EndDate = (DateTime)reservationView.EndDate;

            search.StartDate = DateTime.Now;
            search.EndDate = DateTime.Now.AddDays(3);

            search.VehicleCategoryId = 0;
            search.VehicleMakeID = 0;
            search.ModelId = 0;
            search.NumberOfSeats = 0;
            search.RentalPeriod = 0;
            search.VehicleId = 0;
            //lblLocation.Text = reservationView.StartLocationName;
            //startDateLabel.Text = ((DateTime)reservationView.StartDate).ToString("ddd,MM/dd/yyyy");
            //endDateLabel.Text = ((DateTime)reservationView.EndDate).ToString("ddd,MM/dd/yyyy");
            //startTimeLabel.Text = ((DateTime)reservationView.StartDate).ToString("hh:mm tt");
            //endTimeLabel.Text = ((DateTime)reservationView.EndDate).ToString("hh:mm tt");
            vehicletypeList = new List<string>();
            VehicleFilter = new VehicleFilterSearch();


            vehicleMobileRequest.search = search;
        }




        public VehicleDetailPage(ReservationView reservationView)
        {

            InitializeComponent();
            this.reservationView = reservationView;
            search = new ReservationConfigurationVehicleSearch();
            vehicleMobileRequest = new GetReservationConfigurationMobileRequest();
            vehicleMobileResponse = null;
            search.ClientId = Constants.ClientId;
            //search.LocationId = (int)reservationView.StartLocationId;
            //search.CheckInLocationId = (int)reservationView.EndLocationId;
            search.IsOnline = true;
            //search.StartDate = (DateTime)reservationView.StartDate;
            //search.EndDate = (DateTime)reservationView.EndDate;

            search.StartDate = DateTime.Now;
            search.EndDate = DateTime.Now.AddDays(1);

            search.VehicleCategoryId = 0;
            search.VehicleMakeID = 0;
            search.ModelId = 0;
            search.NumberOfSeats = 0;
            search.RentalPeriod = 0;
            search.VehicleId = 0;
            //lblLocation.Text = reservationView.StartLocationName;
            //startDateLabel.Text = ((DateTime)reservationView.StartDate).ToString("ddd,MM/dd/yyyy");
            //endDateLabel.Text = ((DateTime)reservationView.EndDate).ToString("ddd,MM/dd/yyyy");
            //startTimeLabel.Text = ((DateTime)reservationView.StartDate).ToString("hh:mm tt");
            //endTimeLabel.Text = ((DateTime)reservationView.EndDate).ToString("hh:mm tt");
            vehicletypeList = new List<string>();
            VehicleFilter = new VehicleFilterSearch();


            vehicleMobileRequest.search = search;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var assembly = typeof(BookNow);

            token = App.Current.Properties["currentToken"].ToString();
            customerId = (int)App.Current.Properties["CustomerId"];
            forlistViewItemSource = new List<VehicleViewByTypeForMobile>();

            MessagingCenter.Subscribe<FilterPopup>(this, "FilterUpdated", sender =>
            {
                if (forlistViewItemSource != null)
                {
                    if (forlistViewItemSource.Count > 0)
                    {
                        forlistViewItemSourceWithFiter = new List<VehicleViewByTypeForMobile>();
                        forlistViewItemSourceWithFiter = forlistViewItemSource;
                        if (VehicleFilter.Price > 0)
                        {
                            forlistViewItemSourceWithFiter = filterbyPrice();
                        }
                        if (VehicleFilter.MinPrice > 0)
                        {
                            forlistViewItemSourceWithFiter = filterbyPriceMin();
                        }
                        if (VehicleFilter.VehicleType != null && forlistViewItemSourceWithFiter.Count > 0)
                        {
                            forlistViewItemSourceWithFiter = filterByVehType();
                        }

                        if (VehicleFilter.seatsCount > 0 && forlistViewItemSourceWithFiter.Count > 0)
                        {
                            forlistViewItemSourceWithFiter = filterbySeatCount();
                        }
                        if (VehicleFilter.buggageCount > 0 && forlistViewItemSourceWithFiter.Count > 0)
                        {
                            forlistViewItemSourceWithFiter = filterbyBAgCount();
                        }
                        if (VehicleFilter.DoorsCount > 0 && forlistViewItemSourceWithFiter.Count > 0)
                        {
                            forlistViewItemSourceWithFiter = filterbyDoorCount();
                        }
                        if (VehicleFilter.SortingOrder > -1 && forlistViewItemSourceWithFiter.Count > 0)
                        {
                            forlistViewItemSourceWithFiter = filterbySortingOrder();
                        }
                        if (forlistViewItemSourceWithFiter.Count > 0)
                        {
                            vehicleDetailList.ItemsSource = null;
                            vehicleDetailList.ItemsSource = forlistViewItemSourceWithFiter;
                            vehicleDetailList.HeightRequest = forlistViewItemSourceWithFiter.Count * 295;
                            noVehicleLabel.IsVisible = false;
                            vehicleDetailList.IsVisible = true;
                            VehicleFilter = null;
                            VehicleFilter = new VehicleFilterSearch();
                        }
                        else
                        {
                            vehicleDetailList.IsVisible = false;
                            //noVehicleLabel.IsVisible = true;
                            // buttonGrid.IsVisible = true;
                            noVehicleLabel.IsVisible = true;
                            VehicleFilter = null;
                            VehicleFilter = new VehicleFilterSearch();

                        }


                    }
                }
            });




            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                if (PopupNavigation.Instance.PopupStack[PopupNavigation.Instance.PopupStack.Count - 1].GetType() == typeof(ErrorWithClosePagePopup))
                {
                    await PopupNavigation.Instance.PopAllAsync();
                }
            }


            bool busy = false;
            if (!busy)
            {
                try
                {
                    busy = true;
                    await PopupNavigation.Instance.PushAsync(new LoadingPopup("Getting vehicles informations..."));

                    await Task.Run(() =>
                    {

                        try
                        {
                            vehicleMobileResponse = getVehicleTypesMobileNew(vehicleMobileRequest, token);
                            //vehicleResults = getVehicleTypes(token);
                            //vehicleResponse= getVehicleTypesMobile(token);


                        }
                        catch (Exception ex)
                        {
                            PopupNavigation.Instance.PushAsync(new ErrorWithClosePagePopup(ex.Message));
                        }



                    });
                }
                finally
                {

                    busy = false;
                    if (PopupNavigation.Instance.PopupStack.Count == 1)
                    {
                        await PopupNavigation.Instance.PopAllAsync();
                    }
                    if (PopupNavigation.Instance.PopupStack.Count > 1)
                    {
                        if (PopupNavigation.Instance.PopupStack[PopupNavigation.Instance.PopupStack.Count - 1].GetType() != typeof(ErrorWithClosePagePopup))
                        {
                            await PopupNavigation.Instance.PopAllAsync();
                        }
                    }
                }
                if (vehicleMobileResponse != null)
                {
                    if (vehicleMobileResponse.message.ErrorCode == "200")
                    {
                        if (vehicleMobileResponse.listVehicle.Count > 0)
                        {

                            List<int> vehicleTypeIds = new List<int>();

                            foreach (ReservationVehicleSearchViewModel rvsv in vehicleMobileResponse.listVehicle)
                            {
                                if (rvsv.VehicleStatusId == (int)VehicleStatus.Available)
                                {
                                    vehicleTypeIds.Add(rvsv.VehicleTypeId);
                                    VehicleViewByTypeForMobile typeForMobile = new VehicleViewByTypeForMobile();
                                    typeForMobile.VehicleTypeId = rvsv.VehicleTypeId;
                                    typeForMobile.VehicleType = rvsv.VehicleType;
                                    typeForMobile.Transmission = rvsv.Transmission;
                                    typeForMobile.Seats = rvsv.Seats;
                                    typeForMobile.NoOfLuggage = rvsv.Baggages;
                                    typeForMobile.DailyRate = decimal.Round(((decimal)rvsv.RateDetail.DailyRate + calculateMantatryCharges(rvsv.mantatoryMiscChargeDetails, 1)), 2);
                                    typeForMobile.VehicleTypeImageUrl = rvsv.VehicleTypeImage;
                                    typeForMobile.RateDetail = rvsv.RateDetail;
                                    typeForMobile.MileagePerDay = rvsv.MileagePerDay;
                                    typeForMobile.doors = rvsv.Doors;
                                    typeForMobile.IsVehicleAvailableDescription = rvsv.IsVehicleAvailableDescription;
                                    typeForMobile.HtmlContent = rvsv.HtmlContent;
                                    typeForMobile.SharableLink = rvsv.SharableLink;
                                    typeForMobile.VehicleImageUrl = rvsv.VehicleImageUrl;
                                    typeForMobile.mantatoryMiscChargeDetails = rvsv.mantatoryMiscChargeDetails;
                                    typeForMobile.mantatoryMiscChargeNonTaxableTotalForOneDay = rvsv.mantatoryMiscChargeNonTaxableTotalForOneDay;
                                    typeForMobile.mantatoryMiscChargeTotalForOneDay = rvsv.mantatoryMiscChargeTotalForOneDay;

                                    typeForMobile.sample = rvsv.Sample;
                                    typeForMobile.locationIdList = new List<int>();
                                    if (!typeForMobile.locationIdList.Contains(rvsv.LocationId))
                                    {
                                        typeForMobile.locationIdList.Add(rvsv.LocationId);
                                    }
                                    typeForMobile.vehicleId = rvsv.vehicleId;
                                    typeForMobile.vehicleName = rvsv.VehicleMakeName + " " + rvsv.ModelName + " " + rvsv.Year;

                                    forlistViewItemSource.Add(typeForMobile);
                                    if (!vehicletypeList.Contains(rvsv.VehicleType))
                                    {
                                        vehicletypeList.Add(rvsv.VehicleType);
                                    }

                                }
                                //else
                                //{

                                //    foreach (VehicleViewByTypeForMobile listIntype in forlistViewItemSource)
                                //    {
                                //        if (rvsv.VehicleTypeId == listIntype.VehicleTypeId)
                                //        {
                                //            if (rvsv.Transmission != listIntype.Transmission)
                                //            {
                                //                listIntype.Transmission = "Auto, Manual";
                                //            }
                                //        }

                                //        if (!listIntype.locationIdList.Contains(rvsv.LocationId))
                                //        {
                                //            listIntype.locationIdList.Add(rvsv.LocationId);
                                //        }
                                //    }

                                //}

                            }
                            if (forlistViewItemSource.Count > 0)
                            {
                                vehicleDetailList.ItemsSource = forlistViewItemSource;
                                vehicleDetailList.HeightRequest = (forlistViewItemSource.Count * 320);
                                vehicleDetailList.IsVisible = true;

                            }
                            else
                            {
                                vehicleDetailList.IsVisible = false;
                                //noVehicleLabel.IsVisible = true;
                                // buttonGrid.IsVisible = true;
                                noVehicleLabel.IsVisible = true;

                            }

                        }

                    }
                    else
                    {
                        await PopupNavigation.Instance.PushAsync(new ErrorWithClosePagePopup(vehicleMobileResponse.message.ErrorMessage));
                    }
                }
            }
        }

        private decimal calculateMantatryCharges(List<ReservationMiscDetails> mantatoryMiscChargeDetails, int noOfDays)
        {
            decimal sum = 0;
            if (mantatoryMiscChargeDetails != null)
            {
                foreach (ReservationMiscDetails rmd in mantatoryMiscChargeDetails)
                {
                    if (rmd.Name == "Jax Protection Plan" || rmd.Name == "Jax Maintenance Plan")
                    {
                        switch (rmd.CalculationType)
                        {
                            case "Fixed":
                                sum += (Math.Round(Convert.ToDecimal(rmd.MiscChargePerDay), 2));
                                break;

                            case "Perday":
                                sum += ((Math.Round(Convert.ToDecimal(rmd.MiscChargePerDay), 2)) * noOfDays);
                                break;
                        }
                    }
                }
            }
            return sum;
        }

        private List<VehicleViewByTypeForMobile> filterbyPriceMin()
        {
            return forlistViewItemSourceWithFiter.Where(c => (int)c.DailyRate >= (int)VehicleFilter.MinPrice).ToList();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<FilterPopup>(this, "FilterUpdated");
        }

        private List<VehicleViewByTypeForMobile> filterbySortingOrder()
        {
            if (VehicleFilter.SortingOrder == 0)
            {
                return forlistViewItemSourceWithFiter.OrderBy(x => x.DailyRate).ToList();
            }
            else if (VehicleFilter.SortingOrder == 1)
            {
                return forlistViewItemSourceWithFiter.OrderByDescending(x => x.DailyRate).ToList();
            }
            else
            {
                return forlistViewItemSourceWithFiter;
            }
        }

        private List<VehicleViewByTypeForMobile> filterbyDoorCount()
        {
            return forlistViewItemSourceWithFiter.Where(c => int.Parse(c.doors) == VehicleFilter.DoorsCount).ToList();
        }

        private List<VehicleViewByTypeForMobile> filterbyBAgCount()
        {
            return forlistViewItemSourceWithFiter.Where(c => c.NoOfLuggage == VehicleFilter.buggageCount).ToList();
        }

        private List<VehicleViewByTypeForMobile> filterByVehType()
        {
            return forlistViewItemSourceWithFiter.Where(c => c.VehicleType.ToLower().Contains(VehicleFilter.VehicleType.ToLower())).ToList();
        }

        private List<VehicleViewByTypeForMobile> filterbyPrice()
        {
            return forlistViewItemSourceWithFiter.Where(c => (int)c.DailyRate <= (int)VehicleFilter.Price).ToList();
        }

        private List<VehicleViewByTypeForMobile> filterbySeatCount()
        {
            return forlistViewItemSourceWithFiter.Where(c => c.Seats == VehicleFilter.seatsCount).ToList();
        }

        private GetReservationConfigurationResponse getVehicleTypesMobileNew(GetReservationConfigurationMobileRequest vehicleMobileRequest, string token)
        {
            GetReservationConfigurationResponse vehicleTypeResults = null;
            VehicleController vehicle = new VehicleController();
            try
            {
                vehicleTypeResults = vehicle.getVehicleTypesMobileNew(vehicleMobileRequest, token);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return vehicleTypeResults;
        }

        //private GetVehicleDetailsMobileListResponse getVehicleTypesMobile(string token)
        //{
        //    GetVehicleDetailsMobileListResponse vehicleTypeResults = null;
        //    VehicleController vehicle = new VehicleController();
        //    try
        //    {
        //        vehicleTypeResults = vehicle.getVehicleTypesMobile(token);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return vehicleTypeResults;
        //}

        //private List<VehicleTypeResult> getVehicleTypes(string token)
        //{
        //    List<VehicleTypeResult> vehicleTypeResults = null;
        //    VehicleController vehicle = new VehicleController();
        //    try
        //    {
        //        vehicleTypeResults = vehicle.getVehicleTypes(token);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return vehicleTypeResults;

        //}

        private void BacKBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }


        //private async void LoginIcon_Clicked(object sender, EventArgs e)
        //{
        //    var assembly = typeof(BookNow);
        //    if ((int)App.Current.Properties["CustomerId"] == 0)
        //    {

        //        loginIcon.IconImageSource = ImageSource.FromResource("MaxVonGrafKftMobile.Assets.logOutTool.png", assembly);
        //        await Navigation.PushAsync(new LoginPage());

        //    }
        //    else
        //    {
        //        bool logout = await DisplayAlert("Alert", "Are you sure want to logout", "Yes", "No");
        //        if (logout)
        //        {
        //            App.Current.Properties["CustomerId"] = 0;
        //            loginIcon.IconImageSource = ImageSource.FromResource("MaxVonGrafKftMobile.Assets.LogInTool.png", assembly);
        //            await Navigation.PushAsync(new WelcomPage());
        //        }
        //    }
        //}

        //private void VehicleDetailList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        //{
        //    VehicleViewByTypeForMobile selectedVehicle = vehicleDetailList.SelectedItem as VehicleViewByTypeForMobile;
        //    reservationView.VehicleTypeID = selectedVehicle.VehicleTypeId;
        //    reservationView.VehicleType = selectedVehicle.VehicleType;
        //    Rates rates = JsonConvert.DeserializeObject<Rates>(JsonConvert.SerializeObject(selectedVehicle.RateDetail));
        //    rates.RateId = selectedVehicle.RateDetail.RateID;
        //    List<Rates> rateDewtails = new List<Rates>();
        //    rates.StartDateStr = reservationView.StartDateStr;
        //    rates.EndDateStr = reservationView.EndDateStr;
        //    rateDewtails.Add(rates);
        //    reservationView.RateDetailsList = rateDewtails;
        //    reservationView.TotalDays = rates.TotalDays;
        //    Navigation.PushAsync(new VechicleInformationPage(reservationView, selectedVehicle));
        //}

        private void VehicleDetailList_Refreshing(object sender, EventArgs e)
        {
            vehicleDetailList.ItemsSource = forlistViewItemSource;
            vehicleDetailList.HeightRequest = (forlistViewItemSource.Count * 320) + 20;
            vehicleDetailList.IsVisible = true;
            vehicleDetailList.IsRefreshing = false;
        }

        private void btnFilter_Tapped(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new Popups.FilterPopup(VehicleFilter, vehicletypeList));
        }

        private void btnBookNow_Clicked(object sender, EventArgs e)
        {
            //List<VehicleTypeResult> results = new List<VehicleTypeResult>();
            //VehicleTypeResult selectedVehicle = vehicleDetailList.SelectedItem as VehicleTypeResult;
            //int vehiId = selectedVehicle.VehicleTypeId;
            //foreach(VehicleTypeResult v in vehicleResults)
            //{
            //    if (v.VehicleTypeId == vehiId)
            //    {
            //        v.selected = true;
            //    }
            //    else
            //    {
            //        v.selected = false;
            //    }
            //    results.Add(v);
            //}
            //vehicleDetailList.ItemsSource = results;
            var obj = (Button)sender;
            VehicleViewByTypeForMobile selectedVehicle = obj.BindingContext as VehicleViewByTypeForMobile;

            reservationView.VehicleTypeID = selectedVehicle.VehicleTypeId;
            reservationView.VehicleType = selectedVehicle.VehicleType;

            Rates rates = JsonConvert.DeserializeObject<Rates>(JsonConvert.SerializeObject(selectedVehicle.RateDetail));
            rates.RateId = selectedVehicle.RateDetail.RateID;
            List<Rates> rateDewtails = new List<Rates>();
            //rates.StartDateStr = reservationView.StartDateStr;
            //rates.EndDateStr = reservationView.EndDateStr;
            rateDewtails.Add(rates);
            reservationView.RateDetailsList = rateDewtails;
            reservationView.TotalDays = rates.TotalDays;
            reservationView.VehicleId = selectedVehicle.vehicleId;
            reservationView.VehicleId2 = selectedVehicle.vehicleId;
            Navigation.PushAsync(new BookNow(reservationView, selectedVehicle, selectedVehicle.locationIdList));
        }

        private void vehicleDetailList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            VehicleViewByTypeForMobile selectedVehicle = vehicleDetailList.SelectedItem as VehicleViewByTypeForMobile;
            reservationView.VehicleTypeID = selectedVehicle.VehicleTypeId;
            reservationView.VehicleType = selectedVehicle.VehicleType;
            Rates rates = JsonConvert.DeserializeObject<Rates>(JsonConvert.SerializeObject(selectedVehicle.RateDetail));
            rates.RateId = selectedVehicle.RateDetail.RateID;
            List<Rates> rateDewtails = new List<Rates>();
            //rates.StartDateStr = reservationView.StartDateStr;
            //rates.EndDateStr = reservationView.EndDateStr;
            rateDewtails.Add(rates);
            reservationView.RateDetailsList = rateDewtails;
            reservationView.TotalDays = rates.TotalDays;
            reservationView.VehicleId = selectedVehicle.vehicleId;
            reservationView.VehicleId2 = selectedVehicle.vehicleId;
            Navigation.PushAsync(new BookNow(reservationView, selectedVehicle, selectedVehicle.locationIdList));
        }
    }
}