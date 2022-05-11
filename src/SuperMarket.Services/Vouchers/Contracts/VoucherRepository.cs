using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using System.Collections.Generic;

namespace SuperMarket.Services.Vouchers.Contracts
{
    public interface VoucherRepository : Repository
    {
        void Add(Voucher voucher);
        Stuff GetStuffById(int stuffId);
        Voucher FindById(int id);
        IList<Voucher> GetAllVouchers();
        void Delete(Voucher voucher);
    }
}
