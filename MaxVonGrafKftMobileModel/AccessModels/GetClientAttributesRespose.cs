using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxVonGrafKftMobileModel.AccessModels
{
    public class GetClientAttributesRespose
    {
        public List<NameValueModel> nameValues { get; set; }
        public ApiMessage message { get; set; }
    }

    public class NameValueModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
