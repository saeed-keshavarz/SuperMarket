using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using System.Collections.Generic;

namespace SuperMarket.Services.Categories.Contracts
{
    public interface CategoryService : Service
    {
        void Add(AddCategoryDto dto);
        void Update(int id, UpdateCategoryDto dto);
        IList<Category> GetAll();
        IList<Category> GetAllCategoryWithStuff();
        void Delete(int id);
        Category GetById(int id);
    }
}
