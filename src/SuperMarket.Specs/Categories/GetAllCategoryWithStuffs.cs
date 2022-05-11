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
using SuperMarket.Specs.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static SuperMarket.Specs.BDDHelper;

namespace SuperMarket.Specs.Categories
{
    [Scenario("مشاهده دسته بندی ها با کالا ")]
    [Feature("",
AsA = "فروشنده ",
IWantTo = " دسته بندی کالا را مدیریت کنم  ",
InOrderTo = "در آن کالای خود را تعریف کنم "
)]
    public class GetAllCategoryWithStuffs : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly CategoryService _sut;
        private readonly CategoryRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private Category _category;
        private Stuff _stuff;
        IList<Category> expected;

        public GetAllCategoryWithStuffs(ConfigurationFixture configuration) : base(configuration)
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
        public void And()
        {
            _stuff = StuffFactory.CreateStuff(_category, "پنیر");
            _dataContext.Manipulate(_ => _.Stuffs.Add(_stuff));
        }

        [When("می خواهیم دسته بندی با عنوان ‘لبنیات’ را مشاهده کنیم")]
        public void When()
        {
            expected = _sut.GetAllCategoryWithStuff();
        }

        [Then("دسته بندی  با عنوان ‘لبنیات’  و کالای ‘پنیر’ را باید مشاهده کنیم")]
        public void Then()
        {
            expected.Should().HaveCount(1);
            expected.Should().Contain(_ => _.Title == "لبنیات");
            expected.Should().Contain(_ => _.Stuffs.First().Title == "پنیر");
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
