using SuperMarket.Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.Reports.Contracts
{
    public interface ReportService:Service
    {
        GetProfitByStuffDto GetProfitByStuff(int stuffId, DateTime start, DateTime end);
        GetProfitByCategoryDto GetProfitByCategory(int categpryId, DateTime start, DateTime end);
        GetTotalProfitDto GetTotalProfit(DateTime start, DateTime end);
    }
}
