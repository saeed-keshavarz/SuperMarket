using SuperMarket.Services.Reports.Contracts;
using System;

namespace SuperMarket.Services.Reports
{
    public class ReportAppService : ReportService
    {
        private readonly ReportRepository _repository;

        public ReportAppService(ReportRepository repository)
        {
            _repository = repository;
        }

        public GetProfitByCategoryDto GetProfitByCategory(int categpryId,
            DateTime start, DateTime end)
        {
            decimal cost = 0;
            decimal income = 0;

            var category = _repository.GetCategoryById(categpryId);

            var vouchers = _repository
                .GetAllVoucherRelatedCategoryByDateRange(categpryId, start, end);
            var invoices = _repository
                .GetAllInvoiceRelatedCategoryByDateRange(categpryId, start, end);

            foreach (var item in vouchers)
                cost += item.Quantity * item.Price;

            foreach (var item in invoices)
                income += (item.Quantity * item.Price);

            return new GetProfitByCategoryDto
            {
                Title = category.Title,
                Cost = cost,
                Income = income,
                Profit = income - cost,
            };
        }

        public GetProfitByStuffDto GetProfitByStuff(int stuffId,
            DateTime start, DateTime end)
        {
            decimal cost = 0;
            decimal income = 0;

            var stuff = _repository.GetStuffById(stuffId);

            var vouchers = _repository
                .GetAllVoucherRelatedStuffByDateRange(stuffId, start, end);
            var invoices = _repository
                .GetAllInvoiceRelatedStuffByDateRange(stuffId, start, end);

            foreach (var item in vouchers)
                cost += item.Quantity * item.Price;

            foreach (var item in invoices)
                income += (item.Quantity * item.Price);

            return new GetProfitByStuffDto
            {
                Title = stuff.Title,
                Cost = cost,
                Income = income,
                Profit = income - cost,
            };
        }

        public GetTotalProfitDto GetTotalProfit(DateTime start, DateTime end)
        {
            decimal cost = 0;
            decimal income = 0;

            var vouchers = _repository.GetAllVoucherByDateRange(start, end);
            var invoices = _repository.GetAllInvoiceByDateRange(start, end);

            foreach (var item in vouchers)
                cost += item.Quantity * item.Price;

            foreach (var item in invoices)
                income += (item.Quantity * item.Price);

            return new GetTotalProfitDto
            {
                Cost = cost,
                Income = income,
                Profit = income - cost,
            };
        }
    }
}
