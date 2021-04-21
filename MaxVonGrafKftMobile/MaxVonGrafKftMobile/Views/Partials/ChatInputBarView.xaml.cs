using MaxVonGrafKftMobile.ViewModels;
using MaxVonGrafKftMobileController;
using MaxVonGrafKftMobileModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MaxVonGrafKftMobile.Views.Partials
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatInputBarView : ContentView
    {

        int InquiryID;
        int customerId;
        string _token;
        InquiryChatModel inquiryChatModel;

        public ChatInputBarView()
        {
            InitializeComponent();

            customerId = (int)Application.Current.Properties["CustomerId"];
            _token = Application.Current.Properties["currentToken"].ToString();
            InquiryID = (int)Application.Current.Properties["InquiryID"];
            inquiryChatModel = new InquiryChatModel();

            if (Device.RuntimePlatform == Device.iOS)
            {
                this.SetBinding(HeightRequestProperty, new Binding("Height", BindingMode.OneWay, null, null, null, chatTextInput));
            }
        }
        public void Handle_Completed(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(chatTextInput.Text))
            {
                if (!string.IsNullOrWhiteSpace(chatTextInput.Text))
                {
                    inquiryChatModel.InquiryID = (int)Application.Current.Properties["InquiryID"]; ;
                    inquiryChatModel.UserId = customerId;
                    inquiryChatModel.SenderId = customerId;
                    inquiryChatModel.ReceiverId = Constants.ClientId;
                    inquiryChatModel.SendDate = DateTime.Now;
                    inquiryChatModel.Message = chatTextInput.Text;
                    CustomerController controller = new CustomerController();
                    int id = controller.addInquiryChatModel(inquiryChatModel, _token);
                    (this.Parent.Parent.BindingContext as ChatPageViewModel).OnSendCommand.Execute(null);

                    chatTextInput.Text = string.Empty;
                }
            }
            
        }

        public void UnFocusEntry()
        {
            chatTextInput?.Unfocus();
        }

    }
}