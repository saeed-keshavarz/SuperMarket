using SuperMarket.Entities;
using SuperMarket.Services.Reports.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SuperMarket.Persistence.EF.Reports
{
    public class EFReportRepository : ReportRepository
    {
        private readonly EFDataContext _dataContext;

        public EFReportRepository(EFDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Stuff GetStuffById(int id)
        {
            return _dataContext.Stuffs.Find(id);
        }
        public Category GetCategoryById(int id)
        {
            return _dataContext.Categories.Find(id);
        }

        public List<Voucher> GetAllVoucherByDateRange(DateTime start,
            DateTime end)
        {
            return _dataContext.Vouchers.Where(_ =>
           _.Date >= start &&
           _.Date <= end
           ).ToList();
        }

        public List<Invoice> GetAllInvoiceByDateRange(DateTime start,
            DateTime end)
        {
            return _dataContext.Invoices.Where(_ =>
            _.Date >= start &&
            _.Date <= end
            ).ToList();
        }

        public List<Voucher> GetAllVoucherRelatedStuffByDateRange(int stuffId,
            DateTime start, DateTime end)
        {
            return _dataContext.Vouchers.Where(_ =>
            _.Date >= start &&
            _.Date <= end &&
            _.StuffId == stuffId
            ).ToList();
        }

        public List<Invoice> GetAllInvoiceRelatedStuffByDateRange(int stuffId,
            DateTime start, DateTime end)
        {
            return _dataContext.Invoices.Where(_ =>
            _.Date >= start &&
            _.Date <= end &&
            _.StuffId == stuffId
            ).ToList();
        }

        public List<Voucher> GetAllVoucherRelatedCategoryByDateRange(int categoryId,
            DateTime start, DateTime end)
        {
            return _dataContext.Vouchers.Where(_ =>
            _.Stuff.CategoryId == categoryId &&
            _.Date >= start &&
            _.Date <= end
             ).ToList();
        }

        public List<Invoice> GetAllInvoiceRelatedCategoryByDateRange(int categoryId,
            DateTime start, DateTime end)
        {
            return _dataContext.Invoices.Where(_ =>
            _.Stuff.CategoryId == categoryId &&
            _.Date >= start &&
            _.Date <= end
             ).ToList();
        }
    }
}
