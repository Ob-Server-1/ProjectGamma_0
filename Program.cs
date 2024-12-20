using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProjectGamma_0.controllers;
using ProjectGamma_0.models;
using Microsoft.EntityFrameworkCore;
using System.Text; // Необходим для работы с кодировкой
using System.Threading.Tasks;
using ProjectGamma_0; // Импортируем пространство имен для работы с задачами

var builder = WebApplication.CreateBuilder(args);

// Получаем строку конфигурации для подключения к базе данных
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DBContextController>(options => options.UseSqlite(connection)); // Регистрация сервиса базы данных

builder.Services.AddControllers();
builder.Services.AddScoped<PasswordHasher>(); // Регистрация вашего PasswordHasher как Scoped

// Регистрация JwtProvider с передачей секретного ключа из конфигурации
builder.Services.AddSingleton<JwtProvider>(provider =>
    new JwtProvider(builder.Configuration["JwtOptions:SecretKey"])); // Регистрация вашего JwtProvider

// Настройка аутентификации с использованием JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtOptions:SecretKey"])), // Ваш секретный ключ
    };
});

// Создание приложения
var app = builder.Build();
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); // Включение аутентификации
app.UseAuthorization(); // Включение авторизации

app.MapControllers(); // Этот метод включает маршрутизацию для контроллеров

app.Map("/map", (HttpContext context) =>
{
    context.Response.Redirect("/reg.html");
});

// Создаем базу данных и инициализируем данные
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DBContextController>();
    dbContext.Database.EnsureCreated(); // Создает базу данных, если она еще не существует
}

app.Run();
