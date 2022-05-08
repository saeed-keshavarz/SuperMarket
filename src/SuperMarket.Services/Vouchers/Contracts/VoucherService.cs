using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.Vouchers.Contracts
{
    public interface VoucherService : Service
    {
        void Add(AddVoucherDto dto, int stuffId);
        void Update(int id, UpdateVoucherDto dto, int stuffId, int quantity);
        IList<Voucher> GetAllVouchers();
        void Delete(int id, int stuffId, int quantity);
    }
}
