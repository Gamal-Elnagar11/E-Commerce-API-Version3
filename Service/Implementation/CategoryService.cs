using E_Commerce_API.Models;
using E_Commerce_API.Service.Interface;
using E_Commerce_API.UnitOfWork;

namespace E_Commerce_API.Service.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
 



        public async Task<Category> AddCategoryAsync(Category category, CancellationToken ct = default)
        {
            try
            {


                await _unitOfWork.CategoryRepo.AddAsync(category,ct);
                await _unitOfWork.CompleteAsync(ct);
                return category;
            }
            catch (Exception ex)
            {
                throw new Exception("Add Category has Proble",ex);
            }
        }

        public async Task<Category> DeleteCategoryAsync(int id, CancellationToken ct = default)
        {
            var category = await _unitOfWork.CategoryRepo.GeCategoriesByIdAsync(id,ct);
            if (category == null)
                throw new ArgumentException("Category ID Not Found");

            if (category.Products.Any())
                throw new ArgumentException("This Category has Products");


            // _unitOfWork.CategoryRepo.DeleteCategory(findid);
            category.IsDeleted = true;
              await _unitOfWork.CompleteAsync(ct);
            return category;

        }

        
         
        public  async Task<List<Category>> GetAllCateory(CancellationToken ct = default)
        {
             return  await _unitOfWork.CategoryRepo.GetAllCategoriesAsync(ct);
         }

        public async Task<List<Category>> GetAllCateoryWithProducts(CancellationToken ct = default)
        {
            return await _unitOfWork.CategoryRepo.GetAllCategoriesWithProducts(ct);
        }

        public async Task<Category> GetCategoryByIdAsync(int id, CancellationToken ct = default)
        {
            var result = await _unitOfWork.CategoryRepo.GeCategoriesByIdAsync(id,ct);
            if (result == null)
                throw new ArgumentException("Category ID Not Found");
            if (result.IsDeleted == true)
                throw new ArgumentException("This Category was Deleted");


            return result;
        }

        public async Task<List<Category>> Search(string name, CancellationToken ct = default)
        {
            return await _unitOfWork.CategoryRepo.Search(name,ct);
        }

        public async Task<List<Category>> SearchWithProducts(string name, CancellationToken ct = default)
        {
            return await _unitOfWork.CategoryRepo.SearchwithProducts(name,ct);
        }

        public async Task<Category> UpdateCategoryAsync(int id, Category category, CancellationToken ct = default)
        {
            var findid = await _unitOfWork.CategoryRepo.GeCategoriesByIdAsync(id,ct);
            if (findid == null)
                throw new ArgumentException("Category ID Not Found");

            findid.Name = category.Name;
            

              _unitOfWork.CategoryRepo.UpdateCategory(findid);
             await _unitOfWork.CompleteAsync(ct);
            return findid;

        }

       
    }
}
