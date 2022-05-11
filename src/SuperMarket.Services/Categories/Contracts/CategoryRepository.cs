using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using System.Collections.Generic;

namespace SuperMarket.Services.Categories.Contracts
{
    public interface CategoryRepository : Repository
    {
        void Add(Category category);
        bool IsExistCategoryTitle(string title);
        Category FindById(int id);
        IList<Category> GetAll();
        IList<Category> GetCategoryWithStuffDto();
        void Delete(Category category);
    }
}
