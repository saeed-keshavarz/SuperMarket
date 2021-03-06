using FluentAssertions;
using Supermarket.Test.Tools.Categories;
using Supermarket.Test.Tools.Invoices;
using Supermarket.Test.Tools.Stuffs;
using Supermarket.Test.Tools.Vouchers;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Infrastructure.Test;
using SuperMarket.Persistence.EF;
using SuperMarket.Persistence.EF.Stuffs;
using SuperMarket.Services.Stuffs;
using SuperMarket.Services.Stuffs.Contracts;
using SuperMarket.Services.Stuffs.Exceptions;
using System;
using System.Linq;
using Xunit;

namespace SuperMarket.Services.Test.Unit.Stuffs
{
    public class StuffServiceTest
    {
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly StuffService _sut;
        private readonly StuffRepository _repository;

        public StuffServiceTest()
        {
            _dataContext =
               new EFInMemoryDatabase()
               .CreateDataContext<EFDataContext>();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFStuffRepository(_dataContext);
            _sut = new StuffAppService(_repository, _unitOfWork);
        }

        [Fact]
        public void Add_add_stuff_properly()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            AddStuffDto dto = StuffFactory.GenerateAddStuffDto(category, "شیر");

            _sut.Add(dto);

            _dataContext.Stuffs.Should()
                .Contain(_ =>
                _.Title == dto.Title &&
                _.Inventory == dto.Inventory &&
                _.Unit == dto.Unit &&
                _.MinimumInventory == dto.MinimumInventory &&
                _.MaximumInventory == dto.MaximumInventory);
        }

        [Fact]
        public void Add_throw_DuplicateStuffTitleInStuffException_when_add_new_stuff()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var stuff = StuffFactory.CreateStuff(category, "پنیر");
            _dataContext.Manipulate(_ => _.Stuffs.Add(stuff));

            AddStuffDto dto = StuffFactory.GenerateAddStuffDto(category, "پنیر");

            Action expected = () => _sut.Add(dto);

            expected.Should().Throw<DuplicateStuffTitleInCategoryException>();
        }

        [Fact]
        public void GetAll_returns_stuff_by_id()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var stuff = StuffFactory.CreateStuff(category, "شیر");
            _dataContext.Manipulate(_ => _.Stuffs.Add(stuff));

            var expected = _sut.GetById(stuff.Id);

            expected.Title.Should().Be("شیر");
            expected.Inventory.Should().Be(20);
            expected.MinimumInventory.Should().Be(20);
            expected.MaximumInventory.Should().Be(50);
            expected.Unit.Should().Be("پاکت");
        }

        [Fact]
        public void GetAll_returns_all_stuffs()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var stuffs = StuffFactory.CreateStuffsInDataBase(category.Id);
            _dataContext.Manipulate(_ => _.Stuffs.AddRange(stuffs));

            var expected = _sut.GetAllStuff();

            expected.Should().HaveCount(3);
            expected.Should().Contain(_ =>
            _.Title == "شیر" &&
            _.Inventory == 10 &&
            _.MinimumInventory == 5 &&
            _.MaximumInventory == 50);
            expected.Should().Contain(_ =>
            _.Title == "پنیر" &&
            _.Inventory == 20 &&
            _.MinimumInventory == 5 &&
            _.MaximumInventory == 50);
            expected.Should().Contain(_ =>
            _.Title == "ماست" &&
            _.Inventory == 30 &&
            _.MinimumInventory == 5 &&
            _.MaximumInventory == 50);
        }

        [Fact]
        public void Update_update_stuff_properly()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var stuff = StuffFactory.CreateStuff(category, "شیر");
            _dataContext.Manipulate(_ => _.Stuffs.Add(stuff));

            var dto = StuffFactory.GenerateUpdateStuffDto(category.Id, "پنیر");

            _sut.Update(stuff.Id, dto);

            var expected = _dataContext.Stuffs
                .FirstOrDefault(_ => _.Id == stuff.Id);

            expected.Title.Should().Be(dto.Title);
        }

        [Fact]
        public void Update_throw_DuplicateStuffTitleInStuffException_when_update_stuff()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var stuff1 = StuffFactory.CreateStuff(category, "شیر");
            _dataContext.Manipulate(_ => _.Stuffs.Add(stuff1));

            var stuff2 = StuffFactory.CreateStuff(category, "پنیر");
            _dataContext.Manipulate(_ => _.Stuffs.Add(stuff2));

            var dto = StuffFactory.GenerateUpdateStuffDto(category.Id, "پنیر");

            Action expected = () => _sut.Update(stuff1.Id, dto);

            expected.Should().Throw<DuplicateStuffTitleInCategoryException>();
        }

        [Fact]
        public void Update_Throw_StuffNotFoundException_when_stuff_with_id_is_not_exist()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var dummyStuffId = 1000;
            var dto = StuffFactory.GenerateUpdateStuffDto(category.Id, "پنیر");

            Action expected = () => _sut.Update(dummyStuffId, dto);

            expected.Should().ThrowExactly<StuffNotFoundException>();
        }

        [Fact]
        public void Delete_delete_stuff_properly()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var stuff = StuffFactory.CreateStuff(category, "شیر");
            _dataContext.Manipulate(_ => _.Stuffs.Add(stuff));

            _sut.Delete(category.Id);

            _dataContext.Stuffs.Should().
                NotContain(_ => _.Id == stuff.Id);
        }

        [Fact]
        public void Delete_throw_StuffNotFoundException_when_stuff_with_id_is_not_exist()
        {
            var dummyStuffId = 1000;

            Action expected = () => _sut.Delete(dummyStuffId);

            expected.Should().ThrowExactly<StuffNotFoundException>();
        }

        [Fact]
        public void Delete_throw_CanNotDeleteStuffHasVoucherException_when_delete_stuff_has_voucher()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var stuff = StuffFactory.CreateStuff(category, "شیر");
            _dataContext.Manipulate(_ => _.Stuffs.Add(stuff));

            var voucher = VoucherFactory.CreateVoucher(stuff);
            _dataContext.Manipulate(_ => _.Vouchers.Add(voucher));

            Action expected = () => _sut.Delete(stuff.Id);

            expected.Should().ThrowExactly<CanNotDeleteStuffHasVoucherException>();
        }

        [Fact]
        public void Delete_throw_CanNotDeleteStuffHasInvoiceException_when_delete_stuff_has_invoice()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var stuff = StuffFactory.CreateStuff(category, "شیر");
            _dataContext.Manipulate(_ => _.Stuffs.Add(stuff));

            var invoice = InvoiceFactory.CreateInvoice(stuff);
            _dataContext.Manipulate(_ => _.Invoices.Add(invoice));

            Action expected = () => _sut.Delete(stuff.Id);

            expected.Should().ThrowExactly<CanNotDeleteStuffHasInvoiceException>();
        }
    }
}
