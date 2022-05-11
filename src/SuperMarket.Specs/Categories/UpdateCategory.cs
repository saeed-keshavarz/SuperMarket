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
using System.Linq;
using Xunit;
using static SuperMarket.Specs.BDDHelper;

namespace SuperMarket.Specs.Categories
{
    [Scenario("ویرایش دسته بندی کالا")]
    [Feature("",
   AsA = "فروشنده ",
   IWantTo = " دسته بندی کالا را مدیریت کنم  ",
   InOrderTo = "در آن کالای خود را تعریف کنم "
)]
    public class UpdateCategory : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly CategoryService _sut;
        private readonly CategoryRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private Category _category;
        private UpdateCategoryDto _dto;

        public UpdateCategory(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFCategoryRepository(_dataContext);
            _sut = new CategoryAppService(_repository, _unitOfWork);
        }

        [Given("دسته بندی با عنوان ‘لبنیات’در فهرست دسته بندی کالا وجود دارد")]
        public void Given()
        {
            _category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(_category));
        }

        [When("دسته بندی با عنوان ‘لبنیات’ را به ‘خشکبار’ ویرایش می کنیم")]
        public void When()
        {
            var category = _dataContext.Categories
                .FirstOrDefault(_ => _.Title == _category.Title);

            _dto = CategoryFactory.GenerateUpdateCategoryDto("خشکبار");

            _sut.Update(category.Id, _dto);
        }

        [Then("دسته بندی با عنوان ‘خشکبار’ باید در فهرست دسته بندی کالا وجود داشته باشد")]
        public void Then()
        {
            var expected = _dataContext.Categories.FirstOrDefault();
            expected.Title.Should().Be(_dto.Title);
        }

        [Fact]
        public void Run()
        {
            Runner.RunScenario(_ => Given()
            , _ => When()
            , _ => Then());
        }
    }
}
