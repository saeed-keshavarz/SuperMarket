using Microsoft.AspNetCore.Mvc;
using SuperMarket.Services.Reports.Contracts;
using System;

namespace SuperMarket.RestAPI.Controllers
{
    [Route("api/reports")]
    [ApiController]
    public class ReportController : Controller
    {
        private readonly ReportService _service;

        public ReportController(ReportService service)
        {
            _service = service;
        }

        [HttpGet("stuff/{id}/{start}/{end}")]
        public GetProfitByStuffDto GetProfitDateRange(int id,
            DateTime start, DateTime End)
        {
            return _service.GetProfitByStuff(id, start, End);
        }

        [HttpGet("category/{id}/{start}/{end}")]
        public GetProfitByCategoryDto GetProfitByCategory(int id,
            DateTime start, DateTime End)
        {
            return _service.GetProfitByCategory(id, start, End);
        }

        [HttpGet("{start}/{end}")]
        public GetTotalProfitDto GetTotalProfit(DateTime start, DateTime End)
        {
            return _service.GetTotalProfit(start, End);
        }
    }
}


