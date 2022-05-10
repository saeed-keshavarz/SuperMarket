using Microsoft.AspNetCore.Mvc;
using SuperMarket.Entities;
using SuperMarket.Services.Invoices.Contracts;
using System.Collections.Generic;

namespace SuperMarket.RestAPI.Controllers
{
    [Route("api/invoices")]
    [ApiController]
    public class InvoiceController : Controller
    {
        private readonly InvoiceService _service;
        public InvoiceController(InvoiceService service)
        { 
            _service = service;
        }

        [HttpPost]
        public void Add(AddInvoiceDto dto)
        {
            _service.Add(dto);
        }

        [HttpGet]
        public IList<Invoice> GetAll()
        {
            return _service.GetAllInvoices();
        }

        [HttpGet("{id}")]
        public Invoice GetInvoiceById(int id)
        {
            return _service.GetById(id);
        }

        [HttpPut("{id}")]
        public void Update(int id, UpdateInvoiceDto dto)
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
