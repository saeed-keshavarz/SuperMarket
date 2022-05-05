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
    [Scenario("حذف کالا")]
    [Feature("",
AsA = "فروشنده ",
IWantTo = " کالاها را مدیریت کنم ",
InOrderTo = "و آن را به فروش برسانم "
)]
    public class DeleteStuff : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly StuffService _sut;
        private readonly StuffRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private static Category _category;
        private Stuff _stuff;

        public DeleteStuff(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFStuffRepository(_dataContext);
            _sut = new StuffAppService(_repository, _unitOfWork);
        }

        [Given("دسته بندی کالایی با عنوان ‘لبنیات’ در فهرست دسته بندی کالا وجود دارد")]
        public void Given()
        {
            _category = new Category()
            {
                Title = "لبنیات",
            };

            _dataContext.Manipulate(_ => _.Categories.Add(_category));
        }

        [And("کالایی با عنوان ‘شیر’ و موجودی ‘0’ و واحد ‘پاکت ‘ و حداقل موجودی ‘5’ و حداکثر موجودی ‘20’ در دسته بندی کالا  با عنوان ‘ لبنبات’ وجود دارد")]
        public void And()
        {
            _stuff = new Stuff()
            {
                Title = "شیر",
                Inventory = 0,
                Unit = "پاکت",
                MinimumInventory = 5,
                MaximumInventory = 20,
                CategoryId = _category.Id,
            };

            _dataContext.Manipulate(_ => _.Stuffs.Add(_stuff));
        }

        [When("کالایی با عنوان ‘شیر’ را به ‘پنیر’ در دسته بندی با عنوان ‘لبنیات’ ویرایش می کنیم")]
        public void When()
        {
            _stuff = _dataContext.Stuffs.FirstOrDefault(_ => _.Title == _stuff.Title);

            _sut.Delete(_stuff.Id);
        }

        [Then("")]
        public void Then()
        {
            _dataContext.Stuffs.Should().
                NotContain(_ => _.Title == _stuff.Title);
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
