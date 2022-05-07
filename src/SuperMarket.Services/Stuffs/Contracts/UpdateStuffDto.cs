using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.Stuffs.Contracts
{
    public class UpdateStuffDto
    {
        public string Title { get; set; }
        public string Unit { get; set; }
        public int MinimumInventory { get; set; }
        public int MaximumInventory { get; set; }
        public int CategoryId { get; set; }
    }
}
