using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Services.Stuffs.Contracts;
using SuperMarket.Services.Vouchers.Contracts;
using SuperMarket.Services.Vouchers.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.Vouchers
{
    public class VoucherAppService : VoucherService
    {
        private readonly VoucherRepository _repository;
        private readonly UnitOfWork _unitOfWork;

        public VoucherAppService(
            VoucherRepository repository,
            UnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public void Add(AddVoucherDto dto)
        {
            var stuff = _repository.GetStuffById(dto.StuffId);

            if (stuff.MaximumInventory < stuff.Inventory + dto.Quantity)
            {
                throw new InventoryMoreThanMaximumInventoryInStuffException();
            }

            stuff.Inventory += dto.Quantity;

            var voucher = new Voucher
            {
                Title = dto.Title,
                Date = dto.Date,
                Price = dto.Price,
                Quantity = dto.Quantity,
                StuffId = dto.StuffId,
            };

            _repository.Add(voucher);
            _unitOfWork.Commit();
        }

        public Voucher GetById(int id)
        {
            return _repository.FindById(id);
        }

        public IList<Voucher> GetAllVouchers()
        {
            return _repository.GetAllVouchers();
        }

        public void Update(int id, UpdateVoucherDto dto)
        {
            var voucher = _repository.FindById(id);

            if (voucher == null)
            {
                throw new VoucherNotFoundException();
            }

            if (voucher.StuffId != dto.StuffId)
            {
                var previousStuff = _repository.GetStuffById(voucher.StuffId);
                previousStuff.Inventory -= voucher.Quantity;

                var newStuff = _repository.GetStuffById(dto.StuffId);
                newStuff.Inventory += dto.Quantity;
            }
            else
            {
                var stuff = _repository.GetStuffById(dto.StuffId);
                stuff.Inventory -= voucher.Quantity;
                stuff.Inventory += dto.Quantity;
            }

            voucher.Title = dto.Title;
            voucher.Quantity = dto.Quantity;
            voucher.StuffId = dto.StuffId;
            voucher.Date = dto.Date;
            voucher.Price = dto.Price;

            _unitOfWork.Commit();
        }

        public void Delete(int id)
        {
            var voucher = _repository.FindById(id);

            if (voucher == null)
            {
                throw new VoucherNotFoundException();
            }

            var stuff = _repository.GetStuffById(voucher.StuffId);
            stuff.Inventory -= voucher.Quantity;

            _repository.Delete(voucher);
            _unitOfWork.Commit();
        }
    }
}
