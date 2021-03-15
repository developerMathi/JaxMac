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
    public partial class SuccessPopUp : PopupPage
    {
        private int v;
        private string email;

        public SuccessPopUp(string content)
        {
            InitializeComponent();
            contentText.Text = content;
            v = 0;
        }

        public SuccessPopUp(string content, int v)
        {
            InitializeComponent();
            contentText.Text = content;
            this.v = v;
        }

        public SuccessPopUp(string content, int v,string email)
        {
            InitializeComponent();
            contentText.Text = content;
            this.email = email;
            this.v = v;
            Okbtn.Text = "Continue Change Password";
            LoginBtn.IsVisible = true;
        }

        private void Okbtn_Clicked(object sender, EventArgs e)
        {
            if (v == 0)
            {
                PopupNavigation.Instance.PopAllAsync();
            }
            if (v == 1)
            {
                Navigation.PushAsync(new HomePage());
            }
            if (v == 2)
            {
                Navigation.PushAsync(new MyProfile());
            }
            if (v == 3)
            {
                Navigation.PopModalAsync();
            }
            if (v == 5)
            {
                Navigation.PushAsync(new ChangePasswordWithoutLogin(email));
            }
            if (v == 6)
            {
                Navigation.PushAsync(new LoginPage());
            }
        }

        void LoginBtn_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new LoginPage());
        }
    }
}