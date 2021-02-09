using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxVonGrafKftMobileModel
{
    public class Message
    {
        public int messageId { get; set; }
        public string Text { get; set; }
        public string User { get; set; }
        public string sentTime { get; set; }
    }
}