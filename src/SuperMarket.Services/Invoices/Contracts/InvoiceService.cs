using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.Invoices.Contracts
{
    public interface InvoiceService : Service
    {
        void Add(AddInvoiceDto dto, int stuffId);
        void Update(int id, UpdateInvoiceDto dto, int stuffId, int quantity);
        void Delete(int id, int stuffId, int quantity);
        IList<Invoice> GetAllInvoices();
    }
}
