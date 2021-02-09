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
    public partial class NoNetwork : ContentPage
    {
        private ApiToken apiToken;
        private GetClientDetailsForMobileResponse getClientDetailsForMobile;
        string _token;
        bool IsUpdatesAvailable;

        public NoNetwork()
        {
            InitializeComponent();
        }

        private void refreshBtn_Clicked(object sender, EventArgs e)
        {
            VersionTracking.Track();

            var currentVersion = VersionTracking.CurrentVersion;


            if (!App.Current.Properties.ContainsKey("CustomerId"))
            {
                App.Current.Properties.Add("CustomerId", 0);
            }

            getAndSaveClientSecrets();

            if (_token != null)
            {
                IsUpdatesAvailable = checkForApplicationUpdates(currentVersion);
                if (!IsUpdatesAvailable)
                {
                    if ((int)App.Current.Properties["CustomerId"] == 0)
                    {
                        Navigation.PushAsync(new WelcomPage());
                        //MainPage = new NavigationPage(new enterConfirmationCodePage());
                    }
                    else
                    {
                        Navigation.PushAsync(new HomePage());
                    }
                }
                else
                {
                    //Want to retuen page that reqire the app to update
                    Navigation.PushAsync(new UpdateApplication());
                }
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