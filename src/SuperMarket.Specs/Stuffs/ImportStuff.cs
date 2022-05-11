using FluentAssertions;
using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Infrastructure.Test;
using SuperMarket.Persistence.EF;
using SuperMarket.Persistence.EF.Vouchers;
using SuperMarket.Services.Vouchers;
using SuperMarket.Services.Vouchers.Contracts;
using SuperMarket.Specs.Infrastructure;
using System;
using System.Linq;
using Xunit;
using static SuperMarket.Specs.BDDHelper;

namespace SuperMarket.Specs.Stuffs
{
    [Scenario("ورود کالا")]
    [Feature("",
AsA = "فروشنده ",
IWantTo = " کالاها را مدیریت کنم ",
InOrderTo = "و آن را به فروش برسانم "
)]
    public class ImportStuff : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly VoucherService _sut;
        private readonly VoucherRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private Stuff _stuff;
        private Category _category;
        private AddVoucherDto _dto;

        public ImportStuff(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFVoucherRepository(_dataContext);
            _sut = new VoucherAppService(_repository, _unitOfWork);
        }

        [Given("کالایی با عنوان ‘شیر’ و موجودی ‘10’ و واحد ‘پاکت ‘ و حداقل موجودی ‘5’ و حداکثر موجودی ‘20’ در دسته بندی کالا  با عنوان ‘ لبنبات’ وجود دارد")]
        public void Given()
        {
            _category = new Category()
            {
                Title = "لبنیات",
            };

            _dataContext.Manipulate(_ => _.Categories.Add(_category));

            _stuff = new Stuff()
            {
                Title = "شیر",
                Inventory = 10,
                Unit = "پاکت",
                MinimumInventory = 5,
                MaximumInventory = 20,
                CategoryId = _category.Id,
            };

            _dataContext.Manipulate(_ => _.Stuffs.Add(_stuff));
        }

        [And("هیچ سند ورود کالایی در فهرست سند ورودی کالا وجود ندارد")]
        public void GivenAnd()
        {

        }

        [When("کالایی  با تعداد ‘10’ و قیمت خرید '1000' در تاریخ ‘21/02/1400’  وارد میکنیم")]
        public void When()
        {

            _dto = new AddVoucherDto()
            {
                Title = "سند: " + _stuff.Title + DateTime.Now.ToShortDateString(),
                Date = new DateTime(1400, 02, 21),
                Quantity = 10,
                Price = 1000,
                StuffId = _stuff.Id,
            };

            _sut.Add(_dto);
        }
        [Then("سند ورود کالایی با کد ‘100’ با تعداد ‘10’ در تاریخ ‘21/02/1400’ در فهرست سند ورودی کالا باید وجود داشته باشد")]
        public void Then()
        {
            var expected = _dataContext.Vouchers.FirstOrDefault();
            expected.Title.Should().Be(_dto.Title);
            expected.Date.Should().Be(_dto.Date);
            expected.Quantity.Should().Be(_dto.Quantity);
            expected.Price.Should().Be(_dto.Price);
            expected.StuffId.Should().Be(_dto.StuffId);
        }

        [And("کالایی با عنوان ‘شیر’ و موجودی ‘20’ عدد در فهرست کالا ها باید وجود داشته باشد ")]
        public void ThenAnd()
        {
            var expected = _dataContext.Stuffs.FirstOrDefault();
            expected.Title.Should().Be(_stuff.Title);
            expected.Inventory.Should().Be(20);
        }

        [Fact]
        public void Run()
        {
            Runner.RunScenario(_ => Given()
            , _ => GivenAnd()
            , _ => When()
            , _ => Then()
            , _ => ThenAnd());
        }

    }
}
