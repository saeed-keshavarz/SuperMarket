using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.Reports.Contracts
{
    public class GetProfitByStuffDto
    {
        public string Title { get; set; }
        public decimal Cost { get; set; }
        public decimal Income { get; set; }
        public decimal Profit { get; set; }
    }
}
