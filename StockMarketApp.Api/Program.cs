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
using StockMarketApp.Application.Validations;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ==============================
// 1. CAPA DE SERVICIOS (Antes del Build)
// ==============================

// A. Base de Datos (ACTUALIZADO CON RESILIENCIA)
builder.Services.AddDbContext<StockDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    sqlOptions => 
    {
        // Esto permite que EF encuentre las migraciones en este proyecto
        sqlOptions.MigrationsAssembly("StockMarketApp.Api");
        
        // --- ESTO ES LO NUEVO PARA AZURE ---
        // Habilita reintentos automáticos si la conexión falla temporalmente
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5, 
            maxRetryDelay: TimeSpan.FromSeconds(30), 
            errorNumbersToAdd: null);
    });
});

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
builder.Services.AddFluentValidationAutoValidation(); 
builder.Services.AddValidatorsFromAssemblyContaining<CreateStockRequestValidator>(); 

// E. Controladores y Swagger
builder.Services.AddEndpointsApiExplorer(); // Necesario para Swagger clásico
// E. Controladores y Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "StockMarket API", Version = "v1" });
    
    // Le decimos a Swagger que usamos JWT
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Por favor ingresa el token válido. Ejemplo: 'Bearer eyJhb...'",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    
    // Le decimos a Swagger que pida el token en los endpoints
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
}); // Usamos SwaggerGen para ver la UI bonita


// ==============================
// CONFIGURACIÓN CORS (Permitir a Angular hablar con la API)
// ==============================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        // Angular corre por defecto en el puerto 4200
        policy.WithOrigins("http://localhost:4200") 
              .AllowAnyMethod()  // Permite GET, POST, PUT, DELETE
              .AllowAnyHeader(); // Permite enviar Tokens JWT
    });
});
// ==============================
// 2. CONSTRUCCIÓN (El punto de no retorno)
// ==============================
var app = builder.Build();

// ==============================
// 3. PIPELINE HTTP (Middleware)
// ==============================

// Configuramos Swagger para que se vea SIEMPRE (incluso en Azure/Producción)
// Esto soluciona tu error 404
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// --- ¡ESTAS DOS LÍNEAS ERAN VITALES! ---
app.UseAuthentication(); // 1. ¿Quién eres? (Chequea el Token)
app.UseAuthorization();  // 2. ¿Qué puedes hacer? (Chequea Roles)
// ---------------------------------------

app.MapControllers(); 

app.Run();