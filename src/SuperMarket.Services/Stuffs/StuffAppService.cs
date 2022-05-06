using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Services.Stuffs.Contracts;
using SuperMarket.Services.Stuffs.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.Stuffs
{
    public class StuffAppService:StuffService
    {
        private readonly StuffRepository _repository;
        private readonly UnitOfWork _unitOfWork;

        public StuffAppService(
            StuffRepository repository,
            UnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public void Add(AddStuffDto dto)
        {
            var isTitleDuplicate = _repository.IsExistStuffTitle(dto.Title);

            if (isTitleDuplicate)
            {
                throw new DuplicateStuffTitleInCategoryException();
            }

            var stuff = new Stuff
            {
                Title = dto.Title,
                Inventory = dto.Inventory,
                Unit = dto.Unit,
                MinimumInventory = dto.MinimumInventory,
                MaximumInventory = dto.MaximumInventory,
                CategoryId = dto.CategoryId,
            };
            _repository.Add(stuff);
            _unitOfWork.Commit();
        }

        public void Delete(int id)
        {
            var stuff = _repository.FindById(id);

            if(stuff.Vouchers.Count() > 0)
            {
                throw new CanNotDeleteStuffHasVoucherException();
            }
            if (stuff.Invoces.Count() > 0)
            {
                throw new CanNotDeleteStuffHasInvoiceException();
            }

            _repository.Delete(stuff);
            _unitOfWork.Commit();
        }

        public IList<Stuff> GetAllStuff()
        {
            return _repository.GetAllStuff();
        }

        public void Update(int id, UpdateStuffDto dto)
        {
            var isTitleDuplicate = _repository.IsExistStuffTitle(dto.Title);

            if (isTitleDuplicate)
            {
                throw new DuplicateStuffTitleInCategoryException();
            }
            var stuff = _repository.FindById(id);
            stuff.Title = dto.Title;
            _unitOfWork.Commit();
        }

    }
}
