using FluentAssertions;
using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Infrastructure.Test;
using SuperMarket.Persistence.EF;
using SuperMarket.Persistence.EF.Stuffs;
using SuperMarket.Services.Stuffs;
using SuperMarket.Services.Stuffs.Contracts;
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
    [Scenario("ویرایش کالا")]
    [Feature("",
AsA = "فروشنده ",
IWantTo = " کالاها را مدیریت کنم ",
InOrderTo = "و آن را به فروش برسانم "
)]
    public class GetAllStuff : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly StuffService _sut;
        private readonly StuffRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private static Category _category;
        private Stuff _stuff;
        IList<Stuff> expected;

        public GetAllStuff(ConfigurationFixture configuration) : base(configuration)
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

        [And("کالایی با عنوان ‘پنیر’ در دسته بندی کالا با عنوان ‘لبنیات’ وجود دارد")]
        public void And()
        {
            _stuff = new Stuff()
            {
                Title = "پنیر",
                Inventory = 10,
                Unit = "پاکت",
                MinimumInventory = 5,
                MaximumInventory = 20,
                CategoryId = _category.Id,
            };

            _dataContext.Manipulate(_ => _.Stuffs.Add(_stuff));
        }

        [When("می خواهیم کالاها را مشاهده کنیم")]
        public void When()
        {
            expected = _sut.GetAllStuff();
        }

        [Then("کالاهایی با عنوان ‘شیر’ با موجودی ‘10’ ،  واحد ‘پاکت’ و دسته بندی ‘لبنیات’ و ‘پنیر’ با موجودی ‘10’ ،  واحد ‘پاکت’ و دسته بندی ‘لبنیات’ را باید مشاهده کنیم")]
        public void Then()
        {
            expected.Should().HaveCount(2);
            expected.Should().Contain(_ => _.Title == "پنیر");
            expected.Should().Contain(_ => _.Title == "شیر");
            expected.Should().Contain(_ => _.Category.Title == "لبنیات");
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
