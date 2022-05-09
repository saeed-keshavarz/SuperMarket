using FluentAssertions;
using Supermarket.Test.Tools.Categories;
using Supermarket.Test.Tools.Stuffs;
using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Infrastructure.Test;
using SuperMarket.Persistence.EF;
using SuperMarket.Persistence.EF.Invoices;
using SuperMarket.Services.Invoices;
using SuperMarket.Services.Invoices.Contracts;
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
