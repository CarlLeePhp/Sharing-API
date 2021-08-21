using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sharing_API.Data;
using Sharing_API.DTOs;
using Sharing_API.Interface;
using Sharing_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sharing_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ICategoryRepository _categoryRepository;
        public CategoriesController( DataContext context, ICategoryRepository categoryRepository)
        {
            _context = context;
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            var categories = await _categoryRepository.GetCategoriesAsync();
            return Ok(categories);

        }
        [HttpPost]
        public async Task<ActionResult> AddCategory(CategoryDto categoryDto)
        {
            Category category = new Category();
            category.Description = categoryDto.Description;
            _context.Categories.Add(category);
            if(await _context.SaveChangesAsync() > 0)
            return NoContent();

            return BadRequest("Category was created failed");
        }
        // For what?
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetUser(int id)
        {
            return await _categoryRepository.GetCategoryByIdAsync(id);

        }

       
    }
}
