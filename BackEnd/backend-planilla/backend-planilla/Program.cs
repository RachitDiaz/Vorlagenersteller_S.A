using backend_planilla.Application;
using backend_planilla.Infraestructure;
using backend_planilla.Handlers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);
var issuer = configuration["Jwt:Issuer"];
var audience = configuration["Jwt:Audience"];
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});
builder.Services.AddScoped<IEmpleadoRepository, EmpleadoRepository>();
builder.Services.AddScoped<IGetDeduccionBeneficiosQuery, GetDeduccionBeneficiosQuery>();
builder.Services.AddScoped<ICalculoDeduccionesObligatorias, CalculoDeduccionesObligatorias>();
builder.Services.AddScoped<IBeneficioQuery, BeneficioQuery>();
builder.Services.AddScoped<IPlanillaRepository, PlanillaRepository>();
builder.Services.AddScoped<IGenerarCalculosRepository, GenerarCalculosRepository>();
builder.Services.AddScoped<IGenerarCalculosQuery, GenerarCalculosQuery>();
builder.Services.AddScoped<IGenerarPlanilla, GenerarPlanilla>();
builder.Services.AddScoped<IEmpresaRepository, EmpresaRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:8081", "http://localhost:8080", "http://192.168.0.8:8080")
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IEmpleadoRepository, EmpleadoRepository>();
builder.Services.AddScoped<IBeneficioRepository, BeneficioRepository>();
builder.Services.AddScoped<IBeneficiosRepository, BeneficiosRepository>();
builder.Services.AddScoped<IGetDeduccionBeneficiosQuery, GetDeduccionBeneficiosQuery>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
