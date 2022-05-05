using FluentAssertions;
using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Infrastructure.Test;
using SuperMarket.Persistence.EF;
using SuperMarket.Persistence.EF.Stuffs;
using SuperMarket.Services.Stuffs;
using SuperMarket.Services.Stuffs.Contracts;
using SuperMarket.Services.Stuffs.Exceptions;
using SuperMarket.Specs.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static SuperMarket.Specs.BDDHelper;

namespace SuperMarket.Specs.Stuffs
{
    [Scenario("حذف کالای دارای فاکتور فروش")]
    [Feature("",
AsA = "فروشنده ",
IWantTo = " کالاها را مدیریت کنم ",
InOrderTo = "و آن را به فروش برسانم "
)]
    public class DeleteStuffHasInvoice : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly StuffService _sut;
        private readonly StuffRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private static Category _category;
        private Stuff _stuff;
        Action expected;
        public DeleteStuffHasInvoice(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFStuffRepository(_dataContext);
            _sut = new StuffAppService(_repository, _unitOfWork);
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

        [And("فاکتور فروش کالایی با عنوان ‘فاکتور 100’ و تاریخ ‘15/04/1400’ و تعداد ‘10’ و قیمت ‘10000’ و خریدار ‘کشاورز’ مربوط به کالایی با عنوان ‘شیر’ وجود دارد")]
        public void GivenAnd()
        {
            var invoice = new Invoice()
            {
                Title = "فاکتور 100",
                Quantity = 10,
                Price = 10000,
                Buyer = "کشاورز",
                Date = DateTime.Now,
                StuffId = _stuff.Id,
            };

            _dataContext.Manipulate(_ => _.Invoices.Add(invoice));
        }

        [When("کالا با عنوان ‘شیر’ را حذف می کنیم")]
        public void When()
        {
            _stuff = _dataContext.Stuffs.FirstOrDefault(_ => _.Title == _stuff.Title);

            expected = () => _sut.Delete(_stuff.Id);
        }

        [Then("کالایی با عنوان ‘ شیر ‘ در دسته بندی کالا با عنوان ‘لبنیات’ باید وجود نداشته باشد")]
        public void Then()
        {
            _dataContext.Stuffs.Should().
                Contain(_ => _.Title == _stuff.Title);
        }

        [And("خطایی با عنوان ‘کالا دارای فاکتور غیرقابل حذف است’ باید رخ دهد")]
        public void ThenAnd()
        {
            expected.Should().ThrowExactly<CanNotDeleteStuffHasInvoiceException>();
        }

        [Fact]
        public void Run()
        {
            Runner.RunScenario(
                _ => Given()
            , _ => GivenAnd()
            , _ => When()
            , _ => Then()
            , _ => ThenAnd());
        }
    }
}
