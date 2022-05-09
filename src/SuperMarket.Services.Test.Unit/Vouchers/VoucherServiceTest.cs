using SuperMarket.Infrastructure.Application;
using SuperMarket.Infrastructure.Test;
using SuperMarket.Persistence.EF;
using SuperMarket.Persistence.EF.Vouchers;
using SuperMarket.Services.Vouchers;
using SuperMarket.Services.Vouchers.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.Test.Unit.Vouchers
{
    public class VoucherServiceTest
    {
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly VoucherService _sut;
        private readonly VoucherRepository _repository;
        public VoucherServiceTest()
        {
            _dataContext =
                           new EFInMemoryDatabase()
                           .CreateDataContext<EFDataContext>();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFVoucherRepository(_dataContext);
            _sut = new VoucherAppService(_repository, _unitOfWork);
        }


    }
}
