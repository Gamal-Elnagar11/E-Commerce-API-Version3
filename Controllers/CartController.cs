using AutoMapper;
using E_Commerce_API.DTO.CartDTO;
using E_Commerce_API.Filters;
using E_Commerce_API.Models;
using E_Commerce_API.Service.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme ,Policy = "UserOrAdmin")]
    [ControllerFilterUserAccess]
    public class CartController : ControllerBase
    {
          private readonly ICartService _cartService;
        private readonly IMapper _mapper;

            public CartController(ICartService cartService, IMapper mapper)
            {
                _cartService = cartService;
                _mapper = mapper;
            }
         

        [HttpGet("MyCart")]
            public async Task<IActionResult> GetMyCart(CancellationToken ct = default)
            {
                try
                {
                    var cart = await _cartService.GetOrCreateCart(ct); 
                    var map = _mapper.Map<ResponseCartDTO>(cart);
                    return Ok(map);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
      
       
        
           [HttpPost("add-item-to-cart")]
            public async Task<IActionResult> AddItemToCart(int productId, int quantity, CancellationToken ct = default)
            {
                try
                {
                    var item = await _cartService.AddItemCart(productId, quantity, ct);
                     var map = _mapper.Map<CartItemDTO>(item);
                     return Ok(map);  
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
         
        
          [HttpPut("update-item-in-cart")]
            public async Task<IActionResult> UpdateItemQuantity(int productId, int newQuantity, CancellationToken ct = default)
            {
                try
                {
                    
                    var cart = await _cartService.GetOrCreateCart(ct);
                     var item = await _cartService.UpdateItemCarrQuantity(cart, productId, newQuantity,ct);
                   var map = _mapper.Map<CartItemDTO>(item);
                    return Ok(map);  
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }


              [HttpDelete("remove-item-from-cart")]
            public async Task<IActionResult> RemoveItemFromCart(int productId, CancellationToken ct = default)
            {
                try
                {
                    var cart = await _cartService.GetOrCreateCart(ct);
                     var item = await _cartService.DeleteItemFromCart(cart, productId,ct);
                  var map = _mapper.Map<CartItemDTO>(item);
                    return Ok(map);  
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }


             [HttpDelete("clear")]
            public async Task<IActionResult> ClearCart( CancellationToken ct = default)
            {
                try
                {
                    var cart = await _cartService.GetOrCreateCart(ct);
                     var clearedCart = await _cartService.ClearCart(cart, ct);
                var map = _mapper.Map<ResponseCartDTO>(clearedCart);

                    return Ok(map); 
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        

    }
}

