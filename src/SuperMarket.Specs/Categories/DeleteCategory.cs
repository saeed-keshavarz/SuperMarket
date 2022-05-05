using FluentAssertions;
using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Infrastructure.Test;
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

namespace SuperMarket.Specs.Categories
{
    [Scenario("حذف دسته بندی بدون کالا")]
    [Feature("",
AsA = "فروشنده ",
IWantTo = " دسته بندی کالا را مدیریت کنم  ",
InOrderTo = "در آن کالای خود را تعریف کنم "
)]
    public class DeleteCategory : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly CategoryService _sut;
        private readonly CategoryRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private Category _category;

        public DeleteCategory(ConfigurationFixture configuration) : base(configuration)
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

        [When("دسته بندی با عنوان ‘لبنیات’ را حذف می کنیم")]
        public void When()
        {
            _category = _dataContext.Categories.FirstOrDefault(_ => _.Title == _category.Title);

            _sut.Delete(_category.Id);
        }

        [Then("دسته بندی با عنوان ‘ لبنیات ‘ باید در فهرست دسته بندی کالا وجود نداشته باشد")]
        public void Then()
        {
            _dataContext.Categories.Should().
                NotContain(_ => _.Title == _category.Title);
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
