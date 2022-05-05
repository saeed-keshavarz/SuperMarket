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
    [Scenario("تعریف کالا")]
    [Feature("",
       AsA = "فروشنده ",
       IWantTo = " کالاها را مدیریت کنم ",
       InOrderTo = "آن را به فروش برسانم "
   )]
    public class AddStuffWithDuplicateTitle : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly StuffService _sut;
        private readonly StuffRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private Category _category;
        private AddStuffDto _dto;
        Action expected;

        public AddStuffWithDuplicateTitle(ConfigurationFixture configuration) : base(configuration)
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

        [And("کالایی با عنوان ‘شیر’ و موجودی ‘10’ و واحد ‘پاکت ‘ و حداقل موجودی ‘5’ و حداکثر موجودی ‘20’ در دسته بندی کالا  با عنوان ‘ لبنبات’ وجود دارد")]
        public void GivenAnd()
        {
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
        }

        [When("کالایی با عنوان ‘شیر’ و موجودی ‘10’ و واحد ‘پاکت ‘ و حداقل موجودی ‘5’ و حداکثر موجودی ‘20’ در دسته بندی کالا  با عنوان ‘ لبنبات’ تعریف میکنیم")]
        public void When()
        {
            _dto = new AddStuffDto()
            {
                Title = "شیر",
                Inventory = 10,
                Unit = "پاکت",
                MinimumInventory = 5,
                MaximumInventory = 20,
                CategoryId = _category.Id,
            };

            expected = () => _sut.Add(_dto);
        }

        [Then("تنها یک کالا با عنوان ‘شیر’ و موجودی ‘10’ و واحد ‘پاکت ‘ و حداقل موجودی ‘5’ و حداکثر موجودی ‘20’ در دسته بندی کالا  با عنوان ‘ لبنبات’ باید وجود داشته باشد")]
        public void Then()
        {
            _dataContext.Stuffs.Where(_=>_.Title ==_dto.Title
            && _.CategoryId==_category.Id)
                .Should().HaveCount(1);
        }

        [And("خطایی با عنوان ‘عنوان کالا تکراریست ‘ باید رخ دهد")]
        public void ThenAnd()
        {
            expected.Should().ThrowExactly<DuplicateStuffTitleInCategoryException>();
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
