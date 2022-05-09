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
