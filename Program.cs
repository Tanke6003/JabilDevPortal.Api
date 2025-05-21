using JabilDevPortal.Api.Data;
using JabilDevPortal.Api.Helpers;
using JabilDevPortal.Api.Services.Implementations;
using JabilDevPortal.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularDevPolicy", policy =>
    {
        policy
          .AllowAnyOrigin()  // URL de tu front-end
          .AllowAnyHeader()
          .AllowAnyMethod();
    });
});

// 1. DbContext
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. JWT settings
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
var jwt = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

// 3. Servicios de autenticación
builder.Services.AddScoped<IAuthService, AuthService>();

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

// 7. Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 8. Autorización
builder.Services.AddAuthorization();

var app = builder.Build();

// 9. Middleware
app.UseSwagger();
app.UseSwaggerUI();

// 3) Usa CORS **antes** de autenticación/autorización
app.UseCors("AngularDevPolicy");

app.UseAuthentication();
app.UseAuthorization();

// 10. Rutas de controllers
app.MapControllers();

app.Run();
