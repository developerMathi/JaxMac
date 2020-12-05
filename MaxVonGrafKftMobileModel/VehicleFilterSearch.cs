using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxVonGrafKftMobileModel
{
    public class VehicleFilterSearch
    {
        public decimal Price { get; set; }
        public decimal MinPrice { get; set; }
        public string VehicleType { get; set; }
        public int seatsCount { get; set; }
        public int buggageCount { get; set; }
        public int DoorsCount { get; set; }
        public int SortingOrder { get; set; }
    }
}
