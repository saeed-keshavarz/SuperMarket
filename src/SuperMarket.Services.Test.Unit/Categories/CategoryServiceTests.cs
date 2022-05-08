using FluentAssertions;
using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Infrastructure.Test;
using SuperMarket.Persistence.EF;
using SuperMarket.Persistence.EF.Categories;
using SuperMarket.Services.Categories;
using SuperMarket.Services.Categories.Contracts;
using SuperMarket.Services.Categories.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            AddCategoryDto dto = GenerateAddCategoryDto();

            _sut.Add(dto);

            _dataContext.Categories.Should()
                .Contain(_ => _.Title == dto.Title);
        }

        [Fact]
        public void Add_throw_DuplicateCategoryTitleException_when_add_new_Category()
        {
            var category = CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));
            AddCategoryDto dto = GenerateAddCategoryDto();

            Action expected = () => _sut.Add(dto);

            expected.Should().Throw<DuplicateCategoryTitleException>();
        }


        [Fact]
        public void GetAll_returns_all_categories()
        {
            CreateCategoriesInDataBase();

            var expected = _sut.GetAll();

            expected.Should().HaveCount(3);
            expected.Should().Contain(_ => _.Title == "dummy1");
            expected.Should().Contain(_ => _.Title == "dummy2");
            expected.Should().Contain(_ => _.Title == "dummy3");
        }

        [Fact]
        public void Update_updates_category_properly()
        {
            var category = CreateCategory("Dummy");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var dto = GenerateUpdateCategoryDto("editedDummy");

            _sut.Update(category.Id, dto);

            var expected = _dataContext.Categories
                .FirstOrDefault(_ => _.Id == category.Id);
            expected.Title.Should().Be(dto.Title);
        }

        [Fact]
        public void Update_throw_DuplicateCategoryTitleException_when_update_Category()
        {
            var category1 = CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category1));

            var category2 = CreateCategory("خشکبار");
            _dataContext.Manipulate(_ => _.Categories.Add(category2));

            var dto = GenerateUpdateCategoryDto("خشکبار");

            Action expected = () => _sut.Update(category1.Id, dto);

            expected.Should().ThrowExactly<DuplicateCategoryTitleException>();

        }

        [Fact]
        public void Update_throw_CategoryNotFoundException_when_category_with_given_id_is_not_exist()
        {
            var dummyCategoryId = 1000;
            var dto = GenerateUpdateCategoryDto("EditedDummy");

            Action expected = () => _sut.Update(dummyCategoryId, dto);

            expected.Should().ThrowExactly<CategoryNotFoundException>();
        }

        private static UpdateCategoryDto GenerateUpdateCategoryDto(string title)
        {
            return new UpdateCategoryDto
            {
                Title = title,
            };
        }

        public static Category CreateCategory(string title)
        {
            return new Category
            {
                Title = title
            };
        }

        private void CreateCategoriesInDataBase()
        {
            var categories = new List<Category>
            {
                new Category { Title = "dummy1"},
                new Category { Title = "dummy2"},
                new Category { Title = "dummy3"}
            };
            _dataContext.Manipulate(_ =>
            _.Categories.AddRange(categories));
        }
        private static AddCategoryDto GenerateAddCategoryDto()
        {
            return new AddCategoryDto
            {
                Title = "لبنیات"
            };
        }
    }
}
