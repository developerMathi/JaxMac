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

        public EmailonfirmSuccess()
        {
            InitializeComponent();
            confirmationType = emailConfirmationType.LogIn;
        }

        public EmailonfirmSuccess(emailConfirmationType confirmationType)
        {
            InitializeComponent();
            this.confirmationType = confirmationType;
        }

        private void doneBtn_Clicked(object sender, EventArgs e)
        {
            if (confirmationType == emailConfirmationType.LogIn)
            {
                //Navigation.PushAsync(new NavigationPage( new HomePage()));
                Navigation.PushModalAsync(new HomePage());
            }
            else if (confirmationType == emailConfirmationType.Register)
            {
                Navigation.PushModalAsync(new  LoginPage());
            }
            else
            {
                Navigation.PushModalAsync(new  HomePage());
            }
            
        }

        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            return true;
        }
    }
}