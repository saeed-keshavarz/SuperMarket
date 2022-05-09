using SuperMarket.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Test.Tools.Invoices
{
    public static class InvoiceFactory
    {
        public static Invoice CreateInvoice(Stuff stuff)
        {
            return new Invoice
            {
                Title = "فاکتور " + stuff.Title,
                Date = new DateTime(1401, 02, 18),
                Quantity = 10,
                Price = 1000,
                StuffId = stuff.Id,
                Buyer = "کشاورز",
            };
        }
    }
}
