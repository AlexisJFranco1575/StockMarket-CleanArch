using Microsoft.EntityFrameworkCore;
using StockMarketApp.Infrastructure.Persistence;
using StockMarketApp.Domain.Interfaces;
using StockMarketApp.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using StockMarketApp.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using StockMarketApp.Infrastructure.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using StockMarketApp.Application.Validations; // Para encontrar el validador

var builder = WebApplication.CreateBuilder(args);

// ==============================
// 1. CAPA DE SERVICIOS (Antes del Build)
// ==============================

// A. Base de Datos
builder.Services.AddDbContext<StockDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly("StockMarketApp.Api"))); 

// B. Configuración de Identity (Usuarios)
builder.Services.AddIdentity<AppUser, IdentityRole>(options => 
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 12;
})
.AddEntityFrameworkStores<StockDbContext>();

// C. Configuración de Autenticación (JWT)
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = 
    options.DefaultChallengeScheme = 
    options.DefaultForbidScheme = 
    options.DefaultScheme = 
    options.DefaultSignInScheme = 
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])
        )
    };
});


// Agregamos NewtonsoftJson
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

// D. Repositorios e Inyección de Dependencias
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPortfolioRepository, PortfolioRepository>();
builder.Services.AddFluentValidationAutoValidation(); // Activa la validación automática
builder.Services.AddValidatorsFromAssemblyContaining<CreateStockRequestValidator>(); // Busca todos los validadores en el proyecto

// E. Controladores y Swagger
builder.Services.AddOpenApi(); 

// ==============================
// 2. CONSTRUCCIÓN (El punto de no retorno)
// ==============================
var app = builder.Build();

// ==============================
// 3. PIPELINE HTTP (Middleware)
// ==============================

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// --- ¡ESTAS DOS LÍNEAS ERAN VITALES! ---
app.UseAuthentication(); // 1. ¿Quién eres? (Chequea el Token)
app.UseAuthorization();  // 2. ¿Qué puedes hacer? (Chequea Roles)
// ---------------------------------------

app.MapControllers(); 

app.Run();