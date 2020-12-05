using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxVonGrafKftMobileModel.AccessModels
{
    public class GetAllCustomerMobileCreditCardResponse
    {
        public List<CreditCards> listCard { get; set; }
        public ApiMessage message { get; set; }
    }
}
