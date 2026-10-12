using Microsoft.EntityFrameworkCore;
using NutriPlan.Infrastructure.Data; // Importa tu DbContext

var builder = WebApplication.CreateBuilder(args);

// 1. Configuración de Base de Datos (Inyección de Dependencias)
builder.Services.AddDbContext<NutriPlanDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Agregar servicios de controladores
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 3. Configuración del Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();