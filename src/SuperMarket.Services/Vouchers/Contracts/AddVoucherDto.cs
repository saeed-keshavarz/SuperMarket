using System;

namespace SuperMarket.Services.Vouchers.Contracts
{
    public class AddVoucherDto
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int StuffId { get; set; }
    }
}
