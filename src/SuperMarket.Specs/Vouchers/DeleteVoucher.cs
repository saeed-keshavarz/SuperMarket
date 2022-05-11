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

namespace SuperMarket.Specs.Vouchers
{
    [Scenario("حذف سند ورود")]
    [Feature("",
    AsA = "فروشنده ",
    IWantTo = " سند ورود  کالا را مدیریت کنم  ",
    InOrderTo = "و از آن ها گزارش بگیرم  "
)]
    public class DeleteVoucher : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly VoucherService _sut;
        private readonly VoucherRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private Category _category;
        private Stuff _stuff;
        private Voucher _voucher;
        public DeleteVoucher(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFVoucherRepository(_dataContext);
            _sut = new VoucherAppService(_repository, _unitOfWork);
        }

        [Given("کالایی با عنوان با عنوان ‘شیر’  و کد کالا ‘100’ موجودی ‘10’ در دسته بندی با عنوان ‘لبنیات’ وجود دارد")]
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

        [And("سند ورود کالایی  با عنوان ‘سند  شیر’ در  تاریخ ‘21/02/1400’ و تعداد ‘10’ و قیمت ‘10000’ مربوط به کالای با عنوان ‘شیر’ وجود دارد")]
        public void GivenAnd()
        {
            _voucher = new Voucher()
            {
                Title = "سند شیر",
                Date = new DateTime(1400, 02, 21),
                Quantity = 10,
                Price = 10000,
                StuffId = _stuff.Id,
            };

            _dataContext.Manipulate(_ => _.Vouchers.Add(_voucher));
        }

        [When("سند ورود کالا  با عنوان ‘سند   شیر’ و کد کالا ‘100’ و  تاریخ ‘21/02/1400’ و تعداد ‘10’ و قیمت ‘10000’ را حذف می کنیم")]
        public void When()
        {
            var voucher = _dataContext.Vouchers.FirstOrDefault(_ => _.Title == _voucher.Title);

            _sut.Delete(voucher.Id);
        }

        [Then("سند ورود کالا  با عنوان ‘سند شیر’ و تاریخ ‘21/02/1400’ و تعداد ‘10’ و قیمت ‘10000’ مربوط به کالای با عنوان ‘شیر’ در فهرست سند ورود  نباید  وجود داشته باشد")]
        public void Then()
        {
            _dataContext.Vouchers.Should().
                NotContain(_ => _.Title == _voucher.Title);
        }

        [And("کالایی با عنوان 'شیر' و کد کالا '100' باید موجودی '0' داشته باشد")]
        public void ThenAnd()
        {
            var expected = _dataContext.Stuffs.FirstOrDefault();
            expected.Title.Should().Be(_stuff.Title);
            expected.Inventory.Should().Be(0);
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
