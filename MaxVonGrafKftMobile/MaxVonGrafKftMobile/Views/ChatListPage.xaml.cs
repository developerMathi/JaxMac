using MaxVonGrafKftMobile.Popups;
using MaxVonGrafKftMobileController;
using MaxVonGrafKftMobileModel;
using Rg.Plugins.Popup.Services;
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
    public partial class ChatListPage : ContentPage
    {
        List<Inquiry> inquirisResponseByCustomerId;
        int customerId;
        string _token;
        public ChatListPage()
        {
            InitializeComponent();
            customerId = (int)Application.Current.Properties["CustomerId"];
            _token = Application.Current.Properties["currentToken"].ToString();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            bool busy = false;
            if (!busy)
            {
                try
                {
                    busy = true;
                    await PopupNavigation.Instance.PushAsync(new LoadingPopup("Loading.."));

                    await Task.Run(async () =>
                    {
                        try
                        {
                            inquirisResponseByCustomerId = getInquries();
                        }
                        catch (Exception ex)
                        {
                            await PopupNavigation.Instance.PushAsync(new ErrorWithClosePagePopup(ex.Message));

                        }
                    });
                }
                catch (Exception ex)
                {
                    await PopupNavigation.Instance.PushAsync(new ErrorWithClosePagePopup(ex.Message));

                }
                finally
                {

                    busy = false;
                    if (PopupNavigation.Instance.PopupStack.Count == 1)
                    {
                        await PopupNavigation.Instance.PopAllAsync();
                    }
                    else if (PopupNavigation.Instance.PopupStack.Count > 1)
                    {
                        if (PopupNavigation.Instance.PopupStack[PopupNavigation.Instance.PopupStack.Count - 1].GetType() != typeof(ErrorWithClosePagePopup))
                        {
                            await PopupNavigation.Instance.PopAllAsync();
                        }
                    }
                    if(inquirisResponseByCustomerId != null)
                    {
                        inquirisResponseByCustomerId.Reverse();
                        List<Inquiry> listItems = new List<Inquiry>();
                        
                        chatList.ItemsSource = inquirisResponseByCustomerId;
                    }

                }


            }
        }

        private List<Inquiry> getInquries()
        {
            List<Inquiry> inquiries;
            CustomerController controller = new CustomerController();
            inquiries = controller.getInquries(customerId, _token);
            return inquiries;
        }

        private void btnMenu_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void chatList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Inquiry selectedInquiry = chatList.SelectedItem as Inquiry;
            Application.Current.Properties["InquiryID"]=Convert.ToInt32(selectedInquiry.InquiryId);
            Navigation.PushAsync(new ChatPage(selectedInquiry.Subject,selectedInquiry.Status));
        }

        private void addnewChat_Clicked(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(chatTopic.Text))
            {
                PopupNavigation.Instance.PushAsync(new Error_popup("Please enter a topic to your chat"));
            }
            else
            {
                Inquiry newInquiry = new Inquiry();
                newInquiry.ClientId = Constants.ClientId;
                newInquiry.CustomerId = customerId;
                newInquiry.Subject = chatTopic.Text;
                newInquiry.Description = "Inquiry for maintain chats with customer via mobile application";
                newInquiry.Status = (int)MaxVonGrafKftMobileModel.Constants.InquiryStatus.New;
                newInquiry.ReportedDate = DateTime.Now;
                CustomerController controller = new CustomerController();

                string subj = chatTopic.Text;
                chatTopic.Text = string.Empty;
                int id = controller.addInquiry(newInquiry, _token);
                if (id > 0)
                {
                    Application.Current.Properties["InquiryID"] = id;
                    Navigation.PushAsync(new ChatPage(subj, (int)MaxVonGrafKftMobileModel.Constants.InquiryStatus.New));
                }
            }
        }

        private void addChat_Clicked(object sender, EventArgs e)
        {
            addChat.IsVisible = false;
            addChatgrid.IsVisible = true;
        }
    }
}