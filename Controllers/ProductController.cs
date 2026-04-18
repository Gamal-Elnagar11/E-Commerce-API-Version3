using AutoMapper;
using E_Commerce_API.DTO.ProductDTO;
using E_Commerce_API.Filters;
using E_Commerce_API.Models;
using E_Commerce_API.Service.Interface;
using E_Commerce_API.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
 using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [GlobalExceptionFilter]
    public class ProductController : ControllerBase
    {

        private readonly IProductService _productService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductController(IUnitOfWork unitOfWork, IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }



        [HttpGet("Products")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllProduct(CancellationToken ct = default)
        {

                var resutl1 = await _productService.GetAllProducts().ToListAsync(ct);
                var result2 = _mapper.Map<List<ResponseProduct>>(resutl1);
                return Ok(result2);
        }


        [HttpGet("Search")]
        [AllowAnonymous]
        public async Task<IActionResult> Search(string? name, CancellationToken ct = default)
        {

            if (string.IsNullOrEmpty(name))
                return BadRequest("Search Value is required"); 

            var getproduct = await _productService.Search(name,ct);
                var result = _mapper.Map<List<ProductBaseDTO>>(getproduct);
                return Ok(result);
            
        }


        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductById(int id, CancellationToken ct = default)
        {
           
                var findid = await _productService.GetProductByIdAsync(id,ct);
                if (findid == null)
                    return NotFound("Product ID Not Found");

                var resutl = _mapper.Map<ResponseProduct>(findid);

                return Ok(resutl);
            
        }



        [HttpPost("AddProduct")]
        [ActionFilter]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> AddProduct(AddProductDTO productDTO, CancellationToken ct = default)
        {
            
                var map = _mapper.Map<Product>(productDTO);
                 var result =  await _productService.AddProductAsync(map,ct);

                  var endresult = _mapper.Map<ResponseProduct>(result);
 
                return Ok(endresult);
           

        }



        [HttpPut("{id}")]
        [ActionFilter]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct([FromRoute]int id ,UpdateProductDTO updateDTO, CancellationToken ct = default)
        {
             var findid = await _productService.GetProductByIdAsync(id,ct);
                if (findid == null)
                    return NotFound("Product ID Not Found");

                var findCategory = await _productService.CategoryExistsAsync(updateDTO.CategoryId,ct);
                if (!findCategory)
                    return NotFound("Category ID Not Found");

                _mapper.Map(updateDTO , findid);

                var result = await _productService.UpdateProductAsync(id, findid,ct);

                 var map = _mapper.Map<ResponseProduct>(result);

                return Ok(map);
            
        }




        [HttpPut("/stock/{id}")]
        [ActionFilter]
       // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> UpdateQuantity(int id , int stock, CancellationToken ct = default)
        {
             
                await _productService.UpdateStock(id, stock,ct);
                return NoContent();

            
        }


        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int id, CancellationToken ct = default)
        {
            var findid = await _productService.GetProductByIdAsync(id,ct);
            if (findid == null)
                return NotFound("ID For Product Not Found");

            var result = await _productService.DeleteProductAsync(id,ct);
            var result2 = _mapper.Map<ResponseProduct>(result);
            return Ok(result2);


        }


    }
}
