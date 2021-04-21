using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaxVonGrafKftMobileModel.Constants;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MaxVonGrafKftMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmailonfirmSuccess : ContentPage
    {
        private emailConfirmationType confirmationType;
        private int fromVal;

        public EmailonfirmSuccess()
        {
            InitializeComponent();
            confirmationType = emailConfirmationType.LogIn;
            this.fromVal = 0;
        }

        public EmailonfirmSuccess(emailConfirmationType confirmationType)
        {
            InitializeComponent();
            this.confirmationType = confirmationType;
            this.fromVal = 0;
        }

        public EmailonfirmSuccess(emailConfirmationType confirmationType, int fromVal) : this(confirmationType)
        {
            this.fromVal = fromVal;
        }

        private void doneBtn_Clicked(object sender, EventArgs e)
        {
            if (confirmationType == emailConfirmationType.LogIn)
            {
                //Navigation.PushAsync(new NavigationPage( new HomePage()));

                if (fromVal == 1)
                {
                    gobackAsync();
                }
                else
                {
                    Navigation.PushModalAsync(new HomePage());
                }
                
            }
            else if (confirmationType == emailConfirmationType.Register)
            {
                if (fromVal == 1)
                {
                    gobackAsync();
                }
                else
                {
                    Navigation.PushModalAsync(new LoginPage());
                }
               
            }
            else
            {
                if (fromVal == 1)
                {
                    gobackAsync();
                }
                else
                {
                    Navigation.PushModalAsync(new HomePage());
                }
               
            }
            
        }

        public async void gobackAsync()
        {

            int totalModals = Application.Current.MainPage.Navigation.ModalStack.Count;

            //i set currModal = 1 here to back to page 2, If you wan to go back to 3, you can set currModal = 2, etc...
            // remember to add flase in PopModalAsync to aviod the animation.

            for (int currModal = 2; currModal < totalModals; currModal++)
            {
                await Application.Current.MainPage.Navigation.PopModalAsync(false);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            return true;
        }
    }
}