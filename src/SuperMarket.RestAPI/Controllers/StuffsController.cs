using Microsoft.AspNetCore.Mvc;
using SuperMarket.Entities;
using SuperMarket.Services.Stuffs.Contracts;
using System.Collections.Generic;

namespace SuperMarket.RestAPI.Controllers
{
    [Route("api/stuffs")]
    [ApiController]
    public class StuffsController : Controller
    {
        private readonly StuffService _service;

        public StuffsController(StuffService service)
        {
            _service = service;
        }

        [HttpPost]
        public void Add(AddStuffDto dto)
        {
            _service.Add(dto);
        }

        [HttpGet]
        public IList<Stuff> GetAll()
        {
            return _service.GetAllStuff();
        }

        [HttpGet("{id}")]
        public Stuff GetStuffById(int id)
        {
            return _service.GetById(id);
        }

        [HttpPut("{id}")]
        public void Update(int id, UpdateStuffDto dto)
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
