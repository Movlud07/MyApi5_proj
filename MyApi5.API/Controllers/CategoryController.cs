using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApi5.Business.abstracts;
using MyApi5.Business.Errors;
using MyApi5.Entities.concretes;
using MyApi5.Entities.DTOs.CategoryDtos;
using System.Linq.Expressions;

namespace MyApi5.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;
        public CategoryController(ICategoryService service)
        {
            _service = service;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            if (id <= 0)
            {
                ModelState.AddModelError("Id", "Id must be greater than zero.");
                return BadRequest();
            }
            var category = await _service.GetById(id);
            return Ok(category);
        }
        [HttpGet("by-name/{name}")]
        public async Task<ActionResult> Get(string name)
        {
            if (name is null)
            {
                ModelState.AddModelError("name", "we need more than you write or not");
                return BadRequest();
            }
            var product = await _service.GetByName(name);
            return Ok(product);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _service.GetAll(includes: "Products");
            return Ok(categories);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] CategoryPostDto postDto)
        {
            await _service.Add(postDto);
            return StatusCode(200);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromForm] CategoryUpdateDto updateDto)
        {
            if (id <= 0)
            {
                return BadRequest(new { message = "Kateqoriya ID-si sıfırdan böyük olmalıdır." }); 
            }
            await _service.Update(id, updateDto);
            return NoContent();
            //catch(ElementNotExistException e)
            //{
            //    return NotFound(new { message = e.Message});
            //}
            //catch(DublicateException e)
            //{
            //    return BadRequest(new {message = e.Message});
            //}

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { message = "Id 0 -dan kiçik ola bilməz." });
            }

            await _service.Delete(id);
            return Ok();
        }
    }
}
