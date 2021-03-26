using MaxVonGrafKftMobile.Popups;
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
    public partial class GalleryPage : ContentPage
    {
        private List<Document> imageDocs;
        private string v;

        IDownloader downloader = DependencyService.Get<IDownloader>();
        public GalleryPage(List<Document> imageDocs, string v) 
        {
            InitializeComponent();
            downloader.OnFileDownloaded += OnFileDownloaded;
            this.imageDocs = imageDocs;
            this.v = v;
            titleLabel.Text = v;
            if (v== "gallery")
            {
                titleLabel.Text = "Photos";
            }
            if (v == "documet")
            {
                titleLabel.Text = "Documents";
            }
        }

        private void OnFileDownloaded(object sender, DownloadEventArgs e)
        {
            if (e.FileSaved)
            {
                DisplayAlert("Alert", "File Saved Successfully", "Close");
            }
            else
            {
                DisplayAlert("Alert", "Error while saving the file", "Close");
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if(imageDocs!= null)
            {
                fixlistView(imageDocs);
            }

        }

        private void fixlistView(List<Document> imageDocs)
        {
            List<Document> formattedDocList = new List<Document>();
            foreach (Document d in imageDocs)
            {
                Document newDoc = d;
                float imgSizeKB = d.FileSize / 1024;
                float imgSizeMB = imgSizeKB / 1024;

                if (imgSizeMB > 1)
                {
                    newDoc.sizeLabel = imgSizeMB.ToString("0.00") + " MB";

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
            imageListView.ItemsSource = formattedDocList;
            imageListView.HeightRequest = formattedDocList.Count * 125;
        }



        private void titleBackBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        [Obsolete]
        private void imageListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Document selectedDocument = imageListView.SelectedItem as Document;

            try
            {
                Device.OpenUri(new Uri(selectedDocument.FilePath));
            }
            catch (Exception ex)
            {
                PopupNavigation.Instance.PushAsync(new Error_popup(ex.Message));
            }
            //downloader.DownloadFile("https://app.navotar.com/Images/UploadedFiles\\Client_1028\\4\\93a4d6a5-cc2b-486e-9ad5-1879c72c9fa0.pdf", "JAX");
        }
    }

}