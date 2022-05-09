using SuperMarket.Entities;
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
    }
}
