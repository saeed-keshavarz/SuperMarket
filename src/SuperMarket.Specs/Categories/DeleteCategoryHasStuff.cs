using FluentAssertions;
using Supermarket.Test.Tools.Categories;
using Supermarket.Test.Tools.Stuffs;
using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Infrastructure.Test;
using SuperMarket.Persistence.EF;
using SuperMarket.Persistence.EF.Categories;
using SuperMarket.Services.Categories;
using SuperMarket.Services.Categories.Contracts;
using SuperMarket.Services.Categories.Exceptions;
using SuperMarket.Specs.Infrastructure;
using System;
using System.Linq;
using Xunit;
using static SuperMarket.Specs.BDDHelper;

namespace SuperMarket.Specs.Categories
{
    [Scenario("حذف دسته بندی دارای کالا")]
    [Feature("",
AsA = "فروشنده ",
IWantTo = " دسته بندی کالا را مدیریت کنم  ",
InOrderTo = "در آن کالای خود را تعریف کنم "
)]
    public class DeleteCategoryHasStuff : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly CategoryService _sut;
        private readonly CategoryRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private Category _category;
        private Stuff _stuff;
        Action expected;

        public DeleteCategoryHasStuff(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFCategoryRepository(_dataContext);
            _sut = new CategoryAppService(_repository, _unitOfWork);
        }


        [Given("دسته بندی با عنوان ‘لبنیات’ در فهرست دسته بندی کالا وجود دارد")]
        public void Given()
        {
            _category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(_category));
        }

        [And("کالایی با عنوان ‘پنیر’ در دسته بندی با عنوان ‘لبنیات’ وجود دارد")]
        public void GivenAnd()
        {
            _stuff = StuffFactory.CreateStuff(_category, "پنیر");
            _dataContext.Manipulate(_ => _.Stuffs.Add(_stuff));
        }

        [When("دسته بندی با عنوان ‘لبنیات’ را حذف می کنیم")]
        public void When()
        {
            _category = _dataContext.Categories.FirstOrDefault(_ => _.Title == _category.Title);

            expected = () => _sut.Delete(_category.Id);
        }

        [Then("دسته بندی با عنوان ‘ لبنیات ‘ در فهرست دسته بندی کالا باید وجود داشته باشد")]
        public void Then()
        {
            var expected = _dataContext.Categories.FirstOrDefault();

            expected.Title.Should().Be(_category.Title);
        }

        [And("خطایی با عنوان ‘دسته بندی دارای کالا غیرقابل حذف است’ باید رخ دهد")]
        public void ThenAnd()
        {
            expected.Should().ThrowExactly<CanNotDeleteCategoryHasStuffException>();
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
