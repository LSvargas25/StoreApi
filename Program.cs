using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StoreApi.Interface.Audit;
using StoreApi.Interface.Customer;
using StoreApi.Interface.Invoice;
using StoreApi.Interface.Item;
using StoreApi.Interface.Purchase;
using StoreApi.Interface.Supplier;
using StoreApi.Interface.Tax;
using StoreApi.Interface.User;
using StoreApi.Interface.WareHouse;
using StoreApi.Models;
using StoreApi.Repository.Supplier;
using StoreApi.Repositorys.Supplier;
using StoreApi.Repositorys.User;
using StoreApi.Repositorys.WareHouse;
using StoreApi.Services.Audit;
using StoreApi.Services.Auth;
using StoreApi.Services.Customer;
using StoreApi.Services.Invoice;
using StoreApi.Services.Item;
using StoreApi.Services.Purchase;
using StoreApi.Services.Supplier;
using StoreApi.Services.Tax;
using StoreApi.Services.User;
using StoreApi.Services.WareHouse;
using StoreApi.Tools;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------------------------------------------------------
// 1. CONFIGURACIÓN GENERAL
// -----------------------------------------------------------------------------
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        // Respuestas limpias para errores de validación del modelo
        options.InvalidModelStateResponseFactory = ctx =>
        {
            var errors = ctx.ModelState
                .Where(v => v.Value.Errors.Count > 0)
                .Select(x => new
                {
                    Field = x.Key,
                    Error = x.Value.Errors.First().ErrorMessage
                });

            return new BadRequestObjectResult(new
            {
                message = "Validation Error",
                errors
            });
        };
    });

// -----------------------------------------------------------------------------
// 2. BASE DE DATOS (EF CORE)
// -----------------------------------------------------------------------------
builder.Services.AddDbContext<StoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("StoreDb")));

// -----------------------------------------------------------------------------
// 3. SERVICIOS DEL SISTEMA
// -----------------------------------------------------------------------------
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICustomPasswordService, CustomPasswordService>();
builder.Services.AddScoped<IUserRoleService, UserRoleService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuditService, AuditService>();
builder.Services.AddScoped<ISupplierTypeService, SupplierTypeService>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<ICustomerRoleService, CustomerRoleService>();
builder.Services.AddScoped<IPurchaseTypeService,PurchaseTypeService>();
builder.Services.AddScoped<IItemCategoryService,ItemCategoryService>();
builder.Services.AddScoped<ITaxService, TaxService>();
builder.Services.AddScoped<IInvoiceTypeService, InvoiceTypeService>();
builder.Services.AddScoped<IWareHouseRepository, WareHouseRepository>();
builder.Services.AddScoped<IWareHouseService, WareHouseService>();






builder.Services.AddSingleton(new AesCrypto("YourSecretKeyHere"));

// -----------------------------------------------------------------------------
// 4. JWT AUTHENTICATION
// -----------------------------------------------------------------------------
// -----------------------------------------------------------------------------
// 4. JWT AUTHENTICATION
// -----------------------------------------------------------------------------
var jwt = builder.Configuration.GetSection("Jwt");
var secret = jwt["Secret"];

if (string.IsNullOrWhiteSpace(secret))
    throw new InvalidOperationException("JWT Secret is not configured");

var secretKey = Encoding.UTF8.GetBytes(secret);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(secretKey),

            ClockSkew = TimeSpan.Zero
        };
    });


// -----------------------------------------------------------------------------
// 5. CORS
// -----------------------------------------------------------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("StoreCors", policy =>
    {
        policy.WithOrigins(builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()!)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// -----------------------------------------------------------------------------
// 6. SWAGGER PROFESIONAL (CON JWT + API KEY)
// -----------------------------------------------------------------------------
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();

   




    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Store API",
        Version = "v1",
        Description = "Professional API for Store Management"
    });

    // JWT Bearer auth
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Insert your JWT Bearer token below.",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

// -----------------------------------------------------------------------------
var app = builder.Build();
// -----------------------------------------------------------------------------

// 7. USE SWAGGER
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Store API v1");
  
});

// 8. MIDDLEWARES PERSONALIZADOS
app.UseMiddleware<ApiKeyMiddleware>();       // API KEY
 

// 9. CORS
app.UseCors("StoreCors");

// 10. AUTH
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
