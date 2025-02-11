using DeliveryApi.Application.Services;
using DeliveryApi.Application.Validators;
using DeliveryApi.Domain.Repositories;
using DeliveryApi.Infrastructure.Repositories;
using DeliveryApi.Web.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using DeliveryApi.Application.DTOs;
using DeliveryApi.Application.Interfaces;
using DeliveryApi.Infrastructure.Messaging;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IValidator<OrderDto>, OrderDtoValidator>();
builder.Services.AddScoped<IValidator<OrderItemDto>, OrderItemDtoValidator>();

builder.Services.AddControllers();

var rabbitMqSettings = builder.Configuration.GetSection("RabbitMqSettings").Get<RabbitMqSettings>();
builder.Services.AddSingleton(rabbitMqSettings);

builder.Services.AddSingleton<IMessageProducer, RabbitMqProducer>();

var mongoDbSettings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();

// Registrar MongoDbSettings como um servi�o singleton
builder.Services.AddSingleton(mongoDbSettings);

// Registrar IMongoClient e IMongoDatabase
builder.Services.AddScoped<IMongoClient>(sp => new MongoClient(mongoDbSettings.ConnectionString));
builder.Services.AddScoped<IMongoDatabase>(
    sp => sp.GetRequiredService<IMongoClient>().GetDatabase(mongoDbSettings.DatabaseName)
);

BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

// Registrar validadores do FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<OrderDtoValidator>();

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
builder.Services.AddSingleton(jwtSettings);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.SecretKey)
            )
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "DeliveryApi", Version = "v1" });

    // Adicionar suporte para JWT no Swagger
    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Description =
                "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        }
    );

    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
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
                Array.Empty<string>()
            }
        }
    );
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowAll",
        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
    );
});

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services
    .AddDataProtection()
    .SetApplicationName("DeliveryApi")
    .PersistKeysToFileSystem(new DirectoryInfo("/keys"))
    .DisableAutomaticKeyGeneration();

builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = null;
});

builder.WebHost.UseUrls("http://+:5000");
var app = builder.Build();

app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
