using CarpetCleaningSystem.API.Middlewares;
using CarpetCleaningSystem.Application.Abstractions.Repositories;
using CarpetCleaningSystem.Application.Abstractions.Services;
using CarpetCleaningSystem.Application.Customers.CreateCustomer;
using CarpetCleaningSystem.Application.Customers.GetCustomerById;
using CarpetCleaningSystem.Application.Customers.UpdateCustomer;
using CarpetCleaningSystem.Application.Orders.CancelOrder;
using CarpetCleaningSystem.Application.Orders.CompleteOrder;
using CarpetCleaningSystem.Application.Orders.CreateOrder;
using CarpetCleaningSystem.Application.Orders.GetOrderById;
using CarpetCleaningSystem.Application.Orders.StartProcessing;
using CarpetCleaningSystem.Application.Orders.SubmitOrder;
using CarpetCleaningSystem.Application.Orders.UpdateOrder.ChangeMaterial;
using CarpetCleaningSystem.Infrastructure.Persistence;
using CarpetCleaningSystem.Infrastructure.Repositories;
using CarpetCleaningSystem.Infrastructure.Services;
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
            builder.Services.AddScoped<IPricingService, PricingService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<CreateCustomerHandler>();
            builder.Services.AddScoped<GetCustomerByIdHandler>();
            builder.Services.AddScoped<UpdateCustomerHandler>();
            builder.Services.AddScoped<CreateOrderHandler>();
            builder.Services.AddScoped<GetOrderByIdHandler>();
            builder.Services.AddScoped<ChangeCleaningTypeHandler>();
            builder.Services.AddScoped<ChangeDimensionsHandler>();
            builder.Services.AddScoped<ChangeMaterialHandler>();
            builder.Services.AddScoped<ChangePickUpDateHandler>();
            builder.Services.AddScoped<SubmitOrderHandler>();
            builder.Services.AddScoped<StartProcessingHandler>();
            builder.Services.AddScoped<CompleteOrderHandler>();
            builder.Services.AddScoped<CancelOrderHandler>();


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

