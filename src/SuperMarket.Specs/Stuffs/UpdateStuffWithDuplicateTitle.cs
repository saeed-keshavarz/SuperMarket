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
using System.Linq;
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
    public class UpdateStuffWithDuplicateTitle : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly StuffService _sut;
        private readonly StuffRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private static Category _category;
        private Stuff _stuff;
        private UpdateStuffDto _dto;
        Action expected;

        public UpdateStuffWithDuplicateTitle(ConfigurationFixture configuration) : base(configuration)
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
        public void GivenAnd()
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

        [When("کالایی با عنوان ‘شیر’ را به ‘پنیر’ ویرایش می کنیم")]
        public void When()
        {
            var stuff = _dataContext.Stuffs.FirstOrDefault(_ => _.Title == _stuff.Title);
            _dto = GenerateUpdateStuffDto("پنیر");

            expected = () => _sut.Update(stuff.Id, _dto);
        }

        [Then("تنها یک کالا با عنوان ‘ پنیر’ باید در دسته بندی کالا با عنوان ‘لبنیات’ وجود داشته باشد")]
        public void Then()
        {
            _dataContext.Stuffs.Where(_ => _.Title == _dto.Title
            && _.CategoryId == _category.Id)
                .Should().HaveCount(1);
        }

        [And("")]
        public void ThenAnd()
        {
            expected.Should().ThrowExactly<DuplicateStuffTitleInCategoryException>();
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

        private static UpdateStuffDto GenerateUpdateStuffDto(string title)
        {
            return new UpdateStuffDto
            {
                Title = title,
                Unit = "پاکت",
                MinimumInventory = 5,
                MaximumInventory = 20,
                CategoryId = _category.Id,
            };
        }
    }
}
