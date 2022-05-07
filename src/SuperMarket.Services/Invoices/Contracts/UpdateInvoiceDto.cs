using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.Invoices.Contracts
{
    public class UpdateInvoiceDto
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Buyer { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int StuffId { get; set; }
    }
}
