using FluentAssertions;
using Supermarket.Test.Tools.Categories;
using Supermarket.Test.Tools.Stuffs;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Infrastructure.Test;
using SuperMarket.Persistence.EF;
using SuperMarket.Persistence.EF.Categories;
using SuperMarket.Services.Categories;
using SuperMarket.Services.Categories.Contracts;
using SuperMarket.Services.Categories.Exceptions;
using System;
using System.Linq;
using Xunit;

namespace SuperMarket.Services.Test.Unit.Categories
{
    public class CategoryServiceTests
    {
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryService _sut;
        private readonly CategoryRepository _repository;

        public CategoryServiceTests()
        {
            _dataContext =
               new EFInMemoryDatabase()
               .CreateDataContext<EFDataContext>();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFCategoryRepository(_dataContext);
            _sut = new CategoryAppService(_repository, _unitOfWork);
        }

        [Fact]
        public void Add_adds_category_properly()
        {
            AddCategoryDto dto = CategoryFactory.GenerateAddCategoryDto();

            _sut.Add(dto);

            _dataContext.Categories.Should()
                .Contain(_ => _.Title == dto.Title);
        }

        [Fact]
        public void Add_throw_DuplicateCategoryTitleException_when_add_new_Category()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            AddCategoryDto dto = CategoryFactory.GenerateAddCategoryDto();

            Action expected = () => _sut.Add(dto);

            expected.Should().Throw<DuplicateCategoryTitleException>();
        }

        [Fact]
        public void GetAll_return_category_by_id()
        {
            var category = CategoryFactory.CreateCategory("Dummy");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var expected = _sut.GetById(category.Id);

            expected.Title.Should().Be("Dummy");
        }

        [Fact]
        public void GetAll_returns_all_categories()
        {
            var categories = CategoryFactory.CreateCategoriesInDataBase();
            _dataContext.Manipulate(_ => _.Categories.AddRange(categories));

            var expected = _sut.GetAll();

            expected.Should().HaveCount(3);
            expected.Should().Contain(_ => _.Title == "dummy1");
            expected.Should().Contain(_ => _.Title == "dummy2");
            expected.Should().Contain(_ => _.Title == "dummy3");
        }

        [Fact]
        public void Update_updates_category_properly()
        {
            var category = CategoryFactory.CreateCategory("Dummy");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var dto = CategoryFactory.GenerateUpdateCategoryDto("editedDummy");

            _sut.Update(category.Id, dto);

            var expected = _dataContext.Categories
                .FirstOrDefault(_ => _.Id == category.Id);
            expected.Title.Should().Be(dto.Title);
        }

        [Fact]
        public void Update_throw_DuplicateCategoryTitleException_when_update_Category()
        {
            var category1 = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category1));

            var category2 = CategoryFactory.CreateCategory("خشکبار");
            _dataContext.Manipulate(_ => _.Categories.Add(category2));

            var dto = CategoryFactory.GenerateUpdateCategoryDto("خشکبار");

            Action expected = () => _sut.Update(category1.Id, dto);

            expected.Should().ThrowExactly<DuplicateCategoryTitleException>();
        }

        [Fact]
        public void Update_throw_CategoryNotFoundException_when_category_with_given_id_is_not_exist()
        {
            var dummyCategoryId = 1000;
            var dto = CategoryFactory.GenerateUpdateCategoryDto("EditedDummy");

            Action expected = () => _sut.Update(dummyCategoryId, dto);

            expected.Should().ThrowExactly<CategoryNotFoundException>();
        }

        [Fact]
        public void Delete_delete_category_properly()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            _sut.Delete(category.Id);

            _dataContext.Categories.Should().
                NotContain(_ => _.Id == category.Id);
        }

        [Fact]
        public void Delete_throw_CategoryNotFoundException_when_category_with_given_id_is_not_exist()
        {
            var dummyCategoryId = 1000;

            Action expected = () => _sut.Delete(dummyCategoryId);

            expected.Should().ThrowExactly<CategoryNotFoundException>();
        }

        [Fact]
        public void Delete_throw_CanNotDeleteCategoryHasStuffException_when_category_has_stuff()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var stuff = StuffFactory.CreateStuff(category, "شیر");
            _dataContext.Manipulate(_ => _.Stuffs.Add(stuff));

            Action expected = () => _sut.Delete(category.Id);

            expected.Should().ThrowExactly<CanNotDeleteCategoryHasStuffException>();
        }
    }
}
