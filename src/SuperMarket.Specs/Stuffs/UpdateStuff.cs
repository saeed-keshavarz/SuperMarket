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
    public class UpdateStuff : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly StuffService _sut;
        private readonly StuffRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private static Category _category;
        private Stuff _stuff;
        private UpdateStuffDto _dto;
        Action expected;
        public UpdateStuff(ConfigurationFixture configuration) : base(configuration)
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

        [And("هیچ کالایی با عنوان ‘پنیر’ در دسته بندی کالا با عنوان ‘لبنیات’ وجود ندارد")]
        public void And()
        {

        }

        [When("کالایی با عنوان ‘شیر’ را به ‘پنیر’ در دسته بندی با عنوان ‘لبنیات’ ویرایش می کنیم")]
        public void When()
        {
            var stuff = _dataContext.Stuffs.FirstOrDefault(_ => _.Title == _stuff.Title);
            _dto = GenerateUpdateStuffDto("پنیر");

            _sut.Update(stuff.Id, _dto);
        }

        [Then("کالایی با عنوان ‘پنیر’ باید در دسته بندی کالا با عنوان ‘لبنیات’ وجود داشته باشد")]
        public void Then()
        {
            var expected = _dataContext.Stuffs.FirstOrDefault();
            expected.Title.Should().Be(_dto.Title);
            expected.CategoryId.Should().Be(_category.Id);
        }

        [Fact]
        public void Run()
        {
            Runner.RunScenario(_ => Given()
            , _=> And()
            , _ => When()
            , _ => Then());
        }

        private static UpdateStuffDto GenerateUpdateStuffDto(string title)
        {
            return new UpdateStuffDto
            {
                Title = title,
                Inventory = 10,
                Unit = "پاکت",
                MinimumInventory = 5,
                MaximumInventory = 20,
                CategoryId = _category.Id,
            };
        }
    }
}
