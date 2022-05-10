using Microsoft.AspNetCore.Mvc;
using SuperMarket.Entities;
using SuperMarket.Services.Vouchers.Contracts;
using System.Collections.Generic;

namespace SuperMarket.RestAPI.Controllers
{
    [Route("api/vouchers")]
    [ApiController]
    public class VouchersController : Controller
    {
        private readonly VoucherService _service;

        public VouchersController(VoucherService service)
        {
            _service = service;
        }

        [HttpPost]
        public void Add(AddVoucherDto dto)
        {
            _service.Add(dto);
        }

        [HttpGet]
        public IList<Voucher> GetAll()
        {
            return _service.GetAllVouchers();
        }

        [HttpGet("{id}")]
        public Voucher GetVoucherById(int id)
        {
            return _service.GetById(id);
        }

        [HttpPut("{id}")]
        public void Update(int id, UpdateVoucherDto dto)
        {
            _service.Update(id, dto);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _service.Delete(id);
        }
    }
}
