using SuperMarket.Entities;
using SuperMarket.Services.Categories.Contracts;
using System.Collections.Generic;

namespace Supermarket.Test.Tools.Categories
{
    public static class CategoryFactory
    {
        public static Category CreateCategory(string title)
        {
            return new Category
            {
                Title = title
            };
        }

        public static AddCategoryDto GenerateAddCategoryDto()
        {
            return new AddCategoryDto
            {
                Title = "لبنیات"
            };
        }

        public static UpdateCategoryDto GenerateUpdateCategoryDto(string title)
        {
            return new UpdateCategoryDto
            {
                Title = title,
            };
        }

        public static List<Category> CreateCategoriesInDataBase()
        {
            return new List<Category>
            {
                new Category { Title = "dummy1"},
                new Category { Title = "dummy2"},
                new Category { Title = "dummy3"}
            };
        }
    }
}
