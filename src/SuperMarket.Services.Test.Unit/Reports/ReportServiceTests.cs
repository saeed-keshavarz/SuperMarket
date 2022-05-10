using SuperMarket.Infrastructure.Test;
using SuperMarket.Persistence.EF;
using SuperMarket.Persistence.EF.Reports;
using SuperMarket.Services.Reports;
using SuperMarket.Services.Reports.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.Test.Unit.Reports
{
    public class ReportServiceTests
    {
        private readonly EFDataContext _dataContext;
        private readonly ReportService _sut;
        private readonly ReportRepository _repository;

        public ReportServiceTests()
        {
            _dataContext =
                           new EFInMemoryDatabase()
                           .CreateDataContext<EFDataContext>();
            _repository = new EFReportRepository(_dataContext);
            _sut = new ReportAppService(_repository);
        }
    }
}
