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

        public void Add(AddVoucherDto dto, int stuffId)
        {
            var voucher = new Voucher
            {
                Title = dto.Title,
                Date = dto.Date,
                Price = dto.Price,
                Quantity = dto.Quantity,
                StuffId = dto.StuffId,
            };

            _repository.Add(voucher);

            var stuff = _repository.GetStuffById(stuffId);
            stuff.Inventory += dto.Quantity;

            _unitOfWork.Commit();
        }

        public void Delete(int id, int stuffId, int quantity)
        {
            var voucher = _repository.FindById(id);

            if (voucher == null)
            {
                throw new VoucherNotFoundException();
            }

            var stuff = _repository.GetStuffById(stuffId);
            stuff.Inventory -= quantity;

            _repository.Delete(voucher);
            _unitOfWork.Commit();
        }

        public IList<Voucher> GetAllVouchers()
        {
            return _repository.GetAllVouchers();
        }

        public void Update(int id, UpdateVoucherDto dto, int stuffId, int quantity)
        {
            var voucher = _repository.FindById(id);

            if (voucher == null)
            {
                throw new VoucherNotFoundException();
            }

            voucher.Quantity = dto.Quantity;
            voucher.StuffId = dto.StuffId;
            voucher.Date = dto.Date;
            voucher.Price = dto.Price;

            if (stuffId != dto.StuffId)
            {
                var previousStuff = _repository.GetStuffById(stuffId);
                previousStuff.Inventory -= quantity;

                var newStuff = _repository.GetStuffById(dto.StuffId);
                newStuff.Inventory += dto.Quantity;
            }
            else
            {
                var stuff = _repository.GetStuffById(stuffId);
                stuff.Inventory -= quantity;
                stuff.Inventory += dto.Quantity;
            }

            _unitOfWork.Commit();
        }
    }
}
