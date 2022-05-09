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

            AddInvoiceDto dto = GenerateAddInvoiceDto(stuff, "فاکتور پنیر");

            _sut.Add(dto, stuff.Id);

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
        public void GetAll_returns_all_invoices()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var stuff = StuffFactory.CreateStuff(category, "پنیر");
            _dataContext.Manipulate(_ => _.Stuffs.Add(stuff));

            var invoices = CreateInvoicesInDataBase(stuff.Id);
            _dataContext.Manipulate(_ => _.Invoices.AddRange(invoices));

            var expected = _sut.GetAllInvoices();

            expected.Should().HaveCount(3);
            expected.Should().Contain(_ => _.Title == "فاکتور شیر" && _.Quantity == 10 && _.Price == 1000 && _.StuffId == stuff.Id);
            expected.Should().Contain(_ => _.Title == "فاکتور ماست" && _.Quantity == 20 && _.Price == 2000 && _.StuffId == stuff.Id);
            expected.Should().Contain(_ => _.Title == "فاکتور پنیر" && _.Quantity == 30 && _.Price == 3000 && _.StuffId == stuff.Id);
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

            var dto = GenerateUpdateInvoiceDto(stuff.Id, "فاکتور شیر");

            _sut.Update(invoice.Id, dto, stuff.Id, invoice.Quantity);

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
            var dto = GenerateUpdateInvoiceDto(stuff.Id, "سند شیر");

            Action expected = () => _sut.Update(dummyInvoiceId, dto, stuff.Id, dummyQuantity);

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

            _sut.Delete(invoice.Id, stuff.Id, invoice.Quantity);

            var expected = _dataContext.Stuffs
                .FirstOrDefault(_ => _.Id == stuff.Id);

            expected.Inventory.Should().Be(30);

            _dataContext.Invoices.Should()
                .NotContain(_ => _.Id == invoice.Id);
        }


        private UpdateInvoiceDto GenerateUpdateInvoiceDto(int stuffId, string title)
        {
            return new UpdateInvoiceDto
            {
                Title = title,
                Date = new DateTime(1401, 02, 20),
                Price = 2000,
                Quantity = 20,
                Buyer = "کشاورز",
                StuffId = stuffId,
            };
        }

        private List<Invoice> CreateInvoicesInDataBase(int stuffId)
        {
            return new List<Invoice>
            {
                new Invoice {Title="فاکتور شیر", Date =new DateTime(1401, 02, 18), Quantity=10,StuffId=stuffId,Price=1000 },
                new Invoice {Title="فاکتور ماست", Date =new DateTime(1401, 02, 19), Quantity=20,StuffId=stuffId,Price=2000 },
                new Invoice {Title="فاکتور پنیر", Date =new DateTime(1401, 02, 20), Quantity=30,StuffId=stuffId,Price=3000 },
            };
        }

        private AddInvoiceDto GenerateAddInvoiceDto(Stuff stuff, string title)
        {
            return new AddInvoiceDto
            {
                Title = title,
                Date = new DateTime(1401, 02, 18),
                Quantity = 10,
                Price = 1000,
                Buyer="کشاورز",
                StuffId = stuff.Id,
            };
        }
    }
    
}
