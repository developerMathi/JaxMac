using MaxVonGrafKftMobile.Popups;
using MaxVonGrafKftMobileController;
using MaxVonGrafKftMobileModel;
using MaxVonGrafKftMobileModel.AccessModels;
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
    public partial class BookNow : ContentPage
    {
        private string _token;
        //List<LocationModel> locationModels;
        GetAllLocationForClientIDForMobileResponse locationResponse;
        GetStoreHoursMobileRequest getStoreHoursMobileRequest;
        GetStoreHoursMobileResponse getStoreHoursMobileResponse;
        List<string> locationNames;
        ReservationView reservationView;
        int customerId;
        bool isBookable;
        private VehicleViewByTypeForMobile selectedVehicle;
        private List<int> locationIdList;
        ReservationConfigurationVehicleSearch search;
        GetReservationConfigurationResponse vehicleMobileResponse;
        GetReservationConfigurationMobileRequest vehicleMobileRequest;
        bool isVehicleAvailableOnThoseDays;
        DateTime estTime;



        public BookNow()
        {
            InitializeComponent();
        }


        public BookNow(ReservationView reservationView, VehicleViewByTypeForMobile selectedVehicle, List<int> locationIdList)
        {
            InitializeComponent();
            vehicleMobileRequest = new GetReservationConfigurationMobileRequest();
            vehicleMobileResponse = null;
            search = new ReservationConfigurationVehicleSearch();
            //locationModels = null;
            locationResponse = null;
            getStoreHoursMobileResponse = null;
            getStoreHoursMobileRequest = new GetStoreHoursMobileRequest();
            customerId = 0;
            isBookable = true;
            reservationView = new ReservationView();
            this.reservationView = reservationView;
            this.selectedVehicle = selectedVehicle;
            this.locationIdList = locationIdList;
            isVehicleAvailableOnThoseDays = false;
            estTime = DateTime.Now;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            DateTime timeUtc = DateTime.UtcNow;
            TimeZoneInfo estZone = TimeZoneInfo.FindSystemTimeZoneById("America/New_York");
            estTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, estZone);


            var assembly = typeof(BookNow);

            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                if (PopupNavigation.Instance.PopupStack[PopupNavigation.Instance.PopupStack.Count - 1].GetType() == typeof(ErrorWithClosePagePopup))
                {
                    await PopupNavigation.Instance.PopAllAsync();
                }
            }


            _token = App.Current.Properties["currentToken"].ToString();

            if ((int)App.Current.Properties["CustomerId"] > 0)
            {
                customerId = (int)App.Current.Properties["CustomerId"];
            }

            GetAllLocationForClientIDForMobileRequest locationRequest = new GetAllLocationForClientIDForMobileRequest();
            locationRequest.ClientID = Constants.ClientId;


            bool busy = false;
            if (!busy)
            {
                try
                {
                    busy = true;
                    await PopupNavigation.Instance.PushAsync(new LoadingPopup("Creating book now page..."));

                    await Task.Run(async () =>
                    {
                        try
                        {

                            locationResponse = getAllLocationsByClientId(locationRequest, _token);
                            if (customerId > 0)
                            {
                                isBookable = checkBookable();
                            }

                        }
                        catch (Exception ex)
                        {
                            await PopupNavigation.Instance.PushAsync(new ErrorWithClosePagePopup(ex.Message));
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
                    else if (PopupNavigation.Instance.PopupStack.Count > 1)
                    {
                        if (PopupNavigation.Instance.PopupStack[PopupNavigation.Instance.PopupStack.Count - 1].GetType() != typeof(ErrorWithClosePagePopup))
                        {
                            await PopupNavigation.Instance.PopAllAsync();
                        }
                    }
                    if (!isBookable)
                    {
                        await PopupNavigation.Instance.PushAsync(new Error_popup("You already have an active reservation. You may only reserve one vehicle at a time", true));
                    }

                }
                if (locationResponse != null)
                {
                    locationNames = new List<string>();
                    foreach (LocationModel l in locationResponse.locationList)
                    {
                        if ((bool)l.Active && selectedVehicle.locationIdList.Contains((int)l.LocationId))
                        {
                            locationNames.Add(l.LocationName);
                        }
                    }
                    pickUpLocation.ItemsSource = locationNames;
                    //dropOffLocation.ItemsSource = locationNames;
                    if (locationNames.Count > 0)
                    {
                        pickUpLocation.SelectedIndex = 0;
                    }
                }
                else
                {
                    await Navigation.PopAsync();
                }

                
                
                    pickUpDate.MaximumDate = estTime.AddDays(1);
                pickUpDate.MinimumDate = estTime;


                pickUpDate.Date = estTime;
                //pickUpTime.Time = new TimeSpan(DateTime.Now.TimeOfDay.Add(new TimeSpan(12, 0, 0)).Hours, DateTime.Now.TimeOfDay.Add(new TimeSpan(12, 0, 0)).Minutes, DateTime.Now.TimeOfDay.Add(new TimeSpan(12, 0, 0)).Seconds);
                //dropOffDate.Date = DateTime.Now.AddDays(1);
                //dropOffDate.MinimumDate = DateTime.Now;
                //dropOffTime.Time = new TimeSpan(9, 0, 0);
            }
        }

        private bool checkBookable()
        {
            RegisterController registerController = new RegisterController();
            bool isBookable = registerController.checkBookable(customerId, _token);
            return isBookable;
        }

        private GetAllLocationForClientIDForMobileResponse getAllLocationsByClientId(GetAllLocationForClientIDForMobileRequest locationRequest, string _token)
        {
            CommonController commoncontroller = new CommonController();
            GetAllLocationForClientIDForMobileResponse locations = null;
            try

            {
                locations = commoncontroller.getAllLocationsByClientId(locationRequest, _token);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return locations;
        }

        private void BookNowNxtBtn_Clicked(object sender, EventArgs e)
        {
            DateTime pickupDateTime = pickUpDate.Date + pickUpTime.Time;
            string currentTIME = estTime.ToString("hh:mm tt");

            if (pickupDateTime < estTime)
            {
                PopupNavigation.Instance.PushAsync(new Error_popup("The time that you have selected has past. Please select a time from "+currentTIME+" onwards."));
            }
            else if ( estTime.AddHours(36) < pickupDateTime)
            {
                PopupNavigation.Instance.PushAsync(new Error_popup("Bookings can be made only 36 hours prior to the pickup time."));
            }
            else if (pickUpLocation.SelectedIndex == -1)
            {
                PopupNavigation.Instance.PushAsync(new Error_popup("Please select a pick-up location"));
            }
            //else if (DateTime.Now.AddHours(36) <= pickupDateTime)
            //{
            //    PopupNavigation.Instance.PushAsync(new Error_popup("Your pick up time may be no more than 36 hours from now"));
            //}
            else if (NumberOfDays.SelectedIndex == -1)
            {
                PopupNavigation.Instance.PushAsync(new Error_popup("Please select the number of days for your initial booking. You may extend your rental at any time"));
            }

            else
            {
                DateTime DropOffDateTime = pickupDateTime.AddDays(int.Parse(NumberOfDays.SelectedItem.ToString()));
                int locId = getLocationIdByName(pickUpLocation.SelectedItem.ToString());
                getStoreHoursMobileRequest.locationId = locId;
                try
                {
                    ReservationController reservationController = new ReservationController();
                    getStoreHoursMobileResponse = reservationController.getStoreHoursMobile(getStoreHoursMobileRequest, _token);
                }
                catch (Exception ex)
                {
                    PopupNavigation.Instance.PushAsync(new Error_popup(ex.Message));
                }
                if (getStoreHoursMobileResponse != null)
                {
                    if (getStoreHoursMobileResponse.storeHourModels != null)
                    {
                        if (getStoreHoursMobileResponse.storeHourModels.Count > 0)
                        {
                            bool isPickUpStoreTime = true;
                            bool isDropOffStoreTime = true;
                            bool isPickUpNonHoliday = true;
                            bool isDropOffNonHoliday = true;
                            DayOfWeek date = (pickUpDate.Date + pickUpTime.Time).DayOfWeek;

                            foreach (SearchStoreHourModel sshm in getStoreHoursMobileResponse.storeHourModels)
                            {

                                if ((pickUpDate.Date + pickUpTime.Time).DayOfWeek.ToString() == Enum.GetName(typeof(DayOfWeek), sshm.Day - 1))
                                {
                                    if (((DateTime)sshm.StartTime).TimeOfDay <= pickUpTime.Time && pickUpTime.Time <= ((DateTime)sshm.EndTime).AddHours(-1).TimeOfDay)
                                    {
                                        if (sshm.IsHoliday)
                                        {
                                            isPickUpNonHoliday = false;
                                            PopupNavigation.Instance.PushAsync(new Error_popup("Sorry, the date you selected is a holiday. Please choose another pick up date"));
                                        }
                                        else
                                        {
                                            isPickUpNonHoliday = true;
                                            isPickUpStoreTime = true;
                                        }
                                    }
                                    else
                                    {
                                        isPickUpStoreTime = false;
                                        PopupNavigation.Instance.PushAsync(new Error_popup("Our business hours are " + ((DateTime)sshm.StartTime).ToString("hh:mm tt") + " to " + (((DateTime)sshm.EndTime).AddHours(-1)).ToString("hh:mm tt") + ". Please select a pick up time within those hours."));
                                    }
                                }



                                //if ((DropOffDateTime).DayOfWeek.ToString() == Enum.GetName(typeof(DayOfWeek), sshm.Day-1))
                                //{
                                //    if (((DateTime)sshm.StartTime).TimeOfDay <= DropOffDateTime.TimeOfDay && DropOffDateTime.TimeOfDay <= ((DateTime)sshm.EndTime).TimeOfDay)
                                //    {

                                //        //if (sshm.IsHoliday)
                                //        //{
                                //        //    isDropOffNonHoliday = false;
                                //        //    PopupNavigation.Instance.PushAsync(new Error_popup("Sorry,your drop off date is a holiday. please choose another Drop-off date"));
                                //        //}
                                //        //else
                                //        //{
                                //        //    isDropOffNonHoliday = true;
                                //        //    isDropOffStoreTime = true;
                                //        //}

                                //        isDropOffNonHoliday = true;
                                //        isDropOffStoreTime = true;
                                //    }
                                //    else
                                //    {
                                //        isDropOffStoreTime = false;
                                //        PopupNavigation.Instance.PushAsync(new Error_popup("Sorry, we are only open during " + sshm.StartTimeStr + " to " + sshm.EndTimeStr + " Please select a different drop off time"));
                                //    }
                                //}

                            }
                            if (isPickUpStoreTime && isDropOffStoreTime && isPickUpNonHoliday && isDropOffNonHoliday)
                            {
                                navigateNextPage();
                            }
                        }
                        else
                        {
                            navigateNextPage();
                        }
                    }
                    else
                    {
                        navigateNextPage();
                    }
                }
                else
                {
                    navigateNextPage();
                }
            }
        }

        private async void navigateNextPage()
        {
            DateTime sDate = pickUpDate.Date + pickUpTime.Time;
            DateTime eDate = sDate.AddDays(int.Parse(NumberOfDays.SelectedItem.ToString()));

            reservationView.StartLocationName = pickUpLocation.SelectedItem.ToString();
            reservationView.EndLocationName = pickUpLocation.SelectedItem.ToString();
            reservationView.StartLocationId = getLocationIdByName(pickUpLocation.SelectedItem.ToString());
            reservationView.EndLocationId = getLocationIdByName(pickUpLocation.SelectedItem.ToString());
            reservationView.StartDate = pickUpDate.Date + pickUpTime.Time; ;
            reservationView.EndDate = sDate.AddDays(int.Parse(NumberOfDays.SelectedItem.ToString()));
            reservationView.StartDateStr = ((DateTime)reservationView.StartDate).ToString("MM/dd/yyyy hh:mm tt").Replace("-", "/");
            reservationView.EndDateStr = ((DateTime)reservationView.EndDate).ToString("MM/dd/yyyy hh:mm tt").Replace("-", "/");


            search.LocationId = (int)reservationView.StartLocationId;
            search.CheckInLocationId = (int)reservationView.EndLocationId;
            search.ClientId = Constants.ClientId;
            search.IsOnline = true;
            search.StartDate = (DateTime)reservationView.StartDate;
            search.EndDate = (DateTime)reservationView.EndDate;

            search.VehicleCategoryId = 0;
            search.VehicleMakeID = 0;
            search.ModelId = 0;
            search.NumberOfSeats = 0;
            search.RentalPeriod = 0;
            //search.VehicleId = selectedVehicle.vehicleId;
            //search.VehicleTypeId = selectedVehicle.VehicleTypeId;
            vehicleMobileRequest.search = search;



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
                            vehicleMobileResponse = getVehicleTypesMobileNew(vehicleMobileRequest, _token);
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
                    isVehicleAvailableOnThoseDays = false;
                    if (vehicleMobileResponse != null)
                    {
                        foreach (ReservationVehicleSearchViewModel rvsv in vehicleMobileResponse.listVehicle)
                        {
                            if (selectedVehicle.vehicleId == rvsv.vehicleId)
                            {
                                isVehicleAvailableOnThoseDays = true;
                                reservationView.VehicleTypeID = rvsv.VehicleTypeId;
                                reservationView.VehicleType = rvsv.VehicleType;
                                Rates rates = JsonConvert.DeserializeObject<Rates>(JsonConvert.SerializeObject(rvsv.RateDetail));
                                rates.RateId = rvsv.RateDetail.RateID;
                                List<Rates> rateDewtails = new List<Rates>();
                                //rates.StartDateStr = reservationView.StartDateStr;
                                //rates.EndDateStr = reservationView.EndDateStr;
                                rateDewtails.Add(rates);
                                reservationView.RateDetailsList = rateDewtails;
                                reservationView.TotalDays = rates.TotalDays;
                                reservationView.VehicleId = selectedVehicle.vehicleId;
                                reservationView.VehicleId2 = selectedVehicle.vehicleId;
                                selectedVehicle.RateDetail = rvsv.RateDetail;
                                selectedVehicle.DailyRate = decimal.Round(((decimal)rvsv.RateDetail.RateTotal + calculateMantatryCharges(rvsv.mantatoryMiscChargeDetails, rates.TotalDays)), 2); ;
                                selectedVehicle.mantatoryMiscChargeDetails = rvsv.mantatoryMiscChargeDetails;
                            }
                        }
                    }


                }
            }






            //reservationView.TotalDays =(int)((eDate - sDate).TotalDays);
            reservationView.ClientId = Constants.ClientId;
            if (isVehicleAvailableOnThoseDays)
            {
                await Navigation.PushModalAsync(new VehicleRates(reservationView, selectedVehicle));
            }
            else
            {
                await PopupNavigation.Instance.PushAsync(new Error_popup("Sorry, Selected vehicle not available on those days, Please select a different vehicle. ",1));
            }
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

        private int getLocationIdByName(string v)
        {
            int locationId = 0;
            foreach (LocationModel l in locationResponse.locationList)
            {
                if (l.LocationName == v)
                {
                    locationId = (int)l.LocationId;
                }
            }
            return locationId;
        }

        //public List<LocationModel> GetAllLocation()
        //{
        //    CommonController commoncontroller = new CommonController();
        //    List<LocationModel> locations = commoncontroller.GetAllLocation(_token);
        //    return locations;
        //}

        //private async void ToolbarItem_Clicked(object sender, EventArgs e)
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

     


        private void PickUpDate_Unfocused_1(object sender, FocusEventArgs e)
        {

        }

        private void backBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private void pickUpLocation_Unfocused(object sender, FocusEventArgs e)
        {
            if (pickUpLocation.SelectedIndex != -1)
            {
                NumberOfDays.IsVisible = true;
                pickUpdateGrid.IsVisible = true;
                //dayTitleLabel.IsVisible = true;
                //dayCountGrid.IsVisible = true;
                bookNowNxtBtn.IsVisible = true;
            }
        }

        private void plusBtn_Clicked(object sender, EventArgs e)
        {
            int day = int.Parse(NumberOfDays.SelectedItem.ToString()); ;
            if (day > 0)
            {
                NumberOfDays.SelectedItem = (day + 1).ToString();
            }
            else
            {
                NumberOfDays.SelectedItem = "1";
            }
        }

        private void minusBtn_Clicked(object sender, EventArgs e)
        {
            int day = int.Parse(NumberOfDays.SelectedItem.ToString());
            if (day > 1)
            {
                NumberOfDays.SelectedItem = (day - 1).ToString();
            }
            else
            {
                NumberOfDays.SelectedItem = "1";
            }
        }

        private void PickUpDate_Unfocused(object sender, FocusEventArgs e)
        {

        }

        private void pickUpLocationArrow_Clicked(object sender, EventArgs e)
        {
            pickUpLocation.Focus();
        }

        private void NumberOfDaysArrow_Clicked(object sender, EventArgs e)
        {
            NumberOfDays.Focus();
        }


        private decimal calculateMantatryCharges(List<ReservationMiscDetails> mantatoryMiscChargeDetails, int noOfDays)
        {
            decimal sum = 0;
            if (mantatoryMiscChargeDetails != null)
            {
                foreach (ReservationMiscDetails rmd in mantatoryMiscChargeDetails)
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
            return sum;
        }

    }
}