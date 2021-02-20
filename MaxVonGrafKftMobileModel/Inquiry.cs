using MaxVonGrafKftMobileModel.AccessModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxVonGrafKftMobileModel
{
    public class Inquiry
    {
        public Inquiry()
        {
            inquiryChatModelList = new List<InquiryChatModel>();
        }
        public object InquiryId { get; set; }
        public int? InquiryID { get; set; }

        public string InquiryNumber { get; set; }

        public String Name { get; set; }

        public String Number { get; set; }

        public String Email { get; set; }

        public String Note { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? CreatedStartDate { get; set; }
        public DateTime? CreatedEndDate { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }

        public int ClientId { get; set; }

        public DateTime? LastUpdatedDate { get; set; }
        public int LastUpdatedBy { get; set; }
        public string LastUpdatedByName { get; set; }

        public string LengthOfRental { get; set; }

        public List<InquiryItems> inquiryitems { get; set; }

        public List<Conviction> convictions { get; set; }

        // new inquiry properties
        public int CustomerId { get; set; }
        public int Priority { get; set; }
        public string PriorityName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LicenseNumber { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Company { get; set; }
        public string Country { get; set; }
        public string CountryName { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public int AgreementId { get; set; }
        public int ReservationId { get; set; }
        public string AgreementNumber { get; set; }
        public string ReservationNumber { get; set; }
        public string Subject { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}",
        ApplyFormatInEditMode = true)]
        public DateTime? ReportedDate { get; set; }
        public DateTime? FollowUpDate { get; set; }
        public DateTime? InquiryDate { get; set; }
        public DateTime? FollowUpStartDate { get; set; }
        public DateTime? FollowUpEndDate { get; set; }
        public int Status { get; set; }
        public int Source { get; set; }
        public string StatusName { get; set; }
        public string InboxStatus { get; set; }
        public int AssignedTo { get; set; }
        public string AssignedName { get; set; }
        public string Description { get; set; }
        public string Comment { get; set; }
        public int MessageCount { get; set; }
        public bool IsRead { get; set; }
        public string InquiryDateStr { get; set; }
        public string FollowUpDateStr { get; set; }
        public InquiryChatModel inquiryChatModel { get; set; }
        public List<InquiryChatModel> inquiryChatModelList { get; set; }
        public List<InquiryNote> inquiryNoteList { get; set; }
        public List<CustomerList> custList { get; set; }
        public CustomerSerach customersearch { get; set; }
        public CustomerSeachResult FindResult { get; }
        public List<CustomerSeachResult> SearchList { get; set; }
        public CustomerList CustomerList { get; set; }
        public bool isCreateCustomer { get; set; }
        public DateTime? PickUpDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string PickUpDateStr { get; set; }
        public string ReturnDateStr { get; set; }
        public Decimal? QuotedPrice { get; set; }
        public string Location { get; set; }
        public string LocationName { get; set; }
        public int VehicleId { get; set; }
        public string VehicleLicenseNo { get; set; }

        public String chatAvotarView {
            get { return Subject[0].ToString().ToUpper(); }
            set
            {
               
            }
        }

        public string statusColor
        {
            get
            {
                if (Status == 1)
                {
                    return "2";
                }
                if (Status == 2)
                {
                    return "2";
                }
                else
                {
                    return "5";
                }
            }
            set
            {

            }
        }

        public String statusNameView
        {
            get
            {
                if (Status == 1)
                {
                    return "Online";
                }
                if (Status == 2)
                {
                    return "Online";
                }
                else
                {
                    return "Resolved";
                }
            }
            set
            {

            }
        }
    }
}
