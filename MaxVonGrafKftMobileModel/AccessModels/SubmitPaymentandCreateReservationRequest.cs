using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxVonGrafKftMobileModel.AccessModels
{
    public class SubmitPaymentandCreateReservationRequest
    {
        public BillingInformation billingInformation { get; set; }
        public CreateReservationMobileRequest reservationMobileRequest { get; set; }
    }
}
