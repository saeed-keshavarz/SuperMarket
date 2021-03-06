using SuperMarket.Entities;
using SuperMarket.Services.Vouchers.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace SuperMarket.Persistence.EF.Vouchers
{
    public class EFVoucherRepository : VoucherRepository
    {
        private readonly EFDataContext _dataContext;

        public EFVoucherRepository(EFDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Add(Voucher voucher)
        {
            _dataContext.Vouchers.Add(voucher);
        }

        public Voucher FindById(int id)
        {
            return _dataContext.Vouchers.Find(id);
        }

        public IList<Voucher> GetAllVouchers()
        {
            return _dataContext.Vouchers.ToList();
        }

        public void Delete(Voucher voucher)
        {
            _dataContext.Vouchers.Remove(voucher);
        }

        public Stuff GetStuffById(int stuffId)
        {
            return _dataContext.Stuffs.Find(stuffId);
        }
    }
}
