using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using Microsoft.AspNetCore.Authentication.JwtBearer; // New
using Microsoft.IdentityModel.Tokens; // New
using System.Text;
using TodoApi.Infrastructure.Services;
using TodoApi.Infrastructure.Jobs; // <--- ADD THIS for LicenseExpiryJob
using TodoApi.Middleware;
using Hangfire; // <--- ADD THIS
using Hangfire.MySql; // <--- ADD THIS
using Microsoft.AspNetCore.RateLimiting; // <--- ADD THIS for RateLimiter
using System.Threading.RateLimiting; // <--- ADD THIS for FixedWindow options

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var jwtSecretKey = jwtSettings["Key"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey))
    };
});
builder.Services.AddControllers();

builder.Services.AddScoped< UserService>();


builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
     ServerVersion.AutoDetect(
            builder.Configuration.GetConnectionString("DefaultConnection")
    )
    )
);
builder.Services.AddHangfire(config => 
    config.UseStorage(new MySqlStorage(builder.Configuration.GetConnectionString("DefaultConnection"), new MySqlStorageOptions())));
builder.Services.AddHangfireServer();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddStackExchangeRedisCache(options => {
    options.Configuration = builder.Configuration.GetConnectionString("Redis") ?? "localhost:6379";
});
builder.Services.AddRateLimiter(options => {
    options.AddFixedWindowLimiter("StrictPolicy", opt => {
        opt.PermitLimit = 5;
        opt.Window = TimeSpan.FromMinutes(1);
    });
});

var app = builder.Build();
app.UseHangfireDashboard();

// 3. Schedule Job
RecurringJob.AddOrUpdate<LicenseExpiryJob>("daily-expiry", job => job.RunAsync(), Cron.Daily);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();

app.MapControllers();

app.Run();
