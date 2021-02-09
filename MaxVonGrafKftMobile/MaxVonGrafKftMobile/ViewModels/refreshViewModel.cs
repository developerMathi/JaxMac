using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MaxVonGrafKftMobile.ViewModels
{
    public class refreshViewModel : INotifyPropertyChanged
    {

        const int RefreshDuration = 2;
       
        bool isRefreshing;

        public event EventHandler refreshEvent;

        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set
            {
                isRefreshing = value;
                OnPropertyChanged();
            }
        }

        public ICommand RefreshCommand => new Command(async () => await RefreshItemsAsync());

        async Task RefreshItemsAsync()
        {
            IsRefreshing = true;
            await Task.Delay(TimeSpan.FromSeconds(RefreshDuration));
            refreshPage();
            IsRefreshing = false;
        }

        private void refreshPage()
        {
            Task.Delay(500);
        }


        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
