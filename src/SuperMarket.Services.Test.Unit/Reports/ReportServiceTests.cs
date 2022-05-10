using FluentAssertions;
using Supermarket.Test.Tools.Categories;
using Supermarket.Test.Tools.Invoices;
using Supermarket.Test.Tools.Stuffs;
using Supermarket.Test.Tools.Vouchers;
using SuperMarket.Infrastructure.Test;
using SuperMarket.Persistence.EF;
using SuperMarket.Persistence.EF.Reports;
using SuperMarket.Services.Reports;
using SuperMarket.Services.Reports.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SuperMarket.Services.Test.Unit.Reports
{
    public class ReportServiceTests
    {
        private readonly EFDataContext _dataContext;
        private readonly ReportService _sut;
        private readonly ReportRepository _repository;

        public ReportServiceTests()
        {
            _dataContext =
                           new EFInMemoryDatabase()
                           .CreateDataContext<EFDataContext>();
            _repository = new EFReportRepository(_dataContext);
            _sut = new ReportAppService(_repository);
        }

        [Fact]
        public void Get_get_profit_by_stuff()
        {
            DateTime start = new DateTime(1401, 02, 18);
            DateTime end = new DateTime(1401, 02, 20);

            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var stuff = StuffFactory.CreateStuff(category, "شیر");
            _dataContext.Manipulate(_ => _.Stuffs.Add(stuff));

            var vouchers = VoucherFactory.CreateVouchersInDataBase(stuff.Id);
            _dataContext.Manipulate(_ => _.Vouchers.AddRange(vouchers));

            var invoices = InvoiceFactory.CreateInvoicesInDataBase(stuff.Id);
            _dataContext.Manipulate(_ => _.Invoices.AddRange(invoices));

            var expected = _sut.GetProfitByStuff(stuff.Id, start, end);

            expected.Title.Should().Be(stuff.Title);
            expected.Cost.Should().Be(60000);
            expected.Income.Should().Be(120000);
            expected.Profit.Should().Be(60000);
        }

        [Fact]
        public void Get_get_profit_by_category()
        {
            DateTime start = new DateTime(1401, 02, 18);
            DateTime end = new DateTime(1401, 02, 20);

            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var stuff = StuffFactory.CreateStuff(category, "شیر");
            _dataContext.Manipulate(_ => _.Stuffs.Add(stuff));

            var vouchers = VoucherFactory.CreateVouchersInDataBase(stuff.Id);
            _dataContext.Manipulate(_ => _.Vouchers.AddRange(vouchers));

            var invoices = InvoiceFactory.CreateInvoicesInDataBase(stuff.Id);
            _dataContext.Manipulate(_ => _.Invoices.AddRange(invoices));

            var expected = _sut.GetProfitByCategory(category.Id, start, end);

            expected.Title.Should().Be(category.Title);
            expected.Cost.Should().Be(60000);
            expected.Income.Should().Be(120000);
            expected.Profit.Should().Be(60000);
        }

        [Fact]
        public void Get_total_profit()
        {
            DateTime start = new DateTime(1401, 02, 18);
            DateTime end = new DateTime(1401, 02, 20);

            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var stuff = StuffFactory.CreateStuff(category, "شیر");
            _dataContext.Manipulate(_ => _.Stuffs.Add(stuff));

            var vouchers = VoucherFactory.CreateVouchersInDataBase(stuff.Id);
            _dataContext.Manipulate(_ => _.Vouchers.AddRange(vouchers));

            var invoices = InvoiceFactory.CreateInvoicesInDataBase(stuff.Id);
            _dataContext.Manipulate(_ => _.Invoices.AddRange(invoices));

            var expected = _sut.GetTotalProfit(start, end);

            expected.Cost.Should().Be(60000);
            expected.Income.Should().Be(120000);
            expected.Profit.Should().Be(60000);
        }
    }
}