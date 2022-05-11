using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using System;
using System.Collections.Generic;

namespace SuperMarket.Services.Reports.Contracts
{
    public interface ReportRepository : Repository
    {
        Stuff GetStuffById(int id);
        Category GetCategoryById(int id);
        List<Voucher> GetAllVoucherByDateRange(DateTime start, DateTime end);
        List<Invoice> GetAllInvoiceByDateRange(DateTime start, DateTime end);
        List<Voucher> GetAllVoucherRelatedStuffByDateRange(int stuffId, DateTime start, DateTime end);
        List<Invoice> GetAllInvoiceRelatedStuffByDateRange(int stuffId, DateTime start, DateTime end);
        List<Voucher> GetAllVoucherRelatedCategoryByDateRange(int categoryId, DateTime start, DateTime end);
        List<Invoice> GetAllInvoiceRelatedCategoryByDateRange(int categoryId, DateTime start, DateTime end);
    }
}
