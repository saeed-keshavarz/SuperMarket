using FluentAssertions;
using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Infrastructure.Test;
using SuperMarket.Persistence.EF;
using SuperMarket.Persistence.EF.Invoices;
using SuperMarket.Services.Invoices;
using SuperMarket.Services.Invoices.Contracts;
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
    [Scenario("خروج کالا")]
    [Feature("",
AsA = "فروشنده ",
IWantTo = " کالاها را مدیریت کنم ",
InOrderTo = "و آن را به فروش برسانم "
)]
    public class ExportStuff : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly InvoiceService _sut;
        private readonly InvoiceRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private Stuff _stuff;
        private Category _category;
        private AddInvoiceDto _dto;

        public ExportStuff(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFInvoiceRepository(_dataContext);
            _sut = new InvoiceAppService(_repository, _unitOfWork);
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

        [And("هیچ فاکتور فروش کالایی در فهرست فاکتور فروش کالا وجود ندارد")]
        public void GivenAnd()
        {

        }


        [When("کالایی با کد ‘100’ با تعداد ‘5’ و قیمت فروش  ‘2000’ در تاریخ ‘21/02/1400’  خروج میکنیم.")]
        public void When()
        {

            _dto = new AddInvoiceDto()
            {
                Title = "فاکتور: " + _stuff.Title + DateTime.Now.ToShortDateString(),
                Date = new DateTime(1400, 02, 21),
                Quantity = 5,
                Price = 2000,
                StuffId = _stuff.Id,
                Buyer="کشاورز",
            };

            _sut.Add(_dto, _stuff.Id);
        }

        [Then("فاکتور فروش کالایی با کد کالا ‘100’ با تعداد ‘5’ و خریدار ‘کشاورز’ در تاریخ ‘21/02/1400’ در فهرست فاکتور فروش کالا باید وجود داشته باشد")]
        public void Then()
        {
            var expected = _dataContext.Invoices.FirstOrDefault();
            expected.Title.Should().Be(_dto.Title);
            expected.Date.Should().Be(_dto.Date);
            expected.Quantity.Should().Be(_dto.Quantity);
            expected.Price.Should().Be(_dto.Price);
            expected.StuffId.Should().Be(_dto.StuffId);
        }

        [And("کالایی با عنوان ‘شیر’ و موجودی ‘5’ عدد در فهرست کالا ها باید وجود داشته باشد ")]
        public void ThenAnd()
        {
            var expected = _dataContext.Stuffs.FirstOrDefault();
            expected.Title.Should().Be(_stuff.Title);
            expected.Inventory.Should().Be(5);
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
