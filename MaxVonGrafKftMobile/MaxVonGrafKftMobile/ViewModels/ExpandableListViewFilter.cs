using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MaxVonGrafKftMobile.ViewModels
{
    public class ExpandableListViewFilter : INotifyPropertyChanged
    {
        public string title { get; set; }
        public List<string> childList { get; set; }

        public string _isExpanded = "iconDown.png";

        public string isExpanded 
        {
            get
            {
                return _isExpanded;
            }
            set
            {
                _isExpanded = value;
                PropertyChanged(this, new PropertyChangedEventArgs("isExpanded"));
            }
        }




        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
