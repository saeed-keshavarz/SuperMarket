using FluentAssertions;
using Supermarket.Test.Tools.Categories;
using Supermarket.Test.Tools.Stuffs;
using Supermarket.Test.Tools.Vouchers;
using SuperMarket.Entities;
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

            _dataContext.Stuffs.Should()
                .Contain(_ =>
                _.Inventory == 30);
        }

        [Fact]
        public void GetAll_returns_all_voucher()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var stuff = StuffFactory.CreateStuff(category, "پنیر");
            _dataContext.Manipulate(_ => _.Stuffs.Add(stuff));

            var vouchers = CreateVouchersInDataBase(stuff.Id);
            _dataContext.Manipulate(_ => _.Vouchers.AddRange(vouchers));

            var expected = _sut.GetAllVouchers();

            expected.Should().HaveCount(3);
            expected.Should().Contain(_ => _.Title == "سند شیر" && _.Quantity == 10 && _.Price == 1000 && _.StuffId == stuff.Id);
            expected.Should().Contain(_ => _.Title == "سند ماست" && _.Quantity == 20 && _.Price == 2000 && _.StuffId == stuff.Id);
            expected.Should().Contain(_ => _.Title == "سند پنیر" && _.Quantity == 30 && _.Price == 3000 && _.StuffId == stuff.Id);
        }

        [Fact]
        public void Update_update_stuff_properly()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var stuff = StuffFactory.CreateStuff(category, "شیر");
            _dataContext.Manipulate(_ => _.Stuffs.Add(stuff));

            var voucher = VoucherFactory.CreateVoucher(stuff);
            _dataContext.Manipulate(_ => _.Vouchers.Add(voucher));

            var dto = GenerateUpdateVoucherDto(stuff.Id, "سند شیر");

            _sut.Update(voucher.Id, dto, stuff.Id, voucher.Quantity);

            var expected = _dataContext.Vouchers
                .FirstOrDefault(_=>_.Id==voucher.Id);
            expected.Title.Should().Be(dto.Title);
            expected.Date.Should().Be(dto.Date);
            expected.Price.Should().Be(dto.Price);
            expected.Quantity.Should().Be(dto.Quantity);

            _dataContext.Stuffs.Should()
                .Contain(_ =>
                _.Inventory == 30);
        }



        private UpdateVoucherDto GenerateUpdateVoucherDto(int stuffId, string title)
        {
            return new UpdateVoucherDto
            {
                Title =  title,
                Date = new DateTime(1401, 02, 20),
                Price = 2000,
                Quantity = 20,
                StuffId = stuffId,
            };
        }

        private List<Voucher> CreateVouchersInDataBase(int stuffId)
        {
            return new List<Voucher>
            {
                new Voucher {Title="سند شیر", Date =new DateTime(1401, 02, 18), Quantity=10,StuffId=stuffId,Price=1000 },
                new Voucher {Title="سند ماست", Date =new DateTime(1401, 02, 19), Quantity=20,StuffId=stuffId,Price=2000 },
                new Voucher {Title="سند پنیر", Date =new DateTime(1401, 02, 20), Quantity=30,StuffId=stuffId,Price=3000 },
            };
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
