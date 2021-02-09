using MaxVonGrafKftMobileModel.AccessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxVonGrafKftMobileModel
{
    public class CustomerList
    {
        //public Customer Customer { get; set; }
        public CustomerSerach Customer { get; set; }

        public List<CustomerSeachResult> List { get; set; }
        public List<ColumnListViewModel> columlist { get; set; }

        public int CustomerId { get; set; }
    }
}
