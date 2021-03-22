using MaxVonGrafKftMobile.Popups;
using MaxVonGrafKftMobileController;
using MaxVonGrafKftMobileModel.AccessModels;
using MaxVonGrafKftMobileModel.Constants;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace MaxVonGrafKftMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddPaymentMethodPage : ContentPage
    {

        CustomerCreditCards customerCreditCards;
        UpdateCustomerMobileCreditCardResponse cardResponse;
        string token;

        public AddPaymentMethodPage()
        {
            InitializeComponent();
            token = App.Current.Properties["currentToken"].ToString();
            customerCreditCards = new CustomerCreditCards();
            cardResponse = null;
            //List<string> cardTypes = Enum.GetValues(typeof(CreditCardType)).Cast<CreditCardType>().Select(t => t.ToString()).ToList();
            List<string> cardTypes = new List<string>()
                    {"Visa", "Mastercard", "American Express", "Discover" };
            CardTypePicker.ItemsSource = cardTypes;
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);

        }

        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            string userCardNu = "";
            char[] charSeparators = new char[] { '/' };
            string userExDAte;
            List<string> exList = new List<string>();

            if (CardTypePicker.SelectedIndex == -1)
            {
                await PopupNavigation.Instance.PushAsync(new Error_popup("Please select a card type."));
            }
            else if (string.IsNullOrEmpty(CardNumber.Text))
            {
                await PopupNavigation.Instance.PushAsync(new Error_popup("Please enter valid credit card number"));
            }
            else
            {
                userCardNu = CardNumber.Text;
                userCardNu = userCardNu.Replace(" ", "");

                if (!string.IsNullOrEmpty(txtExpiryDate.Text))
                {
                    userExDAte = txtExpiryDate.Text;
                    exList = userExDAte.Replace(" ", "").Split(charSeparators).ToList<string>();
                }

                bool isIssue = false;

                if (CardTypePicker.SelectedItem.ToString() == "American Express")
                {
                    if (userCardNu.Length != 15)
                    {
                        isIssue = true;
                        CardNumber.Text = null;
                        await PopupNavigation.Instance.PushAsync(new Error_popup("Please enter valid credit card number"));
                    }
                    else if (txtCvv.Text.Length != 4)
                    {
                        isIssue = true;
                        await PopupNavigation.Instance.PushAsync(new Error_popup("Please enter a valid CVV number. Click on the help button for further details."));
                    }

                }
                else
                {
                    if (userCardNu.Length != 16)
                    {
                        isIssue = true;
                        CardNumber.Text = null;
                        await PopupNavigation.Instance.PushAsync(new Error_popup("Please enter valid credit card number"));
                    }
                    else if (txtCvv.Text.Length != 3)
                    {
                        isIssue = true;
                        await PopupNavigation.Instance.PushAsync(new Error_popup("Please enter a valid CVV number. Click on the help button for further details."));
                    }
                }

                if (!isIssue)
                {
                    if (exList == null)
                    {
                        await PopupNavigation.Instance.PushAsync(new Error_popup("Please enter expired date of your card."));
                    }
                    else if (exList.Count != 2)
                    {
                        await PopupNavigation.Instance.PushAsync(new Error_popup("Please check your card expiry date details"));
                    }
                    else if (Convert.ToInt32(exList[0].ToString()) > 12 || Convert.ToInt32(exList[0].ToString()) < 1)
                    {
                        await PopupNavigation.Instance.PushAsync(new Error_popup("Please check the month of expiry date details"));
                    }
                    else if (Convert.ToInt32(exList[1].ToString()) < 0)
                    {
                        await PopupNavigation.Instance.PushAsync(new Error_popup("Please check the year of expiry date details"));
                    }
                    else if (string.IsNullOrEmpty(txtCvv.Text))
                    {
                        await PopupNavigation.Instance.PushAsync(new Error_popup("Please enter CVV number"));
                    }

                    else if (string.IsNullOrEmpty(txtName.Text))
                    {
                        await PopupNavigation.Instance.PushAsync(new Error_popup("Please enter the name details."));
                    }

                    else
                    {
                        customerCreditCards.CustomerId = (int)App.Current.Properties["CustomerId"];
                        customerCreditCards.CreatedDate = DateTime.Now;
                        customerCreditCards.IsDeleted = 0;
                        customerCreditCards.CreditCardNo = userCardNu;
                        customerCreditCards.CreditCardType = Constants.returnNavotarCardTypes(CardTypePicker.SelectedItem.ToString());
                            
                            
                        customerCreditCards.NameOnCard = txtName.Text;
                        customerCreditCards.Month = Convert.ToInt32(exList[0].ToString());
                        customerCreditCards.Year = Convert.ToInt32(exList[1].ToString());
                        customerCreditCards.CreditCardCVSNo = txtCvv.Text;
                        if (customerCreditCards.Year > 0 && customerCreditCards.Month > 0)
                        {

                            var year = (customerCreditCards.Year);
                            if (year < 2000)
                            {
                                year += 2000;
                            }
                            var month = (customerCreditCards.Month);
                            var day = DateTime.DaysInMonth(year, month);
                            var d = new DateTime(year, month, day);
                            customerCreditCards.CreditCardExpiryDate = d;
                            customerCreditCards.CreditCardExpiryDateStr = d.ToString();
                            bool busy = false;
                            if (!busy)
                            {
                                try
                                {
                                    busy = true;
                                    await PopupNavigation.Instance.PushAsync(new LoadingPopup("."));
                                    await Task.Run(async () =>
                                    {
                                        cardResponse = addCreditCard(customerCreditCards, token);
                                    });
                                }
                                catch (Exception ex)
                                {
                                    await PopupNavigation.Instance.PushAsync(new Error_popup(ex.Message));
                                }
                                finally
                                {
                                    busy = false;
                                    await PopupNavigation.Instance.PopAsync();
                                    if (cardResponse != null)
                                    {
                                        if (cardResponse.creditCardID > 0)
                                        {
                                            if (App.Current.Properties.ContainsKey("LastCreditCardId"))
                                            {
                                                App.Current.Properties["LastCreditCardId"] = cardResponse.creditCardID;
                                            }
                                            else
                                            {
                                                App.Current.Properties.Add("LastCreditCardId", cardResponse.creditCardID);
                                            }
                                            await Navigation.PopModalAsync();
                                            //await PopupNavigation.Instance.PushAsync(new SuccessPopUp("Your CrediCard added Successfully.", 3));

                                        }
                                        else
                                        {
                                            await PopupNavigation.Instance.PushAsync(new Error_popup("Sorry, Update failed. Pease try again."));
                                        }
                                    }
                                    else
                                    {
                                        await PopupNavigation.Instance.PushAsync(new Error_popup("Sorry, Update failed. Pease try again."));
                                    }
                                }
                            }

                        }
                    }
                }
            }




          



           
        }

        private UpdateCustomerMobileCreditCardResponse addCreditCard(CustomerCreditCards customerCreditCards, string token)
        {
            CustomerController controller = new CustomerController();
            return controller.AddMobileCustomerCreditCard(customerCreditCards, token);
        }

        private void licenceExpiryDate_DateSelected(object sender, DateChangedEventArgs e)
        {

        }

        private void licimageUplaodViewer_Tapped(object sender, EventArgs e)
        {

        }

        private void CardTypePicker_Unfocused(object sender, FocusEventArgs e)
        {
            if (CardTypePicker.SelectedIndex != -1)
            {
                CardNumber.Text = null;
                txtCvv.Text = null;
                if (CardTypePicker.SelectedItem.ToString() == "American_Express")
                {
                    CardNumber.Mask = "###############";
                    txtCvv.Mask = "####";
                }
                else
                {
                    CardNumber.Mask = "#### #### #### ####";
                    txtCvv.Mask = "###";

                }
            }
        }

        private void CardNumber_Focused(object sender, FocusEventArgs e)
        {
            if (CardTypePicker.SelectedIndex == -1)
            {
                PopupNavigation.Instance.PushAsync(new Error_popup("Please select a card type."));
            }
        }

        private void txtCvv_Focused(object sender, FocusEventArgs e)
        {
            if (CardTypePicker.SelectedIndex == -1)
            {
                PopupNavigation.Instance.PushAsync(new Error_popup("Please select a card type."));
            }
        }

        private void expiryDateTap_Tapped(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new DetailPopUp("Expiration Date", "This date is printed on the front side of the credit card and is an important piece of information that is used for verification purposes. Therefore, it adds up to also act as a security feature. An expiration date basically means that the credit card is not valid and can be used only up to the said date printed on the respective card. All customers will have to renew their credit cards post their expiration date.&#10; For example, if the expiry date printed/embossed on the card is ‘06/19’, this means that the card will expire in the June of 2019."));
        }

        private void cvvTap_Tapped(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new DetailPopUp("CVV", "The CVV Number ('Card Verification Value') on your credit card or debit card is a 3 digit number on VISA®, MasterCard® and Discover® branded credit and debit cards. On your American Express® branded credit or debit card it is a 4 digit numeric code."));
        }

        private void btnBack_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}