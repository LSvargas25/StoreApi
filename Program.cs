using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using StoreApi.Interface.User;
using StoreApi.Models;
using StoreApi.Services.User;
using StoreApi.Tools;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// DbContext
builder.Services.AddDbContext<StoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("StoreDb")));
// Register services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICustomPasswordService, CustomPasswordService>();
builder.Services.AddSingleton(new AesCrypto("YourSecretKeyHere"));
builder.Services.AddScoped<IUserRoleService, UserRoleService>();


// Register your service for DI
builder.Services.AddScoped<IUserRoleService, UserRoleService>();

 


// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Store API",
        Version = "v1"
    });
});

var app = builder.Build();

// Enable Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Store API v1");
    c.RoutePrefix = string.Empty;  
});

// 🔑 Add API key middleware 
app.UseMiddleware<ApiKeyMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
