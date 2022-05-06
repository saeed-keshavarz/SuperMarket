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
    }
}
