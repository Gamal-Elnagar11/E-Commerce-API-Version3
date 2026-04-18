using AutoMapper;
using E_Commerce_API.DTO.CategoryDTO;
using E_Commerce_API.Models;
using E_Commerce_API.Service.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        public CategoryController(ICategoryService categoryService , IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }





        [HttpGet("Get All Category With Products")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllWithProducts(CancellationToken ct = default)
        {
            try
            {
               var resutl = await _categoryService.GetAllCateoryWithProducts(ct);
               var map = _mapper.Map<List<CategorywithProductDTO>>(resutl);
                return Ok(map);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Get All Category")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCategory(CancellationToken ct = default)
        {
            var allcategory = await _categoryService.GetAllCateory(ct);
            var result = _mapper.Map<List<CategoriesDTO>>(allcategory);

            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id, CancellationToken ct = default)
        {
            try
            {
                var getid = await _categoryService.GetCategoryByIdAsync(id,ct);
                var map = _mapper.Map<CategorywithProductDTO>(getid);
                return Ok(map);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("Search Category")]
        [AllowAnonymous]
        public async Task<IActionResult> Search(string? name, CancellationToken ct = default)
        {
            if (name == null)
                 return BadRequest("Search value empty");
            var categories = await _categoryService.Search(name, ct);
            var result = _mapper.Map<List<CategoryName>>(categories);
            return Ok(result);
        }


        [HttpGet("Search With Products")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchWithProducts(string? name, CancellationToken ct = default)
        {
            if (name == null)
                return BadRequest("Search value empty");
            var categories = await _categoryService.SearchWithProducts(name,ct);
            var map = _mapper.Map<List<CategorywithProductDTO>>(categories);
            return Ok(map);
        }


        [HttpPost("AddCategory")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> AddCategory(CategoryName categoriesDTO, CancellationToken ct = default)
        {
            try
            {
                var map = _mapper.Map<Category>(categoriesDTO);
                var category = await _categoryService.AddCategoryAsync(map,ct);
                var result = _mapper.Map<CategoryName>(category);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
 
 
        }


        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> UpdatCategory(int id , CategoryName categoryName, CancellationToken ct = default)
        {
            try
            {
                var map = _mapper.Map<Category>(categoryName);
                var result = await _categoryService.UpdateCategoryAsync(id, map, ct);
                var endresult = _mapper.Map<CategoryName>(result);
                return Ok(endresult);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(int id, CancellationToken ct = default)
        {
            try
            {
                var result = await _categoryService.DeleteCategoryAsync(id,ct);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
