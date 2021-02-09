using MaxVonGrafKftMobile.Popups;
using MaxVonGrafKftMobile.ViewModels;
using MaxVonGrafKftMobileController;
using MaxVonGrafKftMobileModel;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MaxVonGrafKftMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatPage : ContentPage
    {
        int InquiryID;
        List<Inquiry> inquirisResponseByCustomerId;
        int customerId;
        string _token;
        int curInquiryId;

        public ChatPage(string subject, int @new)
        {
            InitializeComponent();
            this.BindingContext = new ChatPageViewModel();
            customerId = (int)Application.Current.Properties["CustomerId"];
            _token = Application.Current.Properties["currentToken"].ToString();
            InquiryID = (int)Application.Current.Properties["InquiryID"];
            TopicLabel.Text = subject;
            curInquiryId = InquiryID;
            if (@new == 3)
            {
                chatInput.IsVisible = false;
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            

            if (InquiryID == 0)
            {
                //check for customer mobile chat inquiry and create if not exist
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
                                //registrationDBModel = getRegistrationDBModel(customerId, _token);
                                inquirisResponseByCustomerId = getInquries();

                                if(inquirisResponseByCustomerId== null)
                                {
                                    addCustomerChatInquery();
                                }
                                else if (inquirisResponseByCustomerId.Count == 0)
                                {
                                    addCustomerChatInquery();
                                }
                                else
                                {
                                    Inquiry customerChatInquiry = null;
                                    bool isChatInqueryAvailable = false;
                                    foreach(Inquiry i in inquirisResponseByCustomerId)
                                    {
                                        if(i.Subject=="Customer mobile chat")
                                        {
                                            isChatInqueryAvailable = true;
                                            customerChatInquiry = i;
                                        }
                                    }
                                    if (isChatInqueryAvailable)
                                    {
                                        updateChatInquiryInformation(Convert.ToInt32(customerChatInquiry.InquiryId) );
                                    }
                                    else
                                    {
                                        addCustomerChatInquery();
                                    }
                                }
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

                    }
                }

            }
            else
            {
                updateChatInquiryInformation((int)InquiryID);
            }


            //this.BindingContext = new ChatPageViewModel();


        }

        private void updateChatInquiryInformation(int inquiryId)
        {
            Application.Current.Properties["InquiryID"] = inquiryId;
            List<InquiryChatModel> inquiryChatModels = null;
            CustomerController controller = new CustomerController();
            inquiryChatModels = controller.getChatModels(inquiryId,_token);

            if(inquiryChatModels != null)
            {
                ObservableCollection<Message> Messages = new ObservableCollection<Message>();
                foreach (InquiryChatModel icm in inquiryChatModels)
                {
                    Message message = new Message();
                    message.messageId = icm.MessageId;
                    message.Text = icm.Message;
                    message.User = icm.SenderId == customerId ? Constants.customerDetails.FirstName : "Jax";
                    message.sentTime = icm.SendDate.ToString("MM/dd/yyyy h:mm tt");
                   
                    
                    if (BindingContext != null)
                    {
                        var vm = BindingContext as ChatPageViewModel;
                        vm.Messages.Insert(0, message);
                    }
                }

               
            }


        }

        private void addCustomerChatInquery()
        {
            Inquiry newInquiry = new Inquiry();
            newInquiry.ClientId = Constants.ClientId;
            newInquiry.CustomerId = customerId;
            newInquiry.Subject = "Customer mobile chat";
            newInquiry.Description = "Inquiry for maintain chats with customer via mobile application";
            newInquiry.Status = (int)MaxVonGrafKftMobileModel.Constants.InquiryStatus.New;
            newInquiry.ReportedDate = DateTime.Now;
            CustomerController controller = new CustomerController();

            int id = controller.addInquiry(newInquiry,_token);
            if (id > 0)
            {
                Application.Current.Properties["InquiryID"] = id;
                curInquiryId = id;
            }
        }

        private List<Inquiry> getInquries()
        {
            List<Inquiry> inquiries;
            CustomerController controller = new CustomerController();
            inquiries = controller.getInquries(customerId,_token);
            return inquiries;
        }

        public void ScrollTap(object sender, System.EventArgs e)
        {
            lock (new object())
            {
                if (BindingContext != null)
                {
                    var vm = BindingContext as ChatPageViewModel;

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        while (vm.DelayedMessages.Count > 0)
                        {
                            vm.Messages.Insert(0, vm.DelayedMessages.Dequeue());
                        }
                        vm.ShowScrollTap = false;
                        vm.LastMessageVisible = true;
                        vm.PendingMessageCount = 0;
                        ChatList?.ScrollToFirst();
                    });


                }

            }
        }

        public void OnListTapped(object sender, ItemTappedEventArgs e)
        {
            chatInput.UnFocusEntry();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Constants.LastMessageId = 0;
        }

        private void btnMenu_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}