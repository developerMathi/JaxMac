using MaxVonGrafKftMobileModel;
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
    public partial class FilterPopup : PopupPage
    {
        private VehicleFilterSearch vehicleFilter;
        private List<string> vehicletypeList;

  
        public FilterPopup(VehicleFilterSearch vehicleFilter, List<string> vehicletypeList)
        {
            InitializeComponent();
            this.vehicleFilter = vehicleFilter;
            this.vehicleFilter = vehicleFilter;
            this.vehicletypeList = vehicletypeList;
            vehtypePicker.ItemsSource = vehicletypeList;
        }

        private void btnClose_Tapped(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }

        private void cancelBtn_Clicked(object sender, EventArgs e)
        {
            vehicleFilter = new VehicleFilterSearch();
            PopupNavigation.Instance.PopAsync();
        }

        private void applyBtn_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPrice.Text))
            {
                vehicleFilter.Price = decimal.Parse(txtPrice.Text);
            }
            if (!string.IsNullOrEmpty(minPriceEntry.Text))
            {
                vehicleFilter.MinPrice = decimal.Parse(minPriceEntry.Text);
            }
            if (!string.IsNullOrEmpty(seatEntry.Text ))
            {
                vehicleFilter.seatsCount = int.Parse(seatEntry.Text);
            }
            if (!string.IsNullOrEmpty(bagEntry.Text))
            {
                vehicleFilter.buggageCount = int.Parse(bagEntry.Text);
            }
            if (!string.IsNullOrEmpty(doorEntry.Text))
            {
                vehicleFilter.DoorsCount = int.Parse(doorEntry.Text);
            }
            if(vehtypePicker.SelectedIndex!= -1)
            {
                vehicleFilter.VehicleType = vehtypePicker.SelectedItem.ToString();
            }
            vehicleFilter.SortingOrder = orderPicker.SelectedIndex;

            MessagingCenter.Send(this, "FilterUpdated");
            PopupNavigation.Instance.PopAsync();
        }
    }
}