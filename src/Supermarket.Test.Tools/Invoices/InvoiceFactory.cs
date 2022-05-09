using SuperMarket.Entities;
using SuperMarket.Services.Invoices.Contracts;
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

        public static AddInvoiceDto GenerateAddInvoiceDto(Stuff stuff, string title)
        {
            return new AddInvoiceDto
            {
                Title = title,
                Date = new DateTime(1401, 02, 18),
                Quantity = 10,
                Price = 1000,
                Buyer = "کشاورز",
                StuffId = stuff.Id,
            };
        }

        public static UpdateInvoiceDto GenerateUpdateInvoiceDto(int stuffId, string title)
        {
            return new UpdateInvoiceDto
            {
                Title = title,
                Date = new DateTime(1401, 02, 20),
                Price = 2000,
                Quantity = 20,
                Buyer = "کشاورز",
                StuffId = stuffId,
            };
        }

        public static List<Invoice> CreateInvoicesInDataBase(int stuffId)
        {
            return new List<Invoice>
            {
                new Invoice {Title="فاکتور شیر", Date =new DateTime(1401, 02, 18), Quantity=10,StuffId=stuffId,Price=1000 },
                new Invoice {Title="فاکتور ماست", Date =new DateTime(1401, 02, 19), Quantity=20,StuffId=stuffId,Price=2000 },
                new Invoice {Title="فاکتور پنیر", Date =new DateTime(1401, 02, 20), Quantity=30,StuffId=stuffId,Price=3000 },
            };
        }
    }
}
