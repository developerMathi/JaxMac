using MaxVonGrafKftMobile.Popups;
using MaxVonGrafKftMobileController;
using MaxVonGrafKftMobileModel.AccessModels;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace MaxVonGrafKftMobile.Views
{
    public class CardDetails : INotifyPropertyChanged
    {
        public int cardId { get; set; }
        public string cardNumber { get; set; }
        public string cardType { get; set; }
        public string ExpiryDt { get; set; }

        public string _isCardSelect = "iconCircleUnselect.png";
        public string isCardSelect
        {
            get
            {
                return _isCardSelect;
            }
            set
            {
                _isCardSelect = value;
                PropertyChanged(this, new PropertyChangedEventArgs("isCardSelect"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }



    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaymentMethodPage : ContentPage
    {
        List<CardDetails> cardDetails = new List<CardDetails>();
        GetAllCustomerMobileCreditCardRequest creditCardRequest;
        GetAllCustomerMobileCreditCardResponse creditCards;
        int customerId;
        string token;

        public PaymentMethodPage()
        {
            InitializeComponent();
            creditCardRequest = new GetAllCustomerMobileCreditCardRequest();
            creditCards = null;
            customerId = (int)App.Current.Properties["CustomerId"];
            token = App.Current.Properties["currentToken"].ToString();
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
            creditCardRequest.CustomerID = customerId;
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
                        string cardnu = cc.CreditCardNoForDisplay.Substring(cc.CreditCardNoForDisplay.Length-4);
                        string editedNumber = "**** **** **** " + cardnu;
                        CardDetails details = new CardDetails() { cardId = (int)cc.CreditCardId, cardNumber = editedNumber, ExpiryDt = cc.Month.ToString("00") + "/" + cc.Year.ToString("00"),cardType=cc.CreditCardType };
                        cardDetails.Add(details);
                    }
                }
            }



            lstCardetail.ItemsSource = cardDetails.ToList();
            lstCardetail.HeightRequest = cardDetails.Count() * 150;

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                await PopupNavigation.Instance.PopAllAsync();
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
                            //agreementResponse = getAgreementMobile(getAgreementByCustomerIdMobileRequest, token);
                            //customerAgreementModels = agreementResponse.listAgreements;
                            creditCards = getAllcrediCardDetails(creditCardRequest, token);
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
                    await PopupNavigation.Instance.PopAllAsync();
                }
            }


            BindDetail(creditCards.listCard);
        }

        private GetAllCustomerMobileCreditCardResponse getAllcrediCardDetails(GetAllCustomerMobileCreditCardRequest creditCardRequest, string token)
        {
            CustomerController controller = new CustomerController();
            GetAllCustomerMobileCreditCardResponse cards = controller.GetAllMobileCustomerCreditCard(creditCardRequest, token);
            return cards;
        }

        private void BooknowBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddPaymentMethodPage());
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
                    imgSelect.Source = "iconCircleUnselect.png";
                }
                else
                {
                    item.isCardSelect = "iconCircleUnselect.png";
                }
            }
        }

        private void btnMenu_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
            //Common.mMasterPage.Master = new HomePageMaster();
            //Common.mMasterPage.IsPresented = true;
        }

        private void btnSelect_Tapped(object sender, EventArgs e)
        {
            if (imgSelect.Source.ToString().Replace("File: ", "") == "iconCircleSelected.png")
            {
                imgSelect.Source = "iconCircleUnselect.png";
            }
            else
            {
                imgSelect.Source = "iconCircleSelected.png";

                foreach (var item in cardDetails)
                {
                    item.isCardSelect = "iconCircleUnselect.png";
                }
            }
        }

        private void cardEdit_Tapped(object sender, EventArgs e)
        {
            var obj = (ImageButton)sender;
            CardDetails selectedCard = obj.BindingContext as CardDetails;

        }

        private async void cardDelete_Tapped(object sender, EventArgs e)
        {
            var obj = (Button)sender;
            CardDetails selectedCard = obj.BindingContext as CardDetails;
            int cardId = selectedCard.cardId;
            CreditCards selectedCreditCard = null;
            foreach (CreditCards cds in creditCards.listCard)
            {
                if (cds.CreditCardId == cardId)
                {
                    selectedCreditCard = cds;
                }
            }
            bool delConfirm = await DisplayAlert("Alert", "Do you want to remove this card details?", "Yes", "Cancel");
            if (delConfirm)
            {
                CustomerController controller = new CustomerController();
                DeleteCreditCardRequest deleteCreditCardRequest = new DeleteCreditCardRequest();
                DeleteCreditCardResponse dcRespose = null;
                deleteCreditCardRequest.id = cardId;
                try
                {
                    dcRespose = controller.deleteCreditCard(deleteCreditCardRequest, token);

                }
                catch (Exception ex)
                {
                    await PopupNavigation.Instance.PushAsync(new Error_popup(ex.Message));
                }
                finally
                {
                    if (dcRespose != null)
                    {
                        if (dcRespose.message.ErrorCode == "200")
                        {
                            this.OnAppearing();
                            await PopupNavigation.Instance.PushAsync(new SuccessPopUp("Card Deleted Successfully."));
                        }
                        else
                        {
                            await PopupNavigation.Instance.PushAsync(new Error_popup(dcRespose.message.ErrorMessage));
                        }
                    }
                }

            }
        }

        private void imgEdit_Clicked(object sender, EventArgs e)
        {

        }
    }
}