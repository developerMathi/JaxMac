using MaxVonGrafKftMobileController;
using MaxVonGrafKftMobileModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MaxVonGrafKftMobile.ViewModels
{
    public class ChatPageViewModel : INotifyPropertyChanged
    {
        public bool ShowScrollTap { get; set; } = false;
        public bool LastMessageVisible { get; set; } = true;
        public bool isSenderBusy { get; set; } = false;
        public int PendingMessageCount { get; set; } = 0;
        public bool PendingMessageCountVisible { get { return PendingMessageCount > 0; } }

        public Queue<Message> DelayedMessages { get; set; } = new Queue<Message>();
        public ObservableCollection<Message> Messages { get; set; } = new ObservableCollection<Message>();
        public string TextToSend { get; set; }
        public ICommand OnSendCommand { get; set; }
        public ICommand MessageAppearingCommand { get; set; }
        public ICommand MessageDisappearingCommand { get; set; }

        public string _TextToSend {
            get { return TextToSend; }
            set
            {
                TextToSend = value;
                OnPropertyChanged();
            }
        }

        public bool _isSenderBusy
        {
            get { return isSenderBusy; }
            set
            {
                isSenderBusy = value;
                OnPropertyChanged();
            }
        }

        public ChatPageViewModel()
        {


            //Messages.Insert(0, new Message() { Text = "Hi", User = "Jax", sentTime = DateTime.Now.ToString("MM/dd/yyyy h:mm tt") });
            //Messages.Insert(0, new Message() { Text = "How are you?", User ="Mathi", sentTime = DateTime.Now.ToString("MM/dd/yyyy h:mm tt") });
            //Messages.Insert(0, new Message() { Text = "What's new?", User = "Jax", sentTime = DateTime.Now.ToString("MM/dd/yyyy h:mm tt") });
            //Messages.Insert(0, new Message() { Text = "How is your family", User ="Mathi", sentTime = DateTime.Now.ToString("MM/dd/yyyy h:mm tt") });
            //Messages.Insert(0, new Message() { Text = "How is your dog?", User ="Mathi", sentTime = DateTime.Now.ToString("MM/dd/yyyy h:mm tt") });
            //Messages.Insert(0, new Message() { Text = "How is your cat?", User ="Mathi", sentTime = DateTime.Now.ToString("MM/dd/yyyy h:mm tt") });
            //Messages.Insert(0, new Message() { Text = "How is your sister?", User = "Jax", sentTime = DateTime.Now.ToString("MM/dd/yyyy h:mm tt") });
            //Messages.Insert(0, new Message() { Text = "When we are going to meet?", User = "Jax", sentTime = DateTime.Now.ToString("MM/dd/yyyy h:mm tt") });
            //Messages.Insert(0, new Message() { Text = "I want to buy a laptop", User = "Jax", sentTime = DateTime.Now.ToString("MM/dd/yyyy h:mm tt") });
            //Messages.Insert(0, new Message() { Text = "Where I can find a good one?", User = "Jax", sentTime = DateTime.Now.ToString("MM/dd/yyyy h:mm tt") });
            //Messages.Insert(0, new Message() { Text = "Also I'm testing this chat", User = "Jax", sentTime = DateTime.Now.ToString("MM/dd/yyyy h:mm tt") });
            //Messages.Insert(0, new Message() { Text = "Oh My God!", User = "Jax", sentTime = DateTime.Now.ToString("MM/dd/yyyy h:mm tt") });
            //Messages.Insert(0, new Message() { Text = " No Problem", User ="Mathi", sentTime = DateTime.Now.ToString("MM/dd/yyyy h:mm tt") });
            //Messages.Insert(0, new Message() { Text = "Hugs and Kisses", User ="Mathi", sentTime = DateTime.Now.ToString("MM/dd/yyyy h:mm tt") });
            //Messages.Insert(0, new Message() { Text = "When we are going to meet?", User = "Jax", sentTime = DateTime.Now.ToString("MM/dd/yyyy h:mm tt") });
            //Messages.Insert(0, new Message() { Text = "I want to buy a laptop", User = "Jax", sentTime = DateTime.Now.ToString("MM/dd/yyyy h:mm tt") });
            //Messages.Insert(0, new Message() { Text = "Where I can find a good one?", User = "Jax", sentTime = DateTime.Now.ToString("MM/dd/yyyy h:mm tt") });
            //Messages.Insert(0, new Message() { Text = "Also I'm testing this chat", User = "Jax", sentTime = DateTime.Now.ToString("MM/dd/yyyy h:mm tt") });
            //Messages.Insert(0, new Message() { Text = "Oh My God!", User = "Jax", sentTime = DateTime.Now.ToString("MM/dd/yyyy h:mm tt") });
            //Messages.Insert(0, new Message() { Text = " No Problem", User = "Jax", sentTime = DateTime.Now.ToString("MM/dd/yyyy h:mm tt") });
            //Messages.Insert(0, new Message() { Text = "Hugs and Kisses", User = "Jax", sentTime = DateTime.Now.ToString("MM/dd/yyyy h:mm tt") });

            MessageAppearingCommand = new Command<Message>(OnMessageAppearing);
            MessageDisappearingCommand = new Command<Message>(OnMessageDisappearing);

            OnSendCommand = new Command(() =>
            {
                if (!string.IsNullOrEmpty(TextToSend))
                {
                    refreshModel();
                    TextToSend = string.Empty;
                }

            });

            //Code to simulate reveing a new message procces
            Device.StartTimer(TimeSpan.FromSeconds(10), () =>
            {
                refreshModel();


                return true;
            });



        }

        private void refreshModel()
        {
            int inquiryId = (int)Application.Current.Properties["InquiryID"];
            string _token = Application.Current.Properties["currentToken"].ToString();
            int customerId = (int)Application.Current.Properties["CustomerId"];
            List<InquiryChatModel> inquiryChatModels = null;
            CustomerController controller = new CustomerController();
            inquiryChatModels = controller.getChatModels(inquiryId, _token);

            if (inquiryChatModels != null)
            {
                foreach (InquiryChatModel icm in inquiryChatModels)
                {
                    int lastmId = 0;
                    if (Messages.Count > 0)
                    {
                        lastmId = Messages[0].messageId;
                    }
                   if(icm.MessageId >lastmId) { 
                        Message message = new Message();
                        message.messageId = icm.MessageId;
                        message.Text = icm.Message;
                        message.User = icm.SenderId == customerId ? Constants.customerDetails.FirstName : "Jax";
                        message.sentTime = icm.SendDate.ToString("MM/dd/yyyy h:mm tt");


                        if (LastMessageVisible)
                        {
                            Messages.Insert(0, message);
                        }
                        else
                        {
                            DelayedMessages.Enqueue(message);
                            PendingMessageCount++;
                        }
                        
                    }
                }
            }
        }

        void OnMessageAppearing(Message message)
        {
            var idx = Messages.IndexOf(message);
            if (idx <= 6)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    while (DelayedMessages.Count > 0)
                    {
                        Messages.Insert(0, DelayedMessages.Dequeue());
                    }
                    ShowScrollTap = false;
                    LastMessageVisible = true;
                    PendingMessageCount = 0;
                });
            }
        }

        void OnMessageDisappearing(Message message)
        {
            var idx = Messages.IndexOf(message);
            if (idx >= 6)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    ShowScrollTap = true;
                    LastMessageVisible = false;
                });

            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}