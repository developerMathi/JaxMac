using System;
using System.Collections.Generic;
using System.Text;

namespace MaxVonGrafKftMobile
{
    public interface IDownloader
    {
        void DownloadFile(string url, string folder);
        event EventHandler<DownloadEventArgs> OnFileDownloaded;
    }
}