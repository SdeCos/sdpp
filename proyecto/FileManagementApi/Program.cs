using FileManagementApi.Data;
using FileManagementApi.Services;
using Microsoft.EntityFrameworkCore;
// CAMBIO: Asegúrate de que NO estás usando Microsoft.AspNetCore.OpenApi si te da conflictos, 
// o simplemente ignora, Swashbuckle funciona por su cuenta.
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddControllers();
// CAMBIO: Agregar Swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    // CAMBIO: Usar la UI de Swagger
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();
app.Run();