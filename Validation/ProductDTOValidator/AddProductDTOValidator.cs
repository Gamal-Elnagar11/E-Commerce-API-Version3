using E_Commerce_API.DTO.ProductDTO;
using FluentValidation;

namespace E_Commerce_API.Validation.ProductDTOValidator
{
    public class AddProductDTOValidator : AbstractValidator<AddProductDTO>
    {
        public AddProductDTOValidator()
        {
            RuleFor(a => a.Name)
                .NotEmpty().WithMessage("Product Name is Required")
                .Length(1, 100).WithMessage("Product Name must bettwen 1 and 100 carachter");

            RuleFor(a => a.Description)
                .NotEmpty().WithMessage("Descriptiopn is required");

            RuleFor(a => a.CategoryId)
                .NotEmpty().WithMessage("CategoryId is required");

            RuleFor(a => a.Stock)
                .GreaterThan(0).WithMessage("Stock must be grater than 0");

            RuleFor(a => a.Price)
                .GreaterThan(0).WithMessage("Price must be grater than 0");

 

            RuleFor(a => a.Image)
                .NotEmpty().WithMessage("Must be uplead Image")
                .Must(ValidSize).WithMessage("Size Grater than 2MB")
                .Must(ValidExtention).WithMessage("Not Allowed the extention must be (jpg , jpeg , png)");




        }
        private bool BeValidURL(string? url)
       => Uri.TryCreate(url, UriKind.Absolute, out _);

        private bool ValidSize(IFormFile file)
        {
            if(file == null) return false;
            return file.Length <= 2 * 1024 * 1024;  // 2MB
        }

        private bool ValidExtention(IFormFile file)
        {
            if(file == null) return false;
            var extentioins = new[] { ".jpg", ".jpeg", ".png" };
            var extentioin = Path.GetExtension(file.FileName).ToLower();
            return extentioins.Contains(extentioin);
        }



    }
}
