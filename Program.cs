using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProjectGamma_0.controllers;
using ProjectGamma_0.models;
using Microsoft.EntityFrameworkCore;
using System.Text; // ��������� ��� ������ � ����������
using System.Threading.Tasks;
using ProjectGamma_0; // ����������� ������������ ���� ��� ������ � ��������

var builder = WebApplication.CreateBuilder(args);

// �������� ������ ������������ ��� ����������� � ���� ������
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DBContextController>(options => options.UseSqlite(connection)); // ����������� ������� ���� ������

builder.Services.AddControllers();
builder.Services.AddScoped<PasswordHasher>(); // ����������� ������ PasswordHasher ��� Scoped

// ����������� JwtProvider � ��������� ���������� ����� �� ������������
builder.Services.AddSingleton<JwtProvider>(provider =>
    new JwtProvider(builder.Configuration["JwtOptions:SecretKey"])); // ����������� ������ JwtProvider

// ��������� �������������� � �������������� JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtOptions:SecretKey"])), // ��� ��������� ����
    };
});

// �������� ����������
var app = builder.Build();
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); // ��������� ��������������
app.UseAuthorization(); // ��������� �����������

app.MapControllers(); // ���� ����� �������� ������������� ��� ������������

app.Map("/map", (HttpContext context) =>
{
    context.Response.Redirect("/reg.html");
});

// ������� ���� ������ � �������������� ������
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DBContextController>();
    dbContext.Database.EnsureCreated(); // ������� ���� ������, ���� ��� ��� �� ����������
}

app.Run();
