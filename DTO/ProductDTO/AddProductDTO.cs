using E_Commerce_API.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce_API.DTO.ProductDTO
{
    public class AddProductDTO
    {
         public string Name { get; set; }
          public string Description { get; set; }
        public decimal Price { get; set; }
         public IFormFile Image { get; set; }
         public int Stock { get; set; }
        public int CategoryId { get; set; }


    }
}
