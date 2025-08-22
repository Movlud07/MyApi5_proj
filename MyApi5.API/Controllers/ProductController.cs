using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApi5.Business.abstracts;
using MyApi5.Business.Errors;
using MyApi5.DataAccess;
using MyApi5.Entities.concretes;
using MyApi5.Entities.DTOs.ProductDtos;
using System.Linq.Expressions;

namespace MyApi5.API.Controllers
{
    //[Authorize(Roles = "User")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;
        public ProductController(IProductService service)
        {
            _service = service;
        }
        [HttpGet("by-id/{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { message = "Id 0 -dan boyuk olmalidir." });
            }
            var product = await _service.GetById(id);
            return Ok(product);
        }
        [HttpGet("by-name/{name}")]
        public async Task<IActionResult> Get(string name)
        {
            if (name == null)
            {
                return BadRequest(new { message = "Name can not be empty or null" });
            }
            var product = await _service.GetByName(name);
            return Ok(product);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _service.GetAll(includes: "Category");
            return Ok(products);
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ProductPostDto postDto)
        {
            await _service.Add(postDto);
            return StatusCode(200);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] ProductUpdateDto updateDto)
        {
            if (id <= 0)
            {
                return BadRequest(new { code = 400, message = "Id can not be less than zero." });
            }
            await _service.Update(id, updateDto);
            return StatusCode(200);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { statusCode = 400, message = "Id can not be less than zero." });
            }
            await _service.Delete(id);
            return StatusCode(200);
        }
    }
}
