using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.Vouchers.Contracts
{
    public interface VoucherRepository : Repository
    {
        void Add(Voucher voucher);
        Stuff GetStuffById(int stuffId);
        Voucher FindById(int id);
        IList<Voucher> GetAllVouchers();
    }
}
