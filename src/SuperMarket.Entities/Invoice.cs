using System;

namespace SuperMarket.Entities
{
    public class Invoice
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Buyer { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public int StuffId { get; set; }
        public Stuff Stuff { get; set; }
    }
}
