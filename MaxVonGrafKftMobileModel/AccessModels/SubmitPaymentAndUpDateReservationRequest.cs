using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxVonGrafKftMobileModel.AccessModels
{
    public class SubmitPaymentAndUpDateReservationRequest
    {
        public BillingInformation billingInformation { get; set; }
        public UpdateReservationMobileRequest reservationMobileRequest { get; set; }
    }
}
