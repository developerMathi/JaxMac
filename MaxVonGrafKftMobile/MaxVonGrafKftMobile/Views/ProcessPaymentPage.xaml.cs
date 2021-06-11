using MaxVonGrafKftMobile.Popups;
using MaxVonGrafKftMobileController;
using MaxVonGrafKftMobileModel;
using MaxVonGrafKftMobileModel.AccessModels;
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
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProcessPaymentPage : ContentPage
    {
        private decimal balanceDue;
        List<CardDetails> cardDetails = new List<CardDetails>();
        GetAllCustomerMobileCreditCardRequest creditCardRequest;
        GetAllCustomerMobileCreditCardResponse creditCards;
        SubmitPaymentResponse PaymentResponse;
        private string token;
        BillingInformation billingInformation;
        SubmitPaymentAndUpDateReservationRequest submitPaymentAndUpDateReservationRequest;
        int customerId;
        private int agreementId;
        private int locationIdForPayment;
        private decimal amountWantToPay;
        private ReservationView reservationView;
        private AgreementReview agreementReview;
        SubmitPaymentAndExtendAgreementRequest submitPaymentAndExtendAgreementRequest;
        bool isExtendAgreement = false;
        GetPromotionMobileRequest promotionMobileRequest;
        GetPromotionMobileResponse promotionMobileResponse;
        GetCalculateSummaryMobileRequest summaryMobileRequest;
        GetCalculateSummaryMobileResponsecs summaryMobileResponsecs;
        int appliedPromoId;
        ExtendAgreementRequest extendAgreemetRequest;
        private ExtendAgreementResponse extendAgreementResponse;

        public ProcessPaymentPage(decimal balanceDue, int agreementId, int locationIdForPayment, AgreementReview agreementReview, ExtendAgreementRequest request)
        {
            InitializeComponent();
            this.balanceDue = balanceDue;
            amountLAbel.Text = "$ " + balanceDue.ToString("0.00");
            creditCardRequest = new GetAllCustomerMobileCreditCardRequest();
            creditCards = null;
            customerId = (int)App.Current.Properties["CustomerId"];
            token = App.Current.Properties["currentToken"].ToString();
            billingInformation = new BillingInformation();
            PaymentResponse = null;
            this.agreementId = agreementId;
            this.locationIdForPayment = locationIdForPayment;
            this.agreementReview = agreementReview;
            submitPaymentAndExtendAgreementRequest = new SubmitPaymentAndExtendAgreementRequest();
            if(agreementReview != null)
            {
                isExtendAgreement = true;
            }
            else if(agreementReview== null)
            {
                isExtendAgreement = false;
            }
            promotionMobileRequest = new GetPromotionMobileRequest();
            promotionMobileResponse = null;
            appliedPromoId = 0;
            extendAgreemetRequest = request;
            extendAgreementResponse = null;

        }

        public ProcessPaymentPage(decimal amountWantToPay, ReservationView reservationView)
        {
            InitializeComponent();
            this.amountWantToPay = amountWantToPay;
            this.reservationView = reservationView;
            amountLAbel.Text = "$ " + amountWantToPay.ToString("0.00");
            creditCardRequest = new GetAllCustomerMobileCreditCardRequest();
            creditCards = null;
            customerId = (int)App.Current.Properties["CustomerId"];
            token = App.Current.Properties["currentToken"].ToString();
            billingInformation = new BillingInformation();
            PaymentResponse = null;
            agreementId = 0;
            submitPaymentAndUpDateReservationRequest = new SubmitPaymentAndUpDateReservationRequest();
            paynowBtn.Text = "Pay and extend now";
            isExtendAgreement = false;
            promotionMobileRequest = new GetPromotionMobileRequest();
            promotionMobileResponse = null;
            summaryMobileRequest = new GetCalculateSummaryMobileRequest();
            summaryMobileResponsecs = null;
            appliedPromoId = 0;
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();

            bool busy = false;
            if (!busy)
            {
                try
                {
                    busy = true;
                    await PopupNavigation.Instance.PushAsync(new LoadingPopup(" "));

                    await Task.Run(() =>
                    {
                        try
                        {
                            if (customerId > 0)
                            {
                                creditCardRequest.CustomerID = customerId;
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


                }
            }
        }

        private GetAllCustomerMobileCreditCardResponse getAllcrediCardDetails(GetAllCustomerMobileCreditCardRequest creditCardRequest, string token)
        {
            CustomerController controller = new CustomerController();
            GetAllCustomerMobileCreditCardResponse cards = controller.GetAllMobileCustomerCreditCard(creditCardRequest, token);
            return cards;
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
                            lastCardId = (int)App.Current.Properties["LastCreditCardId"];
                        }

                        string cardnu = cc.CreditCardNoForDisplay.Substring(cc.CreditCardNoForDisplay.Length - 4);
                        string editedNumber = "**** **** **** " + cardnu;

                        if (cc.CreditCardId == lastCardId)
                        {
                            CardDetails details = new CardDetails() { cardId = (int)cc.CreditCardId, cardNumber = editedNumber, ExpiryDt = cc.Month.ToString("00") + "/" + cc.Year.ToString("00"), cardType = cc.CreditCardType, _isCardSelect = "iconCircleSelected.png" };
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


        private void btnBack_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private void lstCardetail_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            lstCardetail.SelectedItem = null;
            var data = e.Item as CardDetails;
            foreach (var item in cardDetails)
            {
                if (item.cardId == data.cardId)
                {
                    data.isCardSelect = "iconCircleSelected.png";
                }
                else
                {
                    item.isCardSelect = "iconCircleUnselect.png";
                }
            }
        }

        private async void paynowBtn_Clicked(object sender, EventArgs e)
        {
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
                if (isExtendAgreement)
                {
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
                        billingInformation.PaymentAmount = Convert.ToDouble(balanceDue);

                        billingInformation.NameOnCard = selectedCard.NameOnCard;
                        billingInformation.CreditCardNumber = selectedCard.CreditCardNo;
                        billingInformation.CreditCardNumberDisplay = selectedCard.CreditCardNoForDisplay;
                        billingInformation.SecurityCode = selectedCard.CreditCardCVSNo;
                        billingInformation.ExpiryMonth = selectedCard.Month;
                        billingInformation.ExpiryYear = selectedCard.Year;
                        billingInformation.CreditCardType = selectedCard.CreditCardType;
                        billingInformation.CreditCardId = (int)selectedCard.CreditCardId;

                        billingInformation.LocationId = locationIdForPayment;

                        billingInformation.FullName = Constants.customerDetails.FirstName;
                        billingInformation.PaymentInfo = new PaymentReferenceInfo();
                        //billingInformation.PaymentInfo.ResevationId = ReservationMobileResponse.ReserveId;
                        billingInformation.PaymentInfo.AgreementId = agreementId;
                        billingInformation.PaymentInfo.PaymentType = "Advance Payment";
                        billingInformation.PaymentInfo.PaymentDate = DateTime.Now;
                        billingInformation.PaymentInfo.PaymentDateStr = DateTime.Now.ToString("MM/dd/yyyy").Replace("-", "/");
                        billingInformation.PaymentInfo.PaymentMode = PaymentMode.Payment;
                        billingInformation.PaymentInfo.PaymentBy = Constants.customerDetails.FirstName;
                        ReservationController reservationController = new ReservationController();
                        submitPaymentAndExtendAgreementRequest.agreementReview = agreementReview;
                        submitPaymentAndExtendAgreementRequest.billingInformation = billingInformation;
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
                                        PaymentResponse = reservationController.SubmitPaymentAndExtendAgreement(submitPaymentAndExtendAgreementRequest, token);
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
                                await PopupNavigation.Instance.PopAsync();

                                if(PaymentResponse != null)
                                {
                                    if (PaymentResponse.Status)
                                    {
                                        if (PaymentResponse.Data != null)
                                        {
                                            if (PaymentResponse.Data.extendAgreementResponse != null)
                                            {
                                                if (PaymentResponse.Data.extendAgreementResponse.agreementReview.AgreementID > 0)
                                                {
                                                    await PopupNavigation.Instance.PushAsync(new SuccessPopUp("Your rental extension was successful. ", 1));
                                                }
                                                else if (PaymentResponse.RefundStatus)
                                                {
                                                    await PopupNavigation.Instance.PushAsync(new Error_popup("Sorry, Your extention failed. Your payment will refund shortly. "));
                                                }
                                                else
                                                {
                                                    await PopupNavigation.Instance.PushAsync(new Error_popup("Sorry, Your extention failed. Please contact us for your refund. "));
                                                }
                                            }
                                            else if (PaymentResponse.RefundStatus)
                                            {
                                                await PopupNavigation.Instance.PushAsync(new Error_popup("Sorry, Your extention failed. Your payment will refund shortly. "));
                                            }
                                            else
                                            {
                                                await PopupNavigation.Instance.PushAsync(new Error_popup("Sorry, Your extention failed. Please contact us for your refund. "));
                                            }
                                        }
                                        else if (PaymentResponse.RefundStatus)
                                        {
                                            await PopupNavigation.Instance.PushAsync(new Error_popup("Sorry, Your extention failed. Your payment will refund shortly. "));
                                        }
                                        else
                                        {
                                            await PopupNavigation.Instance.PushAsync(new Error_popup("Sorry, Your extention failed. Please contact us for your refund. "));
                                        }
                                    }
                                    else
                                    {
                                        await PopupNavigation.Instance.PushAsync(new Error_popup("SORRY, " + PaymentResponse.message.ToUpper() + ". Please Try Again."));
                                    }
                                }
                                else
                                {
                                    await PopupNavigation.Instance.PushAsync(new Error_popup("SORRY, YOUR PAYMENT PROCESS FAILED, Please try with another card details."));
                                }
                            }
                        }
                    }

                    else
                    {
                        await PopupNavigation.Instance.PushAsync(new Error_popup("Please select a credit card."));
                    }
                }
                else if (agreementId > 0)
                {
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
                        billingInformation.PaymentAmount = Convert.ToDouble(balanceDue);

                        billingInformation.NameOnCard = selectedCard.NameOnCard;
                        billingInformation.CreditCardNumber = selectedCard.CreditCardNo;
                        billingInformation.CreditCardNumberDisplay = selectedCard.CreditCardNoForDisplay;
                        billingInformation.SecurityCode = selectedCard.CreditCardCVSNo;
                        billingInformation.ExpiryMonth = selectedCard.Month;
                        billingInformation.ExpiryYear = selectedCard.Year;
                        billingInformation.CreditCardType = selectedCard.CreditCardType;
                        billingInformation.CreditCardId = (int)selectedCard.CreditCardId;

                        billingInformation.LocationId = locationIdForPayment;

                        billingInformation.FullName = Constants.customerDetails.FirstName;
                        billingInformation.PaymentInfo = new PaymentReferenceInfo();
                        //billingInformation.PaymentInfo.ResevationId = ReservationMobileResponse.ReserveId;
                        billingInformation.PaymentInfo.AgreementId = agreementId;
                        billingInformation.PaymentInfo.PaymentType = "Advance Payment";
                        billingInformation.PaymentInfo.PaymentDate = DateTime.Now;
                        billingInformation.PaymentInfo.PaymentDateStr = DateTime.Now.ToString("MM/dd/yyyy").Replace("-", "/");
                        billingInformation.PaymentInfo.PaymentMode = PaymentMode.Payment;
                        billingInformation.PaymentInfo.PaymentBy = Constants.customerDetails.FirstName;
                        ReservationController reservationController = new ReservationController();

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
                                        PaymentResponse = reservationController.SubmitPayment(billingInformation, token);
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
                                await PopupNavigation.Instance.PopAsync();

                                if (PaymentResponse != null)
                                {
                                    if (PaymentResponse.Status)
                                    {
                                        await PopupNavigation.Instance.PushAsync(new SuccessPopUp("Your payment Successfull. ", 1));
                                    }
                                    else
                                    {
                                        await PopupNavigation.Instance.PushAsync(new Error_popup("SORRY, " + PaymentResponse.message.ToUpper() + ". Please Try Again."));
                                    }
                                }
                                else
                                {
                                    await PopupNavigation.Instance.PushAsync(new Error_popup("SORRY, YOUR PAYMENT PROCESS FAILED, Please try with another card details."));
                                }
                            }
                        }
                    }

                    else
                    {
                        await PopupNavigation.Instance.PushAsync(new Error_popup("Please select a credit card."));
                    }
                }
                else
                {
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
                        billingInformation.PaymentAmount = Convert.ToDouble(amountWantToPay);

                        billingInformation.NameOnCard = selectedCard.NameOnCard;
                        billingInformation.CreditCardNumber = selectedCard.CreditCardNo;
                        billingInformation.CreditCardNumberDisplay = selectedCard.CreditCardNoForDisplay;
                        billingInformation.SecurityCode = selectedCard.CreditCardCVSNo;
                        billingInformation.ExpiryMonth = selectedCard.Month;
                        billingInformation.ExpiryYear = selectedCard.Year;
                        billingInformation.CreditCardType = selectedCard.CreditCardType;
                        billingInformation.CreditCardId = (int)selectedCard.CreditCardId;

                        billingInformation.LocationId = (int)reservationView.StartLocationId;

                        billingInformation.FullName = Constants.customerDetails.FirstName;
                        billingInformation.PaymentInfo = new PaymentReferenceInfo();
                        billingInformation.PaymentInfo.ResevationId = reservationView.ReserveId;
                        //billingInformation.PaymentInfo.AgreementId = agreementId;
                        billingInformation.PaymentInfo.PaymentType = "Advance Payment";
                        billingInformation.PaymentInfo.PaymentDate = DateTime.Now;
                        billingInformation.PaymentInfo.PaymentDateStr = DateTime.Now.ToString("MM/dd/yyyy").Replace("-","/");
                        billingInformation.PaymentInfo.PaymentMode = PaymentMode.Payment;
                        billingInformation.PaymentInfo.PaymentBy = Constants.customerDetails.FirstName;
                        ReservationController reservationController = new ReservationController();
                        submitPaymentAndUpDateReservationRequest.billingInformation = billingInformation;
                        submitPaymentAndUpDateReservationRequest.reservationMobileRequest = new UpdateReservationMobileRequest();

                        submitPaymentAndUpDateReservationRequest.reservationMobileRequest.reservationData = reservationView;
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
                                        PaymentResponse = reservationController.SubmitPaymentAndExtendReservation(submitPaymentAndUpDateReservationRequest, token);
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
                                await PopupNavigation.Instance.PopAsync();

                                if (PaymentResponse != null)
                                {
                                    if (PaymentResponse.Status)
                                    {
                                        if (PaymentResponse.Data != null)
                                        {
                                            if(PaymentResponse.Data.updateReserResponse!= null)
                                            {
                                                if (PaymentResponse.Data.updateReserResponse.ReservID > 0)
                                                {
                                                    await PopupNavigation.Instance.PushAsync(new SuccessPopUp("Your rental extension was successful. ", 1));
                                                }
                                                else if (PaymentResponse.RefundStatus)
                                                {
                                                    await PopupNavigation.Instance.PushAsync(new Error_popup("Sorry, Your extention failed. Your payment will refund shortly. "));
                                                }
                                                else
                                                {
                                                    await PopupNavigation.Instance.PushAsync(new Error_popup("Sorry, Your extention failed. Please contact us for your refund. "));
                                                }
                                            }
                                            else if (PaymentResponse.RefundStatus)
                                            {
                                                await PopupNavigation.Instance.PushAsync(new Error_popup("Sorry, Your extention failed. Your payment will refund shortly. "));
                                            }
                                            else
                                            {
                                                await PopupNavigation.Instance.PushAsync(new Error_popup("Sorry, Your extention failed. Please contact us for your refund. "));
                                            }
                                        }
                                        else if (PaymentResponse.RefundStatus)
                                        {
                                            await PopupNavigation.Instance.PushAsync(new Error_popup("Sorry, Your extention failed. Your payment will refund shortly. "));
                                        }
                                        else
                                        {
                                            await PopupNavigation.Instance.PushAsync(new Error_popup("Sorry, Your extention failed. Please contact us for your refund. "));
                                        }
                                    }
                                    else
                                    {
                                        await PopupNavigation.Instance.PushAsync(new Error_popup("Sorry, Please contact Jax ride share rentals for refund."));

                                    }
                                }
                                else
                                {

                                    await PopupNavigation.Instance.PushAsync(new Error_popup("SORRY, YOUR PAYMENT PROCESS FAILED, Please try with another card details."));

                                }
                            }
                        }
                    }

                    else
                    {
                        await PopupNavigation.Instance.PushAsync(new Error_popup("Please select a credit card."));
                    }
                }
            }

        }

        private void addPaymentMethodBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new AddPaymentMethodPage());
        }

        async void PromoBtn_Clicked(System.Object sender, System.EventArgs e)
        {
            
            if (!string.IsNullOrEmpty(promoCodeEntry.Text))
            {
                promotionMobileRequest.PromotionCode = promoCodeEntry.Text;
                promotionMobileRequest.LocationId = !isExtendAgreement ? (int)reservationView.StartLocationId : 0;
                promotionMobileRequest.VehicleTypeId = !isExtendAgreement ? (int)reservationView.VehicleTypeID : 0;

                bool busy = false;
                if (!busy)
                {
                    try
                    {
                        busy = true;
                        await PopupNavigation.Instance.PushAsync(new LoadingPopup("Checking for promotion..."));
                        ReservationController reservationController = new ReservationController();
                        await Task.Run(() =>
                        {
                            try
                            {
                                promotionMobileResponse = reservationController.checkPromotion(promotionMobileRequest, token);
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
                    if (promotionMobileResponse.promotion == null)
                    {
                        await PopupNavigation.Instance.PushAsync(new Error_popup("Invalid promo code"));
                    }
                    else
                    {
                        if(promotionMobileResponse.promotion.StartDate<=DateTime.Now && DateTime.Now <= promotionMobileResponse.promotion.EndDate)
                        {
                            if (!isExtendAgreement)
                            {
                                if(reservationView.PromotionList== null)
                                {
                                    await PopupNavigation.Instance.PushAsync(new SuccessPopUp("Promo code has been applied successfully!"));
                                    reservationView.PromotionList = new List<PromotionItem>();
                                    applyReservationPromotion(promotionMobileResponse);
                                }
                                else
                                {
                                    var alreadyAppliedPromos = reservationView.PromotionList.FindAll(m => m.PromotionID == promotionMobileResponse.promotion.PromotionID);
                                    if (alreadyAppliedPromos.Count > 0)
                                    {
                                        await PopupNavigation.Instance.PushAsync(new Error_popup("Invalid promo code"));
                                    }
                                    else
                                    {
                                        await PopupNavigation.Instance.PushAsync(new SuccessPopUp("Promo code has been applied successfully!"));
                                        applyReservationPromotion(promotionMobileResponse);
                                    }
                                }
                            }
                            else
                            {
                                if(agreementReview.PromotionList != null)
                                {
                                    var alreadyAppliedAgreePromos = agreementReview.PromotionList.FindAll(m => m.PromotionID == promotionMobileResponse.promotion.PromotionID);
                                    if (alreadyAppliedAgreePromos.Count > 0)
                                    {
                                        await PopupNavigation.Instance.PushAsync(new Error_popup("Invalid promo code"));
                                    }
                                    else
                                    {
                                        await PopupNavigation.Instance.PushAsync(new SuccessPopUp("Promo code has been applied successfully!"));
                                        applyAgreementPromotion(promotionMobileResponse);
                                    }
                                }
                                else
                                {
                                    await PopupNavigation.Instance.PushAsync(new SuccessPopUp("Promo code has been applied successfully!"));
                                    applyAgreementPromotion(promotionMobileResponse);
                                }
                            }
                        }
                        else
                        {
                            await PopupNavigation.Instance.PushAsync(new Error_popup("Invalid promo code"));
                        }
                        
                    }
                }
            }

        }

        private void applyAgreementPromotion(GetPromotionMobileResponse promotionMobileResponse)
        {

            extendAgreemetRequest.addPromotion = new PromotionItem() { PromotionID = promotionMobileResponse.PromResult.PromotionID, PromotionDiscount = (decimal)promotionMobileResponse.PromResult.DiscountValue,PromotionCode=promotionMobileResponse.promotion.PromotionCode };
            extendAgreemetRequest.deletePromotion = null;
            promoTitileLabel.Text = promotionMobileResponse.promotion.PromotionCode;
            promoDetailLabel.Text = "$" + promotionMobileResponse.promotion.DiscountValue.ToString("0.00");
            appliedPromoId = promotionMobileResponse.PromResult.PromotionID;
            hideAddPromotionGrid();
            AgreementController controller = new AgreementController();
            try
            {

                extendAgreementResponse = controller.extendAgreement(extendAgreemetRequest, token);
                
            }
           
            finally
            {
                if (extendAgreementResponse != null)
                {
                    if (extendAgreementResponse.message != null)
                    {
                        if (extendAgreementResponse.message.ErrorCode == "200")
                        {
                            balanceDue = (decimal)extendAgreementResponse.agreementReview.BalanceDue;
                            if (balanceDue <= 0)
                            {
                                balanceDue = 0;
                            }
                            agreementReview = extendAgreementResponse.agreementReview;
                            amountLAbel.Text = "$ " + balanceDue.ToString("0.00");
                            
                            foreach (PromotionItem p in extendAgreementResponse.agreementReview.PromotionList)
                            {
                                if (p.PromotionID == appliedPromoId)
                                {
                                    promoDetailLabel.Text = "$" + p.PromotionDiscount.ToString("0.00");
                                }
                            }

                        }
                    }
                }
            }
        }

        private void applyReservationPromotion(GetPromotionMobileResponse promotionMobileResponse)
        {
            promoTitileLabel.Text = promotionMobileResponse.promotion.PromotionCode;
            promoDetailLabel.Text = "$" + promotionMobileResponse.promotion.DiscountValue.ToString("0.00");
            appliedPromoId = promotionMobileResponse.PromResult.PromotionID;
            hideAddPromotionGrid();

            reservationView.PromotionCode = promoCodeEntry.Text;
            reservationView.PromotionList.Add(new PromotionItem() { PromotionID = promotionMobileResponse.PromResult.PromotionID, PromotionDiscount = (decimal)promotionMobileResponse.PromResult.DiscountValue });

            summaryMobileRequest.reservation = reservationView;
            try
            {
                try
                {
                    summaryMobileResponsecs = getSummaryDetails(summaryMobileRequest, token);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            finally
            {
                //amountWantToPay
                decimal amountWant = 0;
                if (summaryMobileResponsecs.rate.ReservationSummary.EstimatedTotal == null) { amountWant = (decimal)summaryMobileResponsecs.rate.EstimatedTotal - reservationView.AdvancedPayment; }
                else { amountWant = Convert.ToDecimal(summaryMobileResponsecs.rate.ReservationSummary.EstimatedTotal) - reservationView.AdvancedPayment; }

                if (amountWant > 0)
                {
                    amountWantToPay = amountWant;
                }
                else
                {
                    amountWantToPay = 0;
                }
               

                amountLAbel.Text = "$ " + amountWantToPay.ToString("0.00");


                int ordNo = 1;
                foreach (PromotionItem p1 in reservationView.PromotionList)
                {
                    p1.PromotionListId = ordNo;
                    ordNo += 1;
                    
                }


                foreach (PromotionItem p in summaryMobileResponsecs.rate.ReservationSummary.PromotionList)
                {
                   
                    if (p.PromotionID == appliedPromoId)
                    {
                        promoDetailLabel.Text = "$" + p.PromotionDiscount.ToString("0.00");
                    }
                }


                reservationView.BasePrice = Convert.ToDecimal(summaryMobileResponsecs.rate.ReservationSummary.FinalBaseRate);
                reservationView.PreSubTotal = Convert.ToDecimal(summaryMobileResponsecs.rate.ReservationSummary.PreSubTotal);
                reservationView.TotalDiscountOnBaseRate = Convert.ToDecimal(summaryMobileResponsecs.rate.ReservationSummary.PromoDiscountOnBaseRate);
                reservationView.TotalDiscount = Convert.ToDecimal(summaryMobileResponsecs.rate.ReservationSummary.PromoDiscountOnBaseRate);
                reservationView.TotalDiscountOnSubTotal = Convert.ToDecimal(summaryMobileResponsecs.rate.ReservationSummary.PromoDiscountOnSubtotal);
            }
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

        async void promoDeleteBtn_Clicked(System.Object sender, System.EventArgs e)
        {
            if(appliedPromoId>0 && !isExtendAgreement)
            {
                reservationView.PromotionList = reservationView.PromotionList.FindAll(et => et.PromotionID != appliedPromoId);
                summaryMobileRequest.reservation = reservationView;
                try
                {
                    try
                    {
                        summaryMobileResponsecs = getSummaryDetails(summaryMobileRequest, token);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                finally
                {
                    //amountWantToPay
                    decimal amountWant = 0;
                    if (summaryMobileResponsecs.rate.ReservationSummary.EstimatedTotal == null) { amountWant = (decimal)summaryMobileResponsecs.rate.EstimatedTotal - reservationView.AdvancedPayment; }
                    else { amountWant = Convert.ToDecimal(summaryMobileResponsecs.rate.ReservationSummary.EstimatedTotal) - reservationView.AdvancedPayment; }
                    amountWantToPay = amountWant;

                    amountLAbel.Text = "$ " + amountWantToPay.ToString("0.00");

                    reservationView.BasePrice = Convert.ToDecimal(summaryMobileResponsecs.rate.ReservationSummary.FinalBaseRate);
                    reservationView.PreSubTotal = Convert.ToDecimal(summaryMobileResponsecs.rate.ReservationSummary.PreSubTotal);
                    reservationView.TotalDiscountOnBaseRate = Convert.ToDecimal(summaryMobileResponsecs.rate.ReservationSummary.PromoDiscountOnBaseRate);
                    reservationView.TotalDiscount = Convert.ToDecimal(summaryMobileResponsecs.rate.ReservationSummary.PromoDiscountOnBaseRate);
                    reservationView.TotalDiscountOnSubTotal = Convert.ToDecimal(summaryMobileResponsecs.rate.ReservationSummary.PromoDiscountOnSubtotal);
                }
            }
            else if(appliedPromoId > 0 && isExtendAgreement)
            {
                extendAgreemetRequest.deletePromotion = new PromotionItem() { PromotionID = appliedPromoId};
                extendAgreemetRequest.addPromotion = null;
                AgreementController controller = new AgreementController();
                try
                {
                    extendAgreementResponse = controller.extendAgreement(extendAgreemetRequest, token);

                }

                finally
                {
                    if (extendAgreementResponse != null)
                    {
                        if (extendAgreementResponse.message != null)
                        {
                            if (extendAgreementResponse.message.ErrorCode == "200")
                            {

                                balanceDue = (decimal)extendAgreementResponse.agreementReview.BalanceDue;
                                agreementReview = extendAgreementResponse.agreementReview;
                                amountLAbel.Text = "$ " + balanceDue.ToString("0.00");

                            }
                        }
                    }
                }
            }

            viewAddPromotionGrid();
            
        }
        private void hideAddPromotionGrid()
        {
            iconDiscount.IsVisible = false;
            promoCodeEntry.IsVisible = false;
            promoCodeFrame.IsVisible = false;
            PromoBtn.IsVisible = false;
            addedPromotionFrame.IsVisible = true;
        }

        private void viewAddPromotionGrid()
        {
            iconDiscount.IsVisible = true;
            promoCodeEntry.IsVisible = true;
            PromoBtn.IsVisible = true;
            addedPromotionFrame.IsVisible = false;
            promoCodeEntry.Text = null;
            promoCodeFrame.IsVisible = true;
        }
    }
}