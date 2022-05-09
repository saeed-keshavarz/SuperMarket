using Microsoft.AspNetCore.Mvc;
using SuperMarket.Entities;
using SuperMarket.Services.Categories.Contracts;
using System.Collections.Generic;

namespace SuperMarket.RestAPI.Controllers
{
    [Route("api/catgeories")]
    [ApiController]
    public class CategoriesController : Controller
    {
        private readonly CategoryService _service;
        public CategoriesController(CategoryService service)
        {
            _service = service;
        }

        [HttpPost]
        public void Add(AddCategoryDto dto)
        {
            _service.Add(dto);
        }

        [HttpGet]
        public IList<Category> GetAll()
        {
            return _service.GetAll();
        }

        [HttpGet]
        public IList<Category> GetAllCategoryWithStuff()
        {
            return _service.GetAllCategoryWithStuff();
        }

        [HttpPut("{id}")]
        public void Update(int id, UpdateCategoryDto dto)
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
