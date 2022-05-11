using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using System.Collections.Generic;

namespace SuperMarket.Services.Vouchers.Contracts
{
    public interface VoucherService : Service
    {
        void Add(AddVoucherDto dto);
        void Update(int id, UpdateVoucherDto dto);
        IList<Voucher> GetAllVouchers();
        void Delete(int id);
        Voucher GetById(int id);
    }
}
