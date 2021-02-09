using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxVonGrafKftMobileModel.AccessModels
{
    public class GetAllDocumentByCustomerIdResponse
    {
        public List<Document> DocList { get; set; }
        public ApiMessage message { get; set; }
    }
}
