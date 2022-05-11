using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Services.Categories.Contracts;
using SuperMarket.Services.Categories.Exceptions;
using System.Collections.Generic;
using System.Linq;

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

            if (isTitleDuplicate)
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

        public IList<Category> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<Category> GetAllCategoryWithStuff()
        {
            return _repository.GetCategoryWithStuffDto();
        }

        public void Update(int id, UpdateCategoryDto dto)
        {
            var isTitleDuplicate = _repository.IsExistCategoryTitle(dto.Title);

            if (isTitleDuplicate)
            {
                throw new DuplicateCategoryTitleException();
            }

            var category = _repository.FindById(id);

            if (category == null)
            {
                throw new CategoryNotFoundException();
            }
            category.Title = dto.Title;
            _unitOfWork.Commit();
        }

        public void Delete(int id)
        {
            var category = _repository.FindById(id);

            if (category == null)
            {
                throw new CategoryNotFoundException();
            }

            if (category.Stuffs.Count() > 0)
            {
                throw new CanNotDeleteCategoryHasStuffException();
            }

            _repository.Delete(category);
            _unitOfWork.Commit();
        }

        public Category GetById(int id)
        {
            return _repository.FindById(id);
        }
    }
}
