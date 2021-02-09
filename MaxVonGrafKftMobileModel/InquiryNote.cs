using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxVonGrafKftMobileModel
{
    public class InquiryNote
    {
        public int InquiryId { get; set; }
        public int? NoteId { get; set; }
        public int NoteNumber { get; set; }
        public string Note { get; set; }

        public int? CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedDateStr { get; set; }

        public int? UpdatedBy { get; set; }
        public string UpdatedByName { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedDateStr { get; set; }

        public DateTime? FollowUpDate { get; set; }
    }
}
