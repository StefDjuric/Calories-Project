using Calories.API.Application_Exstensions;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using System.Threading.Tasks;

namespace Calories.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddApplicationServices(builder.Configuration);
            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                // Creating test users if needed
                using(var scope = app.Services.CreateScope())
                {
                    var serviceProvider = scope.ServiceProvider;
                    try
                    {
                        await SeedDb.SeedRolesAndUsersAsync(serviceProvider);
                    }catch (Exception ex)
                    {
                        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "An error occurred while seeding the database");
                    }
                }
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseHttpsRedirection();
            app.UseCors(options =>
            {
                options.AllowAnyMethod();
                options.AllowAnyHeader();
                options.AllowAnyOrigin();
            });
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "../Calories.Application/client";

                if (app.Environment.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });

            app.Run();
        }
    }
}
