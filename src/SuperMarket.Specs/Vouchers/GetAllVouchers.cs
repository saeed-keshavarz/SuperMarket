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
using System.Collections.Generic;
using Xunit;
using static SuperMarket.Specs.BDDHelper;

namespace SuperMarket.Specs.Vouchers
{
    [Scenario("مشاهده سند ورود")]
    [Feature("",
     AsA = "فروشنده ",
     IWantTo = " سند ورود  کالا را مدیریت کنم  ",
     InOrderTo = "و از آن ها گزارش بگیرم  "
 )]
    public class GetAllVouchers : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly VoucherService _sut;
        private readonly VoucherRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private Category _category;
        IList<Voucher> expected;

        public GetAllVouchers(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFVoucherRepository(_dataContext);
            _sut = new VoucherAppService(_repository, _unitOfWork);
        }

        [Given("سند ورودی با عنوان ‘سند شیر’ و تاریخ ‘21/02/1400’ و تعداد ‘10’ و قیمت ‘10000’ مربوط به کالای با عنوان ‘شیر’ وجود دارد")]
        public void Given()
        {
            _category = new Category()
            {
                Title = "لبنیات",
            };

            _dataContext.Manipulate(_ => _.Categories.Add(_category));

            var stuff = new Stuff()
            {
                Title = "شیر",
                Inventory = 10,
                Unit = "پاکت",
                MinimumInventory = 5,
                MaximumInventory = 20,
                CategoryId = _category.Id,
            };

            _dataContext.Manipulate(_ => _.Stuffs.Add(stuff));

            var voucher = new Voucher()
            {
                Title = "سند شیر",
                Date = new DateTime(1400, 02, 21),
                Quantity = 10,
                Price = 10000,
                StuffId = stuff.Id,
            };

            _dataContext.Manipulate(_ => _.Vouchers.Add(voucher));
        }

        [And("سند ورودی با عنوان ‘سند پنیر’ و تاریخ ‘21/02/1400’ و تعداد ‘20’ و قیمت ‘20000’ مربوط به کالای با عنوان ‘پنیر’ وجود دارد")]
        public void And()
        {
            var stuff = new Stuff()
            {
                Title = "پنیر",
                Inventory = 10,
                Unit = "پاکت",
                MinimumInventory = 5,
                MaximumInventory = 20,
                CategoryId = _category.Id,
            };
            _dataContext.Manipulate(_ => _.Stuffs.Add(stuff));

            var voucher = new Voucher()
            {
                Title = "سند پنیر",
                Date = new DateTime(1400, 02, 21),
                Quantity = 20,
                Price = 20000,
                StuffId = stuff.Id,
            };

            _dataContext.Manipulate(_ => _.Vouchers.Add(voucher));
        }

        [When("می خواهیم سندهای ورود کالا را مشاهده کنیم")]
        public void When()
        {
            expected = _sut.GetAllVouchers();
        }

        [Then("جزئیات سند ورود کالا با عنوان ‘سند شیر ’ و  جزئیات سند ورود کالا با عنوان ‘سند پنیر’ را باید مشاهده کنیم")]
        public void Then()
        {
            expected.Should().HaveCount(2);
            expected.Should().Contain(_ => _.Title == "سند پنیر");
            expected.Should().Contain(_ => _.Title == "سند شیر");
            expected.Should().Contain(_ => _.Stuff.Title == "شیر");
            expected.Should().Contain(_ => _.Stuff.Title == "پنیر");
            expected.Should().Contain(_ => _.Stuff.Category.Title == "لبنیات");
        }

        [Fact]
        public void Run()
        {
            Runner.RunScenario(_ => Given()
            , _ => And()
            , _ => When()
            , _ => Then());
        }
    }
}
