using System.Collections.Generic;

namespace SuperMarket.Entities
{
    public class Stuff
    {
        public Stuff()
        {
            Invoces = new HashSet<Invoice>();
            Vouchers = new HashSet<Voucher>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public int Inventory { get; set; }
        public string Unit { get; set; }
        public int MinimumInventory { get; set; }
        public int MaximumInventory { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public HashSet<Voucher> Vouchers { get; set; }
        public HashSet<Invoice> Invoces { get; set; }
    }
}
