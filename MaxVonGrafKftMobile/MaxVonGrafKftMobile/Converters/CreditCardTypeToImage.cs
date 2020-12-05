using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace MaxVonGrafKftMobile.Converters
{
    public class CreditCardTypeToImage : IValueConverter
    {
        public  ImageSource Visa { get; set; }
        public  ImageSource Master { get; set; }
        public  ImageSource American_Express { get; set; }
        public  ImageSource Discover { get; set; }
        public  ImageSource Credit_Card { get; set; }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string cardType = value.ToString();
            if (cardType == "Visa")
            {
                return Visa;
            }
            else if (cardType == "Master")
            {
                return Master;
            }
            else if (cardType == "American_Express")
            {
                return American_Express;
            }
            else if (cardType == "Discover")
            {
                return Discover;
            }
            else
            {
                return Credit_Card;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
