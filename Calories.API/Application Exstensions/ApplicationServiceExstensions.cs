using Calories.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Calories.API.Application_Exstensions
{
    public static class ApplicationServiceExstensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<CaloriesDbContext>(options =>
            {
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<CaloriesDbContext>()
                .AddDefaultTokenProviders();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
                    Name = "Authorization",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });
            services.AddCors();
            services.AddAuthentication();
            services.AddAuthorization();
            services.AddControllers();
            services.AddAutoMapper(cfg => { }, AppDomain.CurrentDomain.GetAssemblies());
            return services;
        }
    }
}
