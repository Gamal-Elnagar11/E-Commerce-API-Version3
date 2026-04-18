using E_Commerce_API.Models;
using E_Commerce_API.Service.Interface;
using E_Commerce_API.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_API.Service.Implementation
{
    public class ProductService : IProductService
    {
        private new List<string> _allowedExtention = new List<string> { ".jpg", ".png" };
 


        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        public async Task<Product> AddProductAsync(Product product, CancellationToken ct = default)
        {
              await _unitOfWork.BeginTransactionAsync(ct);
            string SavedFilePath = null;
            try
            {
                if (string.IsNullOrEmpty(product.Name))
                    throw new ArgumentException("Product name is required");
                if (product.Price < 0)
                    throw new ArgumentException("Price must be grater than zero");
                if (product.Stock < 0)
                    throw new ArgumentException("Stock must be grater than zero");

                var category = await _unitOfWork.Repositoey<Category>().GetByIdAsync(product.CategoryId,ct);
                if (category == null)
                    throw new KeyNotFoundException("Category Id Invalid ");


                if (product.Image != null)
                {
                    if (!product.Image.ContentType.StartsWith("image/"))
                        throw new ArgumentException("Invalid file type");

                    if (product.Image.Length > 2_000_000) // 2MB
                        throw new ArgumentException("File too large");


                    if (!_allowedExtention.Contains(Path.GetExtension(product.Image.FileName).ToLower()))    // here get path from image and must contian path .png or .jpg
                        throw new ArgumentException("Only .png and .jpg images are allwoed");

                    // اختر اسم للملف (مثلاً استخدام الوقت أو GUID لتجنب التعارض)
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(product.Image.FileName);

                    // مسار التخزين على السيرفر (wwwroot/images)
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    // احفظ الصورة فعليًا
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await product.Image.CopyToAsync(stream);
                    }

                    SavedFilePath = filePath;
                     product.ImageUrl = "/images/" + fileName;



                }


                var result = await _unitOfWork.ProductRepo.AddAsync(product,ct);    //Repositoey<Product>().AddAsync(product);
                await _unitOfWork.CompleteAsync(ct);
                await _unitOfWork.CommmetTransactionAsync(ct);
                 return result;
            }
            catch(Exception ex)
            {
                if (!string.IsNullOrEmpty(SavedFilePath)
                   && File.Exists(SavedFilePath))
                {
                    File.Delete(SavedFilePath);
                }
                await _unitOfWork.RollebackAsync(ct);
                throw;
            }
        }

          
        public async Task<Product> DeleteProductAsync(int id, CancellationToken ct = default)
        {
            await _unitOfWork.BeginTransactionAsync(ct);
            try
            {
                var result = await _unitOfWork.Repositoey<Product>().GetByIdAsync(id,ct);
                if (result == null)
                    throw new KeyNotFoundException("Product Not Found");

                _unitOfWork.Repositoey<Product>().Delete(result);
                await _unitOfWork.CompleteAsync(ct);

                if (!string.IsNullOrEmpty(result.ImageUrl))
                {
                    // احصل على المسار الكامل للصورة
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", result.ImageUrl.TrimStart('/'));

                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
                await _unitOfWork.CommmetTransactionAsync(ct);    
                return result;
            }
            catch(Exception ex)
            {
                await _unitOfWork.RollebackAsync(ct);
                throw;
            }
        }

       

        public async Task<Product> GetProductByIdAsync(int id, CancellationToken ct = default)
        {
             var product = await _unitOfWork.Repositoey<Product>()
                                  .GetAll()                     // IQueryable
                                  .Include(p => p.Category)     // Include Relations
                                  .FirstOrDefaultAsync(p => p.Id == id,ct);
 
            return product;


        }



        public IQueryable<Product> GetAllProducts()
        {
            return _unitOfWork.Repositoey<Product>().GetAll().Include(a => a.Category);
           
        }


        public async Task<Product> UpdateProductAsync(int id, Product product, CancellationToken ct = default)
             
        {
            var existid = await _unitOfWork.Repositoey<Product>().GetByIdAsync(id,ct);
             
            // خزّن مسار الصورة القديمة قبل التعديل
            string oldImagePath = existid.ImageUrl != null ?
                                  Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existid.ImageUrl.TrimStart('/'))
                                  : null;

            existid.Name = product.Name;
            existid.Description = product.Description;
            existid.CategoryId = product.CategoryId;
            existid.Price = product.Price;
            existid.Stock = product.Stock;

            string newImagePath = null;

            if (product.Image != null)
            {
                if (!product.Image.ContentType.StartsWith("image/"))
                    throw new KeyNotFoundException("Invalid file type");

                if (product.Image.Length > 2_000_000) // 2MB
                    throw new ArgumentException("File too large");


                if (!_allowedExtention.Contains(Path.GetExtension(product.Image.FileName).ToLower()))    // here get path from image and must contian path .png or .jpg
                    throw new ArgumentException("Only .png and .jpg images are allwoed");

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(product.Image.FileName);
                newImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                using (var stream = new FileStream(newImagePath, FileMode.Create))
                {
                    await product.Image.CopyToAsync(stream);
                }

                // خزّن المسار الجديد
                existid.ImageUrl = "/images/" + fileName;
            }

            try
            {
                _unitOfWork.Repositoey<Product>().Update(existid);
                await _unitOfWork.CompleteAsync(ct);

                // احذف الصورة القديمة بعد نجاح DB
                if (oldImagePath != null && File.Exists(oldImagePath))
                    File.Delete(oldImagePath);

                return existid;
            }
            catch (Exception ex) 
            {
                // امسح الصورة الجديدة لو حصل خطأ
                if (newImagePath != null && File.Exists(newImagePath))
                    File.Delete(newImagePath);

                throw;
            }
        }


        public async Task<bool> CategoryExistsAsync(int categoryId, CancellationToken ct = default)
        {
            return await _unitOfWork.Repositoey<Category>()
                                     .GetAll()
                                     .AnyAsync(c => c.Id == categoryId, ct);
        }

        public async Task<List<Product>> Search(string name, CancellationToken ct = default)
        {
            
                return await _unitOfWork.Repositoey<Product>().GetAll().Where(p => p.Name.Contains(name)).ToListAsync(ct);
            
        }

        public async Task<Product> UpdateStock(int id ,int stock, CancellationToken ct = default)
        {
            var product = await _unitOfWork.Repositoey<Product>().GetByIdAsync(id, ct);
            if (product == null)
                throw new KeyNotFoundException("Product Not Found");

            if (stock < 0)
                throw new ArgumentException("Stock cannot be nagative");

            product.Stock = stock;
            await _unitOfWork.CompleteAsync(ct);
            return product;

        }

         
    }
}
