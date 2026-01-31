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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

namespace CarpetCleaningSystem.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // DbContext
            builder.Services.AddDbContext<AppDBContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

            // Identity (Users + Roles)
            builder.Services
                .AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDBContext>()
                .AddDefaultTokenProviders();

            // JWT Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var jwt = builder.Configuration.GetSection("Jwt");

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwt["Issuer"],
                    ValidAudience = jwt["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwt["Key"]!)
                    )
                };
            });

            // Authorization (Roles live here)
            builder.Services.AddAuthorization();

            // Swagger + Bearer token support
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "CarpetCleaningSystem API",
                    Version = "v1"
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "ВЬле: Bearer {token}"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new List<string>()
                    }
                });
            });

            builder.Services.AddControllers()
                .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            // Repositories & UoW
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IPricingService, PricingService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Handlers
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

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<GlobalExceptionMiddleware>();

            // MUST be before MapControllers
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}


