using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Services.Categories.Contracts;
using SuperMarket.Services.Categories.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.Categories
{
    public class CategoryAppService : CategoryService
    {
        private readonly CategoryRepository _repository;
        private readonly UnitOfWork _unitOfWork;

        public CategoryAppService(
            CategoryRepository repository,
            UnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public void Add(AddCategoryDto dto)
        {
            var isTitleDuplicate = _repository.IsExistCategoryTitle(dto.Title);

            if(isTitleDuplicate)
            {
                throw new DuplicateCategoryTitleException();
            }
            var category = new Category
            {
                Title = dto.Title,
            };
            _repository.Add(category);
            _unitOfWork.Commit();
        }
    }
}
