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
using System.Linq;
using Xunit;
using static SuperMarket.Specs.BDDHelper;

namespace SuperMarket.Specs.Invoices
{
    [Scenario("حذف فاکتور فروش")]
    [Feature("",
    AsA = "فروشنده ",
    IWantTo = " فاکتور فروش  کالا را مدیریت کنم  ",
    InOrderTo = "و از آن ها گزارش بگیرم  "
)]
    public class DeleteInvoice : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly InvoiceService _sut;
        private readonly InvoiceRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private Category _category;
        private Stuff _stuff;
        private Invoice _invoice;
        private UpdateInvoiceDto _dto;

        public DeleteInvoice(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFInvoiceRepository(_dataContext);
            _sut = new InvoiceAppService(_repository, _unitOfWork);
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

        [And("فاکتور فروشی  با عنوان ‘فاکتور شیر ’ و تاریخ ‘21/02/1400’ و تعداد ‘10’ و قیمت ‘10000’ مربوط به کالای با عنوان ‘شیر’ وجود دارد")]
        public void GivenAnd()
        {
            _invoice = new Invoice()
            {
                Title = "فاکتور شیر",
                Date = new DateTime(1400, 02, 21),
                Buyer = "آقای کشاورز",
                Quantity = 10,
                Price = 10000,
                StuffId = _stuff.Id,
            };

            _dataContext.Manipulate(_ => _.Invoices.Add(_invoice));
        }

        [When("فاکتور فروش  با عنوان ‘فاکتور  شیر’ و کد کالا ‘100’ و  تاریخ ‘21/02/1400’ و تعداد ‘10’ و قیمت ‘10000’ را حذف می کنیم")]
        public void When()
        {
            var invoice = _dataContext.Invoices.FirstOrDefault(_ => _.Title == _invoice.Title);

            _sut.Delete(invoice.Id);
        }

        [Then("")]
        public void Then()
        {
            _dataContext.Invoices.Should().
                NotContain(_ => _.Title == _invoice.Title);
        }

        [And("کالایی با عنوان 'شیر' و کد کالا '100' باید موجودی '5' داشته باشد")]
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
