using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StoreApi.Interface.Audit;
using StoreApi.Interface.User; 
using StoreApi.Models;
using StoreApi.Repositorys.User;
using StoreApi.Services.Audit;
using StoreApi.Services.Auth;
using StoreApi.Services.User;
using StoreApi.Tools;
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
 


builder.Services.AddSingleton(new AesCrypto("YourSecretKeyHere")); // Cambiar por llave real

// -----------------------------------------------------------------------------
// 4. JWT AUTHENTICATION
// -----------------------------------------------------------------------------
var jwt = builder.Configuration.GetSection("Jwt");
var secretKey = Encoding.UTF8.GetBytes(jwt["Key"]!);

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
            IssuerSigningKey = new SymmetricSecurityKey(secretKey)
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
    c.RoutePrefix = string.Empty;
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
