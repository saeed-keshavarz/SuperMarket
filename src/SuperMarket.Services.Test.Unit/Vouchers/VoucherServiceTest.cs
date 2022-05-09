using FluentAssertions;
using Supermarket.Test.Tools.Categories;
using Supermarket.Test.Tools.Stuffs;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Infrastructure.Test;
using SuperMarket.Persistence.EF;
using SuperMarket.Persistence.EF.Vouchers;
using SuperMarket.Services.Vouchers;
using SuperMarket.Services.Vouchers.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SuperMarket.Services.Test.Unit.Vouchers
{
    public class VoucherServiceTest
    {
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly VoucherService _sut;
        private readonly VoucherRepository _repository;
        public VoucherServiceTest()
        {
            _dataContext =
                           new EFInMemoryDatabase()
                           .CreateDataContext<EFDataContext>();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFVoucherRepository(_dataContext);
            _sut = new VoucherAppService(_repository, _unitOfWork);
        }

        [Fact]
        public void Add_add_voucher_properly()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var stuff = StuffFactory.CreateStuff(category, "پنیر");
            _dataContext.Manipulate(_ => _.Stuffs.Add(stuff));

            AddVoucherDto dto = GenerateAddVoucherDto(stuff, stuff.Title);

            _sut.Add(dto,stuff.Id);

            _dataContext.Vouchers.Should()
                .Contain(_ =>
                _.Title == dto.Title &&
                _.Date==dto.Date &&
                _.Quantity==dto.Quantity &&
                _.Price==dto.Price);
        }

        private AddVoucherDto GenerateAddVoucherDto(Entities.Stuff stuff, string title)
        {
            return new AddVoucherDto
            {
                Title = "سند: " + stuff.Title,
                Date = new DateTime(1401, 02, 18),
                Quantity = 10,
                Price = 1000,
                StuffId = stuff.Id,
            };
        }
    }
}
