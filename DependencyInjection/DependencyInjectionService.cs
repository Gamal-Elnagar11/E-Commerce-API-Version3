using E_Commerce_API.Reposatory.Implementation;
using E_Commerce_API.Reposatory.Interface;
using E_Commerce_API.Service.Implementation;
using E_Commerce_API.Service.Interface;
using E_Commerce_API.Static;
using E_Commerce_API.UnitOfWork;

namespace E_Commerce_API.DependencyInjection
{
    public static class DependencyInjectionService
    {
        public static IServiceCollection DIService(this IServiceCollection Services)
        {
            Services.AddScoped<JWTService>();
            Services.AddScoped<IUnitOfWork, UnitOfWork_Implement>();
            Services.AddScoped<IProductService, ProductService>();
            Services.AddScoped<ICategoryService, CategoryService>();
             Services.AddScoped<ICartService, CartService>();
             Services.AddScoped<IOrderService, OrderService>();
             Services.AddScoped<IOrderRepo, OrderRepo>();
             Services.AddScoped<IFeedbackService, FeedbackService>();
             Services.AddScoped<IdentityUsers>();
             Services.AddHttpClient();
            return Services;
        }
    }
}
