using MaxVonGrafKftMobile.Popups;
using MaxVonGrafKftMobileController;
using MaxVonGrafKftMobileModel;
using MaxVonGrafKftMobileModel.AccessModels;
using MaxVonGrafKftMobileModel.Constants;
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
    public partial class SummaryOfChargesPage : ContentPage
    {
        private ReservationView reservationView;
        GetCalculateSummaryMobileRequest summaryMobileRequest;
        CreateReservationMobileRequest ReservationMobileRequest;
        CreateReservationMobileResponse ReservationMobileResponse;
        GetCalculateSummaryMobileResponsecs summaryMobileResponsecs;
        GetTermsandConditionByTypeRequest termsandConditionByTypeRequest;
        GetTermsandConditionByTypeResponse termsandConditionByTypeResponse;
        private string token;
        private VehicleViewByTypeForMobile selectedVehicle;
        EmailInvoiceRequest emailInvoiceRequest;
        InvoiceEmailResponse emailResponse;
        List<CardDetails> cardDetails = new List<CardDetails>();
        GetAllCustomerMobileCreditCardRequest creditCardRequest;
        GetAllCustomerMobileCreditCardResponse creditCards;
        SubmitPaymentResponse PaymentResponse;

        BillingInformation billingInformation;
        SubmitPaymentandCreateReservationRequest submitPaymentandCreateReservationRequest;
        int customerId;



        //public SummaryOfChargesPage()
        //{

        //    //if ((int)App.Current.Properties["CustomerId"] == 0)
        //    //{
        //    //    loginIcon.IconImageSource = ImageSource.FromResource("MaxVonGrafKftMobile.Assets.LogInTool.png", assembly);

        //    //}
        //    //else
        //    //{
        //    //    loginIcon.IconImageSource = ImageSource.FromResource("MaxVonGrafKftMobile.Assets.logOutTool.png", assembly);
        //    //}
        //}

        public SummaryOfChargesPage(ReservationView reservationView, VehicleViewByTypeForMobile selectedVehicle)
        {
            InitializeComponent();
            var assembly = typeof(SummaryOfChargesPage);
            this.reservationView = reservationView;
            summaryMobileRequest = new GetCalculateSummaryMobileRequest();
            summaryMobileResponsecs = null;
            ReservationMobileResponse = null;
            ReservationMobileRequest = new CreateReservationMobileRequest();
            summaryMobileRequest.reservation = reservationView;
            token = App.Current.Properties["currentToken"].ToString();
            termsandConditionByTypeRequest = new GetTermsandConditionByTypeRequest();
            termsandConditionByTypeRequest.clientId = Constants.ClientId;
            termsandConditionByTypeRequest.typeId = 3;
            termsandConditionByTypeResponse = null;
            this.selectedVehicle = selectedVehicle;
            startDateLabel.Text = ((DateTime)reservationView.StartDate).ToString("ddd MM/dd/yyyy");
            endDateLabel.Text = ((DateTime)reservationView.EndDate).ToString("ddd MM/dd/yyyy");
            startTimeLabel.Text = ((DateTime)reservationView.StartDate).ToString("hh:mm tt");
            endTimeLabel.Text = ((DateTime)reservationView.EndDate).ToString("hh:mm tt");
            if (selectedVehicle.VehicleImageUrl != null)
            {
                vehilcleTypeImage.Source = ImageSource.FromUri(new Uri(selectedVehicle.VehicleImageUrl));
            }
            vehicleSampleLabel.Text = selectedVehicle.vehicleName;
            vehilcleTypeLabel.Text = selectedVehicle.VehicleType;
            priceLabel.Text = "Days: " + selectedVehicle.RateDetail.TotalDays.ToString();
            emailInvoiceRequest = new EmailInvoiceRequest();
            emailResponse = null;
            creditCardRequest = new GetAllCustomerMobileCreditCardRequest();
            creditCards = null;
            customerId = (int)App.Current.Properties["CustomerId"];

            billingInformation = new BillingInformation();
            submitPaymentandCreateReservationRequest = new SubmitPaymentandCreateReservationRequest();
            PaymentResponse = null;

            //imgSelect.Source = "iconCircleSelected.png";
        }

        public void BindDetail(List<CreditCards> listCard)
        {
            cardDetails.Clear();
            if (listCard != null)
            {
                if (listCard.Count > 0)
                {
                    foreach (CreditCards cc in listCard)
                    {
                        int lastCardId = 0;
                        if (App.Current.Properties.ContainsKey("LastCreditCardId"))
                        {
                            lastCardId=(int) App.Current.Properties["LastCreditCardId"] ;
                        }
                        string cardnu = cc.CreditCardNoForDisplay.Substring(cc.CreditCardNoForDisplay.Length-4);
                        string editedNumber = "**** **** **** " + cardnu;

                        if (cc.CreditCardId == lastCardId)
                        {
                            CardDetails details = new CardDetails() { cardId = (int)cc.CreditCardId, cardNumber = editedNumber, ExpiryDt = cc.Month.ToString("00") + "/" + cc.Year.ToString("00"), cardType = cc.CreditCardType, _isCardSelect= "iconCircleSelected.png" };
                            cardDetails.Add(details);
                        }
                        else
                        {
                            CardDetails details = new CardDetails() { cardId = (int)cc.CreditCardId, cardNumber = editedNumber, ExpiryDt = cc.Month.ToString("00") + "/" + cc.Year.ToString("00"), cardType = cc.CreditCardType };
                            cardDetails.Add(details);
                        }
                    }
                }

            }


            if (cardDetails.Count > 0)
            {
                lstCardetail.ItemsSource = cardDetails.ToList();
                lstCardetail.HeightRequest = cardDetails.Count() * 70;
            }
            else
            {
                lstCardetail.IsVisible = false;
            }

        }

        //private async void LoginIcon_Clicked(object sender, EventArgs e)
        //{
        //    var assembly = typeof(SummaryOfChargesPage);
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
        //            await Navigation.PushAsync(new BookNow());
        //        }
        //    }
        //}

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            customerId = (int)App.Current.Properties["CustomerId"];
            if (customerId > 0)
            {
                creditCardRequest.CustomerID = customerId;
            }
            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                if (PopupNavigation.Instance.PopupStack[PopupNavigation.Instance.PopupStack.Count - 1].GetType() == typeof(ErrorWithClosePagePopup))
                {
                    await PopupNavigation.Instance.PopAllAsync();
                }
                else if (PopupNavigation.Instance.PopupStack[PopupNavigation.Instance.PopupStack.Count - 1].GetType() == typeof(ReservationSavedPopup))
                {
                    await PopupNavigation.Instance.PopAllAsync();
                    await Navigation.PushAsync(new HomePageDetail());
                }
                else
                {
                    await PopupNavigation.Instance.PopAsync();
                }

            }

            //if (Constants.IsRegisteredandNotLogin)
            //{
            //    await Navigation.PushAsync(new LoginPage());
            //}
            else
            {

                if (reservationView.CustomerDriverList != null)
                {
                    if (reservationView.CustomerDriverList.Count > 0)
                    {
                        List<Driver> listForListVew = new List<Driver>();
                        foreach (Driver d in reservationView.CustomerDriverList)
                        {
                            if (d.IsDelete == false)
                            {

                                listForListVew.Add(d);
                            }
                        }
                        reservationView.CustomerDriverList = listForListVew;
                        // NoAdditionalDriverLabel.IsVisible = false;
                        // additionalDriverList.IsVisible = true;
                        // additionalDriverList.ItemsSource = listForListVew;
                        // additionalDriverList.HeightRequest = (listForListVew.Count) * 130;
                    }
                    else
                    {
                        ///NoAdditionalDriverLabel.IsVisible = true;
                        // additionalDriverList.IsVisible = false;
                    }
                }
                else
                {
                    // NoAdditionalDriverLabel.IsVisible = true;
                    // additionalDriverList.IsVisible = false;
                }



                bool busy = false;
                if (!busy)
                {
                    try
                    {
                        busy = true;
                        await PopupNavigation.Instance.PushAsync(new LoadingPopup("Getting summary details..."));

                        await Task.Run(() =>
                        {
                            try
                            {
                                summaryMobileResponsecs = getSummaryDetails(summaryMobileRequest, token);
                                termsandConditionByTypeResponse = getTermsAndConditions(termsandConditionByTypeRequest, token);
                                if (customerId > 0)
                                {
                                    creditCards = getAllcrediCardDetails(creditCardRequest, token);
                                }
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
                        if (customerId > 0)
                        {
                            addPaymentMethodBtn.IsVisible = true;
                            if (creditCards != null)
                            {
                                if (creditCards.listCard != null)
                                {
                                    if (creditCards.listCard.Count > 0)
                                    {
                                        BindDetail(creditCards.listCard);
                                        lstCardetail.IsVisible = true;
                                    }
                                    else
                                    {
                                        lstCardetail.IsVisible = false;
                                    }
                                }
                            }
                        }
                        if (customerId == 0)
                        {
                            lstCardetail.IsVisible = false;
                            addPaymentMethodBtn.IsVisible = false;
                        }


                        if (summaryMobileResponsecs != null)
                        {
                            if (summaryMobileResponsecs.message.ErrorCode == "200")
                            {
                                noOfDaysEntry.Text = reservationView.TotalDays.ToString();

                                totalRentalFeeEntry.Text = "$" + summaryMobileResponsecs.rate.ReservationSummary.BaseRate.ToString();

                                if(summaryMobileResponsecs.rate.ReservationSummary.PromoDiscount != null)
                                {
                                    DiscountEntry.Text = "[$" + summaryMobileResponsecs.rate.ReservationSummary.PromoDiscount.ToString() + "]";
                                }
                                totalMisChargeEntry.Text = "$" + (Convert.ToDecimal(summaryMobileResponsecs.rate.ReservationSummary.TotacMiscNonTaxable) + Convert.ToDecimal(summaryMobileResponsecs.rate.ReservationSummary.TotacMiscTaxable)).ToString();

                                if (summaryMobileResponsecs.rate.ReservationSummary.TotalTax == null)
                                {
                                    totalTaxEntry.Text = "$0.00";
                                }
                                else if (summaryMobileResponsecs.rate.ReservationSummary.TotalTax != null)
                                {
                                    totalTaxEntry.Text = "$" + summaryMobileResponsecs.rate.ReservationSummary.TotalTax;
                                }



                                if (summaryMobileResponsecs.rate.ReservationSummary.EstimatedTotal == null)
                                {
                                    totalAmountEntry.Text = "$" + summaryMobileResponsecs.rate.EstimatedTotal.ToString();
                                }
                                else if (summaryMobileResponsecs.rate.ReservationSummary.EstimatedTotal != null)
                                {
                                    totalAmountEntry.Text = "$" + summaryMobileResponsecs.rate.ReservationSummary.EstimatedTotal;
                                }

                                if (summaryMobileResponsecs.rate.ReservationSummary.PromotionList != null && reservationView.PromotionList!= null)
                                {
                                    reservationView.PromotionList = summaryMobileResponsecs.rate.ReservationSummary.PromotionList;
                                }

                            }
                            else
                            {
                                await PopupNavigation.Instance.PushAsync(new ErrorWithClosePagePopup(summaryMobileResponsecs.message.ErrorMessage));
                            }
                        }

                        if (termsandConditionByTypeResponse != null)
                        {
                            if (termsandConditionByTypeResponse.message.ErrorCode == "200")
                            {
                                if (termsandConditionByTypeResponse.termlist != null)
                                {
                                    if (termsandConditionByTypeResponse.termlist.Count > 0)
                                    {
                                        //terms.Text = termsandConditionByTypeResponse.termlist[0].Term;
                                        //tacDescription.Text = termsandConditionByTypeResponse.termlist[0].Description;

                                        // termList.ItemsSource = termsandConditionByTypeResponse.termlist;

                                        //var myDataTemplate = new DataTemplate(() =>
                                        //{
                                        //    var cell = new ViewCell();
                                        //    var termGrid = new Grid();
                                        //    int r = termsandConditionByTypeResponse.termlist.Count;

                                        //    for (int i = 0; i < r; i++)
                                        //    {
                                        //        termGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                                        //    }



                                        //    foreach (Terms terms in termsandConditionByTypeResponse.termlist)
                                        //    {
                                        //        var tCGrid = new Grid() { VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.Fill };

                                        //        tCGrid.Children.Add(new Label()  // adding the item as label
                                        //        {
                                        //            Text = terms.Term,
                                        //            TextColor = Color.Black,
                                        //            HorizontalOptions = LayoutOptions.FillAndExpand,
                                        //            HorizontalTextAlignment = TextAlignment.Center,
                                        //            VerticalTextAlignment = TextAlignment.Center,
                                        //            FontSize = 16
                                        //        }, 0, 0);

                                        //        tCGrid.Children.Add(new Label()  // adding the item as label
                                        //        {
                                        //            Text = terms.Description,
                                        //            TextColor = Color.Gray,
                                        //            HorizontalOptions = LayoutOptions.FillAndExpand,
                                        //            HorizontalTextAlignment = TextAlignment.Center,
                                        //            VerticalTextAlignment = TextAlignment.Center,
                                        //            FontSize = 16
                                        //        }, 0, 1);
                                        //        termGrid.Children.Add(tCGrid);
                                        //    }
                                        //    cell.View = termGrid;
                                        //    return cell;
                                        //});
                                        //data = myDataTemplate;
                                    }
                                }
                            }
                            else
                            {
                                await PopupNavigation.Instance.PushAsync(new ErrorWithClosePagePopup(termsandConditionByTypeResponse.message.ErrorMessage));
                            }
                        }
                    }
                }
            }



        }

        private GetAllCustomerMobileCreditCardResponse getAllcrediCardDetails(GetAllCustomerMobileCreditCardRequest creditCardRequest, string token)
        {
            CustomerController controller = new CustomerController();
            GetAllCustomerMobileCreditCardResponse cards = controller.GetAllMobileCustomerCreditCard(creditCardRequest, token);
            return cards;
        }

        private GetTermsandConditionByTypeResponse getTermsAndConditions(GetTermsandConditionByTypeRequest termsandConditionByTypeRequest, string token)
        {
            GetTermsandConditionByTypeResponse Response = null;
            ReservationController controller = new ReservationController();
            try
            {
                Response = controller.getTermsAndConditions(termsandConditionByTypeRequest, token);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Response;
        }

        private GetCalculateSummaryMobileResponsecs getSummaryDetails(GetCalculateSummaryMobileRequest summaryMobileRequest, string token)
        {
            GetCalculateSummaryMobileResponsecs summaryResponse = null;
            ReservationController controller = new ReservationController();
            try
            {
                summaryResponse = controller.getSummaryDetails(summaryMobileRequest, token);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return summaryResponse;
        }



        private async void BookNowBtn_Clicked(object sender, EventArgs e)
        {
            if (TCcheckBox.IsChecked)
            {
                if ((int)App.Current.Properties["CustomerId"] == 0)
                {
                    await PopupNavigation.Instance.PushAsync(new AskForLogin("Please log-in or sign up to continue your booking"));
                }
                else if (cardDetails.Count == 0)
                {
                    await PopupNavigation.Instance.PushAsync(new Error_popup("You didn't add any credit card details. Please click ADD PAAYMENT METHOD button to add credit card details."));
                }
                else
                {
                    reservationView.CustomerId = (int)App.Current.Properties["CustomerId"];
                    //if(summaryMobileResponsecs.rate.ReservationSummary.PromoDiscount != null)
                    //{
                    //    reservationView.TotalDiscount = Convert.ToDecimal(summaryMobileResponsecs.rate.ReservationSummary.PromoDiscount);
                    //}
                    
                    reservationView.BasePrice = Convert.ToDecimal(summaryMobileResponsecs.rate.ReservationSummary.FinalBaseRate);
                    reservationView.Status = (short)MaxVonGrafKftMobileModel.Constants.ReservationStatuses.Quote;
                    reservationView.StatusId = (short)MaxVonGrafKftMobileModel.Constants.ReservationStatuses.Quote;
                    ReservationMobileRequest.reversationData = reservationView;



                    bool bookingEanable = false;
                    bool busy = false;
                    if (!busy)
                    {
                        try
                        {
                            busy = true;
                            await PopupNavigation.Instance.PushAsync(new LoadingPopup("checking bookable..."));
                            await Task.Run(() =>
                            {
                                try
                                {
                                    //ReservationMobileResponse = createReservationMobile(ReservationMobileRequest, token);
                                    bookingEanable = checkIsbookable(ReservationMobileRequest, token);
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
                            if (bookingEanable)
                            {
                                SubmitPaymentDetails(ReservationMobileRequest);
                            }
                            else
                            {
                                await PopupNavigation.Instance.PushAsync(new Error_popup("You already have an active reservation or rental. You may only reserve one vehicle at a time", true));
                            }
                        }
                    }
                }
            }
            else
            {
                await PopupNavigation.Instance.PushAsync(new Error_popup("Please accept the terms and conditions"));
            }
        }

        private async void SubmitPaymentDetails(CreateReservationMobileRequest reservationMobileRequest)
        {
            submitPaymentandCreateReservationRequest.reservationMobileRequest = reservationMobileRequest;

            int selectedCardId = 0;
            CreditCards selectedCard = null;

            foreach (var item in cardDetails)
            {
                if (item.isCardSelect == "iconCircleSelected.png")
                {
                    selectedCardId = item.cardId;
                }

            }


            if (selectedCardId > 0)
            {
                foreach (CreditCards cc in creditCards.listCard)
                {
                    if (cc.CreditCardId == selectedCardId)
                    {
                        selectedCard = cc;
                    }
                }
            }

            if (selectedCard != null)
            {
                billingInformation.FirstName = Constants.customerDetails.FirstName;
                billingInformation.LastName = Constants.customerDetails.LastName;
                billingInformation.Email = Constants.customerDetails.Email;
                billingInformation.Address = Constants.customerDetails.Address1;
                billingInformation.City = Constants.customerDetails.City;
                billingInformation.State = Constants.customerDetails.StateName;
                billingInformation.ZipCode = Constants.customerDetails.ZipCode;
                billingInformation.Country = Constants.customerDetails.CountryName;
                billingInformation.phone = Constants.customerDetails.hPhone;
                billingInformation.CustomerId = customerId;
                billingInformation.PaymentType = "Advance Payment";
                billingInformation.PaymentAmount = Convert.ToDouble(summaryMobileResponsecs.rate.ReservationSummary.EstimatedTotal);

                billingInformation.NameOnCard = selectedCard.NameOnCard;
                billingInformation.CreditCardNumber = selectedCard.CreditCardNo;
                billingInformation.CreditCardNumberDisplay = selectedCard.CreditCardNoForDisplay;
                billingInformation.SecurityCode = selectedCard.CreditCardCVSNo;
                billingInformation.ExpiryMonth = selectedCard.Month;
                billingInformation.ExpiryYear = selectedCard.Year;
                billingInformation.CreditCardType = selectedCard.CreditCardType;
                billingInformation.CreditCardId = (int)selectedCard.CreditCardId;

                billingInformation.LocationId = (int)ReservationMobileRequest.reversationData.StartLocationId;

                billingInformation.FullName = Constants.customerDetails.FirstName;
                billingInformation.PaymentInfo = new PaymentReferenceInfo();
                //billingInformation.PaymentInfo.ResevationId = ReservationMobileResponse.ReserveId;
                //billingInformation.PaymentInfo.AgreementId = ReservationMobileResponse.ReserveId;
                billingInformation.PaymentInfo.PaymentType = "Advance Payment";
                billingInformation.PaymentInfo.PaymentDate = DateTime.Now;
                billingInformation.PaymentInfo.PaymentDateStr = DateTime.Now.ToString();
                billingInformation.PaymentInfo.PaymentMode = PaymentMode.Payment;
                billingInformation.PaymentInfo.PaymentBy = Constants.customerDetails.FirstName;
                ReservationController reservationController = new ReservationController();
                submitPaymentandCreateReservationRequest.billingInformation = billingInformation;

                if (App.Current.Properties.ContainsKey("LastCreditCardId"))
                {
                    App.Current.Properties["LastCreditCardId"] = (int)selectedCard.CreditCardId;
                }
                else
                {
                    App.Current.Properties.Add("LastCreditCardId", (int)selectedCard.CreditCardId);
                }
                bool busy = false;
                if (!busy)
                {
                    try
                    {
                        busy = true;
                        await PopupNavigation.Instance.PushAsync(new LoadingPopup("."));
                        await Task.Run(async () =>
                        {
                            try
                            {
                                PaymentResponse = reservationController.SubmitPaymentAndCreateReservation(submitPaymentandCreateReservationRequest, token);
                            }
                            catch (Exception ex)
                            {
                                await PopupNavigation.Instance.PushAsync(new ErrorWithClosePagePopup(ex.Message));
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        await PopupNavigation.Instance.PushAsync(new ErrorWithClosePagePopup(ex.Message));
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

                        if (PaymentResponse != null)
                        {
                            if (PaymentResponse.Status)
                            {
                                if(PaymentResponse.Data!= null)
                                {
                                    if(PaymentResponse.Data.reservationRespose!= null)
                                    {
                                        if (PaymentResponse.Data.reservationRespose.ReserveId > 0)
                                        {
                                            await PopupNavigation.Instance.PushAsync(new SuccessPopUp("Congrats! Your payment was successful! You will receive an email once your insurance card has been issued. Please DO NOT attempt to pick up the vehicle until then.",1));
                                        }
                                        else if (PaymentResponse.RefundStatus)
                                        {
                                            await PopupNavigation.Instance.PushAsync(new Error_popup("Sorry, Your booking failed. Your payment will refund shortly. "));
                                        }
                                        else
                                        {
                                            await PopupNavigation.Instance.PushAsync(new Error_popup("Sorry, Your booking failed. Please contact us for your refund. "));
                                        }
                                    }
                                    else if (PaymentResponse.RefundStatus)
                                    {
                                        await PopupNavigation.Instance.PushAsync(new Error_popup("Sorry, Your booking failed. Your payment will refund shortly. "));
                                    }
                                    else
                                    {
                                        await PopupNavigation.Instance.PushAsync(new Error_popup("Sorry, Your booking failed. Please contact us for your refund. "));
                                    }

                                }
                                else if (PaymentResponse.RefundStatus)
                                {
                                    await PopupNavigation.Instance.PushAsync(new Error_popup("Sorry, Your booking failed. Your payment will refund shortly. "));
                                }
                                else
                                {
                                    await PopupNavigation.Instance.PushAsync(new Error_popup("Sorry, Your booking failed. Please contact us for your refund. "));
                                }
                            }
                            else
                            {
                                await PopupNavigation.Instance.PushAsync(new Error_popup("SORRY, YOUR PAYMENT PROCESS FAILED, Please try again with another card details."));

                            }
                        }
                        else
                        {
                            await PopupNavigation.Instance.PushAsync(new Error_popup("SORRY, YOUR PAYMENT PROCESS FAILED, Please try again with another card details."));

                        }
                    }
                }
            }
            else
            {
                await PopupNavigation.Instance.PushAsync(new Error_popup("Please select a valid card details"));
            }
        }

        private void sendReservationEmail()
        {
            ReservationController reservationController = new ReservationController();
            emailInvoiceRequest.ClientID = Constants.ClientId;
            emailInvoiceRequest.TemplateTypeId = (int)EmailPurpose.Reservation;
            emailInvoiceRequest.PurposeId = ReservationMobileResponse.ReserveId;
            emailInvoiceRequest.ReservationId = ReservationMobileResponse.ReserveId;
            emailResponse = reservationController.sendReservationEmail(emailInvoiceRequest, token);

        }

        private CreateReservationMobileResponse createReservationMobile(CreateReservationMobileRequest reservationMobileRequest, string token)
        {
            CreateReservationMobileResponse response = null;
            ReservationController reservationController = new ReservationController();
            RegisterController registerController = new RegisterController();
            try
            {
                bool isBookable = registerController.checkBookable((int)reservationMobileRequest.reversationData.CustomerId, token);
                if (isBookable)
                {
                    response = reservationController.createReservationMobile(reservationMobileRequest, token);
                }
                else
                {
                    ApiMessage message = new ApiMessage();
                    message.ErrorMessage = "Can't bookable";
                    message.ErrorCode = "120";
                    response = new CreateReservationMobileResponse();
                    response.message = message;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }



        private bool checkIsbookable(CreateReservationMobileRequest reservationMobileRequest, string token)
        {
            bool isBookable = false;
            RegisterController registerController = new RegisterController();
            try
            {
                isBookable = registerController.checkBookable((int)reservationMobileRequest.reversationData.CustomerId, token);


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isBookable;
        }


        protected override bool OnBackButtonPressed()
        {
            if (PopupNavigation.Instance.PopupStack.Count > 0) { return true; }



            // Always return true because this method is not asynchronous.
            // We must handle the action ourselves: see above.
            return false;
        }



        /// <summary>
        /// additoinal driver btn click method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addDriverBtn_Clicked(object sender, EventArgs e)
        {
            if ((int)App.Current.Properties["CustomerId"] == 0)
            {
                PopupNavigation.Instance.PushAsync(new AskForLogin("Please log-in to add aditional driver"));
            }
            else
            {
                Navigation.PushAsync(new AddAditionalDriver(reservationView));
            }
        }

        private void additionalDriverList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            // Driver selecteddriver = additionalDriverList.SelectedItem as Driver;
            // Navigation.PushAsync(new DriverDetailPage(selecteddriver, reservationView));
        }

        private void additionalDriverList_Refreshing(object sender, EventArgs e)
        {

        }

        private void btnTerms_Tapped(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new Popups.TermAndConditionPopup(termsandConditionByTypeResponse.termlist));
        }





        //private void btnSelect_Tapped(object sender, EventArgs e)
        //{
        //    if (imgSelect.Source.ToString().Replace("File: ", "") == "iconCircleSelected.png")
        //    {
        //        imgSelect.Source = "iconCircleUnselect.png";
        //    }
        //    else
        //    {
        //        imgSelect.Source = "iconCircleSelected.png";
        //        foreach (var item in cardDetails)
        //        {
        //            item.isCardSelect = "iconCircleUnselect.png";
        //        }
        //    }
        //}

        private void lstCardetail_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            lstCardetail.SelectedItem = null;
            var data = e.Item as CardDetails;
            foreach (var item in cardDetails)
            {
                if (item.cardId == data.cardId)
                {
                    data.isCardSelect = "iconCircleSelected.png";
                    //imgSelect.Source = "iconCircleUnselect.png";
                }
                else
                {
                    item.isCardSelect = "iconCircleUnselect.png";
                }
            }
        }

        private void addPaymentMethodBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddPaymentMethodPage());
        }
    }
}