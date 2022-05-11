using System;

namespace SuperMarket.Services.Invoices.Contracts
{
    public class AddInvoiceDto
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Buyer { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int StuffId { get; set; }
    }
}
