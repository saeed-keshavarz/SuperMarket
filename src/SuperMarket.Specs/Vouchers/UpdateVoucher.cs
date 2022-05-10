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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static SuperMarket.Specs.BDDHelper;

namespace SuperMarket.Specs.Vouchers
{
    [Scenario("ویرایش سند ورود")]
    [Feature("",
      AsA = "فروشنده ",
      IWantTo = " سند ورود  کالا را مدیریت کنم  ",
      InOrderTo = "و از آن ها گزارش بگیرم  "
  )]
    public class UpdateVoucher : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly VoucherService _sut;
        private readonly VoucherRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private Category _category;
        private Stuff _stuff;
        private Voucher _voucher;
        private UpdateVoucherDto _dto;

        public UpdateVoucher(ConfigurationFixture configuration) : base(configuration)
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

        [And("سند ورودی با عنوان ‘سند شیر 21/02/1400’ و تاریخ ‘21/02/1400’ و تعداد ‘10’ و قیمت ‘10000’ مربوط به کالای با عنوان ‘شیر’ وجود دارد")]
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

        [When("سند ورود با عنوان ‘سند شیر 21/02/1400’ و کد کالا ‘100’ و  تاریخ ‘21/02/1400’ و تعداد ‘10’ و قیمت ‘10000’ به ‘سند ورود شیر 21/02/1400’ و کد کالا ‘100’  تاریخ ‘20/02/1400’ و تعداد ‘15’ و قیمت ‘20000’ ویرایش می کنیم")]
        public void When()
        {
            var voucher = _dataContext.Vouchers.FirstOrDefault(_ => _.Title == _voucher.Title);
            _dto = new UpdateVoucherDto()
            {
                Title = "سند: " + _stuff.Title + " " + DateTime.Now.ToShortDateString(),
                Date = new DateTime(1400, 02, 20),
                Quantity = 15,
                Price = 20000,
                StuffId = _stuff.Id,
            };

            _sut.Update(voucher.Id, _dto);

        }

        [Then("سند ورودی با عنوان ‘سند ورود شیر 21/02/1400’ و کد کالا ‘100’ و تاریخ ‘20/02/1400’ و تعداد ‘15’ و قیمت ‘20000’ باید در فهرست سند ورود  وجود داشته باشد")]
        public void Then()
        {
            var expected = _dataContext.Vouchers.FirstOrDefault();
            expected.Date.Should().Be(_dto.Date);
            expected.Quantity.Should().Be(_dto.Quantity);
            expected.Price.Should().Be(_dto.Price);
            expected.StuffId.Should().Be(_dto.StuffId);
        }

        [And("کالایی با عنوان 'شیر' و کد کالا '100' باید موجودی '15' داشته باشد")]
        public void ThenAnd()
        {
            var expected = _dataContext.Stuffs.FirstOrDefault();
            expected.Title.Should().Be(_stuff.Title);
            expected.Inventory.Should().Be(15);

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
