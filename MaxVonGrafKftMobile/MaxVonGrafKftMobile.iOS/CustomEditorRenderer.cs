using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using MaxVonGrafKftMobile.Renders;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomEditer), typeof(EntryRenderer))]


namespace MaxVonGrafKftMobile.iOS
{

    public class CustomEditorRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                //Control.BorderStyle = UITextBorderStyle.None;
                Control.Layer.BorderWidth = 0;
                //Control.Layer.BorderColor = UIColor.Clear.CGColor;
                //Control.TextColor = UIColor.White;
            }
        }
    }
}