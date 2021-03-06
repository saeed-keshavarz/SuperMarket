using FluentAssertions;
using Supermarket.Test.Tools.Categories;
using Supermarket.Test.Tools.Stuffs;
using Supermarket.Test.Tools.Vouchers;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Infrastructure.Test;
using SuperMarket.Persistence.EF;
using SuperMarket.Persistence.EF.Vouchers;
using SuperMarket.Services.Vouchers;
using SuperMarket.Services.Vouchers.Contracts;
using SuperMarket.Services.Vouchers.Exceptions;
using System;
using System.Linq;
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
            _dataContext = new EFInMemoryDatabase()
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

            AddVoucherDto dto = VoucherFactory.GenerateAddVoucherDto(stuff, "سند پنیر");

            _sut.Add(dto);

            _dataContext.Vouchers.Should()
                .Contain(_ =>
                _.Title == dto.Title &&
                _.Date == dto.Date &&
                _.Quantity == dto.Quantity &&
                _.Price == dto.Price);

            _dataContext.Stuffs.Should()
                .Contain(_ =>
                _.Inventory == 30);
        }

        [Fact]
        public void Add_throw_InventoryMoreThanMaximumInventoryInStuffException_when_add_new_voucher()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var stuff = StuffFactory.CreateStuff(category, "شیر");
            stuff.MaximumInventory = 10;
            _dataContext.Manipulate(_ => _.Stuffs.Add(stuff));

            AddVoucherDto dto = VoucherFactory.GenerateAddVoucherDto(stuff, "سند شیر");

            Action expected = () => _sut.Add(dto);

            expected.Should().ThrowExactly<InventoryMoreThanMaximumInventoryInStuffException>();

        }

        [Fact]
        public void GetAll_return_vouchers_by_id()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var stuff = StuffFactory.CreateStuff(category, "شیر");
            _dataContext.Manipulate(_ => _.Stuffs.Add(stuff));

            var voucher = VoucherFactory.CreateVoucher(stuff);
            _dataContext.Manipulate(_ => _.Vouchers.Add(voucher));

            var expected = _sut.GetById(voucher.Id);

            expected.Title.Should().Be("سند شیر");
            expected.Price.Should().Be(1000);
        }

        [Fact]
        public void GetAll_returns_all_vouchers()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var stuff = StuffFactory.CreateStuff(category, "شیر");
            _dataContext.Manipulate(_ => _.Stuffs.Add(stuff));

            var vouchers = VoucherFactory.CreateVouchersInDataBase(stuff.Id);
            _dataContext.Manipulate(_ => _.Vouchers.AddRange(vouchers));

            var expected = _sut.GetAllVouchers();

            expected.Should().HaveCount(3);
            expected.Should().Contain(_ =>
            _.Title == "سند شیر" &&
            _.Quantity == 10 &&
            _.Price == 1000 &&
            _.StuffId == stuff.Id);

            expected.Should().Contain(_ =>
            _.Title == "سند شیر" &&
            _.Quantity == 20 &&
            _.Price == 1000 &&
            _.StuffId == stuff.Id);

            expected.Should().Contain(_ =>
            _.Title == "سند شیر" &&
            _.Quantity == 30 &&
            _.Price == 1000 &&
            _.StuffId == stuff.Id);
        }

        [Fact]
        public void Update_update_voucher_properly()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var stuff = StuffFactory.CreateStuff(category, "شیر");
            _dataContext.Manipulate(_ => _.Stuffs.Add(stuff));

            var voucher = VoucherFactory.CreateVoucher(stuff);
            _dataContext.Manipulate(_ => _.Vouchers.Add(voucher));

            var dto = VoucherFactory.GenerateUpdateVoucherDto(stuff.Id, "سند شیر");

            _sut.Update(voucher.Id, dto);

            var expected = _dataContext.Vouchers
                .FirstOrDefault(_ => _.Id == voucher.Id);
            expected.Title.Should().Be(dto.Title);
            expected.Date.Should().Be(dto.Date);
            expected.Price.Should().Be(dto.Price);
            expected.Quantity.Should().Be(dto.Quantity);
            expected.Stuff.Inventory.Should().Be(30);
        }

        [Fact]
        public void Update_Throw_VoucherNotFoundException_when_voucher_with_id_is_not_exist()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var stuff = StuffFactory.CreateStuff(category, "شیر");
            _dataContext.Manipulate(_ => _.Stuffs.Add(stuff));

            var dummyVoucherId = 1000;
            var dto = VoucherFactory.GenerateUpdateVoucherDto(stuff.Id, "سند شیر");

            Action expected = () => _sut.Update(dummyVoucherId, dto);

            expected.Should().ThrowExactly<VoucherNotFoundException>();
        }

        [Fact]
        public void Delete_delete_voucher_properly()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var stuff = StuffFactory.CreateStuff(category, "شیر");
            _dataContext.Manipulate(_ => _.Stuffs.Add(stuff));

            var voucher = VoucherFactory.CreateVoucher(stuff);
            _dataContext.Manipulate(_ => _.Vouchers.Add(voucher));

            _sut.Delete(voucher.Id);

            var expected = _dataContext.Stuffs
                .FirstOrDefault(_ => _.Id == stuff.Id);

            expected.Inventory.Should().Be(10);

            _dataContext.Vouchers.Should()
                .NotContain(_ => _.Id == voucher.Id);
        }

        [Fact]
        public void Delete_throw_VoucherNotFoundException_when_voucher_with_id_is_not_exist()
        {
            var dummyVoucherId = 1000;

            Action expected = () => _sut.Delete(dummyVoucherId);

            expected.Should().ThrowExactly<VoucherNotFoundException>();
        }
    }
}
