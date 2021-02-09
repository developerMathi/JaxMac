using MaxVonGrafKftMobile.Views.Cells;
using MaxVonGrafKftMobileModel;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MaxVonGrafKftMobile.Helpers
{
    class ChatTemplateSelector : DataTemplateSelector
    {
        DataTemplate incomingDataTemplate;
        DataTemplate outgoingDataTemplate;

        public ChatTemplateSelector()
        {
            this.incomingDataTemplate = new DataTemplate(typeof(IncomingViewCell));
            this.outgoingDataTemplate = new DataTemplate(typeof(OutgoingViewCell));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var messageVm = item as Message;
            if (messageVm == null)
                return null;


            return (messageVm.User == Constants.customerDetails.FirstName) ? outgoingDataTemplate : incomingDataTemplate;
        }

    }
}