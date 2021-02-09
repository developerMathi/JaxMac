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
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MaxVonGrafKftMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IntialLoading : ContentPage
    {

        private ApiToken apiToken;
        private GetClientDetailsForMobileResponse getClientDetailsForMobile;
        string _token;
        bool IsUpdatesAvailable;
        string currentVersion;

        public IntialLoading()
        {
            InitializeComponent();

            if (!App.Current.Properties.ContainsKey("CustomerId"))
            {
                App.Current.Properties.Add("CustomerId", 0);
            }

            if (!App.Current.Properties.ContainsKey("InquiryID"))
            {
                App.Current.Properties.Add("InquiryID", 0);
            }
            currentVersion = VersionTracking.CurrentVersion;

           
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await Task.Delay(300);
            GetClientSecretTokenRequest getClientSecretTokenRequest = new GetClientSecretTokenRequest();
            getClientSecretTokenRequest.ClientId = Constants.ClientId;
            ApiController apiController = new ApiController();
            GetClientSecretTokenResponse clientSecretTokenResponse = null;

            getAndSaveClientSecrets();

            if (_token != null)
            {
                IsUpdatesAvailable = checkForApplicationUpdates(currentVersion);
                if (!IsUpdatesAvailable)
                {
                    if ((int)App.Current.Properties["CustomerId"] == 0)
                    {
                        await Navigation.PushAsync(new WelcomPage());
                        //MainPage = new NavigationPage(new WelcomPage());
                        //MainPage = new NavigationPage(new enterConfirmationCodePage());
                    }
                    else
                    {
                        await Navigation.PushAsync(new HomePage());
                        //MainPage = new NavigationPage(new HomePage());
                    }
                }
                else
                {
                    //Want to retuen page that reqire the app to update
                    await Navigation.PushAsync(new UpdateApplication());
                }
            }
            else
            {
                //Want to return No network page
                await Navigation.PushAsync(new NoNetwork());
            }
        }


        private bool checkForApplicationUpdates(string currentVersion)
        {
            CommonController common = new CommonController();
            bool isUpdateAvailable = false;
            try
            {
                if (Xamarin.Forms.Device.OS == TargetPlatform.iOS)
                {
                    isUpdateAvailable = common.checkForApplicationUpdates(_token, currentVersion, "iOS");
                }

                if (Xamarin.Forms.Device.OS == TargetPlatform.Android)
                {
                    isUpdateAvailable = common.checkForApplicationUpdates(_token, currentVersion, "Android");
                }



            }
            catch (Exception ex)
            {
                isUpdateAvailable = false;
            }
            return isUpdateAvailable;
        }

        private void getAndSaveClientSecrets()
        {
            GetClientSecretTokenRequest getClientSecretTokenRequest = new GetClientSecretTokenRequest();
            getClientSecretTokenRequest.ClientId = Constants.ClientId;
            ApiController apiController = new ApiController();
            GetClientSecretTokenResponse clientSecretTokenResponse = null;
            try
            {
                clientSecretTokenResponse = apiController.GetClientSecretToken(getClientSecretTokenRequest);
            }
            catch (Exception ex)
            {
                PopupNavigation.Instance.PushAsync(new Error_popup(ex.Message));
            }
            if (clientSecretTokenResponse != null)
            {
                GetAccessTokenRequest tokenRequest = new GetAccessTokenRequest();
                tokenRequest.client_id = clientSecretTokenResponse.apiConsumerId;
                tokenRequest.client_secret = clientSecretTokenResponse.apiConsumerSecret;
                tokenRequest.grant_type = "client_credentials";

                try
                {
                    apiToken = apiController.GetAccessToken(tokenRequest);
                }
                catch (Exception ex)
                {
                    apiToken = null;
                }
                if (apiToken != null)
                {
                    _token = apiToken.access_token;
                    CommonController commonController = new CommonController();

                    try
                    {
                        getClientDetailsForMobile = commonController.GetClientDetailsForMobile(_token);
                    }
                    catch (Exception ex)
                    {
                        PopupNavigation.Instance.PushAsync(new Error_popup(ex.Message));
                    }


                    if (getClientDetailsForMobile != null)
                    {
                        if (getClientDetailsForMobile.admin != null)
                        {
                            Constants.admin = getClientDetailsForMobile.admin;
                        }
                    }

                    if (App.Current.Properties.ContainsKey("currentToken"))
                    {
                        App.Current.Properties["currentToken"] = _token;
                    }
                    else
                    {
                        App.Current.Properties.Add("currentToken", _token);
                    }
                }
            }
        }
    }
}