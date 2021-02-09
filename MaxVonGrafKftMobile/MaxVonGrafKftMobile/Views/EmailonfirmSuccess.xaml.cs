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
    public partial class EmailonfirmSuccess : ContentPage
    {
        public EmailonfirmSuccess()
        {
            InitializeComponent();
        }

        private void doneBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new HomePage());
        }

        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            return true;
        }
    }
}