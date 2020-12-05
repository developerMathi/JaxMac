using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxVonGrafKftMobileModel
{
    public enum PaymentMode
    {

        Deposit = 1,
        Payment = 2
    };

    public class PaymentReferenceInfo
    {
        public int AgreementId { get; set; }
        public int ResevationId { get; set; }
        public string Description { get; set; }
        public string PaymentType { get; set; }

        public string PaymentBy { get; set; }

        public string PaymentDateStr { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentMode PaymentMode { get; set; }

    }

    public class BillingInformation
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NameOnCard { get; set; }

        public string Email { get; set; }
        public string CreditCardNumber { get; set; }
        public string CreditCardNumberDisplay { get; set; }
        public string SecurityCode { get; set; }
        public int ExpiryMonth { get; set; }
        public string PaymentType { get; set; }
        public double PaymentAmount { get; set; }
        public int ExpiryYear { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string CreditCardType { get; set; }
        public int LocationId { get; set; }
        public int CustomerId { get; set; }
        public DateTime ClientDate { get; set; }
        public PaymentReferenceInfo PaymentInfo { get; set; }
        public string FullName { get; set; }
        public string ReferenceNumber { get; set; }
        public string CountryAlphaTwoCode { get; set; }
        public string phone { get; set; }
        public string SessionId { get; set; }
        public string reasonId { get; set; }
        public string reasonText { get; set; }
        public string ContactNumber { get; set; }
        public string CreditCardToken { get; set; }
        public int CreditCardId { get; set; }


    }
}