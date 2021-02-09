using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxVonGrafKftMobileModel
{
    public class InquiryChatModel
    {
        public int MessageId { get; set; }
        public int InquiryID { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string Message { get; set; }
        public DateTime SendDate { get; set; }

        public int UserId { get; set; }

    }
}
