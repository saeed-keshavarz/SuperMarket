using FluentAssertions;
using Supermarket.Test.Tools.Categories;
using Supermarket.Test.Tools.Invoices;
using Supermarket.Test.Tools.Stuffs;
using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Infrastructure.Test;
using SuperMarket.Persistence.EF;
using SuperMarket.Persistence.EF.Invoices;
using SuperMarket.Services.Invoices;
using SuperMarket.Services.Invoices.Contracts;
using SuperMarket.Services.Invoices.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SuperMarket.Services.Test.Unit.Invoices
{
    public class InvoiceServiceTest
    {
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly InvoiceService _sut;
        private readonly InvoiceRepository _repository;

        public InvoiceServiceTest()
        {
            _dataContext = new EFInMemoryDatabase()
                                      .CreateDataContext<EFDataContext>();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFInvoiceRepository(_dataContext);
            _sut = new InvoiceAppService(_repository, _unitOfWork);
        }

        [Fact]
        public void Add_add_invoice_properly()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var stuff = StuffFactory.CreateStuff(category, "پنیر");
            _dataContext.Manipulate(_ => _.Stuffs.Add(stuff));

            AddInvoiceDto dto = InvoiceFactory.GenerateAddInvoiceDto(stuff, "فاکتور پنیر");

            _sut.Add(dto);

            _dataContext.Invoices.Should()
                .Contain(_ =>
                _.Title == dto.Title &&
                _.Date == dto.Date &&
                _.Quantity == dto.Quantity &&
                _.Buyer == dto.Buyer &&
                _.Price == dto.Price);

            _dataContext.Stuffs.Should()
                .Contain(_ =>
                _.Inventory == 10);
        }

        [Fact]
        public void GetAll_return_invoice_by_id()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var stuff = StuffFactory.CreateStuff(category, "شیر");
            _dataContext.Manipulate(_ => _.Stuffs.Add(stuff));

            var invoice = InvoiceFactory.CreateInvoice(stuff);
            _dataContext.Manipulate(_ => _.Invoices.Add(invoice));

            var expected = _sut.GetById(invoice.Id);

            expected.Title.Should().Be("فاکتور شیر");
            expected.Price.Should().Be(1000);
        }

            [Fact]
        public void GetAll_returns_all_invoices()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var stuff = StuffFactory.CreateStuff(category, "شیر");
            _dataContext.Manipulate(_ => _.Stuffs.Add(stuff));

            var invoices = InvoiceFactory.CreateInvoicesInDataBase(stuff.Id);
            _dataContext.Manipulate(_ => _.Invoices.AddRange(invoices));

            var expected = _sut.GetAllInvoices();

            expected.Should().HaveCount(3);
            expected.Should().Contain(_ => _.Title == "فاکتور شیر" && _.Quantity == 10 && _.Price == 2000 && _.StuffId == stuff.Id);
            expected.Should().Contain(_ => _.Title == "فاکتور شیر" && _.Quantity == 20 && _.Price == 2000 && _.StuffId == stuff.Id);
            expected.Should().Contain(_ => _.Title == "فاکتور شیر" && _.Quantity == 30 && _.Price == 2000 && _.StuffId == stuff.Id);
        }

        [Fact]
        public void Update_update_invoice_properly()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var stuff = StuffFactory.CreateStuff(category, "شیر");
            _dataContext.Manipulate(_ => _.Stuffs.Add(stuff));

            var invoice = InvoiceFactory.CreateInvoice(stuff);
            _dataContext.Manipulate(_ => _.Invoices.Add(invoice));

            var dto = InvoiceFactory.GenerateUpdateInvoiceDto(stuff.Id, "فاکتور شیر");

            _sut.Update(invoice.Id, dto);

            var expected = _dataContext.Invoices
                .FirstOrDefault(_ => _.Id == invoice.Id);
            expected.Title.Should().Be(dto.Title);
            expected.Date.Should().Be(dto.Date);
            expected.Price.Should().Be(dto.Price);
            expected.Quantity.Should().Be(dto.Quantity);
            expected.Stuff.Inventory.Should().Be(10);
        }

        [Fact]
        public void Update_Throw_InvoiceNotFoundException_when_invoice_with_id_is_not_exist()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var stuff = StuffFactory.CreateStuff(category, "شیر");
            _dataContext.Manipulate(_ => _.Stuffs.Add(stuff));

            var dummyInvoiceId = 1000;
            var dummyQuantity = 10;
            var dto = InvoiceFactory.GenerateUpdateInvoiceDto(stuff.Id, "سند شیر");

            Action expected = () => _sut.Update(dummyInvoiceId, dto);

            expected.Should().ThrowExactly<InvoiceNotFoundException>();
        }

        [Fact]
        public void Delete_delete_invoice_properly()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var stuff = StuffFactory.CreateStuff(category, "شیر");
            _dataContext.Manipulate(_ => _.Stuffs.Add(stuff));

            var invoice = InvoiceFactory.CreateInvoice(stuff);
            _dataContext.Manipulate(_ => _.Invoices.Add(invoice));

            _sut.Delete(invoice.Id);

            var expected = _dataContext.Stuffs
                .FirstOrDefault(_ => _.Id == stuff.Id);

            expected.Inventory.Should().Be(30);

            _dataContext.Invoices.Should()
                .NotContain(_ => _.Id == invoice.Id);
        }

        [Fact]
        public void Delete_throw_InvoiceNotFoundException_when_invoice_with_id_is_not_exist()
        {
            var stuffId = 10;
            var quantity = 10;
            var dummyInvoiceId = 1000;

            Action expected = () => _sut.Delete(dummyInvoiceId);

            expected.Should().ThrowExactly<InvoiceNotFoundException>();
        } 
    }
    
}
