using SuperMarket.Entities;
using SuperMarket.Services.Invoices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Persistence.EF.Invoices
{
    public class EFInvoiceRepository :InvoiceRepository
    {
        private readonly EFDataContext _dataContext;

        public EFInvoiceRepository(EFDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Add(Invoice invoice)
        {
            _dataContext.Invoices.Add(invoice);
        }

        public Stuff GetStuffById(int stuffId)
        {
            return _dataContext.Stuffs.Find(stuffId);
        }
    }
}
