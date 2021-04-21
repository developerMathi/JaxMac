using MaxVonGrafKftMobile.Views;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MaxVonGrafKftMobile.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AskForLogin : PopupPage
    {
        private int fromVal;

        public AskForLogin(string msg)
        {
            InitializeComponent();
            fromVal = 0;
            contentText.Text = msg;
        }

        public AskForLogin(string msg, int fromVal) : this(msg)
        {
            this.fromVal = fromVal;
        }

        private void LoginBtn_Clicked(object sender, EventArgs e)
        {
            if (fromVal == 1)
            {
                Navigation.PushModalAsync(new LoginPage(fromVal));
            }
            else
            {
                Navigation.PushModalAsync(new LoginPage());
            }
            
        }

        private void CancelBtn_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAllAsync();
        }

        private void btnClose_Tapped(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAllAsync();
        }

        private void signUpBtn_Clicked(object sender, EventArgs e)
        {
            if (fromVal == 1)
            {
                Navigation.PushModalAsync(new RegisterPage(fromVal));
            }
            else
            {
                Navigation.PushModalAsync(new RegisterPage());
            }
            
        }
    }
}