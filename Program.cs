using Microsoft.EntityFrameworkCore;
using ProductManagement.Repositories.Implementation;
using ProductManagement.Repositories.Interfaces;
using ProductManagement.Services.Implementation;
using ProductManagement.Services.Interfaces;
using ProductManagement.Data;
namespace ProductManagement
{ 
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();


            builder.Services.AddDbContext<AppDbContext>(options =>
      options.UseNpgsql(

          builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins(
                            "https://product-management-ten-lime.vercel.app",
                            "http://localhost:5173"
                        )
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
            var app = builder.Build();

            app.UseCors("AllowFrontend");
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        } 
    }
}