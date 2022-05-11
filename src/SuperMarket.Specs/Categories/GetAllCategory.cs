using FluentAssertions;
using Supermarket.Test.Tools.Categories;
using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Infrastructure.Test;
using SuperMarket.Persistence.EF;
using SuperMarket.Persistence.EF.Categories;
using SuperMarket.Services.Categories;
using SuperMarket.Services.Categories.Contracts;
using SuperMarket.Specs.Infrastructure;
using System.Collections.Generic;
using Xunit;
using static SuperMarket.Specs.BDDHelper;

namespace SuperMarket.Specs.Categories
{
    [Scenario("مشاهده دسته بندی ها ")]
    [Feature("",
   AsA = "فروشنده ",
   IWantTo = " دسته بندی کالا را مدیریت کنم  ",
   InOrderTo = "در آن کالای خود را تعریف کنم "
)]
    public class GetAllCategory : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly CategoryService _sut;
        private readonly CategoryRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private Category _category;
        IList<Category> expected;

        public GetAllCategory(ConfigurationFixture configuration) : base(configuration)
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

        [And("دسته بندی با عنوان ‘خشکبار’ در فهرست دسته بندی کالا وجود دارد")]
        public void And()
        {
            _category = CategoryFactory.CreateCategory("خشکبار");
            _dataContext.Manipulate(_ => _.Categories.Add(_category));
        }

        [When("می خواهیم دسته بندی ها را مشاهده کنیم")]
        public void When()
        {
            expected = _sut.GetAll();
        }

        [Then("دسته بندی ها با عنوان های ‘لبنیات’  و ‘خشکبار’ را باید مشاهده کنیم")]
        public void Then()
        {
            expected.Should().HaveCount(2);
            expected.Should().Contain(_ => _.Title == "لبنیات");
            expected.Should().Contain(_ => _.Title == "خشکبار");
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
