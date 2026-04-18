
using E_Commerce_API.Controllers;
using E_Commerce_API.Data;
using E_Commerce_API.DependencyInjection;
using E_Commerce_API.Filters;
using E_Commerce_API.Middelwares;
using E_Commerce_API.Models;
 using E_Commerce_API.Static;
using E_Commerce_API.Validation.ProductDTOValidator;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json.Serialization;



namespace E_Commerce_API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
           JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<Application>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("conn")));

            builder.Services.AddProblemDetails();
            builder.Services.AddExceptionHandler<GlobalErrorHandling>();


             builder.Services.AddFluentValidationAutoValidation();
             builder.Services.AddValidatorsFromAssemblyContaining<Program>();

            builder.Services.AddControllers(
                op => op.Filters.Add<GlobalFilter>()
                //op => op.Filters.Add<GlobalExeptionFilter>()

                )

            .AddJsonOptions(options =>    // convert enum from number to string 
            {
                options.JsonSerializerOptions.Converters
                    .Add(new JsonStringEnumConverter());

            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(opt =>
            {
            
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });
                // here create jwt in header for evvery endpoint
                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });

            opt.SwaggerDoc("v1", new OpenApiInfo()   // here V1 -> version 1 to api after audated on this you can set V2
            {
                Version = "v1",
                Title = "E-Commerce API",
                Description = "api to manage project e-commerce",
                TermsOfService = new Uri("http://tempuri.gamal"),    // here linke to page has info to api
                Contact = new OpenApiContact                         // here info to owner
                {
                    Name = "Gamal saeed",
                    Email = "gamalelnagar@gmail.com"
                }

            });
        });

             // 1. أولاً: ضيف الـ Identity
            builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<Application>()
                .AddDefaultTokenProviders();

            // 2. ثانياً: ضيف الـ Authentication بتاع الـ JWT (عشان يغطي على إعدادات الكوكيز بتاعة الـ Identity)
            var jwtSettings = builder.Configuration.GetSection("JWT");
            
            
             builder.Services.AddAuthentication(options =>
             {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,                     // تحقق من الـ Issuer
                    ValidateAudience = true,                   // تحقق من الـ Audience
                    ValidateLifetime = true,                    // تحقق من انتهاء الصلاحية
                    ValidateIssuerSigningKey = true,           // تحقق من التوقيع
                    ValidIssuer = jwtSettings["Issuer"],       // من appsettings.json
                    ValidAudience = jwtSettings["Audience"],   // من appsettings.json
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings["SecretKey"])),
                    RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                    NameClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/name"
                };
            });

            builder.Services.DIService();
            builder.Services.AddAuthorization();
            builder.Services.AddHttpContextAccessor();


            builder.Services.AddAutoMapper(cfg => { }, AppDomain.CurrentDomain.GetAssemblies());
            
            builder.Services.ConfigureApplicationCookie(options =>
            {
                // لو حاول يروح Login redirect خلي الاستجابة 401
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                };

                // لو حاول يروح AccessDenied redirect خلي الاستجابة 403
                options.Events.OnRedirectToAccessDenied = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return Task.CompletedTask;
                };
            });
           
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("UserOrAdmin", policy =>
                        policy.RequireClaim("role", "User", "Admin"));
                // policy.RequireRole("User", "Admin"));
            });
           
            var app = builder.Build();

             if (app.Environment.IsDevelopment())
             {
                app.UseSwagger();
                app.UseSwaggerUI();
             }

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await SeedData.CreateRoles(services);
                await SeedData.CreateAdmin(services);
            }      // create Data Seed
            app.UseExceptionHandler();
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseMiddleware<LogginMiddelware>();
            app.UseAuthentication();
            app.UseAuthorization();
           // app.FeedbackMinimal();
            app.MapControllers();
            app.Run();
        }
    }
}
