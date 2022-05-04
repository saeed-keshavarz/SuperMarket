using FluentAssertions;
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static SuperMarket.Specs.BDDHelper;

namespace SuperMarket.Specs.Categories
{

    [Scenario("ویرایش دسته بندی کالا با عنوان تکراری")]
    [Feature("",
   AsA = "فروشنده ",
   IWantTo = " دسته بندی کالا را مدیریت کنم  ",
   InOrderTo = "در آن کالای خود را تعریف کنم "
)]
    public class UpdateCategoryWithDuplicateTitle : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly CategoryService _sut;
        private readonly CategoryRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private Category _category;
        private UpdateCategoryDto _dto;
        Action expected;

        public UpdateCategoryWithDuplicateTitle(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFCategoryRepository(_dataContext);
            _sut = new CategoryAppService(_repository, _unitOfWork);
        }

        [Given("دسته بندی با عنوان ‘لبنیات’ در فهرست دسته بندی کالا وجود دارد")]
        public void Given()
        {
            _category = new Category()
            {
                Title = "لبنیات",
            };

            _dataContext.Manipulate(_ => _.Categories.Add(_category));
        }

        [And("دسته بندی با عنوان ‘خشکبار’ در فهرست دسته بندی کالا وجود دارد")]
        public void GivenAnd()
        {
            _category = new Category()
            {
                Title = "خشکبار",
            };

            _dataContext.Manipulate(_ => _.Categories.Add(_category));
        }

        [When("دسته بندی با عنوان ‘خشکبار’ را به ‘لبنیات’ ویرایش می کنیم")]
        public void When()
        {
            var category = _dataContext.Categories.FirstOrDefault(_ => _.Title == _category.Title);
            _dto = GenerateUpdateCategoryDto("لبنیات");

            expected = () => _sut.Update(category.Id, _dto);
        }

        [Then("تنها یک دسته بندی با عنوان ‘ لبنیات’ باید در فهرست دسته بندی کالا وجود داشته باشد")]
        public void Then()
        {
            _dataContext.Categories.Where(_ => _.Title == _dto.Title)
               .Should().HaveCount(1);
        }

        [And("خطایی با عنوان ‘عنوان دسته بندی کالا تکراریست ‘ باید رخ دهد.")]
        public void ThenAnd()
        {
            expected.Should().ThrowExactly<DuplicateCategoryTitleException>();
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

        private static UpdateCategoryDto GenerateUpdateCategoryDto(string title)
        {
            return new UpdateCategoryDto
            {
                Title = title,
            };
        }
    }
}
