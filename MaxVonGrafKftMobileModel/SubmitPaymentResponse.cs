using MaxVonGrafKftMobileModel.AccessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxVonGrafKftMobileModel
{
    public class SubmitPaymentResponse
    {
        public string message { get; set; }
        public billData Data { get; set; }
        public bool Status { get; set; }
        public bool RefundStatus { get; set; }
    }


    public class billData
    {
        public BillingInformation billingInfo { get; set; }
        public int paymentLogId { get; set; }
        public string referenceNumber { get; set; }
       public CreateReservationMobileResponse reservationRespose { get; set; }
       public UpdateReservationMobileResponse updateReserResponse { get; set; }
       public ExtendAgreementResponse extendAgreementResponse { get; set; }

    }
}
