using SuperMarket.Entities;
using SuperMarket.Services.Vouchers.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Test.Tools.Vouchers
{
    public static class VoucherFactory
    {
        public static Voucher CreateVoucher(Stuff stuff)
        {
            return new Voucher
            {
                Title = "سند " + stuff.Title,
                Date = new DateTime(1401, 02, 18),
                Quantity = 10,
                Price = 1000,
                StuffId = stuff.Id,
            };
        }

        public static AddVoucherDto GenerateAddVoucherDto(Stuff stuff, string title)
        {
            return new AddVoucherDto
            {
                Title = title,
                Date = new DateTime(1401, 02, 18),
                Quantity = 10,
                Price = 1000,
                StuffId = stuff.Id,
            };
        }

        public static UpdateVoucherDto GenerateUpdateVoucherDto(int stuffId, string title)
        {
            return new UpdateVoucherDto
            {
                Title = title,
                Date = new DateTime(1401, 02, 20),
                Price = 2000,
                Quantity = 20,
                StuffId = stuffId,
            };
        }

        public static List<Voucher> CreateVouchersInDataBase(int stuffId)
        {
            return new List<Voucher>
            {
                new Voucher {Title="سند شیر", Date =new DateTime(1401, 02, 18), Quantity=10,StuffId=stuffId,Price=1000 },
                new Voucher {Title="سند ماست", Date =new DateTime(1401, 02, 19), Quantity=20,StuffId=stuffId,Price=2000 },
                new Voucher {Title="سند پنیر", Date =new DateTime(1401, 02, 20), Quantity=30,StuffId=stuffId,Price=3000 },
            };
        }
    }   
}
