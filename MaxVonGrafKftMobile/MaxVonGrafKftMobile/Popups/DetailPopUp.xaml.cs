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
    public partial class DetailPopUp : PopupPage
    {
        public DetailPopUp(string name, string description)
        {
            InitializeComponent();
            titleLabel.Text = name;
            descriptionText.Text = description;
        }

        private void btnClose_Tapped(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }
    }
}