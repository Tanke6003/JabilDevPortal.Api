using JabilDevPortal.Api.Data;
using JabilDevPortal.Api.Helpers;
using JabilDevPortal.Api.Services.Implementations;
using JabilDevPortal.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BackEnd.Infrastructure.Plugins;
using JabilDevPortal.Api.Data.Models.Config;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options => { options.AddDefaultPolicy(builder => { builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); }); });


// 2. JWT settings
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
var jwt = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

// 3. Servicios de autenticación
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped< PGSQLConnectionPlugin>(provider => new PGSQLConnectionPlugin(new SQLDataBaseSettings()
{
    Database = "JabilDevPortal",
    Server = "18.226.200.67",
    Password = "TuPssw0rdFuerte!",
    User = "sa"
}));


// 4. Añade controllers (¡IMPRESCINDIBLE!)
builder.Services.AddControllers();

// 5. JWT Bearer
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer           = true,
        ValidateAudience         = true,
        ValidateLifetime         = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer              = jwt.Issuer,
        ValidAudience            = jwt.Audience,
        IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Secret))
    };
});

// 6. Resto de servicios de la aplicación
builder.Services.AddScoped<IApplicationService, ApplicationService>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserService, UserService>();

// 7. Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 8. Autorización
builder.Services.AddAuthorization();


var app = builder.Build();
app.UseRouting();

// 9. Middleware
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    c.RoutePrefix = "swagger";
});

app.UseCors(
                options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
            ); // Habilita CORS con la política predeterminada

app.UseAuthentication();
app.UseAuthorization();
// después de app.UseAuthorization();

app.MapGet("/api/health", () => Results.Ok(new { status = "OK" }))
   .AllowAnonymous();


// 10. Rutas de controllers
app.MapControllers();

app.Run();
