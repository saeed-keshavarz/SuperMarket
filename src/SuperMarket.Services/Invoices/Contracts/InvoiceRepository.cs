using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using System.Collections.Generic;

namespace SuperMarket.Services.Invoices.Contracts
{
    public interface InvoiceRepository : Repository
    {
        void Add(Invoice invoice);
        Stuff GetStuffById(int stuffId);
        Invoice FindById(int id);
        IList<Invoice> GetAllInvoices();
        void Delete(Invoice invoice);
    }
}
