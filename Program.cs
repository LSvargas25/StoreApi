using Microsoft.EntityFrameworkCore;
using StoreApi.Models;
using StoreApi.Interface.User;
using StoreApi.Services.User;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// DbContext
builder.Services.AddDbContext<StoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("StoreDb")));

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

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
