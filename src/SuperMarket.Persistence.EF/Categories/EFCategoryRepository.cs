using SuperMarket.Entities;
using SuperMarket.Services.Categories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Persistence.EF.Categories
{
    public class EFCategoryRepository : CategoryRepository
    {
        private readonly EFDataContext _dataContext;
        public EFCategoryRepository(EFDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Add(Category category)
        {
            _dataContext.Categories.Add(category);
        }

        public void Delete(Category category)
        {
            _dataContext.Categories.Remove(category);
        }

        public Category FindById(int id)
        {
            return _dataContext.Categories.Find(id);
        }

        public IList<Category> GetAll()
        {
            return _dataContext.Categories.ToList();
        }

        public IList<Category> GetCategoryWithStuffDto()
        {
            return _dataContext.Categories
                .Select(_ => new Category()
                {
                    Title = _.Title,
                    Stuffs = _.Stuffs.Select(_ => new Stuff
                    {
                        Title = _.Title,
                    }).ToHashSet(),

                }).ToList();
        }

        public bool IsExistCategoryTitle(string title)
        {
            return _dataContext.Categories.Any(_ => _.Title == title);
        }
    }
}
