using FluentAssertions;
using Supermarket.Test.Tools.Categories;
using Supermarket.Test.Tools.Invoices;
using Supermarket.Test.Tools.Stuffs;
using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Infrastructure.Test;
using SuperMarket.Persistence.EF;
using SuperMarket.Persistence.EF.Invoices;
using SuperMarket.Services.Invoices;
using SuperMarket.Services.Invoices.Contracts;
using SuperMarket.Specs.Infrastructure;
using System.Collections.Generic;
using Xunit;
using static SuperMarket.Specs.BDDHelper;

namespace SuperMarket.Specs.Invoices
{
    [Scenario("مشاهده فاکتور فروش حالا")]
    [Feature("",
   AsA = "فروشنده ",
   IWantTo = " فاکتور فروش  کالا را مدیریت کنم  ",
   InOrderTo = "و از آن ها گزارش بگیرم  "
)]
    public class GetAllInvoices : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly InvoiceService _sut;
        private readonly InvoiceRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private Category _category;
        IList<Invoice> expected;
        public GetAllInvoices(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFInvoiceRepository(_dataContext);
            _sut = new InvoiceAppService(_repository, _unitOfWork);
        }

        [Given("فاکتور فروش با عنوان ‘فاکتور شیر ‘ و تاریخ ‘21/02/1400’ و تعداد ‘10’ و قیمت ‘10000’ مربوط به کالای با عنوان ‘شیر’ وجود دارد")]
        public void Given()
        {
            _category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(_category));

            var stuff = StuffFactory.CreateStuff(_category, "شیر");
            _dataContext.Manipulate(_ => _.Stuffs.Add(stuff));

            var invoice = InvoiceFactory.CreateInvoice(stuff);
            _dataContext.Manipulate(_ => _.Invoices.Add(invoice));
        }

        [And("فاکتور فروشی  با عنوان ‘فاکتور پنیر ‘ و تاریخ ‘21/02/1400’ و تعداد ‘20’ و قیمت ‘20000’ مربوط به کالای با عنوان ‘پنیر’ وجود دارد")]
        public void And()
        {
            var stuff = StuffFactory.CreateStuff(_category, "پنیر");
            _dataContext.Manipulate(_ => _.Stuffs.Add(stuff));

            var invoice = InvoiceFactory.CreateInvoice(stuff);
            _dataContext.Manipulate(_ => _.Invoices.Add(invoice));
        }

        [When("می خواهیم فاکتورهای فروش  کالا را مشاهده کنیم")]
        public void When()
        {
            expected = _sut.GetAllInvoices();
        }

        [Then("جزئیات فاکتور فروش  کالا با عنوان ‘فاکتور شیر’ و  جزئیات فاکتور فروش  کالا با عنوان ‘فاکتور پنیر’ را باید مشاهده کنیم")]
        public void Then()
        {
            expected.Should().HaveCount(2);
            expected.Should().Contain(_ => _.Title == "فاکتور پنیر");
            expected.Should().Contain(_ => _.Title == "فاکتور شیر");
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
