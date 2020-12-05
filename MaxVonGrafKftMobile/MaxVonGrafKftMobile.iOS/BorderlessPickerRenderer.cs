using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Foundation;
using MaxVonGrafKftMobile.iOS;
using MaxVonGrafKftMobile.Renders;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BorderlessPicker), typeof(BorderlessPickerRenderer))]

namespace MaxVonGrafKftMobile.iOS
{
    public class BorderlessPickerRenderer : PickerRenderer
    {
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        public static void Init() { }
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if(Control!= null)
            {
                Control.Layer.BorderWidth = 0;
                Control.BorderStyle = UITextBorderStyle.None;
            }
           
        }
    }
}