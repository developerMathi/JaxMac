using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxVonGrafKftMobileModel.AccessModels
{
    public class ExtendAgreementRequest
    {
        public int agreementId { get; set; }
        public string extendDate { get; set; }

        public PromotionItem addPromotion { get; set; }
        public PromotionItem deletePromotion { get; set; }
    }
}
