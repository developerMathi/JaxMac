using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxVonGrafKftMobileModel.AccessModels
{
    [Serializable]
    public partial class InvoiceEmailResponse
    {
        public int statusCode { get; set; }
        public string status { get; set; }
        public string errorMessage { get; set; }
    }
}
