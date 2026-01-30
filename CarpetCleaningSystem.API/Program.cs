using CarpetCleaningSystem.API.Middlewares;
using CarpetCleaningSystem.Application.Abstractions.Repositories;
using CarpetCleaningSystem.Application.Customers.CreateCustomer;
using CarpetCleaningSystem.Application.Customers.GetCustomerById;
using CarpetCleaningSystem.Application.Customers.UpdateCustomer;
using CarpetCleaningSystem.Application.Orders.CreateOrder;
using CarpetCleaningSystem.Application.Orders.GetOrderById;
using CarpetCleaningSystem.Infrastructure.Persistence;
using CarpetCleaningSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace CarpetCleaningSystem.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //  DbContext
            builder.Services.AddDbContext<AppDBContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

            //  Repositories & UoW
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<CreateCustomerHandler>();
            builder.Services.AddScoped<GetCustomerByIdHandler>();
            builder.Services.AddScoped<UpdateCustomerHandler>();
            builder.Services.AddScoped<CreateOrderHandler>();
            builder.Services.AddScoped<GetOrderByIdHandler>();

            //  API stuff
            builder.Services.AddAuthorization();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddControllers()
                .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<GlobalExceptionMiddleware>();

            //app.UseAuthentication();
            //app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}

