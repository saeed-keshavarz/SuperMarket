using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.Invoices.Contracts
{
    public interface InvoiceRepository : Repository
    {
        void Add(Invoice invoice);
        Stuff GetStuffById(int stuffId);
        Invoice FindById(int id);
        IList<Invoice> GetAllInvoices();
    }
}
