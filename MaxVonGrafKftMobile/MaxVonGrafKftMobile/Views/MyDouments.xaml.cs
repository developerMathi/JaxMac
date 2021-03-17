using MaxVonGrafKftMobile.Popups;
using MaxVonGrafKftMobileController;
using MaxVonGrafKftMobileModel.AccessModels;
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
    public partial class MyDouments : ContentPage
    {
        GetMobileCustomerByIDRequest request;
        GetAllDocumentByCustomerIdResponse response;
        List<Document> RegDocs;
        List<Document> InsDocs;
        List<Document> otherDocs;
        //List<Document> sortedList;
        string token;

        public MyDouments()
        {
            InitializeComponent();
            request = new GetMobileCustomerByIDRequest();
            response = null;
            token = App.Current.Properties["currentToken"].ToString();
            request.CustomerId = (int)App.Current.Properties["CustomerId"];
            RegDocs = new List<Document>();
            InsDocs = new List<Document>();
            otherDocs = new List<Document>();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                await PopupNavigation.Instance.PopAllAsync();
            }

            bool busy = false;
            if (!busy)
            {
                try
                {
                    busy = true;
                    await PopupNavigation.Instance.PushAsync(new LoadingPopup("Loading details..."));

                    await Task.Run(async () =>
                    {
                        try
                        {
                            response = getDocuments(request);
                        }
                        catch (Exception ex)
                        {
                            await PopupNavigation.Instance.PushAsync(new Error_popup(ex.Message));
                        }
                    });
                }
                finally
                {
                    busy = false;
                    if (PopupNavigation.Instance.PopupStack.Count == 1)
                    {
                        await PopupNavigation.Instance.PopAllAsync();
                    }
                    if (PopupNavigation.Instance.PopupStack.Count > 1)
                    {
                        if (PopupNavigation.Instance.PopupStack[PopupNavigation.Instance.PopupStack.Count - 1].GetType() != typeof(ErrorWithClosePagePopup))
                        {
                            await PopupNavigation.Instance.PopAllAsync();
                        }
                    }

                    if (response != null)
                    {
                        if (response.message.ErrorCode == "200")
                        {
                            //sortedList = response.DocList.OrderByDescending(x => x.DocumentId).ToList();

                            //fixtoRecentListView(sortedList);


                            RegDocs = new List<Document>();
                            InsDocs = new List<Document>();
                            otherDocs = new List<Document>();

                            float regSize = 0;
                            float insSize = 0;
                            float docSize = 0;
                            foreach (Document d in response.DocList)
                            {
                                if(d.Side != null)
                                {
                                    if (d.Side.ToLower().Contains("license"))
                                    {
                                        RegDocs.Add(d);
                                        regSize += d.FileSize;
                                    }
                                    else if (d.Side.ToLower().Contains("insurance"))
                                    {
                                        InsDocs.Add(d);
                                        insSize += d.FileSize;
                                    }
                                    else
                                    {
                                        otherDocs.Add(d);
                                        docSize += d.FileSize;
                                    }
                                }
                                else
                                {
                                    otherDocs.Add(d);
                                    docSize += d.FileSize;
                                }

                            }

                            if (RegDocs != null)
                            {
                                float RegSizeKB = regSize / 1024;
                                float RegSizeMB = RegSizeKB / 1024;

                                if (RegSizeMB > 1)
                                {
                                    imageSize.Text = RegSizeMB.ToString("0.00");
                                    imageSizeUnit.Text = " MB";
                                }
                                else
                                {
                                    imageSize.Text = RegSizeKB.ToString("0.00");
                                    imageSizeUnit.Text = " KB";
                                }
                                imFileCount.Text = RegDocs.Count.ToString();
                            }
                            if (InsDocs != null)
                            {
                                float InsSizeKB = insSize / 1024;
                                float InsSizeMB = InsSizeKB / 1024;

                                if (InsSizeMB > 1)
                                {
                                    fileSize.Text = InsSizeMB.ToString("0.00");
                                    fileSizeUnit.Text = " MB";
                                }
                                else
                                {
                                    fileSize.Text = InsSizeKB.ToString("0.00");
                                    fileSizeUnit.Text = " KB";
                                }
                                othFilesCount.Text = InsDocs.Count.ToString();
                            }
                            if (otherDocs != null)
                            {
                                float docSizeKB = docSize / 1024;
                                float docSizeMB = docSizeKB / 1024;

                                if (docSizeMB > 1)
                                {
                                    otherFileSize.Text = docSizeMB.ToString("0.00");
                                    otherFileSizeUnit.Text = " MB";
                                }
                                else
                                {
                                    otherFileSize.Text = docSizeKB.ToString("0.00");
                                    otherFileSizeUnit.Text = " KB";
                                }
                                othDocFilesCount.Text = otherDocs.Count.ToString();
                            }
                        }
                        else
                        {
                            await PopupNavigation.Instance.PushAsync(new Error_popup(response.message.ErrorMessage));
                        }
                    }
                    else
                    {
                        await PopupNavigation.Instance.PushAsync(new Error_popup("Sorry, Something went wrong, Please try again."));
                    }
                }
            }


        }

        private void fixtoRecentListView(List<Document> sortedList)
        {
            List<Document> formattedDocList = new List<Document>();
            foreach(Document d in sortedList)
            {
                Document newDoc = d;
                float imgSizeKB = d.FileSize / 1024;
                float imgSizeMB = imgSizeKB / 1024;

                if (imgSizeMB > 1)
                {
                    newDoc.sizeLabel = imgSizeMB.ToString("0.00")+" MB" ;
                    
                }
                else
                {
                    newDoc.sizeLabel = imgSizeKB.ToString("0.00") + " KB";
                }

                if (d.ContentType.Contains("image"))
                {
                    newDoc.previewImageSource = "iconsImage.png";
                }
                else if ((d.ContentType.Contains("pdf")))
                {
                    newDoc.previewImageSource = "iconsPdf.png";
                }
                else if ((d.ContentType.Contains("zip")))
                {
                    newDoc.previewImageSource = "iconsZip.png";
                }
                else if ((d.ContentType.Contains("word")))
                {
                    newDoc.previewImageSource = "iconsWord.png";
                }
                else if ((d.ContentType.Contains("excel")))
                {
                    newDoc.previewImageSource = "iconExcel.png";
                }
                else if ((d.ContentType.Contains("css")))
                {
                    newDoc.previewImageSource = "iconsXML.png";
                }
                else if ((d.ContentType.Contains("html")))
                {
                    newDoc.previewImageSource = "iconsXML.png";
                }
                else if ((d.ContentType.Contains("text")))
                {
                    newDoc.previewImageSource = "iconsText.png";
                }
                else if ((d.ContentType.Contains("xml")))
                {
                    newDoc.previewImageSource = "iconsXML.png";
                }
                else 
                {
                    newDoc.previewImageSource = "iconsText.png";
                }

                formattedDocList.Add(newDoc);
            }
            recentFileList.ItemsSource = formattedDocList;
            recentFileList.HeightRequest = formattedDocList.Count * 125;
        }

        private GetAllDocumentByCustomerIdResponse getDocuments(GetMobileCustomerByIDRequest request)
        {
            GetAllDocumentByCustomerIdResponse response = null;
            LoginController controller = new LoginController();

            response = controller.getDocuments(request, token);
            return response;
        }


        private void titleBackBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void GalleryPan_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new GalleryPage(RegDocs,"Registration Documents"));
        }

        private void docPan_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new GalleryPage(InsDocs,"Insurance Documents"));
        }

        [Obsolete]
        private void recentFileList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Document selectedDocument = recentFileList.SelectedItem as Document;
            try
            {
                Device.OpenUri(new Uri(selectedDocument.FilePath));
            }
            catch(Exception ex)
            {
                PopupNavigation.Instance.PushAsync(new Error_popup(ex.Message));
            }
        }

        void otherdocPan_Tapped(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new GalleryPage(otherDocs, "Other Documents"));
        }
    }
}
