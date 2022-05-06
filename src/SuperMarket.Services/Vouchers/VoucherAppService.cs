using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Services.Stuffs.Contracts;
using SuperMarket.Services.Vouchers.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.Vouchers
{
    public class VoucherAppService:VoucherService
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
    }
}
