using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using System.Collections.Generic;

namespace SuperMarket.Services.Invoices.Contracts
{
    public interface InvoiceService : Service
    {
        void Add(AddInvoiceDto dto);
        Invoice GetById(int id);
        IList<Invoice> GetAllInvoices();
        void Update(int id, UpdateInvoiceDto dto);
        void Delete(int id);
    }
}
