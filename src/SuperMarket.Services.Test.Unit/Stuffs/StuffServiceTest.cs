using FluentAssertions;
using Supermarket.Test.Tools.Categories;
using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Infrastructure.Test;
using SuperMarket.Persistence.EF;
using SuperMarket.Persistence.EF.Stuffs;
using SuperMarket.Services.Stuffs;
using SuperMarket.Services.Stuffs.Contracts;
using SuperMarket.Services.Stuffs.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            AddStuffDto dto = GenerateAddStuffDto(category, "شیر");

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

            var stuff = CreateStuff(category, "پنیر");
            _dataContext.Manipulate(_ => _.Stuffs.Add(stuff));

            AddStuffDto dto = GenerateAddStuffDto(category, "پنیر");

            Action expected = () => _sut.Add(dto);

            expected.Should().Throw<DuplicateStuffTitleInCategoryException>();
        }

        [Fact]
        public void GetAll_returns_all_categories()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            CreateStuffsInDataBase(category.Id);

            var expected = _sut.GetAllStuff();

            expected.Should().HaveCount(3);
            expected.Should().Contain(_ => _.Title == "شیر" && _.Inventory == 10 && _.MinimumInventory == 5 && _.MaximumInventory == 50);
            expected.Should().Contain(_ => _.Title == "پنیر" && _.Inventory == 20 && _.MinimumInventory == 5 && _.MaximumInventory == 50);
            expected.Should().Contain(_ => _.Title == "ماست" && _.Inventory == 30 && _.MinimumInventory == 5 && _.MaximumInventory == 50);
        }

        [Fact]
        public void Update_update_stuff_properly()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var stuff = CreateStuff(category, "شیر");
            _dataContext.Manipulate(_ => _.Stuffs.Add(stuff));

            var dto = GenerateUpdateStuffDto(category.Id, "پنیر");

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

            var stuff1 = CreateStuff(category, "شیر");
            _dataContext.Manipulate(_ => _.Stuffs.Add(stuff1));

            var stuff2 = CreateStuff(category, "پنیر");
            _dataContext.Manipulate(_ => _.Stuffs.Add(stuff2));

            var dto = GenerateUpdateStuffDto(category.Id, "پنیر");

            Action expected = () => _sut.Update(stuff1.Id, dto);

            expected.Should().Throw<DuplicateStuffTitleInCategoryException>();

        }

        [Fact]
        public void Update_Throw_StuffNotFoundException_when_stuff_with_id_is_not_exist()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var dummyStuffId = 1000;
            var dto = GenerateUpdateStuffDto(category.Id, "پنیر");

            Action expected = () => _sut.Update(dummyStuffId, dto);

            expected.Should().ThrowExactly<StuffNotFoundException>();
        }

        [Fact]
        public void Delete_delete_stuff_properly()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var stuff = CreateStuff(category, "شیر");
            _dataContext.Manipulate(_ => _.Stuffs.Add(stuff));

            _sut.Delete(category.Id);

            _dataContext.Stuffs.Should().
                NotContain(_ => _.Id == stuff.Id);
        }

        [Fact]
        public void Delete_throw_StuffNotFoundException_when_stuff_with_id_is_not_exist()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var dummyStuffId = 1000;
            var dto = GenerateUpdateStuffDto(category.Id, "پنیر");

            Action expected = () => _sut.Delete(dummyStuffId);

            expected.Should().ThrowExactly<StuffNotFoundException>();
        }

        [Fact]
        public void Delete_throw_CanNotDeleteStuffHasVoucherException_when_delete_stuff_has_voucher()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var stuff = CreateStuff(category, "شیر");
            _dataContext.Manipulate(_ => _.Stuffs.Add(stuff));

            var voucher = CreateVoucher(stuff);
            _dataContext.Manipulate(_ => _.Vouchers.Add(voucher));

            Action expected = () => _sut.Delete(stuff.Id);

            expected.Should().ThrowExactly<CanNotDeleteStuffHasVoucherException>();
        }

        [Fact]
        public void Delete_throw_CanNotDeleteStuffHasInvoiceException_when_delete_stuff_has_invoice()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var stuff = CreateStuff(category, "شیر");
            _dataContext.Manipulate(_ => _.Stuffs.Add(stuff));

            var invoice = CreateInvoice(stuff);
            _dataContext.Manipulate(_ => _.Invoices.Add(invoice));

            Action expected = () => _sut.Delete(stuff.Id);

            expected.Should().ThrowExactly<CanNotDeleteStuffHasInvoiceException>();

        }

        private static Invoice CreateInvoice(Stuff stuff)
        {
            return new Invoice
            {
                Title = "فاکتور: " + stuff.Title,
                Date = new DateTime(1401, 02, 18),
                Quantity = 10,
                Price = 1000,
                StuffId = stuff.Id,
                Buyer = "کشاورز",
            };
        }

        private static Voucher CreateVoucher(Stuff stuff)
        {
            return new Voucher
            {
                Title = "سند: " + stuff.Title,
                Date = new DateTime(1401, 02, 18),
                Quantity = 10,
                Price = 1000,
                StuffId = stuff.Id,
            };
        }

        private static UpdateStuffDto GenerateUpdateStuffDto(int categoryId, string title)
        {
            return new UpdateStuffDto
            {
                Title = title,
                Unit = "پاکت",
                MinimumInventory = 10,
                MaximumInventory = 50,
                CategoryId = categoryId,
            };
        }

        private void CreateStuffsInDataBase(int categoryId)
        {
            var stuffs = new List<Stuff>
            {
                new Stuff { Title = "شیر", Inventory=10, MinimumInventory=5, MaximumInventory=50,Unit="عدد", CategoryId=categoryId},
                new Stuff { Title = "پنیر", Inventory=20, MinimumInventory=5, MaximumInventory=50,Unit="بسته", CategoryId=categoryId},
                new Stuff { Title = "ماست", Inventory=30, MinimumInventory=5, MaximumInventory=50,Unit="کیلوگرم", CategoryId=categoryId}
            };
            _dataContext.Manipulate(_ =>
            _.Stuffs.AddRange(stuffs));
        }

        private static Stuff CreateStuff(Category category, string title)
        {
            return new Stuff
            {
                Title = title,
                Inventory = 20,
                MinimumInventory = 20,
                MaximumInventory = 50,
                Unit = "پاکت",
                CategoryId = category.Id,
            };
        }

        private static AddStuffDto GenerateAddStuffDto(Category category, string title)
        {
            return new AddStuffDto
            {
                Title = title,
                Inventory = 20,
                MinimumInventory = 20,
                MaximumInventory = 50,
                Unit = "پاکت",
                CategoryId = category.Id,
            };
        }


    }
}
