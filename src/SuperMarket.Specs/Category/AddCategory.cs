using FluentAssertions;
using SuperMarket.Persistence.EF;
using SuperMarket.Persistence.EF.Categories;
using SuperMarket.Services.Categories;
using SuperMarket.Services.Categories.Contracts;
using SuperMarket.Specs.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static SuperMarket.Specs.BDDHelper;

namespace SuperMarket.Specs.Category
{
    [Scenario("تعریف دساه بندی کالا")]
    [Feature("",
        AsA = "فروشنده ",
        IWantTo = " دسته بندی کالا را مدیریت کنم  ",
        InOrderTo = "در آن کالای خود را تعریف کنم "
    )]
    public class AddCategory : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private AddCategoryDto _dto;
        public AddCategory(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
        }
        [Given("هیچ دسته بندی در فهرست دسته بندی کالا وجود ندارد")]
        public void Given()
        {

        }

        [When("دسته بندی با عنوان ‘لبنیات’ تعریف میکنیم")]
        public void When()
        {
            _dto = new AddCategoryDto()
            {
                Title = "لبنیات",
            };
            var _unitOfWork = new EFUnitOfWork(_dataContext);
            CategoryRepository _categoryRepository = new EFCategoryRepository(_dataContext);
            CategoryService _sut = new CategoryAppService(_categoryRepository, _unitOfWork);
            _sut.Add(_dto);
        }

        [Then("دسته بندی با عنوان ‘لبنیات’در فهرست دسته بندی کالا باید وجود داشته باشد")]
        public void Then()
        {
            var expected = _dataContext.Categories.FirstOrDefault();
            expected.Title.Should().Be(_dto.Title);
        }

        [Fact]
        public void Run()
        {
            Given();
            When();
            Then();
        }
    }


}
